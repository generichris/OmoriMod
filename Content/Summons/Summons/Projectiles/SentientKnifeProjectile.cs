using OmoriMod.Content.Summons.Abstract_Classes;
using OmoriMod.Systems.EmotionSystem;

using Terraria;

namespace OmoriMod.Content.Summons.Summons.Projectiles;

public class SentientKnifeProjectile : ModSummonProjectile
{
    public override void SetStaticDefaults()
    {
        SetHomingMinionStaticDefaults();
    }

    public sealed override void SetDefaults()
    {
        Projectile.width = 36;
        Projectile.height = 10;
        Projectile.tileCollide = false; // Makes the minion go through tiles freely

        Projectile.minionSlots = 1f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
        Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles

        SetContactMinionDefaults();
        SetEmotionType(EmotionType.SAD);
    }

    public override bool MinionContactDamage()
    {
        return true;
    }

    public override void AI()
    {
        FlyingContactMinionAI();
    }
}