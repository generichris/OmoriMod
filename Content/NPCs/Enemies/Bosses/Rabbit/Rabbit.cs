using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using OmoriMod.Content.Items.BossRelated.BossBags;
using OmoriMod.Content.Items.BossRelated.RabbitWeapons;
using OmoriMod.Content.Items.BossRelated.YeOldSproutWeapons;
using OmoriMod.Content.Items.Health;
using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.NPCs.Enemies.Bosses.YeOldSprout.Behaviours;
using OmoriMod.Content.NPCs.General_Behaviours.Backgrounds;
using OmoriMod.Systems;
using OmoriMod.Systems.State_Management.Behaviour_Info;
using OmoriMod.Systems.State_Management.NPCs;
using OmoriMod.Util;

namespace OmoriMod.Content.NPCs.Enemies.Bosses.Rabbit
{
    [AutoloadBossHead]

    public class Rabbit : OmoriBossEnemy
    {
        public Rabbit()
        {
            bossName = "Rabbit".OmoriModString();
        }
        private const int State_Jump = 0;
        private const int State_Land = 1;
        private const int State_Dash = 2;

        private ref float State => ref NPC.ai[0];
        private ref float Timer => ref NPC.ai[1];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaultsBossEnemy()
        {
            NPC.width = 144;
            NPC.height = 230;
            NPC.lifeMax = 3000;

            NPC.damage = 30;
            NPC.defense = 10;

            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.NPCDeath9;

            NPC.value = 60f;
            NPC.knockBackResist = 1f;
            NPC.aiStyle = -1;

        }

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];

            if (!target.active || target.dead)
            {
                NPC.velocity.Y += 0.3f;
                return;
            }

            Timer++;

            switch ((int)State)
            {
                case State_Jump:
                    if (Timer >= 40)
                    {
                        Vector2 toTarget = target.Center - NPC.Center;

                        NPC.velocity.Y = -18f;
                        NPC.velocity.X = MathHelper.Clamp(toTarget.X * 0.02f, -10f, 10f);

                        State = State_Land;
                        Timer = 0;
                    }
                    break;

                case State_Land:
                    if (NPC.velocity.Y == 0f && Timer > 5)
                    {
                        DoImpactDamage(target);
                        State = State_Dash;
                        Timer = 0;
                    }
                    break;

                case State_Dash:
                    if (Timer == 1)
                    {
                        Vector2 dashDir = target.Center - NPC.Center;
                        dashDir.Normalize();
                        NPC.velocity = dashDir * 18f;
                    }

                    if (Timer >= 20)
                    {
                        State = State_Jump;
                        Timer = 0;
                    }
                    break;
            }

            NPC.spriteDirection = -NPC.direction;
        }

        private void DoImpactDamage(Player target)
        {
            float impactRange = 200f;
            if (Vector2.Distance(NPC.Center, target.Center) <= impactRange)
            {
                target.Hurt(Terraria.DataStructures.PlayerDeathReason.ByNPC(NPC.whoAmI), 30, target.direction);
            }

            SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule notExpertRule = new(new Conditions.NotExpert());

            notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.LesserHealingPotion, 1, 5, 9));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Tofu>(), 1, 10, 25));
            notExpertRule.OnSuccess(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<Rabbit>()));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<RabbitGun>()));

            npcLoot.Add(notExpertRule);

            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<RabbitBossBag>()));
        }

        public override void OnKill()
        {
            DownedBossSystem.MarkDowned(bossName);
        }
    }
}
