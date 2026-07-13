using OmoriMod.Content.Projectiles.Abstract_Classes;

namespace OmoriMod.Content.Projectiles.Friendly.Bullets.Tier2;

public class AngryBulletPlusProjectile : AngryProjectile
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
        AI_AngryHeatSeekingProjectile();
    }
}