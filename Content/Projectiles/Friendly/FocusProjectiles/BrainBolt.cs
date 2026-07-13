using OmoriMod.Content.DamageClasses;
using OmoriMod.Content.Projectiles.Abstract_Classes;

using Terraria;

namespace OmoriMod.Content.Projectiles.Friendly.FocusProjectiles;

public class BrainBolt : OmoriModProjectile
{
    public override void SetDefaults()
    {
        // Set Focus Projectile defaults
        SetOtherDefaults(width: 4, height: 4, damageType: OmoriDamageClass.FocusDamage, aiStyle: 0, scale: 1.2f, tileCollide: true);

        // Drawing offset for correct positioning
        DrawOffsetX = -7;
        DrawOriginOffsetX = 1;
        DrawOriginOffsetY = -14;
    }

    public override void PostAI()
    {
        // Rotate and flip
        VelocityRotateWith90(flip: true);

        // Rotate an extra bit slightly
        Projectile.rotation += .15f;
    }

    public override void OnKill(int timeLeft)
    {
        OnKillNoDrop(timeLeft, noSound: true);
    }
}