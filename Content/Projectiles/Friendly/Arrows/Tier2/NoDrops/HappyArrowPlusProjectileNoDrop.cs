using OmoriMod.Content.Projectiles.Abstract_Classes;
using OmoriMod.Content.Projectiles.Friendly.Arrows.Tier1.NoDrops;

namespace OmoriMod.Content.Projectiles.Friendly.Arrows.Tier2.NoDrops;

public class HappyArrowPlusProjectileNoDrop : HappyProjectile
{
    public override void SetDefaults()
    {
        SetArrowDefaults();
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
        AI_MultiSplittingProjectile<HappyArrowProjectileNoDropNoDust>(maxAngle: 32, ProjectileAmount: 9);
    }
}