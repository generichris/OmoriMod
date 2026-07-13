using OmoriMod.Content.NPCs.General_Behaviours.Actives.Chase_Player;

namespace OmoriMod.Content.NPCs.Enemies.Regular.SproutMole.Behaviours;

public class SproutMoleChaseBehaviour(int jumpIndex, int exitStatus)
    : ChasePlayerExitOnTooFarFromTarget(
        jumpIndex: jumpIndex,
        exitStatus: exitStatus,
        speed: 2.5f,
        inertia: 20f,
        maxDistance: 800f,
        minFrameTime: 4
        )
{
}