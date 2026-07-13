using OmoriMod.Content.Projectiles.Abstract_Classes;
using OmoriMod.Content.Projectiles.Friendly.Magic.Tier1;

using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Friendly.Magic.Tier2;

public class SadBundleProjectile : SadProjectile
{
    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ModContent.ProjectileType<AngryBundleProjectile>());
    }

    public override void OnKill(int timeLeft)
    {
        OnKillNoDrop(timeLeft, noSound: true);
    }

    public override void AI()
    {
        AI_BundledProjectile<SadBoltProjectile>(damagePerProjectile: 18, ProjectileSpeedOnSpawn: 6, volleys: 4, shotsPerVolley: 5, interval: 60);
    }
}