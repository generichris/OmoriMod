using System;

using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Systems.State_Management.Behaviour_Info;

using Terraria;

namespace OmoriMod.Systems.State_Management.NPCs.NPC_Behaviour;

public static class NPCBehvaiourHelperMethods
{
    public static void FindFrameViaSpeedPercentage(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo, int frameHeight, float maxSpeed, int minFrameTime)
    {
        NPC n = npc.NPC;
        n.spriteDirection = n.direction;

        n.frameCounter++;

        float npcSafeSpeed = Math.Clamp(n.velocity.Length(), 0, maxSpeed);
        float npcSpeedPercentage = npcSafeSpeed / maxSpeed;

        if (npcSpeedPercentage > 0f)
        {
            // Compute frame time: at 100% speed = minFrameTime, at low speed = large number
            // Add +1 to avoid division by zero at very low speeds
            int dynamicFrameTime = (int)Math.Max(minFrameTime / npcSpeedPercentage, minFrameTime);

            if (n.frameCounter % dynamicFrameTime == 0)
            {
                behaviourInfo++;
            }
        }
        n.frame.Y = behaviourInfo.CurrentFrame * frameHeight;
    }

    /// <summary>
    /// Used to pause animation on the <paramref name="selectedFrame"/>.
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="behaviourInfo"></param>
    /// <param name="frameHeight"></param>
    /// <param name="selectedFrame"></param>
    public static void SingleFrame(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo, int frameHeight, int selectedFrame)
    {
        npc.NPC.spriteDirection = npc.NPC.direction;
        if (behaviourInfo.CurrentFrame != selectedFrame)
        {
            behaviourInfo.CurrentFrame = selectedFrame;
            npc.NPC.frame.Y = behaviourInfo.CurrentFrame * frameHeight;
        }
    }
}