using System;

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

    /// <summary>The preferred emotion buff type, resolved once per tick in <see cref="ResetEffects"/>.</summary>
    internal int? PreferredEmotionBuffType { get; private set; }

    /// <summary>Gets whether normal emotion applications are blocked for this player.</summary>
    public bool ImmuneToEmotionChange => false;

    /// <summary>The retained scaling level reached while a capped final-tier emotion remains active.</summary>
    public int ScalingEmotionLevel;

    /// <summary>The emotion family associated with <see cref="ScalingEmotionLevel"/>.</summary>
    public EmotionType ScalingEmotion;

    /// <summary>The progression midpoint selected for the current world difficulty.</summary>
    public int MidEmotionLevel;

    /// <summary>The mana cost reserved by Sad mitigation for the current incoming hit.</summary>
    internal int PendingSadManaDamage { get; set; }

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
        int boundedFinalTier = Math.Clamp(
            finalTier,
            1,
            EmotionStatTuning.PlayerMaxEmotionLevel);
        if (ScalingEmotion != emotion || ScalingEmotionLevel < boundedFinalTier)
        {
            ScalingEmotion = emotion;
            ScalingEmotionLevel = boundedFinalTier;
            return;
        }

        ScalingEmotionLevel = Math.Min(
            ScalingEmotionLevel,
            EmotionStatTuning.PlayerMaxEmotionLevel);
    }

    /// <summary>
    /// Explicitly increases a capped final-tier emotion by one effective level.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when the level increased; otherwise <see langword="false"/> at the cap.
    /// </returns>
    internal bool TryAmplifyEmotion(EmotionType emotion, int finalTier)
    {
        EnsureScalingEmotion(emotion, finalTier);
        if (ScalingEmotionLevel >= EmotionStatTuning.PlayerMaxEmotionLevel)
        {
            EmotionLevel = ScalingEmotionLevel;
            return false;
        }

        ScalingEmotionLevel++;
        EmotionLevel = ScalingEmotionLevel;
        return true;
    }

    internal bool TryApplySyncedScalingState(
        EmotionType emotion,
        int scalingLevel,
        bool requireActiveBuff)
    {
        int? activeBuffType = null;
        EmotionScalingMode activeScalingMode = EmotionScalingMode.Disabled;
        if (requireActiveBuff)
        {
            activeBuffType = EmotionSystem.GetEmotionType(Player);
            if (activeBuffType.HasValue
                && ModContent.GetModBuff(activeBuffType.Value) is EmotionBuff activeEmotion)
            {
                activeScalingMode = activeEmotion.ScalingMode;
            }
        }

        if (!EmotionSystem.IsValidScalingSync(
                emotion,
                scalingLevel,
                ScalingEmotion,
                ScalingEmotionLevel,
                activeBuffType,
                activeScalingMode,
                requireActiveBuff))
        {
            return false;
        }

        ScalingEmotion = emotion;
        ScalingEmotionLevel = scalingLevel;
        return true;
    }

    private void ResetScalingEmotionLevel()
    {
        int? emotionType = PreferredEmotionBuffType;
        if (!emotionType.HasValue
            || !EmotionSystem.IsFinalEmotionTier(emotionType.Value)
            || ModContent.GetModBuff(emotionType.Value) is not EmotionBuff emotionBuff
            || emotionBuff.ScalingMode != EmotionScalingMode.Capped
            || EmotionSystem.GetMaxEmotionTier(emotionBuff.Emotion) is not int maxTier)
        {
            ScalingEmotion = EmotionType.None;
            ScalingEmotionLevel = 0;
            return;
        }

        EnsureScalingEmotion(emotionBuff.Emotion, maxTier);
    }

    /// <summary>Clears transient emotion state and restores valid final-tier scaling state each tick.</summary>
    public override void ResetEffects()
    {
        Emotion = EmotionType.None;
        ActiveEmotionBuff = null;
        EmotionLevel = 0;
        PreferredEmotionBuffType = EmotionSystem.GetEmotionType(Player);
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
        PendingSadManaDamage = 0;
        ActiveEmotionBuff?.ModifyPlayerIncomingDamage(Player, EmotionLevel, ref modifiers);
    }

    /// <summary>Dispatches post-damage behavior supplied by the active emotion.</summary>
    public override void OnHurt(Player.HurtInfo info)
    {
        try
        {
            EmotionSystem.HandlePlayerHurt(Player, info);
        }
        finally
        {
            PendingSadManaDamage = 0;
        }
    }

    public override void CopyClientState(ModPlayer targetCopy)
    {
        var clone = (EmotionPlayer)targetCopy;
        clone.ScalingEmotion = ScalingEmotion;
        clone.ScalingEmotionLevel = ScalingEmotionLevel;
    }

    public override void SendClientChanges(ModPlayer clientPlayer)
    {
        var oldState = (EmotionPlayer)clientPlayer;
        if (oldState.ScalingEmotion != ScalingEmotion
            || oldState.ScalingEmotionLevel != ScalingEmotionLevel)
        {
            SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }
    }

    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
        ModPacket packet = global::OmoriMod.OmoriMod.Mod.GetPacket();
        packet.Write((byte)OmoriModMessageType.SyncEmotionPlayer);
        packet.Write((byte)Player.whoAmI);
        packet.Write((byte)ScalingEmotion);
        packet.Write((byte)ScalingEmotionLevel);
        packet.Send(toWho, fromWho);
    }
}
