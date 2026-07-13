using OmoriMod.Content.Projectiles.Abstract_Classes;

using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Friendly.Magic.Tier1;

public class HappyBoltProjectileNoDust : HappyProjectile
{
    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ModContent.ProjectileType<AngryBoltProjectile>());
    }

    public override void OnKill(int timeLeft)
    {
        OnKillNoDrop(timeLeft);
    }
}