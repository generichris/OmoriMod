using System;

using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Systems.EmotionSystem;

/// <summary>
/// Provides read-only access to registered emotion buff metadata.
/// </summary>
public interface IEmotionRegistry
{
    int? GetEmotionBuffType(
        EmotionType emotion,
        int emotionLevel,
        EmotionBuffVariant variant = EmotionBuffVariant.Standard);

    int? GetEmotionBuffType(
        Type familyType,
        int emotionLevel,
        EmotionBuffVariant variant = EmotionBuffVariant.Standard);

    EmotionType? GetEmotion(int buffType);

    int? GetEmotionTier(int buffType);

    int? GetMaxEmotionTier(EmotionType emotion);

    bool IsFinalEmotionTier(int buffType);

    EmotionBuffVariant? GetEmotionVariant(int buffType);
}