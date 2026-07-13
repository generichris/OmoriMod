using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using OmoriMod.Content.Items.Accessories;
using OmoriMod.Content.Items.BuffItems;
using OmoriMod.Content.Items.Health;
using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.NPCs.General_Behaviours.Actives;
using OmoriMod.Content.NPCs.General_Behaviours.Backgrounds;
using OmoriMod.Systems.State_Management.NPCs;

namespace OmoriMod.Content.NPCs.Enemies.Regular.ForestBunny
{
    public class ForestBunny : OmoriBehaviourNPC
    {
        int frameIndex = 0;
        int jumpTimer = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BlueSlime];
        }

        public override void SetDefaults()
        {
            NPC.width = 16;
            NPC.height = 16;
            NPC.scale = 2f;
            NPC.lifeMax = 20;

            NPC.damage = 6;
            NPC.defense = 4;

            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath9;

            NPC.value = 10f;
            NPC.knockBackResist = 0.8f;
            NPC.netUpdate = true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tofu>(), 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AirHorn>(), 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PartyPopper>(), 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RainCloud>(), 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RabbitsFoot>(), 5));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float spawnModifier = .4f;
            return SpawnCondition.OverworldDaySlime.Chance * spawnModifier +
                SpawnCondition.Underground.Chance * spawnModifier +
                SpawnCondition.Cavern.Chance * spawnModifier;
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            NPC.spriteDirection = NPC.direction;

            if (NPC.velocity.Y == 0f)
            {
                if (System.Math.Abs(NPC.velocity.X) > 0.01f)
                {
                    NPC.velocity.X *= 0.5f;
                }
                else
                {
                    NPC.velocity.X = 0f;
                }

                jumpTimer++;
                if (jumpTimer >= 240)
                {
                    jumpTimer = 0;
                    NPC.velocity.Y = -8.5f;
                    NPC.velocity.X = 4.5f * NPC.direction;
                    NPC.netUpdate = true;
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
