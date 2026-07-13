using System;

using Microsoft.Xna.Framework;

using OmoriMod.Content.Dusts;
using OmoriMod.Content.NPCs.Global;
using OmoriMod.Content.Players;
using OmoriMod.Systems.EmotionSystem;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Buffs.Abstract;

public abstract class EmotionBuff : ModBuff, IEmotionObject
{
    public EmotionType Emotion { get; protected set; }

    public int emotionLevel;

    /// <summary>
    /// How often dust spawns for this <see cref="EmotionBuff"/>. A value of 2 means 2 dust is spawned every second.
    /// </summary>
    protected int dustSpawnFrequency;

    public virtual bool IsIncompatibleWith(EmotionBuff otherBuff)
    {
        return Emotion != otherBuff.Emotion;
    }

    public virtual int? NextTierEmotion => nextStageEmotionType;

    public int? nextStageEmotionType;

    protected Color dustColor;

    public virtual void UpdateTier4EmotionBuff(Player player, ref int buffIndex) { }
    public virtual void UpdateEmotionBuff(Player player, ref int buffIndex) { }
    public virtual void UpdateEmotionBuff(NPC npc, ref int buffIndex) { }

    public override void Update(Player player, ref int buffIndex)
    {
        var modPlayer = player.GetModPlayer<EmotionPlayer>();
        modPlayer.Emotion = Emotion;
        modPlayer.ActiveEmotionBuff = this;

        DustHandler(player, ref buffIndex);
        UpdateEmotionBuff(player, ref buffIndex);
        UpdateTier4EmotionBuff(player, ref buffIndex);
    }

    public override void Update(NPC npc, ref int buffIndex)
    {
        var emotionNPC = npc.GetGlobalNPC<EmotionNPC>();
        emotionNPC.Emotion = Emotion;
        emotionNPC.ActiveEmotionBuff = this;

        UpdateEmotionBuff(npc, ref buffIndex);
    }

    // Virtual Modifiers
    public virtual void ModifyPlayerDefense(Player player) { }
    public virtual void ModifyNPCDefense(NPC npc) { }
    public virtual void ModifyPlayerMovement(Player player) { }
    public virtual void ModifyNPCMovement(NPC npc) { }

    public virtual void ModifyPlayerOutgoingDamage(ref NPC.HitModifiers modifiers) { }
    public virtual void ModifyPlayerOutgoingDamage(ref Player.HurtModifiers modifiers) { }

    public virtual void ModifyNPCOutgoingDamage(ref Player.HurtModifiers modifiers) { }

    // Happy Hit Modifiers (Player attacking NPC)
    public virtual void ModifyPlayerHitNPC(ref NPC.HitModifiers modifiers) { }
    // Happy Hit Modifiers (Player attacking Player/Self?)
    public virtual void ModifyPlayerHitPlayer(ref Player.HurtModifiers modifiers) { }

    // Sad Damage Reduction (Player taking damage)
    public virtual void ModifyPlayerIncomingDamage(ref Player.HurtModifiers modifiers) { }

    // Sad Mana Conversion (NPC hitting Player)
    public virtual void OnPlayerHurt(Player player, Player.HurtInfo hurtInfo) { }


    /// <summary>
    /// Calculates a linear rate of change using <paramref name="rate"/> until
    /// <see cref="emotionLevel"/> = <paramref name="rateChange"/>. Then shifts to linear rate of change using percentage of remaining emotion levels up until
    /// <paramref name="maxEmotionLevel"/> to reach <paramref name="maxPercentage"/>.
    /// </summary>
    /// <param name="max">The maximum return value of this function. In percentage form.</param>
    /// <param name="rate">The rate of change for the linear function until <see cref="emotionLevel"/> = <paramref name="rateChange"/>.</param>
    /// <param name="startingValue">The starting value for this function.</param>
    /// <param name="maxEmotionLevel">The maximum value that <see cref="emotionLevel"/> can be.</param>
    /// <param name="rateChange">The <see cref="emotionLevel"/> when the percentage remaining linear function begins.</param>
    /// <returns></returns>
    protected float LinearPerLevel(float max, float rate, int maxEmotionLevel, float startingValue = 0f, int rateChange = 3)
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


    protected float ExponentialGrowthPerLevel(float perLvl, float startingValue = 0)
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


    /// <param name="emotionMidLevel">The value of <see cref="emotionLevel"/> which will result in the function outputting <paramref name="maxValue"/> + <paramref name="minValue"/> / 2</param>
    protected float LogisticGrowthPerLevel(float perLvl, float maxValue, float emotionMidLevel, float minValue = 0f)
    {
        float percentMaxValue = maxValue / 100;
        float percentMinValue = minValue / 100;
        float percentPerLvl = perLvl / 100;

        float range = percentMaxValue - percentMinValue;

        float exponent = -percentPerLvl * (emotionLevel - emotionMidLevel);
        float value = range / (1 + MathF.Exp(exponent));

        return value + percentMinValue;
    }

    protected float LinearPerLevel(float perLvl, float startingValue = 0)
    {
        // turn values into percents
        float percentPerLvl = perLvl / 100;
        float percentStartingValue = startingValue / 100;

        return percentPerLvl * emotionLevel + percentStartingValue;
    }

    private void DustHandler(Player player, ref int buffIndex)
    {
        int dustFrequency = 60 / dustSpawnFrequency;

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
            newColor: dustColor
            );
        }
    }


    protected virtual void Tier4ModifyBuffText(ref string buffName, ref string tip, ref int rare)
    {
        string buffTip = $" Level: {emotionLevel - 3}";
        tip += buffTip;
    }
}