using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using OmoriMod.Content.Projectiles.Abstract_Classes;

namespace OmoriMod.Content.Projectiles.Friendly.SproutMole
{
    public class SproutMoleMissileProjectile : OmoriModProjectile
    {
        private const float FallSpeed = 14f;
        private const float HomingRange = 700f;
        private const float ExplosionRadius = 180f;
        private const int ExplosionDamage = 30;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 78;
            Projectile.scale = 0.2f; // source art is 270x450px, scale down to ~54x90

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            // Slight homing that only ever nudges the missile's horizontal aim - it never
            // pulls the missile back upward, so it can only "home" while it's falling.
            // Targets whichever enemy is closest to the player, not closest to the missile.
            NPC target = FindClosestNPCToPoint(owner.Center, HomingRange);
            if (target != null)
            {
                float direction = target.Center.X - Projectile.Center.X;
                float desiredXVelocity = MathHelper.Clamp(direction * 0.02f, -6f, 6f);
                Projectile.velocity.X += (desiredXVelocity - Projectile.velocity.X) / 40f;
            }

            // Always falls, never slows down or reverses upward
            if (Projectile.velocity.Y < FallSpeed)
            {
                Projectile.velocity.Y += 0.5f;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 150, default, 1f);
        }

        private static NPC FindClosestNPCToPoint(Vector2 point, float maxDetectDistance)
        {
            NPC closestNPC = null;
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.CanBeChasedBy())
                {
                    float sqrDistance = Vector2.DistanceSquared(npc.Center, point);

                    if (sqrDistance < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistance;
                        closestNPC = npc;
                    }
                }
            }

            return closestNPC;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Explode();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            Projectile.Kill();
            return false;
        }

        private void Explode()
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc.CanBeChasedBy() && Vector2.Distance(npc.Center, Projectile.Center) <= ExplosionRadius)
                    {
                        npc.SimpleStrikeNPC(ExplosionDamage, 0, false, Projectile.knockBack, Projectile.DamageType);
                    }
                }
            }

            for (int i = 0; i < 40; i++)
            {
                Vector2 dustVelocity = Main.rand.NextVector2Circular(6f, 6f);
                Dust.NewDust(Projectile.Center - new Vector2(ExplosionRadius / 2f), (int)ExplosionRadius, (int)ExplosionRadius, DustID.Smoke, dustVelocity.X, dustVelocity.Y);
            }

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
        }
    }
}