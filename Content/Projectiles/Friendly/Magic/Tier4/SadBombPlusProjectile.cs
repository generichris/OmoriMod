using OmoriMod.Content.Projectiles.Abstract_Classes;
using OmoriMod.Content.Projectiles.Friendly.Magic.Tier2;

using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Friendly.Magic.Tier4;

public class SadBombPlusProjectile : SadProjectile
{
    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ModContent.ProjectileType<AngryBombPlusProjectile>());
    }

    public override void OnKill(int timeLeft)
    {
        OnKillNoDrop(timeLeft);
    }

    public override bool PreAI()
    {
        MakeDust();
        return true;
    }

    public override void AI()
    {
        AI_MagicBombProjectileWithFlip<SadBundleProjectileNoDust>(damagePerProjectile: 32, ProjectileSpeedOnSpawn: 6, volleys: 6, shotsPerVolley: 4, interval: 30);
    }
}