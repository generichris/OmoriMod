using System.Collections.Generic;
using System.Linq;

using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Buffs.AngryBuff;
using OmoriMod.Content.Buffs.HappyBuff;
using OmoriMod.Content.Buffs.SadBuff;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Systems.EmotionSystem;

public class EmotionSystem : ModSystem
{
    public const int EMOTION_TIME_IN_SECONDS = 60;
    public const float EMOTIONAL_ADVANTAGE_VALUE_PER_LEVEL = 0.07f;
    public const int PLAYER_MAX_EMOTION_LEVEL = 43;
    public const int NPC_MAX_EMOTION_LEVEL = 1;

    public static readonly HashSet<int> TIER3_EMOTION_TYPES = [
        ModContent.BuffType<Furious>(),
        ModContent.BuffType<Manic>(),
        ModContent.BuffType<Miserable>(),
    ];

    public static readonly HashSet<int> TIER4_EMOTION_TYPES = [
        ModContent.BuffType<Livid>(),
        ModContent.BuffType<Hysterical>(),
        ModContent.BuffType<Despondent>(),
    ];

    public override void Load()
    {
        // Any initialization if needed
    }

    public override void Unload()
    {
        // Any cleanup if needed
    }

    /// <summary>
    /// returns the type of the <see cref="EmotionBuff"/> currently on the <see cref="Entity"/>. If no buff exists, returns null.
    /// </summary>
    public static int? GetEmotionType(Entity entity)
    {
        if (entity is NPC npc)
        {
            foreach (int buffID in npc.buffType)
            {
                if (ModContent.GetModBuff(buffID) is EmotionBuff currentBuff)
                {
                    return currentBuff.Type;
                }
            }
        }
        if (entity is Player player)
        {
            foreach (int buffID in player.buffType)
            {
                if (ModContent.GetModBuff(buffID) is EmotionBuff currentBuff)
                {
                    return currentBuff.Type;
                }
            }
        }
        return null;
    }

    public static int GetEmotionLevel(IEmotionEntity entity)
    {
        if (entity.ActiveEmotionBuff != null)
        {
            return entity.ActiveEmotionBuff.emotionLevel;
        }
        return 0;
    }

    /// <summary>
    /// Returns the emotional advantage level of the attacker and the target.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    /// <returns><c>0</c> means no advantage. 
    /// Any <c>positive value</c> means the attacker has advantage. 
    /// Any <c>negative value</c> means the defender has advantage.</returns>
    public static int CalculateAdvantage(IEmotionEntity attacker, IEmotionEntity defender)
    {
        bool? attackerAdvantage = attacker.CheckForAdvantage(defender);
        if (attackerAdvantage == null) { return 0; }
        if (attackerAdvantage == true)
        {
            return GetEmotionLevel(attacker) - GetEmotionLevel(defender) + 1;
        }
        else
        {
            return GetEmotionLevel(defender) - GetEmotionLevel(attacker) + 1;
        }
    }

    private static void RemoveEmotion(Entity entity, int emotionType)
    {
        if (entity is NPC npc)
        {
            if (Main.dedServ || Main.netMode == NetmodeID.SinglePlayer)
            {
                npc.DelBuff(npc.FindBuffIndex(emotionType));
            }
            else
            {
                npc.RequestBuffRemoval(emotionType);
            }
        }
        if (entity is Player player)
        {
            player.ClearBuff(emotionType);
        }
    }

    public static void ClearAllEmotions(Entity entity)
    {
        if (entity is NPC npc)
        {
            foreach (int buffID in npc.buffType)
            {
                if (ModContent.GetModBuff(buffID) is EmotionBuff)
                {
                    RemoveEmotion(entity, buffID);
                }
            }
        }
        if (entity is Player player)
        {
            foreach (int buffID in player.buffType)
            {
                if (ModContent.GetModBuff(buffID) is EmotionBuff)
                {
                    RemoveEmotion(entity, buffID);
                }
            }
        }
    }

    /// <summary>
    /// Removes any emotions that are incompatible with the provided emotion type T.
    /// </summary>
    public static void RemoveIncompatibleEmotions<T>(Entity entity) where T : EmotionBuff
    {
        T buffInstance = ModContent.GetInstance<T>();
        if (buffInstance == null) return;

        if (entity is NPC npc)
        {
            // To avoid modifying collection while iterating, collect removals first
            List<int> buffsToRemove = [];
            foreach (int buffID in npc.buffType)
            {
                ModBuff modBuff = ModContent.GetModBuff(buffID);
                if (modBuff is EmotionBuff currentBuff && buffInstance.IsIncompatibleWith(currentBuff))
                {
                    buffsToRemove.Add(buffID);
                }
            }
            foreach (int id in buffsToRemove) RemoveEmotion(entity, id);
        }
        if (entity is Player player)
        {
            List<int> buffsToRemove = [];
            foreach (int buffID in player.buffType)
            {
                ModBuff modBuff = ModContent.GetModBuff(buffID);
                if (modBuff is EmotionBuff currentBuff && buffInstance.IsIncompatibleWith(currentBuff))
                {
                    buffsToRemove.Add(buffID);
                }
            }
            foreach (int id in buffsToRemove) RemoveEmotion(entity, id);
        }
    }

    /// <summary>
    /// Returns true if the provided emmotion can be applied.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="player"></param>
    /// <returns></returns>
    public static bool CanApplyEmotion<T>(Player player) where T : EmotionBuff
    {
        T buffInstance = ModContent.GetInstance<T>();
        if (buffInstance == null) return false;

        foreach (int buffID in player.buffType)
        {
            ModBuff modBuff = ModContent.GetModBuff(buffID);
            // Check if current buff is incompatible with T
            if (modBuff is EmotionBuff currentBuff && buffInstance.IsIncompatibleWith(currentBuff))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Applies the buff provided or promotes a pre-existing emotion
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="player"></param>
    /// <param name="baseBuffType"></param>
    /// <param name="duration"></param>
    public static void ApplyOrPromoteBuff<T>(Player player, int baseBuffType, int duration) where T : EmotionBuff
    {
        RemoveIncompatibleEmotions<T>(player);

        // Check which emotion buff the player currently has
        foreach (int buffID in player.buffType)
        {
            if (ModContent.GetModBuff(buffID) is T currentBuff)
            {
                // Try to promote emotion
                int? nextStage = currentBuff.NextTierEmotion;
                if (nextStage.HasValue)
                {
                    player.ClearBuff(buffID);
                    player.AddBuff(nextStage.Value, duration);
                }
                else
                {
                    // reapply max lvl emotion
                    player.AddBuff(currentBuff.Type, duration);
                }
                return; // Handled, don't apply base buff
            }
        }

        // If no same-type buff was found, apply base
        player.AddBuff(baseBuffType, duration);
    }

    /// <summary>
    /// Special method to apply the tier 4 version of emotion buffs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="player"></param>
    /// <param name="baseBuffType"></param>
    /// <param name="duration"></param>
    public static void ApplyTier4Emotion<T>(Player player, int baseBuffType, int duration) where T : EmotionBuff
    {
        RemoveIncompatibleEmotions<T>(player);

        // Find any and tier 3 or lower buffs and remove them
        foreach (int buffID in player.buffType)
        {
            if (ModContent.GetModBuff(buffID) is T currentBuff)
            {
                if (currentBuff.emotionLevel <= 3)
                {
                    player.ClearBuff(buffID);
                }
            }
        }

        player.AddBuff(baseBuffType, duration);
    }
}