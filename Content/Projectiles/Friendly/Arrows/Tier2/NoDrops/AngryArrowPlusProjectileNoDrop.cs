using OmoriMod.Content.Projectiles.Abstract_Classes;

namespace OmoriMod.Content.Projectiles.Friendly.Arrows.Tier2.NoDrops;

public class AngryArrowPlusProjectileNoDrop : AngryProjectile
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
        AI_AngryHeatSeekingProjectile();
    }
}