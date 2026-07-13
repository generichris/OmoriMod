using Microsoft.Xna.Framework;

using OmoriMod.Content.Projectiles.Abstract_Classes;
using OmoriMod.Content.Projectiles.Friendly.Bullets.Tier1;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Tests;

public class TestProjectile : HappyProjectile
{
    public override void SetDefaults()
    {
        SetBulletDefaults();
    }

    public override void OnKill(int timeLeft)
    {
        OnKillNoDrop(timeLeft, noSound: true);
    }

    public override bool PreAI()
    {
        MakeDust();
        return true;
    }

    public override void AI()
    {
        AI_Timer++;

        if (AI_Timer == 5)
        {
            Quaternion Projectile1Q = new Quaternion(0, 0, 1, 0.120f);
            Quaternion Projectile2Q = new Quaternion(0, 0, 1, -0.120f);
            Quaternion Projectile3Q = new Quaternion(0, 0, 1, 0.085f);
            Quaternion Projectile4Q = new Quaternion(0, 0, 1, -0.085f);
            Quaternion Projectile5Q = new Quaternion(0, 0, 1, 0.050f);
            Quaternion Projectile6Q = new Quaternion(0, 0, 1, -0.050f);
            Quaternion Projectile7Q = new Quaternion(0, 0, 1, 0.015f);
            Quaternion Projectile8Q = new Quaternion(0, 0, 1, -0.015f);
            Vector2 Projectile1 = Vector2.Transform(Projectile.velocity, Projectile1Q);
            Vector2 Projectile2 = Vector2.Transform(Projectile.velocity, Projectile2Q);
            Vector2 Projectile3 = Vector2.Transform(Projectile.velocity, Projectile3Q);
            Vector2 Projectile4 = Vector2.Transform(Projectile.velocity, Projectile4Q);
            Vector2 Projectile5 = Vector2.Transform(Projectile.velocity, Projectile5Q);
            Vector2 Projectile6 = Vector2.Transform(Projectile.velocity, Projectile6Q);
            Vector2 Projectile7 = Vector2.Transform(Projectile.velocity, Projectile7Q);
            Vector2 Projectile8 = Vector2.Transform(Projectile.velocity, Projectile8Q);
            Projectile1 = Vector2.Negate(Projectile1);
            Projectile2 = Vector2.Negate(Projectile2);
            Projectile3 = Vector2.Negate(Projectile3);
            Projectile4 = Vector2.Negate(Projectile4);
            Projectile5 = Vector2.Negate(Projectile5);
            Projectile6 = Vector2.Negate(Projectile6);
            Projectile7 = Vector2.Negate(Projectile7);
            Projectile8 = Vector2.Negate(Projectile8);

            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile1,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile2,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile3,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile4,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile5,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile6,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile7,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile8,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
        }

        if (AI_Timer == 20)
        {
            Quaternion Projectile1Q = new Quaternion(0, 0, 1, 0.120f);
            Quaternion Projectile2Q = new Quaternion(0, 0, 1, -0.120f);
            Quaternion Projectile3Q = new Quaternion(0, 0, 1, 0.085f);
            Quaternion Projectile4Q = new Quaternion(0, 0, 1, -0.085f);
            Quaternion Projectile5Q = new Quaternion(0, 0, 1, 0.050f);
            Quaternion Projectile6Q = new Quaternion(0, 0, 1, -0.050f);
            Quaternion Projectile7Q = new Quaternion(0, 0, 1, 0.015f);
            Quaternion Projectile8Q = new Quaternion(0, 0, 1, -0.015f);
            Vector2 Projectile1 = Vector2.Transform(Projectile.velocity, Projectile1Q);
            Vector2 Projectile2 = Vector2.Transform(Projectile.velocity, Projectile2Q);
            Vector2 Projectile3 = Vector2.Transform(Projectile.velocity, Projectile3Q);
            Vector2 Projectile4 = Vector2.Transform(Projectile.velocity, Projectile4Q);
            Vector2 Projectile5 = Vector2.Transform(Projectile.velocity, Projectile5Q);
            Vector2 Projectile6 = Vector2.Transform(Projectile.velocity, Projectile6Q);
            Vector2 Projectile7 = Vector2.Transform(Projectile.velocity, Projectile7Q);
            Vector2 Projectile8 = Vector2.Transform(Projectile.velocity, Projectile8Q);
            Projectile1 = Vector2.Negate(Projectile1);
            Projectile2 = Vector2.Negate(Projectile2);
            Projectile3 = Vector2.Negate(Projectile3);
            Projectile4 = Vector2.Negate(Projectile4);
            Projectile5 = Vector2.Negate(Projectile5);
            Projectile6 = Vector2.Negate(Projectile6);
            Projectile7 = Vector2.Negate(Projectile7);
            Projectile8 = Vector2.Negate(Projectile8);

            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile1,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile2,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile3,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile4,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile5,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile6,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile7,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile8,
                ModContent.ProjectileType<HappyBulletProjectileNoDust>(), 4, Projectile.knockBack, Projectile.owner);
        }
    }
}