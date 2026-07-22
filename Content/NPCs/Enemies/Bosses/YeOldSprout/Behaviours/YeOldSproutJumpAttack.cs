using Microsoft.Xna.Framework;

using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.Systems.State_Management.Behaviour_Info;
using OmoriMod.Content.Systems.State_Management.NPCs.NPC_Behaviour;
using OmoriMod.Content.Util;

using Terraria;

namespace OmoriMod.Content.NPCs.Enemies.Bosses.YeOldSprout.Behaviours;

public class YeOldSproutJumpAttack(int chaseBehaviourIndex, float speed = 3.5f, float inertia = 25f) : NPCBehaviour(chaseBehaviourIndex)
{
    TickTimer tickTimer = new TickTimer(seconds: 6, ticks: 0);
    bool falling;
    protected override void OnStart(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        npc.AI_Timer = 0;
        npc.NPC.frameCounter = 0;
        falling = false;
        tickTimer.Reset();
        behaviourInfo.SelectAnimation("jump up");
    }

    private bool CollideIn12Ticks(NPC npc)
    {
        Vector2 predictedPos = npc.position + (npc.velocity * 12);

        // Get NPC's hitbox at predicted position
        Rectangle predictedHitbox = new Rectangle(
            (int)predictedPos.X,
            (int)predictedPos.Y,
            npc.width,
            npc.height
        );

        // Check if this hitbox would collide with solid tiles
        return Collision.SolidCollision(predictedHitbox.TopLeft(), predictedHitbox.Width, predictedHitbox.Height);
    }

    protected override void AI(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo)
    {
        NPC n = npc.NPC;
        tickTimer--;

        if (n.velocity.Y > 0 && !falling && CollideIn12Ticks(n))
        {
            falling = true;
            n.frameCounter = 0;
        }
        else if (n.velocity.Y <= 0) { falling = false; }

        if (tickTimer.IsDone && n.collideY)
        {
            behaviourInfo.ExitStatus = _defaultExitStatus;
            return;
        }

        // jump
        if (npc.AI_Timer <= 0)
        {
            // jump
            n.velocity.Y += -20f;
            npc.AI_Timer = 15;
            n.frameCounter = 0;
        }
        else if (n.collideY)
        {
            npc.AI_Timer--;
        }

        tickTimer--;
        npc.MoveHorizontal(speed, inertia, npc.DirectionToTarget());
    }

    protected override void FindFrame(OmoriBehaviourNPC npc, BehaviourInfo behaviourInfo, int frameHeight)
    {
        if (falling)
        {
            behaviourInfo.SelectAnimation("jump down");
        }
        else
        {
            behaviourInfo.SelectAnimation("jump up");
        }

        NPC n = npc.NPC;
        n.spriteDirection = n.direction;


        int frameTime = 3;
        n.frameCounter++;

        if (n.frameCounter % (frameTime * 1) == 0 && n.frameCounter < (frameTime * 1) + 1)
        {
            behaviourInfo++;
        }
        if (n.frameCounter % (frameTime * 2) == 0 && n.frameCounter < (frameTime * 2) + 1)
        {
            behaviourInfo++;
        }
        if (n.frameCounter % (frameTime * 3) == 0 && n.frameCounter < (frameTime * 3) + 1)
        {
            behaviourInfo++;
        }

        n.frame.Y = behaviourInfo.CurrentFrame * frameHeight;
    }
}