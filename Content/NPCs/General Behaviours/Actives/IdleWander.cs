using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.Systems.State_Management.Behaviour_Info;
using OmoriMod.Content.Systems.State_Management.NPCs.NPC_Behaviour;

using Terraria;

namespace OmoriMod.Content.NPCs.General_Behaviours.Actives;

public class IdleWander(int SurpriseIndex, float speed = 1f, int minFrameTime = 10) : NPCBehaviour(SurpriseIndex)
{
    protected override void FindFrame(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo, int frameHeight)
    {
        NPCBehvaiourHelperMethods.FindFrameViaSpeedPercentage(npc, behaviourInfo, frameHeight, speed, minFrameTime);
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
            bool leftOrRight = Main.rand.NextBool(2);
            n.direction = leftOrRight ? 1 : -1;
            n.netUpdate = true;
        }

        n.velocity.X = n.direction;

        if (n.collideX)
        {
            n.velocity.Y += -0.5f;
        }

        if (n.HasValidTarget && Main.player[n.target].Distance(n.Center) < 500f)
        {
            behaviourInfo.ExitStatus = _defaultExitStatus;
        }

        npc.AI_Timer++;
        if (npc.AI_Timer >= 120)
        {
            npc.AI_Timer = 0;
        }
    }
}