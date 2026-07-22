using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.Systems.State_Management.Behaviour_Info;
using OmoriMod.Content.Systems.State_Management.NPCs.NPC_Behaviour;
using OmoriMod.Content.Util;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.NPCs.Enemies.Bosses.YeOldSprout.Behaviours;

public class SpawnSproutMinions(TickTimer timeBetweenSpawnAttempts, int sproutsSpawned = 2, float initalSproutSpeed = 7f) : NPCBackgroundBehaviour()
{
    TickTimer timer = timeBetweenSpawnAttempts;
    protected override void OnStart(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        base.OnStart(npc, behaviourInfo);
        timer.Reset();
    }
    protected override void AI(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        NPC n = npc.NPC;
        if (timer.IsDone)
        {
            timer.Reset();
            if (Main.rand.NextBool(2))
            {
                foreach (var vec in npc.CreateVectors(sproutsSpawned, 60f))
                {
                    int sproutIndex = NPC.NewNPC(n.GetSource_FromAI(), (int)n.Center.X, (int)n.Center.Y, ModContent.NPCType<MiniMole>());
                    NPC sprout = Main.npc[sproutIndex];
                    sprout.velocity = vec * initalSproutSpeed;
                }
            }
        }
        else
        {
            timer--;
        }
    }
}