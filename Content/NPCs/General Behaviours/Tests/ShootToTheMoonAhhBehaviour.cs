using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Systems.State_Management.Behaviour_Info;
using OmoriMod.Systems.State_Management.NPCs.NPC_Behaviour;

namespace OmoriMod.Content.NPCs.General_Behaviours.Tests;

/// <summary>
/// Fucking launches the NPC into orbit. For testing
/// </summary>
public class ShootToTheMoonAhhBehaviour() : NPCBehaviour(0)
{

    protected override void AI(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        npc.NPC.velocity.Y = -80f;
    }
}