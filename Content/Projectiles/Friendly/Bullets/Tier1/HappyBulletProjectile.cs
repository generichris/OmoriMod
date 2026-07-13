using OmoriMod.Content.Projectiles.Abstract_Classes;

namespace OmoriMod.Content.Projectiles.Friendly.Bullets.Tier1;

public class HappyBulletProjectile : HappyProjectile
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
}