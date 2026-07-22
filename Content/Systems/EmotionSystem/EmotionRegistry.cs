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
public sealed class EmotionRegistry : ModSystem
{
    private readonly record struct EmotionLookupKey(EmotionType Emotion, int Tier, EmotionBuffVariant Variant);
    private readonly record struct EmotionFamilyLookupKey(Type FamilyType, int Tier, EmotionBuffVariant Variant);
    private readonly record struct EmotionBuffMetadata(EmotionType Emotion, int Tier, EmotionBuffVariant Variant);
    private readonly record struct EmotionScalingRegistration(EmotionScalingMode Mode, int BuffType);

    private static readonly Dictionary<EmotionLookupKey, int> EmotionBuffTypes = [];
    private static readonly Dictionary<EmotionFamilyLookupKey, int> EmotionBuffTypesByFamily = [];
    private static readonly Dictionary<int, EmotionBuffMetadata> EmotionMetadataByBuffType = [];
    private static readonly Dictionary<EmotionType, int> MaxEmotionTierByType = [];
    private static readonly Dictionary<EmotionType, EmotionScalingRegistration> StandardScalingByType = [];

    private static void ClearRegistries()
    {
        EmotionBuffTypes.Clear();
        EmotionBuffTypesByFamily.Clear();
        EmotionMetadataByBuffType.Clear();
        MaxEmotionTierByType.Clear();
        StandardScalingByType.Clear();
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

    private static void RegisterEmotionBuff(EmotionBuff emotionBuff, EmotionBuffMetadata metadata)
    {
        EmotionLookupKey emotionKey = new(metadata.Emotion, metadata.Tier, metadata.Variant);
        AddUniqueRegistration(EmotionBuffTypes, emotionKey, emotionBuff);

        Type familyType = emotionBuff.GetType();
        while (familyType != null
            && familyType != typeof(EmotionBuff)
            && typeof(EmotionBuff).IsAssignableFrom(familyType))
        {
            EmotionFamilyLookupKey familyKey = new(familyType, metadata.Tier, metadata.Variant);
            AddUniqueRegistration(EmotionBuffTypesByFamily, familyKey, emotionBuff);
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
            EmotionMetadataByBuffType.Add(buffType, metadata);

            if (variant != EmotionBuffVariant.Standard)
            {
                continue;
            }

            ValidateStandardScalingMode(emotionBuff);

            if (!MaxEmotionTierByType.TryGetValue(emotionBuff.Emotion, out int currentMax) || tier > currentMax)
            {
                MaxEmotionTierByType[emotionBuff.Emotion] = tier;
            }
        }

        ValidateStandardEmotionTiers();
    }

    private static void ValidateStandardScalingMode(EmotionBuff emotionBuff)
    {
        if (!StandardScalingByType.TryGetValue(emotionBuff.Emotion, out EmotionScalingRegistration existing))
        {
            StandardScalingByType.Add(
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
    }

    private static void ValidateStandardEmotionTiers()
    {
        foreach ((EmotionType emotion, int maxTier) in MaxEmotionTierByType)
        {
            if (maxTier > EmotionStatTuning.PlayerMaxEmotionLevel)
            {
                throw new InvalidOperationException(
                    $"Emotion '{emotion}' has a final tier of {maxTier}, which exceeds PlayerMaxEmotionLevel ({EmotionStatTuning.PlayerMaxEmotionLevel}).");
            }

            for (int tier = 1; tier <= maxTier; tier++)
            {
                EmotionLookupKey key = new(emotion, tier, EmotionBuffVariant.Standard);
                if (!EmotionBuffTypes.ContainsKey(key))
                {
                    throw new InvalidOperationException(
                        $"Emotion '{emotion}' is missing standard tier {tier}. Standard emotion tiers must be contiguous from 1 through {maxTier}.");
                }
            }
        }
    }

    /// <summary>
    /// Gets the registered buff type for an emotion, tier, and variant.
    /// </summary>
    /// <returns>The tModLoader buff type, or <see langword="null"/> when no match is registered.</returns>
    internal static int? GetEmotionBuffType(
        EmotionType emotion,
        int emotionLevel,
        EmotionBuffVariant variant = EmotionBuffVariant.Standard)
    {
        EmotionLookupKey key = new(emotion, emotionLevel, variant);
        return EmotionBuffTypes.TryGetValue(key, out int buffType) ? buffType : null;
    }

    /// <summary>
    /// Gets the registered buff type belonging to the requested emotion-buff family, tier, and variant.
    /// </summary>
    /// <typeparam name="T">The concrete or base <see cref="EmotionBuff"/> family to search.</typeparam>
    /// <returns>The tModLoader buff type, or <see langword="null"/> when no match is registered.</returns>
    internal static int? GetEmotionBuffType<T>(
        int emotionLevel,
        EmotionBuffVariant variant = EmotionBuffVariant.Standard)
        where T : EmotionBuff
    {
        EmotionFamilyLookupKey key = new(typeof(T), emotionLevel, variant);
        return EmotionBuffTypesByFamily.TryGetValue(key, out int buffType) ? buffType : null;
    }

    /// <summary>Gets the next standard tier for an emotion.</summary>
    /// <returns>The next buff type, or <see langword="null"/> if it is not registered.</returns>
    internal static int? GetNextTierEmotionType(EmotionType currentEmotionType, int currentEmotionLevel)
    {
        return GetEmotionBuffType(currentEmotionType, currentEmotionLevel + 1);
    }

    /// <summary>Gets the next standard tier in the supplied buff's registered family.</summary>
    /// <returns>The next buff type, or <see langword="null"/> at the final tier or for unregistered buffs.</returns>
    internal static int? GetNextTierEmotionType<T>(T currentEmotion) where T : EmotionBuff
    {
        return !EmotionMetadataByBuffType.TryGetValue(currentEmotion.Type, out EmotionBuffMetadata metadata)
            || IsFinalEmotionTier(currentEmotion.Type)
            ? null
            : GetEmotionBuffType(metadata.Emotion, metadata.Tier + 1);
    }

    /// <summary>Gets the tier declared by a registered emotion buff type.</summary>
    internal static int? GetEmotionTier(int buffType)
    {
        return EmotionMetadataByBuffType.TryGetValue(buffType, out EmotionBuffMetadata metadata)
            ? metadata.Tier
            : null;
    }

    /// <summary>Gets the highest registered standard tier for an emotion.</summary>
    internal static int? GetMaxEmotionTier(EmotionType emotion)
    {
        return MaxEmotionTierByType.TryGetValue(emotion, out int maxTier) ? maxTier : null;
    }

    /// <summary>Determines whether a buff type is the final registered standard tier of its emotion.</summary>
    internal static bool IsFinalEmotionTier(int buffType)
    {
        return EmotionMetadataByBuffType.TryGetValue(buffType, out EmotionBuffMetadata metadata)
            && metadata.Variant == EmotionBuffVariant.Standard
            && MaxEmotionTierByType.TryGetValue(metadata.Emotion, out int maxTier)
            && metadata.Tier == maxTier;
    }

    /// <summary>Gets the registered duration variant of an emotion buff type.</summary>
    internal static EmotionBuffVariant? GetEmotionVariant(int buffType)
    {
        return EmotionMetadataByBuffType.TryGetValue(buffType, out EmotionBuffMetadata metadata)
            ? metadata.Variant
            : null;
    }
}
