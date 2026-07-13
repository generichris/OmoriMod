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

namespace OmoriMod.Content.NPCs.Enemies.Bosses.Snaley.Stage_1;

[AutoloadBossHead]

public class Snaley : OmoriBossEnemy
{
    public Snaley()
    {
        bossName = "Snaley".OmoriModString();
    }

    private ref float State => ref NPC.ai[0];
    private ref float Timer => ref NPC.ai[1];

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 2;
    }

    public override void SetDefaultsBossEnemy()
    {
        NPC.width = 50;
        NPC.height = 58;
        NPC.lifeMax = 2500;

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

        NPC.spriteDirection = NPC.direction;
    }

    //public override void ModifyNPCLoot(NPCLoot npcLoot)
    //{
    //    LeadingConditionRule notExpertRule = new(new Conditions.NotExpert());

    //    notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.LesserHealingPotion, 1, 5, 9));
    //    notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Tofu>(), 1, 10, 25));
    //    notExpertRule.OnSuccess(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<Rabbit>()));
    //    notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<RabbitGun>()));

    //    npcLoot.Add(notExpertRule);

    //    npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<RabbitBossBag>()));
    //}

    public override void OnKill()
    {
        DownedBossSystem.MarkDowned(bossName);
    }
}
