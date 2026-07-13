using Microsoft.Xna.Framework;

using OmoriMod.Content.Dusts;
using OmoriMod.Systems.EmotionSystem;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Abstract_Classes;

/// <summary>
/// An abstract class for projectiles that inflict emotions.
/// Use <see cref="AngryProjectile"/>, <see cref="HappyProjectile"/>, or <see cref="SadProjectile"/> 
/// to set emotions. If <see cref="Emotion"/> is not set, it will default to <see cref="EmotionType.NONE"/>.
/// </summary>
public abstract class EmotionProjectile : OmoriModProjectile, IOnHitEmotionObject
{

    public EmotionType Emotion { get; protected set; }

    /// <summary>
    /// Used to set the <see cref="Emotion"/>
    /// </summary>
    /// <param name="emotion">The emotion to be set.</param>
    protected void SetEmotionType(EmotionType emotion)
    {
        Emotion = emotion;
    }


    /// <summary>
    /// A hook method that allows emotion projectiles to call <see cref="OnHitNPC(NPC, NPC.HitInfo, int)"/> without breaking the emotion system.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="hit">The damage.</param>
    /// <param name="damageDone">The actual damage dealt to/taken by the NPC.</param>
    public virtual void OnHitNPCEmotion(NPC target, NPC.HitInfo hit, int damageDone) { }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        ((IOnHitEmotionObject)this).InflictEmotion(target);
        OnHitNPCEmotion(target, hit, damageDone);
    }

    /// <summary>
    /// Creates a new <see cref="EmotionDust"/>, with its color determined by <see cref="Emotion"/>
    /// </summary>
    public void MakeDust()
    {
        switch (Emotion)
        {
            case EmotionType.NONE:
                Dust.NewDust(Projectile.Center, 2, 2, ModContent.DustType<EmotionDust>(), 0f, 0f, 0, Color.White);
                break;
            case EmotionType.SAD:
                Dust.NewDust(Projectile.Center, 2, 2, ModContent.DustType<EmotionDust>(), 0f, 0f, 0, Color.Blue);
                break;
            case EmotionType.ANGRY:
                Dust.NewDust(Projectile.Center, 2, 2, ModContent.DustType<EmotionDust>(), 0f, 0f, 0, Color.Red);
                break;
            case EmotionType.HAPPY:
                Dust.NewDust(Projectile.Center, 2, 2, ModContent.DustType<EmotionDust>(), 0f, 0f, 0, Color.Yellow);
                break;

        }
    }
}