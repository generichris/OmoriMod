using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.NPCs.General_Behaviours.Actives.Chase_Player;
using OmoriMod.Content.NPCs.General_Behaviours.Backgrounds;
using OmoriMod.Systems.State_Management.NPCs;

using Terraria;
using Terraria.ID;

namespace OmoriMod.Content.NPCs.Enemies.Bosses.YeOldSprout;

public class MiniMole : OmoriBehaviourNPC
{
    private const int _frames = 8;
    private const float _speed = 2f;
    private const float _inertia = 20f;
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = _frames;
    }
    public override void SetDefaults()
    {
        NPC.width = 36;
        NPC.height = 48;
        NPC.lifeMax = 30;
        NPC.scale = 0.7f;

        NPC.damage = 6;
        NPC.defense = 2;

        NPC.HitSound = SoundID.NPCHit7;
        NPC.DeathSound = SoundID.NPCDeath9;

        NPC.value = 0f;
        NPC.knockBackResist = 1f;
        NPC.aiStyle = -1;

        behaviourManager = new NPCBehaviourManager(this, _frames);
        behaviourManager.AddBehaviour(new ChasePlayer(1,
            speed: _speed,
            inertia: _inertia,
            minFrameTime: 4
            ));
        behaviourManager.AddBehaviour(new ChasePlayerJump(0,
            speed: _speed,
            inertia: _inertia
            ));

        behaviourManager.AddBackgroundBehaviour(new FaceMovementDirection());
        behaviourManager.AddBackgroundBehaviour(new TargetClosestPlayer());
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