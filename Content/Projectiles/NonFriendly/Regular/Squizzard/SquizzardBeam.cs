using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OmoriMod.Content.Buffs.AngryBuff;
using OmoriMod.Content.Buffs.HappyBuff;
using OmoriMod.Content.Buffs.SadBuff;
using OmoriMod.Content.Projectiles.Abstract_Classes;

namespace OmoriMod.Content.Projectiles.NonFriendly.Regular.Squizzard
{
    public class SquizzardBeam : OmoriModProjectile
    {
        private bool hasHitTarget = false;
        private int postHitTimer = 0;

        float homingRange = 400f;
        float homingSpeed = 0.02f;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 0;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300; 
            Projectile.ignoreWater = true;  
            Projectile.tileCollide = true; 
        }

            public override void AI()
            {   

            int shooterIndex = (int)Projectile.ai[1];

            if (shooterIndex >= 0 && shooterIndex < Main.maxNPCs)
            {
                NPC shooter = Main.npc[shooterIndex];


                if (!shooter.active || (shooter.type != ModContent.NPCType<global::OmoriMod.Content.NPCs.Enemies.Regular.Squizzard.Squizzard>()))
                {
                    Projectile.Kill();
                    return;
                }
            }
            
            
            else
            {
                Projectile.Kill();
                return;
            }



            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(0f);

            Player targetPlayer = null;
            float closestDistance = homingRange;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player p = Main.player[i];
                if (p.active && !p.dead)
                {
                    float distance = Vector2.Distance(Projectile.Center, p.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        targetPlayer = p;
                    }
                }
            }

            if (targetPlayer != null)
            {
                Vector2 desiredVelocity = targetPlayer.Center - Projectile.Center;
                desiredVelocity.Normalize();
                desiredVelocity *= 8f;

                Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, homingSpeed);
            }

            //lighting 
            Lighting.AddLight(Projectile.Center, 1f, 0.0f, 0.0f);

            if (hasHitTarget)
            {
                postHitTimer++;
                if (postHitTimer >= 1000)
                {
                    postHitTimer = 0;
                    hasHitTarget = false;
                }
            }
        }


        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {

            int happyID = ModContent.BuffType<Happy>();
            int sadID = ModContent.BuffType<Sad>();
            int angryID = ModContent.BuffType<Angry>();

            if (target.HasBuff(happyID) || target.HasBuff(sadID) || target.HasBuff(angryID))
            {
                Projectile.Kill();
                return;
            }

            int[] potentialBuffs = new int[] { happyID, sadID, angryID };
            int randomIndex = Main.rand.Next(potentialBuffs.Length);
            int selectedBuff = potentialBuffs[randomIndex];

            target.AddBuff(selectedBuff, 1000);

            Projectile.Kill();
        }


    }
}
