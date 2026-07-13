using OmoriMod.Content.Buffs.AngryBuff;
using OmoriMod.Content.Buffs.FearBuff;
using OmoriMod.Content.Buffs.HappyBuff;
using OmoriMod.Content.Buffs.SadBuff;
using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.NPCs.Global;
using OmoriMod.Content.Projectiles.Abstract_Classes;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Systems.EmotionSystem;


/// <summary>
/// An interface for objects that apply emotions
/// Acts like an abstract class for <see cref="EmotionProjectile"/> and <see cref="EmotionItem"/>
/// </summary>
public interface IOnHitEmotionObject : IEmotionObject
{

    /// <summary>
    /// Inflicts an emotion determined by <see cref="Emotion"/> on an enemy. Does not inflict anything if the <see cref="EmotionNPC"/> is immune to emotion changes
    /// </summary>
    /// <param name="target">The <see cref="NPC"/> the emotion will be applied to.</param>
    /// <param name="ticks">The amount of ticks the emotion will be applied for.</param>
    public virtual void InflictEmotion(NPC target, int ticks = 600)
    {
        if (!target.GetGlobalNPC<EmotionNPC>().ImmuneToEmotionChange)
        {
            switch (Emotion)
            {
                case EmotionType.NONE:
                    break;
                case EmotionType.SAD:
                    if (!target.HasBuff<Happy>() && !target.HasBuff<Angry>())
                    {
                        target.AddBuff(ModContent.BuffType<Sad>(), ticks);
                    }
                    break;
                case EmotionType.ANGRY:
                    if (!target.HasBuff<Happy>() && !target.HasBuff<Sad>())
                    {
                        target.AddBuff(ModContent.BuffType<Angry>(), ticks);
                    }
                    break;
                case EmotionType.HAPPY:
                    if (!target.HasBuff<Angry>() && !target.HasBuff<Sad>())
                    {
                        target.AddBuff(ModContent.BuffType<Happy>(), ticks);
                    }
                    break;
                case EmotionType.FEAR:
                    if (!target.HasBuff<Happy>() && !target.HasBuff<Angry>() && !target.HasBuff<Sad>())
                    {
                        target.AddBuff(ModContent.BuffType<Fear>(), ticks);
                    }
                    break;
            }
        }
    }
}