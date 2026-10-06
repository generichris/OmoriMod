using System;
using System.Collections.Generic;

using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Systems.EmotionSystem.Interfaces;

namespace OmoriMod.Content.Systems.EmotionSystem;

/// <summary>
/// Implements registry-backed emotion queries and decisions independently of tModLoader startup.
/// </summary>
internal sealed class EmotionService
{
    internal static EmotionService Empty { get; } = new(EmptyEmotionRegistry.Instance);

    private readonly IEmotionRegistry _registry;

    internal EmotionService(IEmotionRegistry registry)
    {
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
    }

    internal int? GetEmotionBuffType(
        EmotionType emotion,
        int emotionLevel,
        EmotionBuffVariant variant = EmotionBuffVariant.Standard)
    {
        return _registry.GetEmotionBuffType(emotion, emotionLevel, variant);
    }

    internal int? GetEmotionBuffType<T>(
        int emotionLevel,
        EmotionBuffVariant variant = EmotionBuffVariant.Standard)
        where T : EmotionBuff
    {
        return _registry.GetEmotionBuffType(typeof(T), emotionLevel, variant);
    }

    internal int? GetNextTierEmotionType(EmotionType currentEmotionType, int currentEmotionLevel)
    {
        return GetEmotionBuffType(currentEmotionType, currentEmotionLevel + 1);
    }

    internal int? GetNextTierEmotionType<T>(T currentEmotion) where T : EmotionBuff
    {
        int? currentTier = GetEmotionTier(currentEmotion.Type);
        EmotionType? registeredEmotion = _registry.GetEmotion(currentEmotion.Type);

        return !currentTier.HasValue
            || !registeredEmotion.HasValue
            || IsFinalEmotionTier(currentEmotion.Type)
                ? null
                : GetEmotionBuffType(registeredEmotion.Value, currentTier.Value + 1);
    }

    internal int? GetEmotionTier(int buffType)
    {
        return _registry.GetEmotionTier(buffType);
    }

    internal int? GetMaxEmotionTier(EmotionType emotion)
    {
        return _registry.GetMaxEmotionTier(emotion);
    }

    internal bool IsFinalEmotionTier(int buffType)
    {
        return _registry.IsFinalEmotionTier(buffType);
    }

    internal EmotionBuffVariant? GetEmotionVariant(int buffType)
    {
        return _registry.GetEmotionVariant(buffType);
    }

    internal int? GetPreferredEmotionBuffType(IEnumerable<int> candidateBuffTypes)
    {
        int? preferredBuffType = null;
        int preferredTier = 0;
        EmotionBuffVariant preferredVariant = EmotionBuffVariant.NoTime;

        foreach (int buffType in candidateBuffTypes)
        {
            int? tier = GetEmotionTier(buffType);
            EmotionBuffVariant? variant = GetEmotionVariant(buffType);
            if (!tier.HasValue || !variant.HasValue)
            {
                continue;
            }

            bool isPreferredVariant =
                variant.Value == EmotionBuffVariant.Standard
                && preferredVariant != EmotionBuffVariant.Standard;
            bool isHigherTierInSameVariant =
                variant.Value == preferredVariant
                && tier.Value > preferredTier;
            if (!preferredBuffType.HasValue
                || isPreferredVariant
                || isHigherTierInSameVariant)
            {
                preferredBuffType = buffType;
                preferredTier = tier.Value;
                preferredVariant = variant.Value;
            }
        }

        return preferredBuffType;
    }

    internal IReadOnlyList<int> GetRegisteredEmotionBuffTypes(IEnumerable<int> candidateBuffTypes)
    {
        List<int> registeredBuffTypes = [];
        foreach (int buffType in candidateBuffTypes)
        {
            if (_registry.GetEmotion(buffType).HasValue)
            {
                registeredBuffTypes.Add(buffType);
            }
        }

        return registeredBuffTypes;
    }

    internal bool IsValidScalingSync(
        EmotionType incomingEmotion,
        int incomingLevel,
        EmotionType currentEmotion,
        int currentLevel,
        int? activeBuffType,
        EmotionScalingMode activeScalingMode,
        bool requireActiveBuff)
    {
        if (incomingEmotion == EmotionType.None)
        {
            if (incomingLevel != 0)
            {
                return false;
            }

            if (!requireActiveBuff)
            {
                return true;
            }

            bool hasActiveCappedFinalEmotion =
                activeBuffType.HasValue
                && GetEmotionVariant(activeBuffType.Value) == EmotionBuffVariant.Standard
                && IsFinalEmotionTier(activeBuffType.Value)
                && activeScalingMode == EmotionScalingMode.Capped;
            return !hasActiveCappedFinalEmotion;
        }

        if (!Enum.IsDefined(incomingEmotion)
            || incomingLevel < 1
            || incomingLevel > EmotionStatTuning.PlayerMaxEmotionLevel)
        {
            return false;
        }

        int? finalTier = GetMaxEmotionTier(incomingEmotion);
        if (!finalTier.HasValue || incomingLevel < finalTier.Value)
        {
            return false;
        }

        if (!requireActiveBuff)
        {
            return true;
        }

        if (!activeBuffType.HasValue
            || _registry.GetEmotion(activeBuffType.Value) != incomingEmotion
            || GetEmotionVariant(activeBuffType.Value) != EmotionBuffVariant.Standard
            || !IsFinalEmotionTier(activeBuffType.Value)
            || activeScalingMode != EmotionScalingMode.Capped)
        {
            return false;
        }

        if (currentEmotion != incomingEmotion || currentLevel < finalTier.Value)
        {
            return incomingLevel == finalTier.Value
                || incomingLevel == finalTier.Value + 1;
        }

        return incomingLevel >= currentLevel
            && incomingLevel <= currentLevel + 1;
    }

    internal EmotionApplicationDecision GetApplicationDecision(
        EmotionApplicationRequest request,
        Type emotionFamilyType,
        int? currentBuffType,
        EmotionScalingMode scalingMode,
        int scalingLevel)
    {
        if (!currentBuffType.HasValue)
        {
            if (request == EmotionApplicationRequest.Amplifier)
            {
                return EmotionApplicationDecision.Blocked;
            }

            int? tierOneBuffType = _registry.GetEmotionBuffType(
                emotionFamilyType,
                emotionLevel: 1,
                EmotionBuffVariant.Standard);
            return tierOneBuffType.HasValue
                ? new EmotionApplicationDecision(EmotionApplicationAction.ApplyTierOne, tierOneBuffType)
                : EmotionApplicationDecision.Blocked;
        }

        int buffType = currentBuffType.Value;
        int? currentTier = GetEmotionTier(buffType);
        EmotionType? emotion = _registry.GetEmotion(buffType);
        EmotionBuffVariant? variant = GetEmotionVariant(buffType);
        int? maximumTier = emotion.HasValue
            ? GetMaxEmotionTier(emotion.Value)
            : null;

        if (!currentTier.HasValue
            || !maximumTier.HasValue
            || variant != EmotionBuffVariant.Standard
            || currentTier.Value > maximumTier.Value)
        {
            return EmotionApplicationDecision.Blocked;
        }

        if (scalingMode == EmotionScalingMode.Disabled)
        {
            if (request == EmotionApplicationRequest.Amplifier)
            {
                return EmotionApplicationDecision.Blocked;
            }

            if (currentTier.Value == maximumTier.Value)
            {
                return new EmotionApplicationDecision(EmotionApplicationAction.RefreshCurrent, buffType);
            }

            int? nextDisabledBuffType = GetEmotionBuffType(emotion.Value, currentTier.Value + 1);
            return nextDisabledBuffType.HasValue
                ? new EmotionApplicationDecision(EmotionApplicationAction.PromoteNextTier, nextDisabledBuffType)
                : EmotionApplicationDecision.Blocked;
        }

        if (currentTier.Value < maximumTier.Value)
        {
            bool isTierBeforeFinal = currentTier.Value == maximumTier.Value - 1;
            if (request == EmotionApplicationRequest.Amplifier && !isTierBeforeFinal)
            {
                return EmotionApplicationDecision.Blocked;
            }

            if (request == EmotionApplicationRequest.RegularItem && isTierBeforeFinal)
            {
                return new EmotionApplicationDecision(EmotionApplicationAction.RefreshCurrent, buffType);
            }

            int? nextBuffType = GetEmotionBuffType(emotion.Value, currentTier.Value + 1);
            return nextBuffType.HasValue
                ? new EmotionApplicationDecision(EmotionApplicationAction.PromoteNextTier, nextBuffType)
                : EmotionApplicationDecision.Blocked;
        }

        if (request == EmotionApplicationRequest.Amplifier)
        {
            EmotionApplicationAction action =
                scalingLevel >= EmotionStatTuning.PlayerMaxEmotionLevel
                    ? EmotionApplicationAction.RefreshCurrent
                    : EmotionApplicationAction.AmplifyCurrent;
            return new EmotionApplicationDecision(action, buffType);
        }

        return maximumTier.Value == 1 && scalingLevel <= maximumTier.Value
            ? new EmotionApplicationDecision(EmotionApplicationAction.RefreshCurrent, buffType)
            : EmotionApplicationDecision.Blocked;
    }

    internal int GetEmotionTier(IEmotionEntity entity)
    {
        EmotionBuff activeEmotion = entity.ActiveEmotionBuff;
        return activeEmotion == null
            ? 0
            : GetEmotionTier(activeEmotion.Type) ?? activeEmotion.EmotionTier;
    }

    internal int CalculateAdvantage(IEmotionEntity attacker, IEmotionEntity defender)
    {
        bool? attackerAdvantage = CheckForAdvantage(attacker.Emotion, defender.Emotion);
        if (!attackerAdvantage.HasValue)
        {
            return 0;
        }

        int advantageMagnitude = Math.Abs(GetEmotionTier(attacker) - GetEmotionTier(defender)) + 1;
        return attackerAdvantage.Value ? advantageMagnitude : -advantageMagnitude;
    }

    private static bool? CheckForAdvantage(EmotionType attacker, EmotionType defender)
    {
        return attacker switch
        {
            EmotionType.Sad when defender == EmotionType.Happy => true,
            EmotionType.Sad when defender == EmotionType.Angry => false,
            EmotionType.Angry when defender == EmotionType.Sad => true,
            EmotionType.Angry when defender == EmotionType.Happy => false,
            EmotionType.Happy when defender == EmotionType.Angry => true,
            EmotionType.Happy when defender == EmotionType.Sad => false,
            _ => null
        };
    }

    private sealed class EmptyEmotionRegistry : IEmotionRegistry
    {
        internal static EmptyEmotionRegistry Instance { get; } = new();

        public int? GetEmotionBuffType(
            EmotionType emotion,
            int emotionLevel,
            EmotionBuffVariant variant = EmotionBuffVariant.Standard) => null;

        public int? GetEmotionBuffType(
            Type familyType,
            int emotionLevel,
            EmotionBuffVariant variant = EmotionBuffVariant.Standard) => null;

        public EmotionType? GetEmotion(int buffType) => null;

        public int? GetEmotionTier(int buffType) => null;

        public int? GetMaxEmotionTier(EmotionType emotion) => null;

        public bool IsFinalEmotionTier(int buffType) => false;

        public EmotionBuffVariant? GetEmotionVariant(int buffType) => null;
    }
}
