using OmoriMod.Content.Projectiles.Abstract_Classes;
using OmoriMod.Content.Projectiles.Friendly.Magic.Tier1;

using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Friendly.Magic.Tier2;

public class AngryBundleProjectile : AngryProjectile
{
    public override void SetDefaults()
    {
        SetOtherDefaults(width: 46, height: 42, damageType: DamageClass.Magic, aiStyle: -1, penetration: -1, scale: 1, tileCollide: true);
    }

    public override void OnKill(int timeLeft)
    {
        OnKillNoDrop(timeLeft, noSound: true);
    }

    public override void AI()
    {
        AI_BundledProjectile<AngryBoltProjectile>(damagePerProjectile: 18, ProjectileSpeedOnSpawn: 6, volleys: 4, shotsPerVolley: 5, interval: 60);
    }
}