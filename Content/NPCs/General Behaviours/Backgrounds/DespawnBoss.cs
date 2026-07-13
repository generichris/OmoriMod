using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Systems.State_Management.Behaviour_Info;
using OmoriMod.Systems.State_Management.NPCs.NPC_Behaviour;

using Terraria;

namespace OmoriMod.Content.NPCs.General_Behaviours.Backgrounds;

public class DespawnBoss() : NPCBackgroundBehaviour()
{
    protected override void AI(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        NPC n = npc.NPC;
        if (!n.HasValidTarget)
        {
            n.TargetClosest(false);
            if (!n.HasValidTarget || Main.player[n.target].Distance(n.Center) > 3000)
            {
                n.noTileCollide = true;
                if (n.timeLeft > 60) { n.timeLeft = 60; }
            }
        }
    }
}