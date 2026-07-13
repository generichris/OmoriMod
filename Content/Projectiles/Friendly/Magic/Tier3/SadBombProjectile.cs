using OmoriMod.Content.Projectiles.Abstract_Classes;
using OmoriMod.Content.Projectiles.Friendly.Magic.Tier1;

using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Friendly.Magic.Tier3;

public class SadBombProjectile : SadProjectile
{
    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ModContent.ProjectileType<AngryBombProjectile>());
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
        AI_MagicBombProjectile<SadBoltProjectile>(damagePerProjectile: 18, ProjectileSpeedOnSpawn: 6, volleys: 6, shotsPerVolley: 8, interval: 30);
    }
}