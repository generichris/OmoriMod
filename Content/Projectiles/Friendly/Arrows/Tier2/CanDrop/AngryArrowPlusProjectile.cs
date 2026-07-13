using OmoriMod.Content.Items.Ammo.Arrows.Regular.Tier2;
using OmoriMod.Content.Projectiles.Abstract_Classes;

namespace OmoriMod.Content.Projectiles.Friendly.Arrows.Tier2.CanDrop;

public class AngryArrowPlusProjectile : AngryProjectile
{
    public override void SetDefaults()
    {
        SetArrowDefaults();
    }

    public override void OnKill(int timeLeft)
    {
        OnKillWithDrop<AngryArrowPlus>(timeLeft);
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