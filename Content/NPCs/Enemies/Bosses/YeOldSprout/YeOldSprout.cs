using OmoriMod.Content.Items.BossRelated.BossBags;
using OmoriMod.Content.Items.BossRelated.YeOldSproutWeapons;
using OmoriMod.Content.Items.Health;
using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.NPCs.Enemies.Bosses.YeOldSprout.Behaviours;
using OmoriMod.Content.NPCs.General_Behaviours.Backgrounds;
using OmoriMod.Content.Systems;
using OmoriMod.Content.Systems.State_Management.Behaviour_Info;
using OmoriMod.Content.Systems.State_Management.NPCs;
using OmoriMod.Content.Util;

using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.NPCs.Enemies.Bosses.YeOldSprout;

[AutoloadBossHead]

public class YeOldSprout : OmoriBossEnemy
{
    public YeOldSprout()
    {
        bossName = "YeOldSprout".OmoriModString();
    }

    private const int _frames = 24;
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = _frames;
    }
    public override void SetDefaultsBossEnemy()
    {
        NPC.width = 80;
        NPC.height = 80;
        NPC.lifeMax = 1000;

        NPC.damage = 15;
        NPC.defense = 10;

        NPC.HitSound = SoundID.NPCHit7;
        NPC.DeathSound = SoundID.NPCDeath9;

        NPC.value = 10f;
        NPC.knockBackResist = 0.25f;
        NPC.aiStyle = -1;

        behaviourManager = new NPCBehaviourManager(this, _frames);
        behaviourManager.AddBehaviour(new YeOldSproutChaseBehaviour(1));
        behaviourManager.AddBehaviour(new YeOldSproutJumpAttack(0));

        behaviourManager.AddBackgroundBehaviour(new FaceMovementDirection());
        behaviourManager.AddBackgroundBehaviour(new DespawnBoss());
        behaviourManager.AddBackgroundBehaviour(new TargetClosestPlayer(new TickTimer(seconds: 3, ticks: 0)));
        behaviourManager.AddBackgroundBehaviour(new SpawnSproutMinions(new TickTimer(seconds: 10, ticks: 0)));

        behaviourManager.AddAnimation("walking", new EntityAnimation(0, 9));
        behaviourManager.AddAnimation("jump up", new EntityAnimation(11, 15));
        behaviourManager.AddAnimation("jump down", new EntityAnimation(19, 23));
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {

        // Do NOT misuse the ModifyNPCLoot and OnKill hooks: the former is only used for registering drops, the latter for everything else

        // The order in which you add loot will appear as such in the Bestiary. To mirror vanilla boss order:
        // 1. Trophy
        // 2. Classic Mode ("not expert")
        // 3. Expert Mode (usually just the treasure bag)
        // 4. Master Mode (relic first, pet last, everything else in between)

        // How to drop things
        // npcLoot.Add(ItemDropRule.Common(itemId, chanceDenominator, minimumDropped, maximumDropped))

        // ItemDropRule.BossBag()
        // ItemDropRule.MasterModeCommonDrop()
        // ItemDropRule.MasterModeDropOnAllPlayers()


        // All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
        LeadingConditionRule notExpertRule = new(new Conditions.NotExpert());



        // Add all drops except for the bag. These drops only get added if the difficulty is not expert

        int[] weaponOptions = [
            ModContent.ItemType<SproutShotgun>(),
            ModContent.ItemType<SproutScythe>()
        ];
        notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, weaponOptions));


        notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.LesserHealingPotion, 1, 5, 9));
        notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Tofu>(), 1, 10, 25));
        notExpertRule.OnSuccess(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<YeOldSprout>()));

        // add non expert drops
        npcLoot.Add(notExpertRule);

        // add expert drops
        npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<YeOldBossBag>()));

    }


    public override void OnKill()
    {
        DownedBossSystem.MarkDowned(bossName);
    }

    public override void AI()
    {
        behaviourManager.PerformAIViaExitStatus();
    }

    public override void FindFrame(int frameHeight)
    {
        behaviourManager.PerformFindFrame(frameHeight);
    }
}