using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Abstract_Classes;


/// <summary>
/// Stage 2 of <see cref="OmoriModProjectile"/> Contains helper methods
/// Goes into <see cref="OmoriModProjectile"/>.
/// Derives from <see cref="ConstructModProjectile"/>
/// </summary>
public abstract class HelperMethodModProjectile : ConstructModProjectile
{
    // ROTATION AND GRAVITY HELPER METHODS

    protected static float ARROW_GRAVITY => 0.1f;
    protected static float KNIFE_GRAVITY => 0.4f;

    protected static float TERMINAL => 16f;

    /// <summary>
    /// A helper method for gravity.
    /// </summary>
    /// <param name="grav"> The amount of gravity applied.</param>
    /// <param name="terminalVelocity">Maximum Y speed.</param>
    protected void TheGravityOfTheSituation(float grav, float terminalVelocity)
    {
        if (Projectile.velocity.Y > terminalVelocity)
            Projectile.velocity.Y = terminalVelocity;

        if (Projectile.velocity.Y < terminalVelocity)
            Projectile.velocity.Y += grav;
    }

    /// <summary>
    /// Rotates the Projectile by the velocity.
    /// If <paramref name="flip"/> is set to true, the Projectile will be flipped.
    /// </summary>
    /// <param name="flip">Whether the rotation should be flipped.</param>
    protected void VelocityRotate(bool flip = false)
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        if (flip) { Projectile.rotation += MathHelper.Pi; }
    }

    /// <summary>
    /// Rotates the Projectile by the velocity. Flips 90 degrees afterwards.
    /// If <paramref name="flip"/> is set to true, the Projectile will be flipped.
    /// </summary>
    /// <param name="flip">Whether the rotation should be flipped.</param>
    protected void VelocityRotateWith90(bool flip = false)
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        if (flip) { Projectile.rotation += MathHelper.Pi; }
        Projectile.rotation += MathHelper.PiOver2;
    }

    /// <summary>
    /// Rotated the Projectile based on its direction.
    /// </summary>
    /// <param name="rotation">The amount of rotation applied to the projectile.</param>
    protected void RotateBasedOnDirection(float rotation)
    {
        if (Projectile.direction > 0)
        {
            Projectile.rotation += rotation;
        }
        else
        {
            Projectile.rotation -= rotation;
        }
    }

    // ROTATION AND GRAVITY HELPER METHODS




    // SPLITTING HELPER METHODS

    /// <summary>
    /// Splits 1 projectile into many projectiles.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="ModProjectile"/> to be created.</typeparam>
    /// <param name="ProjectileAmount">The amount of projectiles to create.</param>
    /// <param name="damage">How much damage each created projectile does.</param>
    /// <param name="maxAngle">The maximum angle from the horizontal where projectiles may be created.</param>
    /// <param name="startingSpeed">The speed of each created projectile.</param>
    /// <param name="knockBack">The knockback of each created projectile.</param>
    /// <param name="kill">Whether or no the original projectile should be killed.</param>
    protected virtual void SetSplit<T>(int ProjectileAmount, int damage, int maxAngle, float startingSpeed, float knockBack, bool kill = true) where T : ModProjectile
    {
        // make sure to only run this code for the projectile owner
        if (Main.myPlayer == Projectile.owner)
        {
            HashSet<int> angles = new HashSet<int>();

            int updatedProjectileAmount = ProjectileAmount;

            if (ProjectileAmount % 2 == 1)
            {
                updatedProjectileAmount--;

                if (kill) { angles.Add(0); }

            }

            // divide by 2 to do 1 side
            float angleDistances = (float)(maxAngle / (updatedProjectileAmount / 2));
            int currentAngle = 0;

            for (int i = 0; i < updatedProjectileAmount / 2; i++)
            {
                currentAngle += (int)(angleDistances);
                angles.Add(currentAngle);
                angles.Add(-currentAngle);
            }

            foreach (int angleDegrees in angles)
            {
                Vector2 velocity = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
                float angle = MathHelper.ToRadians(angleDegrees);
                Matrix matrix = Matrix.CreateRotationZ(angle);

                velocity = Vector2.Transform(velocity, matrix);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<T>(), damage, knockBack, Projectile.owner);
            }

            if (kill) { Projectile.Kill(); }
        }
    }

    /// <summary>
    /// Helper method to spawn extra projectiles specifically for volley projectile AIs.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="damagePerProjectile"></param>
    /// <param name="ProjectileSpeedOnSpawn"></param>
    /// <param name="shotsPerVolley"></param>
    /// <param name="interval"></param>
    /// <param name="flipAngle"></param>
    protected void VolleyProjectileSpawning<T>(int damagePerProjectile, int ProjectileSpeedOnSpawn, int shotsPerVolley, int interval, float flipAngle) where T : ModProjectile
    {
        // make sure to only run this code for the projectile owner
        if (Main.myPlayer == Projectile.owner)
        {
            if (AI_Timer % interval == 0)
            {
                // only implement flip if a flip angle is specified and if it is time for a flip
                float currentFlipAngle = 0;
                if (flipAngle != 0 && AI_Timer % (interval * 2) == 0)
                {
                    currentFlipAngle = flipAngle;
                }

                Vector2[] spawnedProjectiles = GenerateCircleOfUnitVectors(shotsPerVolley, currentFlipAngle);
                foreach (Vector2 vector in spawnedProjectiles)
                {
                    // Normalize and scale
                    Vector2 velocity = vector * ProjectileSpeedOnSpawn;
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity,
                        ModContent.ProjectileType<T>(), damagePerProjectile, Projectile.knockBack, Projectile.owner);
                }
            }
        }
    }

    /// <summary>
    /// Helper method to generate a circle of unit vectors.
    /// </summary>
    /// <param name="count">The amount of vectors generated.</param>
    /// <param name="flipAngle">The angle that the vector is flipped by.</param>
    /// <returns>A list of the generated unit vectors.</returns>
    protected Vector2[] GenerateCircleOfUnitVectors(int count, float flipAngle = 0)
    {
        Vector2[] vecs = new Vector2[count];
        // Full circle divided by Projectile count
        float angleStep = MathHelper.TwoPi / count;

        // Choose to flip only if a flip angle is specified
        float startAngle = flipAngle == 0 ? 3 * MathHelper.PiOver2 : flipAngle;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + i * angleStep;
            // Unit vector on circle
            vecs[i] = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        return vecs;
    }

    // SPLITTING HELPER METHODS




    // ANIMATION HELPER METHODS

    /// <summary>
    /// Adds light of <paramref name="color"/> color to the position specified.
    /// </summary>
    /// <param name="position">The coordinates of the light</param>
    /// <param name="color">The <see cref="Color"/> of the light</param>
    protected static void AddLight(Vector2 position, Color color)
    {
        float r = color.R / 255f;
        float g = color.G / 255f;
        float b = color.B / 255f;

        Lighting.AddLight(position, r, g, b);
    }

    /// <summary>
    /// Statically set offsets for a shake animation.
    /// </summary>
    protected static readonly Vector2[] ShakeOffsets =
    {
        new(1, 0), new(0, 1), new(-1, 1), new(-1, -1), new(1, -1)
    };

    /// <summary>
    /// Helper method to shake the center of a projectile.
    /// </summary>
    protected void ShakeCenter()
    {
        Vector2 offset = ShakeOffsets[(int)AI_Timer % ShakeOffsets.Length];
        Projectile.Center += offset;
        Projectile.rotation += ((int)AI_Timer % 20) * 0.1f;
    }

    // ANIMATION HELPER METHODS

    /// <summary>
    /// Adjusts the projectile's velocity to weakly home in on a <paramref name="target"/>. 
    /// The homing effect is smoothed using <paramref name="homingResist"/>, allowing 
    /// the projectile to gradually curve toward the target rather than instantly snapping.
    /// If the <paramref name="target"/> is invalid or coincides with the projectile's position,
    /// the <paramref name="defaultVector"/> is used for normalization.
    /// </summary>
    /// <param name="target">The entity that the projectile will attempt to home in on.</param>
    /// <param name="speed">The constant speed that the projectile should maintain while homing.</param>
    /// <param name="homingResist">
    /// A resistance factor that controls how strongly the projectile adjusts its direction. 
    /// Higher values result in smoother, weaker homing. 
    /// (e.g. 20 = very gradual turn, 2 = very sharp turn)
    /// </param>
    /// <param name="defaultVector">
    /// A fallback vector used for normalization when the target direction cannot be calculated. 
    /// Defaults to <c>Vector2.Zero</c>.
    /// </param>
    protected void WeakHoming(Entity target, float speed, float homingResist, Vector2 defaultVector = new Vector2())
    {
        Vector2 desiredVelocity = (target.Center - Projectile.Center).SafeNormalize(defaultVector) * speed;
        Projectile.velocity = (Projectile.velocity * (homingResist - 1) + desiredVelocity) / homingResist;

        Projectile.velocity = Projectile.velocity.SafeNormalize(defaultVector) * speed;
    }


    // OTHER HELPER METHODS

    /// <summary>
    /// Returns true if the projectile is touching the ground.
    /// </summary>
    /// <returns></returns>
    public bool IsTouchingGround()
    {
        // The rectangle just below the projectile
        Rectangle projRect = Projectile.Hitbox;
        projRect.Y += 1; // move 1 pixel down

        return Collision.SolidTiles(projRect.Left, projRect.Top, projRect.Width, projRect.Height);
    }

    /// <summary>
    /// Returns true if the projectile is motionless
    /// </summary>
    /// <returns></returns>
    public bool IsStopped()
    {
        return Math.Abs(Projectile.velocity.X) < 0.01f && Math.Abs(Projectile.velocity.Y) < 0.01f;
    }

    /// <summary>
    /// Returns true if the projectile is at rest on the ground
    /// </summary>
    /// <returns></returns>
    public bool IsAtRest()
    {
        return IsTouchingGround() && IsStopped();
    }

    /// <summary>
    /// Slows a Projectile down until it gets below the <paramref name="zeroThreshold"/>. Then the speed gets set to 0.
    /// </summary>
    /// <param name="slowPercentage">The percentage of speed that should be retained.</param>
    /// <param name="zeroThreshold">The speed in which triggers the velocity to be set to 0.</param>
    protected void SlowProjectile(float slowPercentage, float zeroThreshold = 0)
    {
        SlowProjectileX(slowPercentage, zeroThreshold);
        SlowProjectileY(slowPercentage, zeroThreshold);
    }

    /// <summary>
    /// Slows a Projectile's X speed down until it gets below the <paramref name="zeroThreshold"/>. Then the speed gets set to 0.
    /// </summary>
    /// <param name="slowPercentage">The percentage of speed that should be retained.</param>
    /// <param name="zeroThreshold">The speed in which triggers the velocity to be set to 0.</param>
    protected void SlowProjectileX(float slowPercentage, float zeroThreshold = 0)
    {

        if (Math.Abs(Projectile.velocity.Length()) > zeroThreshold)
        {
            Projectile.velocity.X *= slowPercentage;
        }
        else
        {
            Projectile.velocity.X = 0;
        }
    }

    /// <summary>
    /// Slows a Projectile's Y speed down until it gets below the <paramref name="zeroThreshold"/>. Then the speed gets set to 0.
    /// </summary>
    /// <param name="slowPercentage">The percentage of speed that should be retained.</param>
    /// <param name="zeroThreshold">The speed in which triggers the velocity to be set to 0.</param>
    protected void SlowProjectileY(float slowPercentage, float zeroThreshold = 0)
    {

        if (Math.Abs(Projectile.velocity.Length()) > zeroThreshold)
        {
            Projectile.velocity.Y *= slowPercentage;
        }
        else
        {
            Projectile.velocity.Y = 0;
        }
    }

    protected void AirResistance(float gravity, float terminalVelocity, float slowPercentage, float zeroThreshold = 0)
    {
        SlowProjectileX(slowPercentage, zeroThreshold);
        TheGravityOfTheSituation(gravity, terminalVelocity);
    }

    /// <summary>
    /// A helper method that finds the closest Player within the <paramref name="maxDetectDistance"/>. If no Player is found, null is returned.
    /// If <paramref name="maxDetectDistance"/> is not specified, the closest Player (regardless of distance) will be returned
    /// </summary>
    /// <param name="maxDetectDistance">The maximum distance that a Player can be from the projectile to be returned by this algorithm.
    /// Must be above 0</param>
    /// <returns></returns>
    protected Player FindClosestPlayer(float maxDetectDistance = -1)
    {
        Player target = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
        return (target.Center - Projectile.Center).Length() > maxDetectDistance && maxDetectDistance > 0 ? null : target;
    }

    /// <summary>
    /// A helper method that finds the closest NPC within the <paramref name="maxDetectDistance"/>. If no NPC is found, null is returned
    /// </summary>
    /// <param name="maxDetectDistance">The maximum distance that an NPC can be from the player to be returned by this algorithm.</param>
    /// <returns></returns>
    protected NPC FindClosestNPC(float maxDetectDistance)
    {
        NPC closestNPC = null;
        // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
        float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

        // Loop through all NPCs(max always 200)
        for (int k = 0; k < Main.maxNPCs; k++)
        {
            NPC target = Main.npc[k];
            // Check if NPC able to be targeted. It means that NPC is
            // 1. active (alive)
            // 2. chaseable (e.g. not a cultist archer)
            // 3. max life bigger than 5 (e.g. not a critter)
            // 4. can take damage (e.g. moonlord core after all it's parts are downed)
            // 5. hostile (!friendly)
            // 6. not immortal (e.g. not a target dummy)
            if (target.CanBeChasedBy())
            {
                // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                // Check if it is within the radius
                if (sqrDistanceToTarget < sqrMaxDetectDistance)
                {
                    sqrMaxDetectDistance = sqrDistanceToTarget;
                    closestNPC = target;
                }
            }
        }

        return closestNPC;
    }

    // OTHER HELPER METHODS
}