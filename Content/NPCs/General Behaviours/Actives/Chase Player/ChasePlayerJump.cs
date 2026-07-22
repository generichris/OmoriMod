using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.Systems.State_Management.Behaviour_Info;
using OmoriMod.Content.Systems.State_Management.NPCs.NPC_Behaviour;

using Terraria;

namespace OmoriMod.Content.NPCs.General_Behaviours.Actives.Chase_Player;

public class ChasePlayerJump(int chaseIndex, float speed, float inertia) : NPCBehaviour(chaseIndex)
{
    protected override void FindFrame(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo, int frameHeight)
    {
        NPCBehvaiourHelperMethods.SingleFrame(npc, behaviourInfo, frameHeight, selectedFrame: 2);
    }

    protected override void OnStart(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        npc.AI_Timer = 0;
    }

    protected override void AI(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        NPC n = npc.NPC;
        if (npc.AI_Timer == 0)
        {
            n.velocity.Y += -10f;
        }

        npc.MoveHorizontal(speed, inertia, npc.DirectionToTarget());

        if (npc.AI_Timer > 3 && n.collideY)
        {
            behaviourInfo.ExitStatus = _defaultExitStatus;
        }
        npc.AI_Timer++;
    }
}