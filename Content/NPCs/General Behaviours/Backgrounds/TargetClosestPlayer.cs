using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.Systems.State_Management.Behaviour_Info;
using OmoriMod.Content.Systems.State_Management.NPCs.NPC_Behaviour;
using OmoriMod.Content.Util;

using Terraria;

namespace OmoriMod.Content.NPCs.General_Behaviours.Backgrounds;

/// <summary>
/// Targets the closest player every <paramref name="timer"/> seconds.
/// </summary>
/// <param name="timer">How long between <see cref="NPC.TargetClosest(bool)"/> calls. If <c>null</c>, then this gets called every tick<see cref=""/></param>
/// <param name="faceTarget">Whether the npc should face its target.</param>
public class TargetClosestPlayer(TickTimer timer = null, bool faceTarget = false) : NPCBackgroundBehaviour()
{
    private TickTimer _timer = timer;

    protected override void OnStart(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        npc.NPC.TargetClosest(faceTarget);
    }
    protected override void AI(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        if (_timer is null)
        {
            npc.NPC.TargetClosest(faceTarget);
        }
        else
        {
            // target new person if the time is done or if the npc no longer has a target for some reason
            if (_timer.IsDone || !npc.NPC.HasValidTarget)
            {
                npc.NPC.TargetClosest(faceTarget);
                _timer.Reset();
            }
            else
            {
                _timer--;
            }
        }
    }
}