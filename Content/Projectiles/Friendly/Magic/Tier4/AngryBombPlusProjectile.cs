using OmoriMod.Content.Projectiles.Abstract_Classes;
using OmoriMod.Content.Projectiles.Friendly.Magic.Tier2;

using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Friendly.Magic.Tier4;

public class AngryBombPlusProjectile : AngryProjectile
{
    public override void SetDefaults()
    {
        SetOtherDefaults(width: 24, height: 24, damageType: DamageClass.Magic, aiStyle: 0, penetration: 5, scale: 1, tileCollide: true);
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
        AI_MagicBombProjectileWithFlip<AngryBundleProjectileNoDust>(damagePerProjectile: 32, ProjectileSpeedOnSpawn: 6, volleys: 6, shotsPerVolley: 4, interval: 30);
    }
}