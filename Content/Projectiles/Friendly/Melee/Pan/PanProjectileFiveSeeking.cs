using OmoriMod.Content.Projectiles.Abstract_Classes;
using OmoriMod.Content.Projectiles.Friendly.Melee.Bat;

using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Friendly.Melee.Pan;

public class PanProjectileFiveSeeking : HappyProjectile
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
        AI_SplittingProjectile<PanProjectileSeeking>(maxAngle: 20, ProjectileAmount: 5);
    }
}