using System;

using Microsoft.Xna.Framework;

using OmoriMod.Content.Dusts;
using OmoriMod.Content.NPCs.Global;
using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem;
using OmoriMod.Content.Systems.EmotionSystem.Interfaces;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Buffs.Abstract;

/// <summary>
/// Base class for registered emotion buffs and the dispatch point for emotion-specific
/// player, NPC, combat, tooltip, scaling, and visual behavior.
/// </summary>
/// <remarks>
/// Concrete tier buffs declare immutable content metadata such as <see cref="EmotionTier"/>.
/// Emotion-family base classes override the effect hooks, while this class synchronizes the
/// active buff into <see cref="EmotionPlayer"/> or <see cref="EmotionNPC"/> each tick.
/// </remarks>
public abstract class EmotionBuff : ModBuff, IEmotionObject
{
    /// <summary>Gets the emotion family represented by this buff.</summary>
    public EmotionType Emotion { get; protected set; }

    /// <summary>
    /// Gets the tier declared by this buff type.
    /// </summary>
    /// <remarks>
    /// This is content metadata shared by every entity using the buff. Runtime player and NPC
    /// levels belong to their respective <see cref="IEmotionEntity"/> implementations.
    /// </remarks>
    public int EmotionTier { get; protected set; }

    /// <summary>
    /// Gets the policy used when a player reapplies this family's final standard tier.
    /// </summary>
    /// <remarks>
    /// Family base classes may override this property. Every standard tier registered to the
    /// same <see cref="EmotionType"/> must declare the same value. NPC and no-time buffs do not
    /// participate in final-tier player scaling.
    /// </remarks>
    public virtual EmotionScalingMode ScalingMode => EmotionScalingMode.Capped;

    /// <summary>
    /// The number of emotion-dust particles spawned per second while this buff is active on a player.
    /// </summary>
    protected int _dustSpawnFrequency;

    /// <summary>
    /// Determines whether this buff cannot coexist with another emotion buff.
    /// </summary>
    /// <remarks>The default policy makes different emotion families mutually exclusive.</remarks>
    public virtual bool IsIncompatibleWith(EmotionBuff otherBuff)
    {
        return Emotion != otherBuff.Emotion;
    }

    /// <summary>The color assigned to emotion dust emitted by this buff.</summary>
    protected Color _dustColor;

    /// <summary>Runs emotion-family behavior during a player's buff update.</summary>
    public virtual void UpdateEmotionBuff(Player player, ref int buffIndex) { }
    /// <summary>Runs emotion-family behavior during an NPC's buff update.</summary>
    public virtual void UpdateEmotionBuff(NPC npc, ref int buffIndex) { }

    /// <summary>Synchronizes player emotion state, scaling, visuals, and family-specific effects.</summary>
    public override void Update(Player player, ref int buffIndex)
    {
        var modPlayer = player.GetModPlayer<EmotionPlayer>();
        modPlayer.Emotion = Emotion;
        modPlayer.ActiveEmotionBuff = this;

        UpdateEmotionLevel(modPlayer);
        DustHandler(player, ref buffIndex);
        UpdateEmotionBuff(player, ref buffIndex);
    }

    /// <summary>Synchronizes NPC emotion state and family-specific effects.</summary>
    public override void Update(NPC npc, ref int buffIndex)
    {
        var emotionNpc = npc.GetGlobalNPC<EmotionNPC>();
        emotionNpc.Emotion = Emotion;
        emotionNpc.ActiveEmotionBuff = this;
        emotionNpc.EmotionLevel = EmotionSystem.GetEmotionTier(Type) ?? EmotionTier;

        UpdateEmotionBuff(npc, ref buffIndex);
    }

    private void UpdateEmotionLevel(EmotionPlayer modPlayer)
    {
        int registeredTier = EmotionSystem.GetEmotionTier(Type) ?? EmotionTier;
        if (!EmotionSystem.IsFinalEmotionTier(Type)
            || ScalingMode == EmotionScalingMode.Disabled)
        {
            modPlayer.EmotionLevel = registeredTier;
            return;
        }

        int? finalTier = EmotionSystem.GetMaxEmotionTier(Emotion);
        if (!finalTier.HasValue)
        {
            return;
        }

        modPlayer.EnsureScalingEmotion(Emotion, finalTier.Value);
        modPlayer.EmotionLevel = modPlayer.ScalingEmotionLevel;
    }

    /// <summary>
    /// Applies the family's scaling policy when a player's final standard tier is reapplied.
    /// </summary>
    /// <remarks>
    /// Capped families increment their retained scaling level up to the configured maximum.
    /// Disabled families leave their effective level fixed. Both policies allow tModLoader's
    /// normal duration refresh behavior, while non-final tiers use the base implementation.
    /// </remarks>
    public override bool ReApply(Player player, int time, int buffIndex)
    {
        if (!EmotionSystem.IsFinalEmotionTier(Type))
        {
            return base.ReApply(player, time, buffIndex);
        }

        if (ScalingMode == EmotionScalingMode.Disabled)
        {
            return false;
        }

        int? finalTier = EmotionSystem.GetMaxEmotionTier(Emotion);
        if (!finalTier.HasValue)
        {
            return base.ReApply(player, time, buffIndex);
        }

        EmotionPlayer modPlayer = player.GetModPlayer<EmotionPlayer>();
        modPlayer.EnsureScalingEmotion(Emotion, finalTier.Value);
        if (modPlayer.ScalingEmotionLevel < EmotionStatTuning.PlayerMaxEmotionLevel)
        {
            modPlayer.ScalingEmotionLevel++;
        }
        modPlayer.EmotionLevel = modPlayer.ScalingEmotionLevel;

        return false;
    }

    /// <summary>Applies this emotion's defense effect to a player.</summary>
    public virtual void ModifyPlayerDefense(Player player, int emotionLevel) { }
    /// <summary>Applies this emotion's defense effect to an NPC.</summary>
    public virtual void ModifyNpcDefense(NPC npc, int emotionLevel) { }
    /// <summary>Applies this emotion's movement effect to a player.</summary>
    public virtual void ModifyPlayerMovement(Player player, int emotionLevel) { }
    /// <summary>Applies this emotion's movement effect to an NPC.</summary>
    public virtual void ModifyNpcMovement(NPC npc, int emotionLevel) { }

    /// <summary>Modifies damage when an emotional player attacks an NPC.</summary>
    public virtual void ModifyPlayerOutgoingDamage(int emotionLevel, ref NPC.HitModifiers modifiers) { }
    /// <summary>Modifies damage when an emotional player attacks another player.</summary>
    public virtual void ModifyPlayerOutgoingDamage(int emotionLevel, ref Player.HurtModifiers modifiers) { }

    /// <summary>Modifies damage when an emotional NPC attacks a player.</summary>
    public virtual void ModifyNpcOutgoingDamage(int emotionLevel, ref Player.HurtModifiers modifiers) { }
    /// <summary>Modifies damage when an emotional NPC attacks another NPC.</summary>
    public virtual void ModifyNpcHitNpc(int emotionLevel, ref NPC.HitModifiers modifiers) { }

    /// <summary>Applies non-damage hit effects when an emotional player attacks an NPC.</summary>
    public virtual void ModifyPlayerHitNpc(int emotionLevel, ref NPC.HitModifiers modifiers) { }
    /// <summary>Applies non-damage hit effects when an emotional player attacks another player.</summary>
    public virtual void ModifyPlayerHitPlayer(int emotionLevel, ref Player.HurtModifiers modifiers) { }

    /// <summary>Modifies incoming damage before an emotional player is hurt.</summary>
    public virtual void ModifyPlayerIncomingDamage(int emotionLevel, ref Player.HurtModifiers modifiers) { }

    /// <summary>Runs emotion-specific behavior after a player takes damage.</summary>
    public virtual void OnPlayerHurt(Player player, int emotionLevel, Player.HurtInfo hurtInfo) { }


    /// <summary>
    /// Calculates a two-phase percentage curve for an emotion stat.
    /// </summary>
    /// <remarks>
    /// Levels through <paramref name="rateChange"/> use <paramref name="rate"/> per level.
    /// Remaining levels interpolate toward <paramref name="max"/> at <paramref name="maxEmotionLevel"/>.
    /// The result includes <paramref name="startingValue"/>, is capped at <paramref name="max"/>,
    /// and is returned as a decimal multiplier rather than a whole percentage.
    /// </remarks>
    /// <param name="emotionLevel">The effective emotion level.</param>
    /// <param name="max">The maximum whole-percentage value.</param>
    /// <param name="rate">The whole-percentage gain per level during the initial phase.</param>
    /// <param name="maxEmotionLevel">The level at which the curve reaches its maximum.</param>
    /// <param name="startingValue">The flat whole-percentage value added to the curve.</param>
    /// <param name="rateChange">The final level of the initial linear phase.</param>
    /// <returns>The scaled value as a decimal multiplier.</returns>
    protected static float LinearPerLevel(int emotionLevel, float max, float rate, int maxEmotionLevel, float startingValue = 0f, int rateChange = 3)
    {
        float result;

        if (emotionLevel <= rateChange)
        {
            // Phase 1: simple linear
            result = emotionLevel * rate;
        }
        else
        {
            // Phase 2: linear interpolation to max
            float initialValue = rateChange * rate;
            float t = (emotionLevel - rateChange) / (float)(maxEmotionLevel - rateChange);
            result = MathHelper.Lerp(initialValue, max, t);
        }

        result += startingValue;
        return Math.Min(result, max) / 100f;
    }

    /// <summary>Calculates a two-phase percentage curve from an <see cref="EmotionStatScaling"/> definition.</summary>
    protected static float LinearPerLevel(
        int emotionLevel,
        EmotionStatScaling scaling,
        int maxEmotionLevel,
        int rateChange = 3)
    {
        return LinearPerLevel(
            emotionLevel,
            scaling.MaximumPercent,
            scaling.RatePercent,
            maxEmotionLevel,
            scaling.StartingPercent,
            rateChange);
    }

    /// <summary>Calculates uncapped compound percentage growth by emotion level.</summary>
    protected static float ExponentialGrowthPerLevel(int emotionLevel, float perLvl, float startingValue = 0)
    {
        // turn values into percents
        float percentPerLvl = perLvl / 100;
        float percentStartingValue = startingValue / 100;

        // add 1 to account for base percent
        float baseMultiplier = 1 + percentPerLvl;
        // remove 1 to isolate growth
        float growth = MathF.Pow(baseMultiplier, emotionLevel) - 1;

        return growth + percentStartingValue;
    }

    /// <summary>Calculates logistic percentage growth bounded by minimum and maximum values.</summary>
    /// <param name="emotionLevel">The effective emotion level.</param>
    /// <param name="perLvl">The whole-percentage steepness of the curve.</param>
    /// <param name="maxValue">The upper whole-percentage bound.</param>
    /// <param name="emotionMidLevel">The emotion level at the midpoint of the output range.</param>
    /// <param name="minValue">The lower whole-percentage bound.</param>
    /// <returns>The scaled value as a decimal multiplier.</returns>
    protected static float LogisticGrowthPerLevel(int emotionLevel, float perLvl, float maxValue, float emotionMidLevel, float minValue = 0f)
    {
        float percentMaxValue = maxValue / 100;
        float percentMinValue = minValue / 100;
        float percentPerLvl = perLvl / 100;

        float range = percentMaxValue - percentMinValue;

        float exponent = -percentPerLvl * (emotionLevel - emotionMidLevel);
        float value = range / (1 + MathF.Exp(exponent));

        return value + percentMinValue;
    }

    /// <summary>Calculates uncapped linear percentage growth by emotion level.</summary>
    protected static float LinearPerLevel(int emotionLevel, float perLvl, float startingValue = 0)
    {
        // turn values into percents
        float percentPerLvl = perLvl / 100;
        float percentStartingValue = startingValue / 100;

        return percentPerLvl * emotionLevel + percentStartingValue;
    }

    private void DustHandler(Player player, ref int buffIndex)
    {
        int dustFrequency = 60 / _dustSpawnFrequency;

        if (player.buffTime[buffIndex] % dustFrequency == 0)
        {
            Dust.NewDust(
            Position: player.Center,
            Width: 2,
            Height: 2,
            Type: ModContent.DustType<EmotionDust>(),
            SpeedX: 0f,
            SpeedY: 0f,
            Alpha: 0,
            newColor: _dustColor
            );
        }
    }


    /// <summary>
    /// Gets the effective level to display in a buff tooltip, including final-tier player scaling.
    /// </summary>
    protected int GetTooltipEmotionLevel()
    {
        int registeredTier = EmotionSystem.GetEmotionTier(Type) ?? EmotionTier;
        Player localPlayer = Main.LocalPlayer;
        if (localPlayer == null || !localPlayer.active || !localPlayer.HasBuff(Type))
        {
            return registeredTier;
        }

        EmotionPlayer emotionPlayer = localPlayer.GetModPlayer<EmotionPlayer>();
        if (emotionPlayer.ActiveEmotionBuff?.Type == Type && emotionPlayer.EmotionLevel > 0)
        {
            return emotionPlayer.EmotionLevel;
        }

        return ScalingMode == EmotionScalingMode.Capped
            && EmotionSystem.IsFinalEmotionTier(Type)
            && emotionPlayer.ScalingEmotion == Emotion
            && emotionPlayer.ScalingEmotionLevel >= registeredTier
                ? emotionPlayer.ScalingEmotionLevel
                : registeredTier;
    }

    /// <summary>Appends the post-final-tier scaling level to a final-tier buff tooltip.</summary>
    protected void FinalTierModifyBuffText(int emotionLevel, ref string buffName, ref string tip, ref int rare)
    {
        if (ScalingMode == EmotionScalingMode.Disabled
            || !EmotionSystem.IsFinalEmotionTier(Type))
        {
            return;
        }

        int? finalTier = EmotionSystem.GetMaxEmotionTier(Emotion);
        if (finalTier.HasValue)
        {
            tip += $" Level: {emotionLevel - finalTier.Value + 1}";
        }
    }
}
