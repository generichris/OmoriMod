using Microsoft.Xna.Framework;

using OmoriMod.Content.Dusts;
using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem;
using OmoriMod.Content.Systems.EmotionSystem.Interfaces;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Abstract_Classes;

/// <summary>
/// Base class for projectiles that apply an emotion to NPCs on hit and participate in
/// emotional-advantage calculations when hitting another player.
/// </summary>
/// <remarks>
/// Derive from <see cref="AngryProjectile"/>, <see cref="HappyProjectile"/>, or
/// <see cref="SadProjectile"/> when the projectile's emotion is fixed. Direct subclasses
/// default to <see cref="EmotionType.None"/>.
/// </remarks>
public abstract class EmotionProjectile : OmoriModProjectile, IOnHitEmotionObject
{

    /// <summary>Gets the emotion applied or represented by this projectile.</summary>
    public EmotionType Emotion { get; protected set; }

    /// <summary>
    /// Sets the emotion represented by this projectile.
    /// </summary>
    /// <param name="emotion">The emotion to be set.</param>
    protected void SetEmotionType(EmotionType emotion)
    {
        Emotion = emotion;
    }


    /// <summary>
    /// Provides an extension hook that runs after the base class applies its emotion to an NPC.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="hit">The damage.</param>
    /// <param name="damageDone">The actual damage dealt to/taken by the NPC.</param>
    public virtual void OnHitNPCEmotion(NPC target, NPC.HitInfo hit, int damageDone) { }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        EmotionSystem.ApplyEmotion(target, Emotion);
        OnHitNPCEmotion(target, hit, damageDone);
    }

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
    {
        if (Projectile.owner < 0 || Projectile.owner >= Main.maxPlayers)
        {
            return;
        }

        Player owner = Main.player[Projectile.owner];
        if (!owner.active || owner.whoAmI == target.whoAmI)
        {
            return;
        }

        EmotionSystem.ApplyCombatModifiers(
            owner.GetModPlayer<EmotionPlayer>(),
            target.GetModPlayer<EmotionPlayer>(),
            ref modifiers);
    }

    /// <summary>
    /// Spawns an <see cref="EmotionDust"/> particle colored for this projectile's emotion.
    /// </summary>
    public void MakeDust()
    {
        switch (Emotion)
        {
            case EmotionType.None:
                Dust.NewDust(Projectile.Center, 2, 2, ModContent.DustType<EmotionDust>(), 0f, 0f, 0, Color.White);
                break;
            case EmotionType.Sad:
                Dust.NewDust(Projectile.Center, 2, 2, ModContent.DustType<EmotionDust>(), 0f, 0f, 0, Color.Blue);
                break;
            case EmotionType.Angry:
                Dust.NewDust(Projectile.Center, 2, 2, ModContent.DustType<EmotionDust>(), 0f, 0f, 0, Color.Red);
                break;
            case EmotionType.Happy:
                Dust.NewDust(Projectile.Center, 2, 2, ModContent.DustType<EmotionDust>(), 0f, 0f, 0, Color.Yellow);
                break;

        }
    }
}
