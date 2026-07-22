using System;

using Microsoft.Xna.Framework;

using OmoriMod.Content.NPCs.Global;
using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem;

using Terraria;

namespace OmoriMod.Content.Buffs.Abstract;

/// <summary>
/// Implements the Angry emotion family: increased outgoing damage at the cost of defense.
/// </summary>
/// <remarks>
/// Concrete Angry buffs only declare their tier and visual frequency; all shared stat scaling,
/// combat hooks, incompatibility cleanup, and tooltip text are handled here.
/// </remarks>
public abstract class AngryEmotionBase : EmotionBuff
{
    public static float GetPlayerDamageIncreasePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Angry.PlayerStats.DamageIncrease,
        EmotionStatTuning.PlayerMaxEmotionLevel);

    public static float GetNpcDamageIncreasePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Angry.NpcStats.DamageIncrease,
        EmotionStatTuning.NpcMaxEmotionLevel);

    public static float GetPlayerDefenseDecreasePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Angry.PlayerStats.DefenseDecrease,
        EmotionStatTuning.PlayerMaxEmotionLevel);

    public static float GetNpcDefenseDecreasePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Angry.NpcStats.DefenseDecrease,
        EmotionStatTuning.NpcMaxEmotionLevel);

    protected AngryEmotionBase()
    {
        Emotion = EmotionType.Angry;
        _dustColor = Color.Red;
    }

    public override void UpdateEmotionBuff(Player player, ref int buffIndex)
    {
        EmotionSystem.RemoveIncompatibleEmotions<AngryEmotionBase>(player);
        ModifyPlayerDefense(player, player.GetModPlayer<EmotionPlayer>().EmotionLevel);
    }

    public override void UpdateEmotionBuff(NPC npc, ref int buffIndex)
    {
        EmotionSystem.RemoveIncompatibleEmotions<AngryEmotionBase>(npc);
        ModifyNpcDefense(npc, npc.GetGlobalNPC<EmotionNPC>().EmotionLevel);
    }

    public override void ModifyPlayerOutgoingDamage(int emotionLevel, ref NPC.HitModifiers modifiers)
    {
        modifiers.SourceDamage += GetPlayerDamageIncreasePercent(emotionLevel);
    }

    public override void ModifyPlayerOutgoingDamage(int emotionLevel, ref Player.HurtModifiers modifiers)
    {
        modifiers.SourceDamage += GetPlayerDamageIncreasePercent(emotionLevel);
    }

    public override void ModifyNpcOutgoingDamage(int emotionLevel, ref Player.HurtModifiers modifiers)
    {
        modifiers.SourceDamage += GetNpcDamageIncreasePercent(emotionLevel);
    }

    public override void ModifyNpcHitNpc(int emotionLevel, ref NPC.HitModifiers modifiers)
    {
        modifiers.SourceDamage += GetNpcDamageIncreasePercent(emotionLevel);
    }

    public override void ModifyPlayerDefense(Player player, int emotionLevel)
    {
        player.statDefense *= (1 - GetPlayerDefenseDecreasePercent(emotionLevel));
    }

    public override void ModifyNpcDefense(NPC npc, int emotionLevel)
    {
        npc.defense = (int)(npc.defDefense * (1.0f - GetNpcDefenseDecreasePercent(emotionLevel)));
    }

    /// <summary>Allows a concrete Angry tier to append or replace tier-specific tooltip content.</summary>
    public virtual void AngryModifyBuffText(ref string buffName, ref string tip, ref int rare) { }
    public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
    {
        int emotionLevel = GetTooltipEmotionLevel();
        int damageUp = (int)MathF.Round(GetPlayerDamageIncreasePercent(emotionLevel) * 100);
        int defenseDown = (int)MathF.Round(GetPlayerDefenseDecreasePercent(emotionLevel) * 100);
        string buffTip = $"Attack up by {damageUp}%!" +
            $" Defense down by {defenseDown}%!";
        tip = buffTip;
        AngryModifyBuffText(ref buffName, ref tip, ref rare);
        FinalTierModifyBuffText(emotionLevel, ref buffName, ref tip, ref rare);
    }
}
