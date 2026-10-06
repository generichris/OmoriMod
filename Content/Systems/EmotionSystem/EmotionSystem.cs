using System;
using System.Collections.Generic;

using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.NPCs.Global;
using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem.Interfaces;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Systems.EmotionSystem;

/// <summary>
/// Provides the gameplay-facing API for querying, applying, promoting, removing, and resolving emotions.
/// </summary>
/// <remarks>
/// Registration details are delegated to <see cref="EmotionRegistry"/>. Runtime state lives on
/// <see cref="EmotionPlayer"/> and <see cref="EmotionNPC"/>, while <see cref="EmotionBuff"/>
/// subclasses implement emotion-specific stat and combat effects.
/// </remarks>
public static class EmotionSystem
{
    private static EmotionService s_service = EmotionService.Empty;

    internal static void InitializeRegistry(IEmotionRegistry registry)
    {
        s_service = new EmotionService(registry);
    }

    internal static void ResetRegistry()
    {
        s_service = EmotionService.Empty;
    }

    /// <summary>Gets the registered buff type for an emotion, tier, and duration variant.</summary>
    /// <returns>The buff type, or <see langword="null"/> if no matching registration exists.</returns>
    public static int? GetEmotionBuffType(
        EmotionType emotion,
        int emotionLevel,
        EmotionBuffVariant variant = EmotionBuffVariant.Standard)
    {
        return s_service.GetEmotionBuffType(emotion, emotionLevel, variant);
    }

    /// <summary>Gets the registered buff type in a buff family for a tier and duration variant.</summary>
    /// <typeparam name="T">The concrete or base emotion-buff family to search.</typeparam>
    /// <returns>The buff type, or <see langword="null"/> if no matching registration exists.</returns>
    public static int? GetEmotionBuffType<T>(
        int emotionLevel,
        EmotionBuffVariant variant = EmotionBuffVariant.Standard)
        where T : EmotionBuff
    {
        return s_service.GetEmotionBuffType<T>(emotionLevel, variant);
    }

    /// <summary>Gets the next registered standard buff tier for an emotion.</summary>
    /// <returns>The next buff type, or <see langword="null"/> if it does not exist.</returns>
    public static int? GetNextTierEmotionType(EmotionType currentEmotionType, int currentEmotionLevel)
    {
        return s_service.GetNextTierEmotionType(currentEmotionType, currentEmotionLevel);
    }

    /// <summary>Gets the next registered standard tier in an emotion buff's family.</summary>
    /// <returns>The next buff type, or <see langword="null"/> at the final tier or when unregistered.</returns>
    public static int? GetNextTierEmotionType<T>(T currentEmotion) where T : EmotionBuff
    {
        return s_service.GetNextTierEmotionType(currentEmotion);
    }

    /// <summary>Gets the registered tier of an emotion buff type.</summary>
    /// <returns>The declared tier, or <see langword="null"/> for an unregistered buff type.</returns>
    public static int? GetEmotionTier(int buffType)
    {
        return s_service.GetEmotionTier(buffType);
    }

    /// <summary>Gets the highest registered standard tier for an emotion.</summary>
    /// <returns>The final tier, or <see langword="null"/> if the emotion has no standard buffs.</returns>
    public static int? GetMaxEmotionTier(EmotionType emotion)
    {
        return s_service.GetMaxEmotionTier(emotion);
    }

    /// <summary>Determines whether a buff type is the final registered standard tier of its emotion.</summary>
    public static bool IsFinalEmotionTier(int buffType)
    {
        return s_service.IsFinalEmotionTier(buffType);
    }

    /// <summary>Gets the registered duration variant of an emotion buff type.</summary>
    /// <returns>The variant, or <see langword="null"/> for an unregistered buff type.</returns>
    public static EmotionBuffVariant? GetEmotionVariant(int buffType)
    {
        return s_service.GetEmotionVariant(buffType);
    }

    internal static bool IsValidScalingSync(
        EmotionType incomingEmotion,
        int incomingLevel,
        EmotionType currentEmotion,
        int currentLevel,
        int? activeBuffType,
        EmotionScalingMode activeScalingMode,
        bool requireActiveBuff)
    {
        return s_service.IsValidScalingSync(
            incomingEmotion,
            incomingLevel,
            currentEmotion,
            currentLevel,
            activeBuffType,
            activeScalingMode,
            requireActiveBuff);
    }

    /// <summary>
    /// Gets the fixed-size buff-type array owned by a supported entity.
    /// </summary>
    /// <param name="entity">The player or NPC whose buffs are requested.</param>
    /// <returns>The entity's buff-type array, or an empty array for unsupported entity types.</returns>
    private static int[] GetBuffListOfEntity(Entity entity)
    {
        return entity switch
        {
            NPC npc => npc.buffType,
            Player player => player.buffType,
            _ => []
        };
    }

    /// <summary>
    /// Gets the tModLoader buff type of the first active <see cref="EmotionBuff"/> on an entity.
    /// </summary>
    /// <param name="entity">The player or NPC to inspect.</param>
    /// <returns>The active emotion buff type, or <see langword="null"/> when none is present.</returns>
    public static int? GetEmotionType(Entity entity)
    {
        return s_service.ResolvePreferredEmotionBuffType(GetBuffListOfEntity(entity));
    }

    /// <summary>
    /// Determines whether a buff is the registered emotion that should supply an entity's
    /// resolved state and effects for the current tick.
    /// </summary>
    internal static bool IsPreferredEmotionBuff(Entity entity, int buffType)
    {
        int? preferredBuffType = GetEmotionType(entity);
        return !preferredBuffType.HasValue || preferredBuffType.Value == buffType;
    }

    /// <summary>Gets the stat-scaling level currently resolved for an emotion-aware entity.</summary>
    public static int GetEmotionLevel(IEmotionEntity entity)
    {
        return entity.EmotionLevel;
    }

    /// <summary>
    /// Gets the registered tier of an entity's active emotion buff.
    /// </summary>
    public static int GetEmotionTier(IEmotionEntity entity)
    {
        return s_service.GetEmotionTier(entity);
    }

    /// <summary>
    /// Calculates the signed strength of emotional advantage between an attacker and defender.
    /// </summary>
    /// <param name="attacker">The entity initiating the hit.</param>
    /// <param name="defender">The entity receiving the hit.</param>
    /// <returns>
    /// Zero when neither side has advantage; a positive value when the attacker has advantage;
    /// otherwise, a negative value when the defender has advantage. Magnitude is the absolute
    /// tier difference plus one.
    /// </returns>
    public static int CalculateAdvantage(IEmotionEntity attacker, IEmotionEntity defender)
    {
        return s_service.CalculateAdvantage(attacker, defender);
    }

    private static void ApplyAdvantage(int advantage, ref NPC.HitModifiers modifiers)
    {
        modifiers.SourceDamage += EmotionStatTuning.EmotionalAdvantageValuePerLevel * advantage;
    }

    private static void ApplyAdvantage(int advantage, ref Player.HurtModifiers modifiers)
    {
        modifiers.SourceDamage += EmotionStatTuning.EmotionalAdvantageValuePerLevel * advantage;
    }

    /// <summary>
    /// Applies emotional advantage and the attacker's active emotion effects to an NPC hit.
    /// </summary>
    public static void ApplyCombatModifiers(
        IEmotionEntity attacker,
        IEmotionEntity defender,
        ref NPC.HitModifiers modifiers)
    {
        ApplyAdvantage(CalculateAdvantage(attacker, defender), ref modifiers);

        if (attacker is EmotionPlayer)
        {
            attacker.ActiveEmotionBuff?.ModifyPlayerOutgoingDamage(attacker.EmotionLevel, ref modifiers);
            attacker.ActiveEmotionBuff?.ModifyPlayerHitNpc(attacker.EmotionLevel, ref modifiers);
            return;
        }

        attacker.ActiveEmotionBuff?.ModifyNpcHitNpc(attacker.EmotionLevel, ref modifiers);
    }

    /// <summary>
    /// Applies emotional advantage and the attacker's active emotion effects to a player hit.
    /// </summary>
    public static void ApplyCombatModifiers(
        IEmotionEntity attacker,
        IEmotionEntity defender,
        ref Player.HurtModifiers modifiers)
    {
        ApplyAdvantage(CalculateAdvantage(attacker, defender), ref modifiers);

        if (attacker is EmotionPlayer)
        {
            attacker.ActiveEmotionBuff?.ModifyPlayerOutgoingDamage(attacker.EmotionLevel, ref modifiers);
            attacker.ActiveEmotionBuff?.ModifyPlayerHitPlayer(attacker.EmotionLevel, ref modifiers);
            return;
        }

        attacker.ActiveEmotionBuff?.ModifyNpcOutgoingDamage(attacker.EmotionLevel, ref modifiers);
    }

    /// <summary>
    /// Dispatches post-hurt behavior to the player's active emotion buff.
    /// </summary>
    public static void HandlePlayerHurt(Player player, Player.HurtInfo hurtInfo)
    {
        EmotionPlayer emotionPlayer = player.GetModPlayer<EmotionPlayer>();
        emotionPlayer.ActiveEmotionBuff?.OnPlayerHurt(player, emotionPlayer.EmotionLevel, hurtInfo);
    }

    /// <summary>
    /// Removes a specific emotion buff using the correct player or NPC networking path.
    /// </summary>
    /// <param name="entity">The player or NPC that owns the buff.</param>
    /// <param name="emotionType">The tModLoader buff type to remove.</param>
    private static void RemoveEmotion(Entity entity, int emotionType)
    {
        switch (entity)
        {
            case NPC npc when Main.dedServ || Main.netMode == NetmodeID.SinglePlayer:
                npc.DelBuff(npc.FindBuffIndex(emotionType));
                break;
            case NPC npc:
                npc.RequestBuffRemoval(emotionType);
                break;
            case Player player:
                player.ClearBuff(emotionType);
                break;
        }
    }


    /// <summary>
    /// Removes every active <see cref="EmotionBuff"/> from a player or NPC.
    /// </summary>
    /// <param name="entity">The player or NPC whose emotions should be cleared.</param>
    public static void ClearAllEmotions(Entity entity)
    {
        IReadOnlyList<int> buffsToRemove =
            s_service.GetRegisteredEmotionBuffTypes(GetBuffListOfEntity(entity));
        foreach (int buffId in buffsToRemove)
        {
            RemoveEmotion(entity, buffId);
        }
    }

    /// <summary>
    /// Removes active emotions that are incompatible with the specified emotion-buff family.
    /// </summary>
    public static void RemoveIncompatibleEmotions<T>(Entity entity) where T : EmotionBuff
    {
        int? representativeBuffType = GetEmotionBuffType<T>(1);
        if (!representativeBuffType.HasValue
            || ModContent.GetModBuff(representativeBuffType.Value) is not EmotionBuff buffInstance)
        {
            return;
        }

        RemoveIncompatibleEmotions(entity, buffInstance);
    }

    /// <summary>
    /// Removes active emotions that the given emotion buff cannot coexist with. Allocates only
    /// when something actually needs removing.
    /// </summary>
    internal static void RemoveIncompatibleEmotions(Entity entity, EmotionBuff emotion)
    {
        int[] buffs = GetBuffListOfEntity(entity);

        // Removal shifts the buff array, so collect first and remove afterwards.
        List<int> buffsToRemove = null;
        for (int i = 0; i < buffs.Length; i++)
        {
            int buffId = buffs[i];
            if (buffId <= 0)
            {
                continue;
            }

            if (ModContent.GetModBuff(buffId) is EmotionBuff currentBuff && emotion.IsIncompatibleWith(currentBuff))
            {
                buffsToRemove ??= new List<int>();
                buffsToRemove.Add(buffId);
            }
        }

        if (buffsToRemove == null)
        {
            return;
        }

        foreach (int id in buffsToRemove) RemoveEmotion(entity, id);
    }

    /// <summary>
    /// Determines whether the specified emotion-buff family is compatible with an entity's active emotions.
    /// </summary>
    /// <typeparam name="T">The concrete or base emotion-buff family to test.</typeparam>
    /// <param name="entity">The player or NPC to inspect.</param>
    /// <returns><see langword="true"/> when the family is registered and no active emotion rejects it.</returns>
    public static bool CanApplyEmotion<T>(Entity entity) where T : EmotionBuff
    {
        int? representativeBuffType = GetEmotionBuffType<T>(1);
        if (!representativeBuffType.HasValue
            || ModContent.GetModBuff(representativeBuffType.Value) is not EmotionBuff buffInstance)
        {
            return false;
        }

        return CanApplyEmotion(entity, buffInstance);
    }

    private static bool CanApplyEmotion(Entity entity, EmotionBuff emotion)
    {
        int[] buffs = GetBuffListOfEntity(entity);
        foreach (int buffId in buffs)
        {
            ModBuff modBuff = ModContent.GetModBuff(buffId);
            // Check if current buff is incompatible with T
            if (modBuff is EmotionBuff currentBuff && emotion.IsIncompatibleWith(currentBuff))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Applies the standard tier-one buff for an emotion to an eligible NPC.
    /// </summary>
    public static bool ApplyEmotion(NPC target, EmotionType emotion, int duration = 600)
    {
        if (emotion == EmotionType.None
            || target.GetGlobalNPC<EmotionNPC>().ImmuneToEmotionChange)
        {
            return false;
        }

        int? buffType = GetEmotionBuffType(emotion, 1);
        if (!buffType.HasValue
            || ModContent.GetModBuff(buffType.Value) is not EmotionBuff emotionBuff
            || !CanApplyEmotion(target, emotionBuff))
        {
            return false;
        }

        target.AddBuff(buffType.Value, duration);
        return true;
    }

    /// <summary>
    /// Determines whether a standard emotion can be applied, refreshed, or promoted for a player.
    /// </summary>
    public static bool CanApplyOrPromoteEmotion<T>(Player player) where T : EmotionBuff
    {
        if (!CanApplyEmotion<T>(player))
        {
            return false;
        }

        T currentEmotion = GetCurrentEmotion<T>(player);
        return GetApplicationDecision(
            player,
            EmotionApplicationRequest.RegularItem,
            typeof(T),
            currentEmotion).CanApply;
    }

    private static T GetCurrentEmotion<T>(Player player) where T : EmotionBuff
    {
        List<int> candidateBuffTypes = [];
        foreach (int buffId in player.buffType)
        {
            if (ModContent.GetModBuff(buffId) is T)
            {
                candidateBuffTypes.Add(buffId);
            }
        }

        int? preferredBuffType = s_service.GetPreferredEmotionBuffType(candidateBuffTypes);
        return preferredBuffType.HasValue
            ? ModContent.GetModBuff(preferredBuffType.Value) as T
            : null;
    }

    private static EmotionApplicationDecision GetApplicationDecision(
        Player player,
        EmotionApplicationRequest request,
        Type emotionFamilyType,
        EmotionBuff currentEmotion)
    {
        int scalingLevel = 0;
        EmotionScalingMode scalingMode = EmotionScalingMode.Capped;
        if (currentEmotion != null)
        {
            EmotionPlayer emotionPlayer = player.GetModPlayer<EmotionPlayer>();
            int registeredTier = GetEmotionTier(currentEmotion.Type) ?? currentEmotion.EmotionTier;
            scalingLevel = emotionPlayer.ScalingEmotion == currentEmotion.Emotion
                ? emotionPlayer.ScalingEmotionLevel
                : registeredTier;
            scalingMode = currentEmotion.ScalingMode;
        }

        return s_service.GetApplicationDecision(
            request,
            emotionFamilyType,
            currentEmotion?.Type,
            scalingMode,
            scalingLevel);
    }

    private static bool ExecuteApplicationDecision(
        Player player,
        EmotionBuff currentEmotion,
        EmotionApplicationDecision decision,
        int duration)
    {
        if (!decision.CanApply || !decision.BuffType.HasValue)
        {
            return false;
        }

        switch (decision.Action)
        {
            case EmotionApplicationAction.ApplyTierOne:
            case EmotionApplicationAction.RefreshCurrent:
                player.AddBuff(decision.BuffType.Value, duration);
                return true;
            case EmotionApplicationAction.PromoteNextTier when currentEmotion != null:
                player.ClearBuff(currentEmotion.Type);
                player.AddBuff(decision.BuffType.Value, duration);
                return true;
            case EmotionApplicationAction.AmplifyCurrent when currentEmotion != null:
                int? finalTier = GetMaxEmotionTier(currentEmotion.Emotion);
                if (!finalTier.HasValue)
                {
                    return false;
                }

                player.GetModPlayer<EmotionPlayer>().TryAmplifyEmotion(
                    currentEmotion.Emotion,
                    finalTier.Value);
                player.AddBuff(decision.BuffType.Value, duration);
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Determines whether the player's current standard emotion can be promoted to or refreshed at its final tier.
    /// </summary>
    public static bool CanApplyFinalTierEmotion(Player player)
    {
        int? buffType = GetEmotionType(player);
        if (!buffType.HasValue
            || ModContent.GetModBuff(buffType.Value) is not EmotionBuff emotionBuff)
        {
            return false;
        }

        EmotionApplicationDecision decision = GetApplicationDecision(
            player,
            EmotionApplicationRequest.Amplifier,
            emotionBuff.GetType(),
            emotionBuff);
        return decision.CanApply && CanApplyEmotion(player, emotionBuff);
    }

    /// <summary>
    /// Promotes the player's current standard emotion to its final tier, or refreshes it when already final.
    /// </summary>
    public static bool ApplyFinalTierEmotion(Player player, int duration)
    {
        int? buffType = GetEmotionType(player);
        if (!buffType.HasValue
            || ModContent.GetModBuff(buffType.Value) is not EmotionBuff currentEmotion
            || !CanApplyEmotion(player, currentEmotion))
        {
            return false;
        }

        EmotionApplicationDecision decision = GetApplicationDecision(
            player,
            EmotionApplicationRequest.Amplifier,
            currentEmotion.GetType(),
            currentEmotion);
        if (!decision.CanApply)
        {
            return false;
        }

        RemoveIncompatibleEmotions(player, currentEmotion);
        return ExecuteApplicationDecision(player, currentEmotion, decision, duration);
    }

    /// <summary>
    /// Applies tier one of an emotion family, promotes an existing standard tier, or refreshes the current tier.
    /// </summary>
    /// <typeparam name="T">The concrete or base emotion-buff family to apply.</typeparam>
    /// <param name="player">The player whose emotion should be changed.</param>
    /// <param name="duration">The applied or refreshed buff duration in ticks.</param>
    /// <param name="canPromoteToFinalTier">
    /// Whether this operation may cross into or reapply a capped final tier. Passing
    /// <see langword="true"/> grants amplifier-equivalent behavior.
    /// </param>
    /// <returns><see langword="true"/> when an emotion was applied, promoted, or refreshed.</returns>
    public static bool ApplyOrPromoteEmotion<T>(Player player, int duration, bool canPromoteToFinalTier = false) where T : EmotionBuff
    {
        T currentEmotion = GetCurrentEmotion<T>(player);
        EmotionApplicationRequest request = canPromoteToFinalTier
            ? EmotionApplicationRequest.Amplifier
            : EmotionApplicationRequest.RegularItem;
        EmotionApplicationDecision decision = GetApplicationDecision(
            player,
            request,
            typeof(T),
            currentEmotion);
        if (!decision.CanApply)
        {
            return false;
        }

        RemoveIncompatibleEmotions<T>(player);
        return ExecuteApplicationDecision(player, currentEmotion, decision, duration);
    }
}
