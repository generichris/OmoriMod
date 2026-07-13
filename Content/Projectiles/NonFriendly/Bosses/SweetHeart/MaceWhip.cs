using System;

using Microsoft.Xna.Framework;

using OmoriMod.Content.Projectiles.Abstract_Classes;

using Terraria;

namespace OmoriMod.Content.Projectiles.NonFriendly.Bosses.SweetHeart;

public class MaceWhip : OmoriModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 16;
        Projectile.height = 16;
        Projectile.hostile = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = true;
        Projectile.DefaultToWhip();
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        return false;
    }

    public void ThrowWhip(Entity owner, float initialSpeed)
    {
        // change to player
        NPC npc = FindClosestNPC(10000f);

        Vector2 velocity = (npc.Center - owner.Center).SafeNormalize(Vector2.Zero) * initialSpeed;
        Projectile.velocity = velocity;
    }
    public void SpinWhip(Entity owner, float swingSpeed, float swingRadius, float startingAngle)
    {
        Projectile.tileCollide = false;
        float angle = startingAngle + Projectile.timeLeft * swingSpeed;

        Projectile.Center = owner.Center + swingRadius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        Projectile.rotation = (Projectile.Center - owner.Center).ToRotation();
    }

    public bool RetractWhip(Entity owner, float initialSpeed, float Radius)
    {
        float distance = (owner.Center - Projectile.Center).Length();
        Vector2 distanceVector = owner.Center - Projectile.Center;
        Vector2 velocity = (owner.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * initialSpeed;
        // TODO: CHANGE TO A RANGE NEAR RADIUS AND AJUST DISTANCE AND DISTANCE VECTOR
        // TODO: change Projectile.center to be locked with owner.center + distance at angle before swing
        if (distance <= Radius)
        {
            return true;

        }
        else if (IsStopped())
        {
            Projectile.velocity = velocity;
            retractionCounter++;
        }
        else
        {
            if (retractionCounter < 3)
            {
                AirResistance(ARROW_GRAVITY, TERMINAL, 0.96f, 0.5f);
            }
            else
            {
                Projectile.tileCollide = false;
                Projectile.velocity = velocity;
            }
        }
        return false;
    }


    protected float phase
    {
        get => Projectile.ai[1];
        set => Projectile.ai[1] = value;
    }

    protected float retractionCounter
    {
        get => Projectile.ai[2];
        set => Projectile.ai[2] = value;
    }

    public override void AI()
    {
        // Entity owner = Main.npc[(int)Projectile.ai[0]]; // pass Sweetheart’s whoAmI as ai[0]

        Entity owner = Main.LocalPlayer;

        float initialSpeed = 8f;
        float swingSpeed = .01f;
        float swingRadius = 120f;
        float initialAngle = 0f;

        if (!owner.active)
        {
            Projectile.Kill();
            return;
        }

        if (AI_Timer == 30 && phase == 0f)
        {
            ThrowWhip(owner, initialSpeed);
            phase = 1f;
        }
        else if (IsStopped() && phase == 1f)
        {
            phase = 2f;
        }
        else if (phase == 2f)
        {
            if (RetractWhip(owner, initialSpeed, swingRadius))
            {
                phase = 3f;
                Projectile.velocity = Vector2.Zero;
                AI_Timer = 0;
            }
        }
        else if (AI_Timer >= 60 * 3 + 5 && phase == 3f)
        {
            Projectile.Kill();
        }
        else if (AI_Timer >= 5 && phase == 3f)
        {
            if (AI_Timer == 5)
            {
                initialAngle = (Projectile.Center - owner.Center).ToRotation();
            }
            SpinWhip(owner, swingSpeed, swingRadius, initialAngle);
        }
        else if (phase == 1f)
        {
            AirResistance(ARROW_GRAVITY, TERMINAL, 0.97f, 0.5f);
        }

        AI_Timer++;
    }
}