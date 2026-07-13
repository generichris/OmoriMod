using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OmoriMod.Content.Projectiles.Abstract_Classes;

namespace OmoriMod.Content.Summons.Summons.Projectiles
{
    public class GShrooberPoisonProjectile : OmoriModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 8;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.minion = false;
            Projectile.DamageType = DamageClass.Summon;

            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 200);
        }
    }
}