using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Systems.EmotionSystem;

namespace OmoriMod.Tests.TestDoubles;

internal sealed class FakeEmotionRegistry : IEmotionRegistry
{
    private readonly record struct EmotionKey(EmotionType Emotion, int Tier, EmotionBuffVariant Variant);
    private readonly record struct FamilyKey(Type FamilyType, int Tier, EmotionBuffVariant Variant);
    private readonly record struct Metadata(EmotionType Emotion, int Tier, EmotionBuffVariant Variant);

    private readonly Dictionary<EmotionKey, int> _buffTypes = [];
    private readonly Dictionary<FamilyKey, int> _familyBuffTypes = [];
    private readonly Dictionary<int, Metadata> _metadata = [];
    private readonly Dictionary<EmotionType, int> _maximumTiers = [];

    internal FakeEmotionRegistry Add(
        EmotionType emotion,
        int tier,
        EmotionBuffVariant variant,
        Type familyType,
        int buffType)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(tier, 1);
        ArgumentNullException.ThrowIfNull(familyType);

        if (!typeof(EmotionBuff).IsAssignableFrom(familyType))
        {
            throw new ArgumentException(
                $"{familyType.FullName} must derive from {typeof(EmotionBuff).FullName}.",
                nameof(familyType));
        }

        AddUnique(_buffTypes, new EmotionKey(emotion, tier, variant), buffType);
        AddUnique(_metadata, buffType, new Metadata(emotion, tier, variant));

        Type? currentFamily = familyType;
        while (currentFamily != null
            && currentFamily != typeof(EmotionBuff)
            && typeof(EmotionBuff).IsAssignableFrom(currentFamily))
        {
            AddUnique(_familyBuffTypes, new FamilyKey(currentFamily, tier, variant), buffType);
            currentFamily = currentFamily.BaseType;
        }

        if (variant == EmotionBuffVariant.Standard
            && (!_maximumTiers.TryGetValue(emotion, out int maximumTier) || tier > maximumTier))
        {
            _maximumTiers[emotion] = tier;
        }

        return this;
    }

    public int? GetEmotionBuffType(
        EmotionType emotion,
        int emotionLevel,
        EmotionBuffVariant variant = EmotionBuffVariant.Standard)
    {
        return _buffTypes.TryGetValue(new EmotionKey(emotion, emotionLevel, variant), out int buffType)
            ? buffType
            : null;
    }

    public int? GetEmotionBuffType(
        Type familyType,
        int emotionLevel,
        EmotionBuffVariant variant = EmotionBuffVariant.Standard)
    {
        return _familyBuffTypes.TryGetValue(new FamilyKey(familyType, emotionLevel, variant), out int buffType)
            ? buffType
            : null;
    }

    public EmotionType? GetEmotion(int buffType)
    {
        return _metadata.TryGetValue(buffType, out Metadata metadata) ? metadata.Emotion : null;
    }

    public int? GetEmotionTier(int buffType)
    {
        return _metadata.TryGetValue(buffType, out Metadata metadata) ? metadata.Tier : null;
    }

    public int? GetMaxEmotionTier(EmotionType emotion)
    {
        return _maximumTiers.TryGetValue(emotion, out int maximumTier) ? maximumTier : null;
    }

    public bool IsFinalEmotionTier(int buffType)
    {
        return _metadata.TryGetValue(buffType, out Metadata metadata)
            && metadata.Variant == EmotionBuffVariant.Standard
            && _maximumTiers.TryGetValue(metadata.Emotion, out int maximumTier)
            && metadata.Tier == maximumTier;
    }

    public EmotionBuffVariant? GetEmotionVariant(int buffType)
    {
        return _metadata.TryGetValue(buffType, out Metadata metadata) ? metadata.Variant : null;
    }

    private static void AddUnique<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        where TKey : notnull
    {
        if (!dictionary.TryAdd(key, value))
        {
            throw new InvalidOperationException($"Duplicate fake emotion registration for '{key}'.");
        }
    }
}