using OmoriMod.Content.Projectiles.Abstract_Classes;

using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Friendly.Melee.Bat;

public class BatProjectile : AngryProjectile
{
    public override void SetDefaults()
    {
        SetOtherDefaults(width: 32, height: 32, damageType: DamageClass.Melee, aiStyle: 0, penetration: 1, scale: 1, tileCollide: true, timeLeft: 300, alpha: 50);
    }

    public override void OnKill(int timeleft)
    {
        OnKillNoDrop(timeleft, noSound: true);
        MakeDust();
    }

    public override void AI()
    {
        AI_ScytheProjectile(ticksStationaryUntilDespawn: 60, rotation: 0.5f);
    }
}