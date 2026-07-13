using OmoriMod.Content.Projectiles.Abstract_Classes;

namespace OmoriMod.Content.Projectiles.Friendly.Bullets.Tier2;

public class SadBulletPlusProjectile : SadProjectile
{
    public override void SetDefaults()
    {
        SetBulletDefaults();
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
        float speedDesired = 20f;
        float gravRequested = 0f;
        bool gravity = false;
        AI_SetSpeedProjectile(speedDesired, gravRequested, gravity);
    }
}