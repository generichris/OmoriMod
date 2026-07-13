using OmoriMod.Content.Projectiles.Abstract_Classes;

using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Friendly.Melee.Bat;

public class BatProjectileSeeking : AngryProjectile
{
    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ModContent.ProjectileType<BatProjectile>());
    }

    public override void OnKill(int timeleft)
    {
        OnKillNoDrop(timeleft, noSound: true);
        MakeDust();
    }

    public override void AI()
    {
        AI_SeekingScytheProjectile(ticksStationaryUntilDespawn: 60, rotation: 0.5f, seekingDistance: 300);
    }
}