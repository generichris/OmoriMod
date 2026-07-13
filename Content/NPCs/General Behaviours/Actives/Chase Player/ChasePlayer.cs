using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Systems.State_Management.Behaviour_Info;
using OmoriMod.Systems.State_Management.NPCs.NPC_Behaviour;

using Terraria;

namespace OmoriMod.Content.NPCs.General_Behaviours.Actives.Chase_Player;

public class ChasePlayer(int jumpIndex, float speed, float inertia, int minFrameTime = 5, int exitStatus = -1) : NPCBehaviour(exitStatus)
{
    protected override void FindFrame(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo, int frameHeight)
    {
        NPCBehvaiourHelperMethods.FindFrameViaSpeedPercentage(npc, behaviourInfo, frameHeight, speed, minFrameTime);
    }


    protected override void OnStart(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        npc.AI_Timer = 0;
    }

    /// <summary>
    /// Override to make this exit with a condition
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="behaviourInfo"></param>
    /// <returns></returns>
    protected virtual bool ExitCondition(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo) { return false; }

    protected override void AI(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        NPC n = npc.NPC;

        npc.MoveHorizontal(speed, inertia, npc.DirectionToTarget());
        npc.AI_Timer++;

        if (ExitCondition(npc, behaviourInfo))
        {
            behaviourInfo.ExitStatus = _defaultExitStatus;
            return;
        }
        if (n.collideX)
        {
            behaviourInfo.ExitStatus = jumpIndex;
            return;
        }
    }
}