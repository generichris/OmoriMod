using OmoriMod.Content.Projectiles.Abstract_Classes;

namespace OmoriMod.Content.Projectiles.Friendly.Arrows.Tier1.NoDrops;

public class AngryArrowProjectileNoDrop : AngryProjectile
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
}