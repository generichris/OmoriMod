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
using OmoriMod.Content.Projectiles.NonFriendly.Regular.UFO;
using OmoriMod.Systems.State_Management.NPCs;

namespace OmoriMod.Content.NPCs.Enemies.Regular.UFO
{
    public class UFO : OmoriBehaviourNPC
    {
        private const int _frames = 2;

        public ref float AttackTimer => ref NPC.ai[0];

        private static bool _isSpawningHorde = false;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = _frames;
        }

        public override void SetDefaults()
        {
            NPC.width = 8;
            NPC.height = 14;
            NPC.scale = 2f;
            NPC.lifeMax = 10;
            NPC.damage = 10;
            NPC.defense = 2;

            AIType = NPCID.Gastropod;
            NPC.aiStyle = 122;
            NPC.noGravity = true;

            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath9;

            NPC.value = 10f;
            NPC.knockBackResist = 0.8f;
            NPC.netUpdate = true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OrangeJuice>(), 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AppleJuice>(), 2));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Sky.Chance * 0.4f;
        }

        public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
        {
            if (_isSpawningHorde)
            {
                return;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                _isSpawningHorde = true;

                try
                {
                    int hordeSize = Main.rand.Next(3, 6);

                    for (int i = 0; i < hordeSize; i++)
                    {
                        int spawnOffsetOffsetX = Main.rand.Next(-80, 81);
                        int spawnOffsetOffsetY = Main.rand.Next(-80, 81);

                        int packMemberIndex = NPC.NewNPC(
                            source,
                            (int)NPC.position.X + spawnOffsetOffsetX,
                            (int)NPC.position.Y + spawnOffsetOffsetY,
                            Type
                        );

                        if (packMemberIndex < Main.maxNPCs)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, number: packMemberIndex);
                        }
                    }
                }
                finally
                {
                    _isSpawningHorde = false;
                }
            }
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            NPC.spriteDirection = -NPC.direction;

            AttackTimer++;

            if (AttackTimer >= 90f)
            {
                AttackTimer = 0f;

                NPC.TargetClosest(true);
                Player target = Main.player[NPC.target];

                if (target.active && !target.dead && NPC.HasValidTarget)
                {
                    Vector2 shootDirection = target.Center - NPC.Center;
                    shootDirection.Normalize();

                    float shootSpeed = 7f;
                    Vector2 velocity = shootDirection * shootSpeed;

                    int type = ModContent.ProjectileType<UFOLaser>();
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
                            NPC.whoAmI
                        );

                        if (proj != Main.maxProjectiles)
                        {
                            Main.projectile[proj].friendly = false;
                            Main.projectile[proj].hostile = true;
                        }
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            if (NPC.frameCounter >= 12.0)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
                {
                    NPC.frame.Y = 0;
                }
            }
        }
    }
}
