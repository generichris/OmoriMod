using System;
using System.Collections.Generic;

using OmoriMod.Content.Buffs.Abstract;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Systems.EmotionSystem;

/// <summary>
/// Discovers loaded <see cref="EmotionBuff"/> types and builds the lookup tables used by
/// <see cref="EmotionSystem"/> for tier, family, variant, and progression queries.
/// </summary>
/// <remarks>
/// Standard tiers are validated during content setup. Each emotion family must begin at tier one,
/// remain contiguous, use one scaling policy, and not exceed the configured player tier limit.
/// No-time variants are registered for lookup but do not define standard-family policy or limits.
/// </remarks>
public sealed class EmotionRegistry : ModSystem, IEmotionRegistry
{
    private readonly record struct EmotionLookupKey(EmotionType Emotion, int Tier, EmotionBuffVariant Variant);
    private readonly record struct EmotionFamilyLookupKey(Type FamilyType, int Tier, EmotionBuffVariant Variant);
    private readonly record struct EmotionBuffMetadata(EmotionType Emotion, int Tier, EmotionBuffVariant Variant);
    private readonly record struct EmotionScalingRegistration(EmotionScalingMode Mode, int BuffType);

    private readonly Dictionary<EmotionLookupKey, int> _emotionBuffTypes = [];
    private readonly Dictionary<EmotionFamilyLookupKey, int> _emotionBuffTypesByFamily = [];
    private readonly Dictionary<int, EmotionBuffMetadata> _emotionMetadataByBuffType = [];
    private readonly Dictionary<EmotionType, int> _maxEmotionTierByType = [];
    private readonly Dictionary<EmotionType, EmotionScalingRegistration> _standardScalingByType = [];

    /// <summary>Connects the gameplay facade to this registry instance.</summary>
    public override void Load()
    {
        EmotionSystem.InitializeRegistry(this);
    }

    private void ClearRegistries()
    {
        _emotionBuffTypes.Clear();
        _emotionBuffTypesByFamily.Clear();
        _emotionMetadataByBuffType.Clear();
        _maxEmotionTierByType.Clear();
        _standardScalingByType.Clear();
    }

    private static void AddUniqueRegistration<TKey>(Dictionary<TKey, int> registry, TKey key, EmotionBuff emotionBuff)
        where TKey : notnull
    {
        if (registry.TryGetValue(key, out int existingBuffType))
        {
            ModBuff existingBuff = ModContent.GetModBuff(existingBuffType);
            string existingName = existingBuff?.FullName ?? existingBuffType.ToString();
            throw new InvalidOperationException(
                $"Duplicate emotion buff registration for {key}: '{existingName}' and '{emotionBuff.FullName}'.");
        }

        registry.Add(key, emotionBuff.Type);
    }

    private void RegisterEmotionBuff(EmotionBuff emotionBuff, EmotionBuffMetadata metadata)
    {
        EmotionLookupKey emotionKey = new(metadata.Emotion, metadata.Tier, metadata.Variant);
        AddUniqueRegistration(_emotionBuffTypes, emotionKey, emotionBuff);

        Type familyType = emotionBuff.GetType();
        while (familyType != null
            && familyType != typeof(EmotionBuff)
            && typeof(EmotionBuff).IsAssignableFrom(familyType))
        {
            EmotionFamilyLookupKey familyKey = new(familyType, metadata.Tier, metadata.Variant);
            AddUniqueRegistration(_emotionBuffTypesByFamily, familyKey, emotionBuff);
            familyType = familyType.BaseType;
        }
    }

    /// <summary>
    /// Rebuilds emotion metadata after all mod content has been registered by tModLoader.
    /// </summary>
    public override void PostSetupContent()
    {
        ClearRegistries();

        for (int buffType = 0; buffType < BuffLoader.BuffCount; buffType++)
        {
            if (ModContent.GetModBuff(buffType) is not EmotionBuff emotionBuff)
            {
                continue;
            }

            int tier = emotionBuff.EmotionTier;
            if (tier < 1)
            {
                throw new InvalidOperationException(
                    $"Emotion buff '{emotionBuff.FullName}' must declare an emotion level of at least 1, but declared {tier}.");
            }

            EmotionBuffVariant variant = Main.buffNoTimeDisplay[buffType]
                ? EmotionBuffVariant.NoTime
                : EmotionBuffVariant.Standard;
            EmotionBuffMetadata metadata = new(emotionBuff.Emotion, tier, variant);

            RegisterEmotionBuff(emotionBuff, metadata);
            _emotionMetadataByBuffType.Add(buffType, metadata);

            if (variant != EmotionBuffVariant.Standard)
            {
                continue;
            }

            ValidateStandardScalingMode(emotionBuff);

            if (!_maxEmotionTierByType.TryGetValue(emotionBuff.Emotion, out int currentMax) || tier > currentMax)
            {
                _maxEmotionTierByType[emotionBuff.Emotion] = tier;
            }
        }

        ValidateStandardEmotionTiers();
    }

    private void ValidateStandardScalingMode(EmotionBuff emotionBuff)
    {
        if (!_standardScalingByType.TryGetValue(emotionBuff.Emotion, out EmotionScalingRegistration existing))
        {
            _standardScalingByType.Add(
                emotionBuff.Emotion,
                new EmotionScalingRegistration(emotionBuff.ScalingMode, emotionBuff.Type));
            return;
        }

        if (existing.Mode == emotionBuff.ScalingMode)
        {
            return;
        }

        ModBuff existingBuff = ModContent.GetModBuff(existing.BuffType);
        string existingName = existingBuff?.FullName ?? existing.BuffType.ToString();
        throw new InvalidOperationException(
            $"Emotion '{emotionBuff.Emotion}' mixes standard scaling modes: " +
            $"'{existingName}' declares {existing.Mode}, while '{emotionBuff.FullName}' declares {emotionBuff.ScalingMode}. " +
            "Every standard tier in an emotion family must use the same EmotionScalingMode.");
    }

    /// <summary>Releases all cached emotion registration data when the mod unloads.</summary>
    public override void Unload()
    {
        ClearRegistries();
        EmotionSystem.ResetRegistry();
    }

    private void ValidateStandardEmotionTiers()
    {
        foreach ((EmotionType emotion, int maxTier) in _maxEmotionTierByType)
        {
            if (maxTier > EmotionStatTuning.PlayerMaxEmotionLevel)
            {
                throw new InvalidOperationException(
                    $"Emotion '{emotion}' has a final tier of {maxTier}, which exceeds PlayerMaxEmotionLevel ({EmotionStatTuning.PlayerMaxEmotionLevel}).");
            }

            for (int tier = 1; tier <= maxTier; tier++)
            {
                EmotionLookupKey key = new(emotion, tier, EmotionBuffVariant.Standard);
                if (!_emotionBuffTypes.ContainsKey(key))
                {
                    throw new InvalidOperationException(
                        $"Emotion '{emotion}' is missing standard tier {tier}. Standard emotion tiers must be contiguous from 1 through {maxTier}.");
                }
            }
        }
    }

    int? IEmotionRegistry.GetEmotionBuffType(
        EmotionType emotion,
        int emotionLevel,
        EmotionBuffVariant variant)
    {
        EmotionLookupKey key = new(emotion, emotionLevel, variant);
        return _emotionBuffTypes.TryGetValue(key, out int buffType) ? buffType : null;
    }

    int? IEmotionRegistry.GetEmotionBuffType(
        Type familyType,
        int emotionLevel,
        EmotionBuffVariant variant)
    {
        EmotionFamilyLookupKey key = new(familyType, emotionLevel, variant);
        return _emotionBuffTypesByFamily.TryGetValue(key, out int buffType) ? buffType : null;
    }

    EmotionType? IEmotionRegistry.GetEmotion(int buffType)
    {
        return _emotionMetadataByBuffType.TryGetValue(buffType, out EmotionBuffMetadata metadata)
            ? metadata.Emotion
            : null;
    }

    int? IEmotionRegistry.GetEmotionTier(int buffType)
    {
        return _emotionMetadataByBuffType.TryGetValue(buffType, out EmotionBuffMetadata metadata)
            ? metadata.Tier
            : null;
    }

    int? IEmotionRegistry.GetMaxEmotionTier(EmotionType emotion)
    {
        return _maxEmotionTierByType.TryGetValue(emotion, out int maxTier) ? maxTier : null;
    }

    bool IEmotionRegistry.IsFinalEmotionTier(int buffType)
    {
        return _emotionMetadataByBuffType.TryGetValue(buffType, out EmotionBuffMetadata metadata)
            && metadata.Variant == EmotionBuffVariant.Standard
            && _maxEmotionTierByType.TryGetValue(metadata.Emotion, out int maxTier)
            && metadata.Tier == maxTier;
    }

    EmotionBuffVariant? IEmotionRegistry.GetEmotionVariant(int buffType)
    {
        return _emotionMetadataByBuffType.TryGetValue(buffType, out EmotionBuffMetadata metadata)
            ? metadata.Variant
            : null;
    }
}