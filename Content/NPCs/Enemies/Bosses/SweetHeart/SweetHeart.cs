using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.NPCs.General_Behaviours.Backgrounds;
using OmoriMod.Content.Systems;
using OmoriMod.Content.Systems.State_Management.NPCs;
using OmoriMod.Content.Util;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.NPCs.Enemies.Bosses.SweetHeart;

[AutoloadBossHead]
public class SweetHeart : OmoriBossEnemy
{

    private const int _frames = 1;
    public SweetHeart()
    {
        bossName = "SweetHeart".OmoriModString();
    }
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 1;
    }
    public override void SetDefaultsBossEnemy()
    {
        NPC.width = 40;
        NPC.height = 82;
        NPC.lifeMax = 10000;

        NPC.damage = 60;
        NPC.defense = 30;

        NPC.HitSound = SoundID.NPCHit7;
        NPC.DeathSound = SoundID.NPCDeath9;

        NPC.value = 10000f;
        NPC.knockBackResist = 0.05f;
        NPC.aiStyle = NPCAIStyleID.Slime;

        behaviourManager = new NPCBehaviourManager(this, _frames);

        behaviourManager.AddBackgroundBehaviour(new FaceMovementDirection());
        behaviourManager.AddBackgroundBehaviour(new DespawnBoss());
        behaviourManager.AddBackgroundBehaviour(new TargetClosestPlayer(new TickTimer(seconds: 3, ticks: 0)));
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {

    }


    public override void OnKill()
    {
        DownedBossSystem.MarkDowned(bossName);
    }
}