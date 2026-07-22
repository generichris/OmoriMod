using System;

using Microsoft.Xna.Framework;

using OmoriMod.Content.NPCs.Global;
using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem;

using Terraria;

namespace OmoriMod.Content.Buffs.Abstract;

/// <summary>
/// Implements the Fear emotion family: increased movement speed and life regen, at the cost of
/// outgoing damage and accuracy.
/// </summary>
/// <remarks>
/// Concrete Fear buffs only declare their tier and visual frequency; all shared stat scaling,
/// combat hooks, incompatibility cleanup, and tooltip text are handled here.
/// </remarks>
public abstract class FearEmotionBase : EmotionBuff
{
    public static float GetPlayerMovementSpeedIncreasePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Fear.PlayerStats.MovementSpeedIncrease,
        EmotionStatTuning.PlayerMaxEmotionLevel);

    public static float GetNpcMovementSpeedIncreasePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Fear.NpcStats.MovementSpeedIncrease,
        EmotionStatTuning.NpcMaxEmotionLevel);

    public static float GetPlayerMissChancePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Fear.PlayerStats.MissChance,
        EmotionStatTuning.PlayerMaxEmotionLevel);

    public static float GetNpcMissChancePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Fear.NpcStats.MissChance,
        EmotionStatTuning.NpcMaxEmotionLevel);

    public static float GetPlayerDamageDecreasePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Fear.PlayerStats.DamageDecrease,
        EmotionStatTuning.PlayerMaxEmotionLevel);

    public static float GetNpcDamageDecreasePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Fear.NpcStats.DamageDecrease,
        EmotionStatTuning.NpcMaxEmotionLevel);

    /// <summary>The player's life-regen increase, in raw lifeRegen units.</summary>
    public static float GetPlayerLifeRegenIncreaseAmount(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Fear.PlayerStats.LifeRegenIncrease,
        EmotionStatTuning.PlayerMaxEmotionLevel) * 100f;

    protected FearEmotionBase()
    {
        Emotion = EmotionType.Fear;
        _dustColor = Color.Gray;
    }

    public override void UpdateEmotionBuff(Player player, ref int buffIndex)
    {
        EmotionSystem.RemoveIncompatibleEmotions<FearEmotionBase>(player);
        int emotionLevel = player.GetModPlayer<EmotionPlayer>().EmotionLevel;
        ModifyPlayerMovement(player, emotionLevel);
        ApplyPlayerLifeRegen(player, emotionLevel);
    }

    public override void UpdateEmotionBuff(NPC npc, ref int buffIndex)
    {
        EmotionSystem.RemoveIncompatibleEmotions<FearEmotionBase>(npc);
        ModifyNpcMovement(npc, npc.GetGlobalNPC<EmotionNPC>().EmotionLevel);
    }

    public override void ModifyPlayerMovement(Player player, int emotionLevel)
    {
        player.moveSpeed *= 1 + GetPlayerMovementSpeedIncreasePercent(emotionLevel);
    }

    public override void ModifyNpcMovement(NPC npc, int emotionLevel)
    {
        float modifier = GetNpcMovementSpeedIncreasePercent(emotionLevel);
        Vector2 change;
        if (npc.noGravity) { change = npc.velocity * modifier; }
        else { change = new Vector2(npc.velocity.X * modifier, 0); }
        Vector2 newPos = npc.position + change;

        // If the new speed collides with something, don't add it
        if (!Collision.SolidCollision(newPos, npc.width, npc.height))
        {
            npc.position = newPos;
        }
    }

    private static void ApplyPlayerLifeRegen(Player player, int emotionLevel)
    {
        player.lifeRegen += (int)GetPlayerLifeRegenIncreaseAmount(emotionLevel);
    }

    public override void ModifyPlayerOutgoingDamage(int emotionLevel, ref NPC.HitModifiers modifiers)
    {
        modifiers.SourceDamage *= 1 - GetPlayerDamageDecreasePercent(emotionLevel);
    }

    public override void ModifyNpcOutgoingDamage(int emotionLevel, ref Player.HurtModifiers modifiers)
    {
        modifiers.SourceDamage *= 1 - GetNpcDamageDecreasePercent(emotionLevel);
    }

    public override void ModifyPlayerHitNpc(int emotionLevel, ref NPC.HitModifiers modifiers)
    {
        if (Main.rand.NextFloat() < GetPlayerMissChancePercent(emotionLevel))
        {
            modifiers.FinalDamage *= 0f;
        }
    }

    public override void ModifyPlayerHitPlayer(int emotionLevel, ref Player.HurtModifiers modifiers)
    {
        if (Main.rand.NextFloat() < GetPlayerMissChancePercent(emotionLevel))
        {
            modifiers.FinalDamage *= 0f;
        }
    }

    public override void ModifyNpcHitNpc(int emotionLevel, ref NPC.HitModifiers modifiers)
    {
        if (Main.rand.NextFloat() < GetNpcMissChancePercent(emotionLevel))
        {
            modifiers.FinalDamage *= 0f;
        }
    }

    /// <summary>Allows a concrete Fear tier to append or replace tier-specific tooltip content.</summary>
    public virtual void FearModifyBuffText(ref string buffName, ref string tip, ref int rare) { }
    public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
    {
        int emotionLevel = GetTooltipEmotionLevel();
        int speedUp = (int)MathF.Round(GetPlayerMovementSpeedIncreasePercent(emotionLevel) * 100);
        int miss = (int)MathF.Round(GetPlayerMissChancePercent(emotionLevel) * 100);
        int damageDown = (int)MathF.Round(GetPlayerDamageDecreasePercent(emotionLevel) * 100);
        string buffTip = $"Speed up by {speedUp}%!" +
            $" Hit chance down by {miss}%!" +
            $" Damage down by {damageDown}%!" +
            $" Regenerating life!";
        tip = buffTip;
        FearModifyBuffText(ref buffName, ref tip, ref rare);
        FinalTierModifyBuffText(emotionLevel, ref buffName, ref tip, ref rare);
    }
}
