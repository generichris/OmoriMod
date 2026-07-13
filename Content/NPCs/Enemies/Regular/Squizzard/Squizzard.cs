using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using OmoriMod.Content.Items.BuffItems;
using OmoriMod.Content.Items.Health;
using OmoriMod.Content.Items.Mana;
using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.NPCs.General_Behaviours.Actives;
using OmoriMod.Content.NPCs.General_Behaviours.Backgrounds;
using OmoriMod.Systems.State_Management.NPCs;

namespace OmoriMod.Content.NPCs.Enemies.Regular.Squizzard
{
    public class Squizzard : OmoriBehaviourNPC
    {
        private float movementTimer = 0f;
        private float AttackTimer = 0f;
        private bool isPulser = false;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 23;
            NPC.height = 32;
            NPC.damage = 25;
            NPC.defense = 10;
            NPC.scale = 2f;

            NPC.lifeMax = 550;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;

            NPC.noGravity = true;

            NPC.aiStyle = -1;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            //change to 0.15 once the weeping willow has been added
            return SpawnCondition.Ocean.Chance * 0f;
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player target = Main.player[NPC.target];
            NPC.spriteDirection = NPC.direction;

            movementTimer++;

            if (!isPulser)
            {
                NPC.velocity.X *= 0.95f;
                NPC.velocity.Y += 0.05f;

                if (NPC.velocity.Y > 0.8f) NPC.velocity.Y = 0.8f;

                if (movementTimer >= 150f)
                {
                    movementTimer = 0f;
                    isPulser = true;
                }
            }
            else
            {
                if (movementTimer == 1f)
                {
                    Vector2 pulseDirection = target.Center - NPC.Center;
                    pulseDirection.Normalize();

                    float pulseSpeed = 11f;
                    NPC.velocity = pulseDirection * pulseSpeed;

                    NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
                }

                NPC.velocity *= 0.97f;

                if (movementTimer == 15f)
                {
                    if (target.active && !target.dead)
                    {
                        Vector2 shootDirection = target.Center - NPC.Center;
                        shootDirection.Normalize();

                        float shootSpeed = 7f;
                        Vector2 velocity = shootDirection * shootSpeed;

                        int type = ModContent.ProjectileType<global::OmoriMod.Content.Projectiles.NonFriendly.Regular.Squizzard.SquizzardBeam>();
                        int damage = 6;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int proj = Projectile.NewProjectile(
                                NPC.GetSource_FromAI(),
                                NPC.Center,
                                velocity,
                                type,
                                damage,
                                0f,
                                Main.myPlayer,
                                0f,
                                NPC.whoAmI //for the thing 
                            );

                            if (proj != Main.maxProjectiles)
                            {
                                Main.projectile[proj].friendly = false;
                                Main.projectile[proj].hostile = true;
                            }
                        }
                    }
                }

                if (movementTimer >= 60f)
                {
                    movementTimer = 0f;
                    isPulser = false;
                }
            }

            if (!NPC.wet)
            {
                NPC.velocity.Y += 0.2f;
                NPC.velocity.X *= 0.9f;
            }
        }
        
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            if (NPC.frameCounter >= 8) // speed
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= 4 * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

    }
}
