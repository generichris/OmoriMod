using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Systems.EmotionSystem;
using OmoriMod.Content.Systems.EmotionSystem.Interfaces;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Players;

/// <summary>
/// Stores a player's resolved emotion state and forwards player-specific combat hooks
/// to the active <see cref="EmotionBuff"/>.
/// </summary>
/// <remarks>
/// The active buff repopulates <see cref="Emotion"/>, <see cref="ActiveEmotionBuff"/>, and
/// <see cref="EmotionLevel"/> each tick after <see cref="ResetEffects"/> clears transient state.
/// Final-tier scaling is retained separately so repeated applications can raise the effective
/// stat level without introducing additional buff types.
/// </remarks>
public class EmotionPlayer : ModPlayer, IEmotionEntity
{
    /// <summary>Gets or sets the emotion resolved from the player's active emotion buff.</summary>
    public EmotionType Emotion { get; set; }

    /// <summary>Gets or sets the buff currently responsible for the player's emotion effects.</summary>
    public EmotionBuff ActiveEmotionBuff { get; set; }

    /// <summary>Gets or sets the level currently used for player stat scaling.</summary>
    public int EmotionLevel { get; set; }

    /// <summary>Gets whether normal emotion applications are blocked for this player.</summary>
    public bool ImmuneToEmotionChange => false;

    /// <summary>The retained scaling level reached while a capped final-tier emotion remains active.</summary>
    public int ScalingEmotionLevel;

    /// <summary>The emotion family associated with <see cref="ScalingEmotionLevel"/>.</summary>
    public EmotionType ScalingEmotion;

    /// <summary>The progression midpoint selected for the current world difficulty.</summary>
    public int MidEmotionLevel;

    private void ResetMidEmotionLevel()
    {
        MidEmotionLevel = Main.hardMode ? 10 : 6;
    }

    /// <summary>
    /// Initializes or restores final-tier scaling for an emotion without reducing an existing level.
    /// </summary>
    /// <param name="emotion">The final-tier emotion currently active.</param>
    /// <param name="finalTier">The minimum scaling level declared by that emotion's final tier.</param>
    public void EnsureScalingEmotion(EmotionType emotion, int finalTier)
    {
        if (ScalingEmotion != emotion || ScalingEmotionLevel < finalTier)
        {
            ScalingEmotion = emotion;
            ScalingEmotionLevel = finalTier;
        }
    }

    private void ResetScalingEmotionLevel()
    {
        int? emotionType = EmotionSystem.GetEmotionType(Player);
        if (!emotionType.HasValue
            || !EmotionSystem.IsFinalEmotionTier(emotionType.Value)
            || ModContent.GetModBuff(emotionType.Value) is not EmotionBuff emotionBuff
            || emotionBuff.ScalingMode != EmotionScalingMode.Capped
            || !EmotionSystem.GetMaxEmotionTier(emotionBuff.Emotion).HasValue)
        {
            ScalingEmotion = EmotionType.None;
            ScalingEmotionLevel = 0;
            return;
        }

        EnsureScalingEmotion(
            emotionBuff.Emotion,
            EmotionSystem.GetMaxEmotionTier(emotionBuff.Emotion).Value);
    }

    /// <summary>Clears transient emotion state and restores valid final-tier scaling state each tick.</summary>
    public override void ResetEffects()
    {
        Emotion = EmotionType.None;
        ActiveEmotionBuff = null;
        EmotionLevel = 0;
        ResetMidEmotionLevel();
        ResetScalingEmotionLevel();
    }

    /// <summary>Removes the hidden bridge buff used by emotion-granting items before buffs update.</summary>
    public override void PreUpdateBuffs()
    {
        // Remove dummy buff
        Player.ClearBuff(ModContent.BuffType<DummyBuff>());
    }

    /// <summary>Applies incoming-damage behavior supplied by the active emotion.</summary>
    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        ActiveEmotionBuff?.ModifyPlayerIncomingDamage(EmotionLevel, ref modifiers);
    }

    /// <summary>Dispatches post-damage behavior supplied by the active emotion.</summary>
    public override void OnHurt(Player.HurtInfo info)
    {
        EmotionSystem.HandlePlayerHurt(Player, info);
    }
}
