using System;

using Microsoft.Xna.Framework;

using OmoriMod.Content.NPCs.Global;
using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem;

using Terraria;

namespace OmoriMod.Content.Buffs.Abstract;

/// <summary>
/// Implements the Sad emotion family: increased defense and health-damage-to-mana conversion
/// at the cost of movement speed.
/// </summary>
/// <remarks>
/// Concrete Sad buffs only declare their tier and visual frequency; this class handles shared
/// stat scaling, incoming-damage behavior, post-hurt mana loss, incompatibility cleanup, and tooltips.
/// </remarks>
public abstract class SadEmotionBase : EmotionBuff
{
    public static float GetPlayerDefenseIncreasePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Sad.PlayerStats.DefenseIncrease,
        EmotionStatTuning.PlayerMaxEmotionLevel);

    public static float GetNpcDefenseIncreasePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Sad.NpcStats.DefenseIncrease,
        EmotionStatTuning.NpcMaxEmotionLevel);

    public static float GetPlayerMovementSpeedDecreasePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Sad.PlayerStats.MovementSpeedDecrease,
        EmotionStatTuning.PlayerMaxEmotionLevel);

    public static float GetNpcMovementSpeedDecreasePercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Sad.NpcStats.MovementSpeedDecrease,
        EmotionStatTuning.NpcMaxEmotionLevel);

    public static float GetHealthDamageToManaDamageConversionPercent(int emotionLevel) => LinearPerLevel(
        emotionLevel,
        EmotionStatTuning.Sad.PlayerStats.HealthDamageToManaConversion,
        EmotionStatTuning.PlayerMaxEmotionLevel);

    protected SadEmotionBase()
    {
        Emotion = EmotionType.Sad;
        _dustColor = Color.Blue;
    }

    public override void UpdateEmotionBuff(Player player, ref int buffIndex)
    {
        EmotionSystem.RemoveIncompatibleEmotions<SadEmotionBase>(player);
        int emotionLevel = player.GetModPlayer<EmotionPlayer>().EmotionLevel;
        ModifyPlayerDefense(player, emotionLevel);
        ModifyPlayerMovement(player, emotionLevel); // Sad also reduces speed
    }

    public override void UpdateEmotionBuff(NPC npc, ref int buffIndex)
    {
        EmotionSystem.RemoveIncompatibleEmotions<SadEmotionBase>(npc);
        int emotionLevel = npc.GetGlobalNPC<EmotionNPC>().EmotionLevel;
        ModifyNpcDefense(npc, emotionLevel);
        ModifyNpcMovement(npc, emotionLevel);
    }

    public override void ModifyPlayerDefense(Player player, int emotionLevel)
    {
        player.statDefense *= 1 + GetPlayerDefenseIncreasePercent(emotionLevel);
    }

    public override void ModifyPlayerMovement(Player player, int emotionLevel)
    {
        player.moveSpeed *= 1 - GetPlayerMovementSpeedDecreasePercent(emotionLevel);
    }

    public override void ModifyNpcDefense(NPC npc, int emotionLevel)
    {
        npc.defense = (int)(npc.defDefense * (1.0f + GetNpcDefenseIncreasePercent(emotionLevel)));
    }

    public override void ModifyNpcMovement(NPC npc, int emotionLevel)
    {
        // CalculateNewPosition logic from Helper
        float modifier = -GetNpcMovementSpeedDecreasePercent(emotionLevel);
        Vector2 change;
        if (npc.noGravity) { change = npc.velocity * modifier; }
        else { change = new Vector2(npc.velocity.X * modifier, 0); }
        npc.position += change;
    }

    public override void ModifyPlayerIncomingDamage(int emotionLevel, ref Player.HurtModifiers modifiers)
    {
        modifiers.SourceDamage -= GetHealthDamageToManaDamageConversionPercent(emotionLevel);
    }

    public override void OnPlayerHurt(Player player, int emotionLevel, Player.HurtInfo hurtInfo)
    {
        float manaChange = hurtInfo.SourceDamage * GetHealthDamageToManaDamageConversionPercent(emotionLevel);
        if ((int)(player.statMana - manaChange) > 0)
        {
            player.statMana = (int)(player.statMana - manaChange);
        }
        else
        {
            player.statMana = 0;
        }
    }

    /// <summary>Allows a concrete Sad tier to append or replace tier-specific tooltip content.</summary>
    public virtual void SadModifyBuffText(ref string buffName, ref string tip, ref int rare) { }
    public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
    {
        int emotionLevel = GetTooltipEmotionLevel();
        int defenseUp = (int)MathF.Round(GetPlayerDefenseIncreasePercent(emotionLevel) * 100);
        int speedDown = (int)MathF.Round(GetPlayerMovementSpeedDecreasePercent(emotionLevel) * 100);
        int mana = (int)MathF.Round(GetHealthDamageToManaDamageConversionPercent(emotionLevel) * 100);
        string buffTip = $"Defense up by {defenseUp}%!" +
            $" Speed down by {speedDown}%!" +
            $" {mana}% of damage convertd to mana damage!";
        tip = buffTip;
        SadModifyBuffText(ref buffName, ref tip, ref rare);
        FinalTierModifyBuffText(emotionLevel, ref buffName, ref tip, ref rare);
    }
}
