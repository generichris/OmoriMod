using OmoriMod.Content.Projectiles.Abstract_Classes;

namespace OmoriMod.Content.Projectiles.Friendly.Arrows.Tier2.NoDrops;

public class SadArrowPlusProjectileNoDrop : SadProjectile
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
        float speedDesired = 17f;
        float gravRequested = 0.11f;
        bool gravity = true;
        AI_SetSpeedProjectile(speedDesired, gravRequested, gravity);
    }
}