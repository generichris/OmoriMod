using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.NPCs.General_Behaviours.Actives.Chase_Player;
using OmoriMod.Systems.State_Management.Behaviour_Info;
using OmoriMod.Util;

using Terraria;

namespace OmoriMod.Content.NPCs.Enemies.Bosses.YeOldSprout.Behaviours;

public class YeOldSproutChaseBehaviour(int exitStatus)
    : ChasePlayerExitOnTimeOut(
        exitStatus: exitStatus,
        speed: 3.5f,
        inertia: 25f,
        new TickTimer(seconds: 6, ticks: 0)
        )
{
    protected override void OnStart(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        behaviourInfo.SelectAnimation("walking");
        base.OnStart(npc, behaviourInfo);
    }

    protected override void FindFrame(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo, int frameHeight)
    {
        NPC n = npc.NPC;
        n.spriteDirection = n.direction;

        n.frameCounter++;
        if (n.frameCounter % 7 == 0)
        {
            behaviourInfo++;
        }
        n.frame.Y = behaviourInfo.CurrentFrame * frameHeight;
    }
}