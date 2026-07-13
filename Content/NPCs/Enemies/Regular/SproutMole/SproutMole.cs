using OmoriMod.Content.Items.BuffItems;
using OmoriMod.Content.Items.Health;
using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.NPCs.Enemies.Regular.SproutMole.Behaviours;
using OmoriMod.Content.NPCs.General_Behaviours.Actives;
using OmoriMod.Content.NPCs.General_Behaviours.Backgrounds;
using OmoriMod.Systems.State_Management.NPCs;

using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace OmoriMod.Content.NPCs.Enemies.Regular.SproutMole;

public class SproutMole : OmoriBehaviourNPC
{
    private const int _frames = 8;

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = _frames;
    }

    public override void SetDefaults()
    {
        NPC.width = 38;
        NPC.height = 64;
        NPC.scale = 0.7f;
        NPC.lifeMax = 40;

        NPC.damage = 10;
        NPC.defense = 4;

        NPC.HitSound = SoundID.NPCHit7;
        NPC.DeathSound = SoundID.NPCDeath9;

        NPC.value = 10f;
        NPC.knockBackResist = 0.8f;
        NPC.aiStyle = -1;
        NPC.netUpdate = true;

        behaviourManager = new NPCBehaviourManager(this, _frames);
        behaviourManager.AddBehaviour(new IdleWander(1));
        behaviourManager.AddBehaviour(new SuprisedJump(2));
        behaviourManager.AddBehaviour(new SproutMoleChaseBehaviour(3, 0));
        behaviourManager.AddBehaviour(new SproutMoleJumpChaseBehaviour(2));

        behaviourManager.AddBackgroundBehaviour(new FaceMovementDirection());
        behaviourManager.AddBackgroundBehaviour(new TargetClosestPlayer());
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tofu>(), 1));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AirHorn>(), 5));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PartyPopper>(), 5));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RainCloud>(), 5));

        LeadingConditionRule hardmodeRule = new(new Conditions.IsHardmode());
        hardmodeRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Firecracker>(), 20));
        npcLoot.Add(hardmodeRule);
    }


    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        float spawnModifier = .4f;
        // good chance of spawning if day on surface and underground / Caverns
        return SpawnCondition.OverworldDaySlime.Chance * spawnModifier +
            SpawnCondition.Underground.Chance * spawnModifier +
            SpawnCondition.Cavern.Chance * spawnModifier;
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