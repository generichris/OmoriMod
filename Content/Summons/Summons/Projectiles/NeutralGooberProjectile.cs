using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

using OmoriMod.Content.Summons.Abstract_Classes;
using OmoriMod.Content.Summons.Summons.Buffs;

namespace OmoriMod.Content.Summons.Summons.Projectiles
{
    public class NeutralGooberProjectile : ModSummonProjectile
    {
        private int ShootTimer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        private const int ShootCooldown = 50;
        private const float ShootRange = 500f;

        public override void SetStaticDefaults()
        {
            SetHomingMinionStaticDefaults();
            Main.projFrames[Projectile.type] = 13;
        }

        public sealed override void SetDefaults()
        {
            Projectile.width = 31;
            Projectile.height = 14;
            Projectile.tileCollide = false;
            Projectile.scale = 2f;

            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;

            Projectile.friendly = false; // this minion deals damage via its poison shots, not contact
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<NeutralGooberBuff>());
                return false;
            }

            if (owner.HasBuff(ModContent.BuffType<NeutralGooberBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (!CheckActive(owner))
            {
                return;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 8)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
            }
        }

        private void HoverMovement(float distanceToIdlePosition, Vector2 vectorToIdlePosition)
        {
            float speed = 4f;
            float inertia = 80f;

            if (distanceToIdlePosition > 600f)
            {
                speed = 12f;
                inertia = 60f;
            }

            if (distanceToIdlePosition > 20f)
            {
                vectorToIdlePosition.Normalize();
                vectorToIdlePosition *= speed;
                Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
            }
            else if (Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity.X = -0.15f;
                Projectile.velocity.Y = -0.05f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            var sourceRect = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);
            Vector2 origin = sourceRect.Size() / 2f;
            SpriteEffects effects = Projectile.velocity.X < 0f ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                sourceRect,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                effects,
                0
            );

            return false;
        }

        public override bool MinionContactDamage()
        {
            return false;
        }
    }
}