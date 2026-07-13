using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Systems.State_Management.Behaviour_Info;
using OmoriMod.Systems.State_Management.NPCs.NPC_Behaviour;

using Terraria;

namespace OmoriMod.Content.NPCs.General_Behaviours.Actives;

public class SuprisedJump(int chaseIndex) : NPCBehaviour(chaseIndex)
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
            n.velocity.Y = -6f;
        }

        npc.AI_Timer++;

        if (n.collideY)
        {
            behaviourInfo.ExitStatus = _defaultExitStatus;
        }


    }
}