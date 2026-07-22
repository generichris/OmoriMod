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
    /// <summary>Gets the registered buff type for an emotion, tier, and duration variant.</summary>
    /// <returns>The buff type, or <see langword="null"/> if no matching registration exists.</returns>
    public static int? GetEmotionBuffType(
        EmotionType emotion,
        int emotionLevel,
        EmotionBuffVariant variant = EmotionBuffVariant.Standard)
    {
        return EmotionRegistry.GetEmotionBuffType(emotion, emotionLevel, variant);
    }

    /// <summary>Gets the registered buff type in a buff family for a tier and duration variant.</summary>
    /// <typeparam name="T">The concrete or base emotion-buff family to search.</typeparam>
    /// <returns>The buff type, or <see langword="null"/> if no matching registration exists.</returns>
    public static int? GetEmotionBuffType<T>(
        int emotionLevel,
        EmotionBuffVariant variant = EmotionBuffVariant.Standard)
        where T : EmotionBuff
    {
        return EmotionRegistry.GetEmotionBuffType<T>(emotionLevel, variant);
    }

    /// <summary>Gets the next registered standard buff tier for an emotion.</summary>
    /// <returns>The next buff type, or <see langword="null"/> if it does not exist.</returns>
    public static int? GetNextTierEmotionType(EmotionType currentEmotionType, int currentEmotionLevel)
    {
        return EmotionRegistry.GetNextTierEmotionType(currentEmotionType, currentEmotionLevel);
    }

    /// <summary>Gets the next registered standard tier in an emotion buff's family.</summary>
    /// <returns>The next buff type, or <see langword="null"/> at the final tier or when unregistered.</returns>
    public static int? GetNextTierEmotionType<T>(T currentEmotion) where T : EmotionBuff
    {
        return EmotionRegistry.GetNextTierEmotionType(currentEmotion);
    }

    /// <summary>Gets the registered tier of an emotion buff type.</summary>
    /// <returns>The declared tier, or <see langword="null"/> for an unregistered buff type.</returns>
    public static int? GetEmotionTier(int buffType)
    {
        return EmotionRegistry.GetEmotionTier(buffType);
    }

    /// <summary>Gets the highest registered standard tier for an emotion.</summary>
    /// <returns>The final tier, or <see langword="null"/> if the emotion has no standard buffs.</returns>
    public static int? GetMaxEmotionTier(EmotionType emotion)
    {
        return EmotionRegistry.GetMaxEmotionTier(emotion);
    }

    /// <summary>Determines whether a buff type is the final registered standard tier of its emotion.</summary>
    public static bool IsFinalEmotionTier(int buffType)
    {
        return EmotionRegistry.IsFinalEmotionTier(buffType);
    }

    /// <summary>Gets the registered duration variant of an emotion buff type.</summary>
    /// <returns>The variant, or <see langword="null"/> for an unregistered buff type.</returns>
    public static EmotionBuffVariant? GetEmotionVariant(int buffType)
    {
        return EmotionRegistry.GetEmotionVariant(buffType);
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
        int[] buffs = GetBuffListOfEntity(entity);
        foreach (int buffId in buffs)
        {
            if (ModContent.GetModBuff(buffId) is EmotionBuff currentBuff)
            {
                return currentBuff.Type;
            }
        }
        return null;
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
        EmotionBuff activeEmotion = entity.ActiveEmotionBuff;
        return activeEmotion == null
            ? 0
            : GetEmotionTier(activeEmotion.Type) ?? activeEmotion.EmotionTier;
    }

    /// <summary>
    /// Determines which side wins the Angry-Happy-Sad advantage triangle.
    /// </summary>
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
        bool? attackerAdvantage = CheckForAdvantage(attacker.Emotion, defender.Emotion);

        if (!attackerAdvantage.HasValue)
        {
            return 0;
        }

        // The emotion triangle always determines who wins. Tier distance only
        // determines the strength of that win, never reverses its direction.
        int advantageMagnitude = Math.Abs(GetEmotionTier(attacker) - GetEmotionTier(defender)) + 1;
        return attackerAdvantage.Value ? advantageMagnitude : -advantageMagnitude;
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
        int[] buffs = GetBuffListOfEntity(entity);
        foreach (int buffId in buffs)
        {
            if (ModContent.GetModBuff(buffId) is EmotionBuff)
            {
                RemoveEmotion(entity, buffId);
            }
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

    private static void RemoveIncompatibleEmotions(Entity entity, EmotionBuff emotion)
    {
        int[] buffs = GetBuffListOfEntity(entity);

        List<int> buffsToRemove = [];
        foreach (int buffId in buffs)
        {
            ModBuff modBuff = ModContent.GetModBuff(buffId);
            if (modBuff is EmotionBuff currentBuff && emotion.IsIncompatibleWith(currentBuff))
            {
                buffsToRemove.Add(buffId);
            }
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

        foreach (int buffId in player.buffType)
        {
            if (ModContent.GetModBuff(buffId) is T currentEmotion)
            {
                return GetEmotionVariant(buffId) == EmotionBuffVariant.Standard
                    && !IsCappedFinalTierEmotion(currentEmotion);
            }
        }

        return GetEmotionBuffType<T>(1).HasValue;
    }

    /// <summary>
    /// Determines whether a buff is the final standard tier of a family that supports capped scaling.
    /// </summary>
    private static bool IsCappedFinalTierEmotion(EmotionBuff emotionBuff)
    {
        return emotionBuff.ScalingMode == EmotionScalingMode.Capped
            && IsFinalEmotionTier(emotionBuff.Type);
    }


    /// <summary>
    /// Determines whether a registered buff belongs to a promotable standard emotion progression.
    /// </summary>
    /// <param name="buffType">The tModLoader buff type being checked.</param>
    /// <param name="currentTier">The buff's registered tier.</param>
    /// <param name="maxTier">The highest registered standard tier for its emotion.</param>
    private static bool IsPromotableEmotion(int buffType, int? currentTier, int? maxTier)
    {
        return currentTier.HasValue
            && maxTier.HasValue
            && GetEmotionVariant(buffType) == EmotionBuffVariant.Standard;
    }

    private static bool PromoteEmotion(EmotionBuff currentEmotion, Player player, int duration, bool canPromoteToFinalTier)
    {
        int? currentTier = GetEmotionTier(currentEmotion.Type);
        int? maxTier = GetMaxEmotionTier(currentEmotion.Emotion);

        if (currentTier == null || maxTier == null) { return false; }
        if (!IsPromotableEmotion(currentEmotion.Type, currentTier, maxTier)) { return false; }

        if (currentTier.Value == maxTier.Value)
        {
            if (IsCappedFinalTierEmotion(currentEmotion) && !canPromoteToFinalTier)
            {
                return false;
            }

            player.AddBuff(currentEmotion.Type, duration);
            return true;
        }

        bool isTierBeforeFinal = currentTier.Value == maxTier.Value - 1;
        if (isTierBeforeFinal && !canPromoteToFinalTier)
        {
            player.AddBuff(currentEmotion.Type, duration);
            return true;
        }

        int? nextEmotionType = GetNextTierEmotionType(currentEmotion);
        if (!nextEmotionType.HasValue) { return false; }

        player.ClearBuff(currentEmotion.Type);
        player.AddBuff(nextEmotionType.Value, duration);
        return true;

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

        int? currentTier = GetEmotionTier(buffType.Value);
        int? maxTier = GetMaxEmotionTier(emotionBuff.Emotion);
        return GetEmotionVariant(buffType.Value) == EmotionBuffVariant.Standard
            && currentTier.HasValue
            && maxTier.HasValue
            && currentTier.Value >= maxTier.Value - 1
            && CanApplyEmotion(player, emotionBuff);
    }

    /// <summary>
    /// Promotes the player's current standard emotion to its final tier, or refreshes it when already final.
    /// </summary>
    public static bool ApplyFinalTierEmotion(Player player, int duration)
    {
        if (!CanApplyFinalTierEmotion(player))
        {
            return false;
        }

        int buffType = GetEmotionType(player).Value;
        EmotionBuff currentEmotion = (EmotionBuff)ModContent.GetModBuff(buffType);
        RemoveIncompatibleEmotions(player, currentEmotion);
        return PromoteEmotion(currentEmotion, player, duration, canPromoteToFinalTier: true);
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
        if (!canPromoteToFinalTier)
        {
            foreach (int buffId in player.buffType)
            {
                if (ModContent.GetModBuff(buffId) is T currentEmotion
                    && IsCappedFinalTierEmotion(currentEmotion))
                {
                    return false;
                }
            }
        }

        // first, remove incompatible emotions
        RemoveIncompatibleEmotions<T>(player);

        // next, check if the player has a promotable emotion
        foreach (int buffId in player.buffType)
        {
            if (ModContent.GetModBuff(buffId) is T currentEmotion)
            {
                // Non-standard variants such as accessory emotions cannot be promoted.
                return GetEmotionVariant(buffId) == EmotionBuffVariant.Standard && PromoteEmotion(currentEmotion, player, duration, canPromoteToFinalTier);
            }
        }

        // promotable emotion not found, apply tier 1 version of emotion
        int? buffType = GetEmotionBuffType<T>(1);
        if (!buffType.HasValue) { return false; }

        player.AddBuff(buffType.Value, duration);
        return true;

    }
}
