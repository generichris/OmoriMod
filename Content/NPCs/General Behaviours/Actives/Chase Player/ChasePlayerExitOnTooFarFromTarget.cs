using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.Systems.State_Management.Behaviour_Info;
using OmoriMod.Content.Systems.State_Management.NPCs.NPC_Behaviour;

using Terraria;

namespace OmoriMod.Content.NPCs.General_Behaviours.Actives.Chase_Player;

public class ChasePlayerExitOnTooFarFromTarget(int jumpIndex, int exitStatus, float speed, float inertia, float maxDistance, int minFrameTime) : ChasePlayer(jumpIndex, speed, inertia, minFrameTime, exitStatus)
{
    private readonly float _maxDistance = maxDistance;
    private readonly int _minFrameTime = minFrameTime;
    private readonly float _speed = speed;

    protected override void FindFrame(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo, int frameHeight)
    {
        NPCBehvaiourHelperMethods.FindFrameViaSpeedPercentage(npc, behaviourInfo, frameHeight, _speed, _minFrameTime);
    }

    protected override void OnStart(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        npc.AI_Timer = 0;
    }

    protected override bool ExitCondition(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        return Main.player[npc.NPC.target].Distance(npc.NPC.Center) > _maxDistance;
    }
}