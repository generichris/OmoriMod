using OmoriMod.Content.Projectiles.Abstract_Classes;

using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Friendly.BossRelated.YeOldSprout;

public class SproutScytheProjectile : OmoriModProjectile
{
    public override void SetDefaults()
    {
        // Set magic Projectile defaults
        SetOtherDefaults(width: 32, height: 32, damageType: DamageClass.Magic, penetration: 3, aiStyle: 0, scale: 1f, timeLeft: 365);
    }

    public override void OnKill(int timeLeft)
    {
        OnKillNoDrop(timeLeft);
    }

    public override void AI()
    {
        AI_ScytheProjectile(ticksStationaryUntilDespawn: 60, rotation: 0.7f);
    }
}