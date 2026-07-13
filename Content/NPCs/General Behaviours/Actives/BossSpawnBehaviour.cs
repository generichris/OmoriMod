using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Systems.State_Management.Behaviour_Info;
using OmoriMod.Systems.State_Management.NPCs.NPC_Behaviour;

namespace OmoriMod.Content.NPCs.General_Behaviours.Actives;

public class BossSpawnBehaviour(int exitSpawnStatus) : NPCBehaviour(exitSpawnStatus)
{

    protected override void OnStart(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        base.OnStart(npc, behaviourInfo);
    }

    /// <summary>
    /// TODO: Create extra spawning mechanics for bosses
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="behaviourInfo"></param>
    protected override void AI(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        behaviourInfo.ExitStatus = _defaultExitStatus;
    }

    protected override void FindFrame(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo, int frameHeight)
    {
        base.FindFrame(npc, behaviourInfo, frameHeight);
    }
}