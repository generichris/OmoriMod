using Microsoft.Xna.Framework;

using OmoriMod.Content.Projectiles.Abstract_Classes;

using Terraria;

namespace OmoriMod.Content.Projectiles.NonFriendly.Bosses.SweetHeart;

public class HomingHeart : OmoriModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 16;
        Projectile.height = 16;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 300;

        Projectile.hostile = true;
        Projectile.friendly = false;

        Projectile.ignoreWater = true;
    }

    public override void AI()
    {
        float turnResistance = 30f;
        float speed = 4f;

        Player target = FindClosestPlayer();

        WeakHoming(target, speed, turnResistance);
        VelocityRotate();
        AddLight(Projectile.Center, Color.HotPink);
    }
}