using System;

using Microsoft.Xna.Framework;

using OmoriMod.Content.NPCs.Global;
using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem;

using Terraria;

namespace OmoriMod.Content.Buffs.Abstract;

/// <summary>
/// Implements the Happy emotion family: increased movement and critical-hit chance at the cost of accuracy.
/// </summary>
/// <remarks>
/// Concrete Happy buffs only declare their tier and visual frequency; all shared stat scaling,
/// combat hooks, incompatibility cleanup, and tooltip text are handled here.
/// </remarks>
public abstract class HappyEmotionBase : EmotionBuff
{
    public static float GetPlayerMovementSpeedIncreasePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Happy.PlayerStats.MovementSpeedIncrease,
        EmotionStatTuning.PlayerMaxEmotionLevel);

    public static float GetNpcMovementSpeedIncreasePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Happy.NpcStats.MovementSpeedIncrease,
        EmotionStatTuning.NpcMaxEmotionLevel);

    public static float GetPlayerExtraCritChancePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Happy.PlayerStats.ExtraCritChance,
        EmotionStatTuning.PlayerMaxEmotionLevel);

    public static float GetNpcExtraCritChancePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Happy.NpcStats.ExtraCritChance,
        EmotionStatTuning.NpcMaxEmotionLevel);

    public static float GetPlayerMissChancePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Happy.PlayerStats.MissChance,
        EmotionStatTuning.PlayerMaxEmotionLevel);

    public static float GetNpcMissChancePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Happy.NpcStats.MissChance,
        EmotionStatTuning.NpcMaxEmotionLevel);

    protected HappyEmotionBase()
    {
        Emotion = EmotionType.Happy;
        _dustColor = Color.Yellow;
    }

    public override void UpdateEmotionBuff(Player player, ref int buffIndex)
    {
        EmotionSystem.RemoveIncompatibleEmotions<HappyEmotionBase>(player);
        ModifyPlayerMovement(player, player.GetModPlayer<EmotionPlayer>().EmotionLevel);
    }

    public override void UpdateEmotionBuff(NPC npc, ref int buffIndex)
    {
        EmotionSystem.RemoveIncompatibleEmotions<HappyEmotionBase>(npc);
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

    public override void ModifyPlayerHitNpc(int emotionLevel, ref NPC.HitModifiers modifiers)
    {
        if (Main.rand.NextFloat() < GetPlayerMissChancePercent(emotionLevel))
        {
            modifiers.FinalDamage *= 0f;
            return;
        }

        if (Main.rand.NextFloat() < GetPlayerExtraCritChancePercent(emotionLevel))
        {
            modifiers.SetCrit();
        }
    }

    public override void ModifyPlayerHitPlayer(int emotionLevel, ref Player.HurtModifiers modifiers)
    {
        if (Main.rand.NextFloat() < GetPlayerMissChancePercent(emotionLevel))
        {
            modifiers.FinalDamage *= 0f;
            return;
        }

        if (Main.rand.NextFloat() < GetPlayerExtraCritChancePercent(emotionLevel))
        {
            modifiers.SourceDamage *= 1.5f;
        }
    }

    public override void ModifyNpcOutgoingDamage(int emotionLevel, ref Player.HurtModifiers modifiers)
    {
        if (Main.rand.NextFloat() < GetNpcMissChancePercent(emotionLevel))
        {
            modifiers.FinalDamage *= 0f;
            return;
        }

        if (Main.rand.NextFloat() < GetNpcExtraCritChancePercent(emotionLevel))
        {
            modifiers.SourceDamage *= 1.5f;
        }
    }

    public override void ModifyNpcHitNpc(int emotionLevel, ref NPC.HitModifiers modifiers)
    {
        if (Main.rand.NextFloat() < GetNpcMissChancePercent(emotionLevel))
        {
            modifiers.FinalDamage *= 0f;
            return;
        }

        if (Main.rand.NextFloat() < GetNpcExtraCritChancePercent(emotionLevel))
        {
            modifiers.SetCrit();
        }
    }

    /// <summary>Allows a concrete Happy tier to append or replace tier-specific tooltip content.</summary>
    public virtual void HappyModifyBuffText(ref string buffName, ref string tip, ref int rare) { }
    public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
    {
        int emotionLevel = GetTooltipEmotionLevel();
        int speedUp = (int)MathF.Round(GetPlayerMovementSpeedIncreasePercent(emotionLevel) * 100);
        int extraCrit = (int)MathF.Round(GetPlayerExtraCritChancePercent(emotionLevel) * 100);
        int miss = (int)MathF.Round(GetPlayerMissChancePercent(emotionLevel) * 100);
        string buffTip = $"Speed up by {speedUp}%!" +
            $" Crit rate up by {extraCrit}%!" +
            $" Hit chance down by {miss}%!";
        tip = buffTip;
        HappyModifyBuffText(ref buffName, ref tip, ref rare);
        FinalTierModifyBuffText(emotionLevel, ref buffName, ref tip, ref rare);
    }
}
