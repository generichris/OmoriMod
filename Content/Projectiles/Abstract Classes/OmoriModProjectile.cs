using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Abstract_Classes;

/// <summary>
/// Final stage of <see cref="OmoriModProjectile"/> Contains AI methods.
/// Derives from <see cref="HelperMethodModProjectile"/>.
/// </summary>
public abstract class OmoriModProjectile : HelperMethodModProjectile
{

    // AIS

    /// <summary>
    /// The AI for seeking scythe projectiles.
    /// </summary>
    /// <param name="ticksStationaryUntilDespawn">The amount of ticks that the scythe can be motionless before despawning.</param>
    /// <param name="rotation">The rotation per tick the scythe will rotate.</param>
    /// <param name="seekingDistance">The distance in which the scythe will seek out targets.</param>
    protected void AI_SeekingScytheProjectile(int ticksStationaryUntilDespawn, float rotation, int seekingDistance)
    {
        AI_Timer++;
        RotateBasedOnDirection(rotation: rotation);
        SlowProjectile(slowPercentage: 0.97f, zeroThreshold: 0.5f);

        float maxDetectRadius = seekingDistance; // The maximum radius at which a Projectile can detect a target
        float XSpeed = (float)Math.Pow(Projectile.velocity.X, 2);
        float YSpeed = (float)Math.Pow(Projectile.velocity.Y, 2);
        float ProjectileSpeed = (float)Math.Pow(XSpeed + YSpeed, .5);

        // Trying to find NPC closest to the Projectile
        NPC closestNPC = FindClosestNPC(maxDetectRadius);
        if (closestNPC == null)
            return;

        // If found, change the velocity of the Projectile and turn it in the direction of the target
        // Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero
        if (AI_Timer > 15)
        {
            Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * ProjectileSpeed;
        }


        if (Projectile.velocity == Vector2.Zero)
        {
            if (AI_Timer > ticksStationaryUntilDespawn)
            {
                AI_Timer = 0;
            }
            AI_Timer++;

            if (AI_Timer == ticksStationaryUntilDespawn)
            {
                Projectile.Kill();
            }
        }
    }

    /// <summary>
    /// The AI for scythe projectiles.
    /// </summary>
    /// <param name="ticksStationaryUntilDespawn">The amount of ticks that the scythe can be motionless before despawning.</param>
    /// <param name="rotation">The rotation per tick the scythe will rotate.</param>
    protected void AI_ScytheProjectile(int ticksStationaryUntilDespawn, float rotation)
    {
        RotateBasedOnDirection(rotation: rotation);
        SlowProjectile(slowPercentage: 0.95f, zeroThreshold: 0.5f);
        if (Projectile.velocity == Vector2.Zero)
        {
            AI_Timer++;

            if (AI_Timer > ticksStationaryUntilDespawn)
            {
                Projectile.Kill();
            }
        }
    }

    /// <summary>
    /// The AI for upgraded angry heat seeking projectiles.
    /// </summary>
    protected void AI_AngryHeatSeekingProjectile()
    {
        AI_Timer++;
        float maxDetectRadius = 500f; // The maximum radius at which a Projectile can detect a target
        float ProjectileSpeed = 20.5f; // The speed at which the Projectile moves towards the target

        // Trying to find NPC closest to the Projectile
        NPC closestNPC = FindClosestNPC(maxDetectRadius);
        if (closestNPC == null)
            return;

        // If found, change the velocity of the Projectile and turn it in the direction of the target
        // Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero
        if (AI_Timer > 15)
        {
            Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * ProjectileSpeed;
        }
    }

    /// <summary>
    /// The AI for projectiles that split multiple times.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="ModProjectile"/> to be created.</typeparam>
    /// <param name="maxAngle">The maximum angle from the horizontal where projectiles may be created.</param>
    /// <param name="ProjectileAmount">The amount of projectiles to create.</param>
    /// <param name="firstSplitDelay">The delay between the splitting starting.</param>
    /// <param name="splitTimer">The time between splits.</param>
    /// <param name="splits">How many times this projectile will split.</param>
    protected void AI_MultiSplittingProjectile<T>(int maxAngle, int ProjectileAmount, int firstSplitDelay = 10, int splitTimer = 10, int splits = 2) where T : ModProjectile
    {
        float newAITime = AI_Timer - firstSplitDelay;
        if (AI_Timer >= firstSplitDelay && newAITime <= splitTimer * splits)
        {
            if (newAITime % splitTimer == 0)
            {
                float speed = Projectile.velocity.Length();
                SetSplit<T>(ProjectileAmount, Projectile.damage, maxAngle, speed, Projectile.knockBack, false);
            }

        }
        AI_Timer++;
    }

    /// <summary>
    /// The AI for projctiles that split.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="ModProjectile"/> to be created.</typeparam>
    /// <param name="maxAngle">The maximum angle from the horizontal where projectiles may be created.</param>
    /// <param name="ProjectileAmount">The amount of projectiles to create.</param>
    protected void AI_SplittingProjectile<T>(int maxAngle, int ProjectileAmount) where T : ModProjectile
    {
        if (AI_Timer == 0)
        {
            float speed = Projectile.velocity.Length();
            SetSplit<T>(ProjectileAmount, Projectile.damage, maxAngle, speed, Projectile.knockBack);
        }
        AI_Timer++;
    }

    /// <summary>
    /// The AI for custom speed projectiles.
    /// </summary>
    /// <param name="totalSpeed">Maximum speed the projectile can go.</param>
    /// <param name="gravGiven">The amount of gravity the projectile can experience. Ignored if <paramref name="gravity"/> is False.</param>
    /// <param name="gravity">Whenther the projectile experiences gravity or not.</param>
    protected virtual void AI_SetSpeedProjectile(float totalSpeed, float gravGiven, bool gravity)
    {

        Projectile.velocity.Normalize();
        Projectile.velocity *= totalSpeed;

        VelocityRotate(flip: true);

        // 0.1f for normal arrow gravity, 0.4f for knife gravity
        // float arrowGrav = 0.1f;
        // float knifeGrav = 0.4f;
        // float defaultTerminal = 16f;

        if (gravity)
        {
            TheGravityOfTheSituation(gravGiven, totalSpeed);
        }
    }

    /// <summary>
    /// The AI for volley projectiles.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="damagePerProjectile"></param>
    /// <param name="ProjectileSpeedOnSpawn"></param>
    /// <param name="volleys"></param>
    /// <param name="shotsPerVolley"></param>
    /// <param name="interval"></param>
    /// <param name="bundle"></param>
    /// <param name="flipAngle"></param>
    private void VolleyProjectileAI<T>(int damagePerProjectile, int ProjectileSpeedOnSpawn, int volleys, int shotsPerVolley, int interval, bool bundle, float flipAngle = 0) where T : ModProjectile
    {
        AI_Timer++;
        // only shake if bundle AI
        if (bundle) { ShakeCenter(); }
        VolleyProjectileSpawning<T>(damagePerProjectile, ProjectileSpeedOnSpawn, shotsPerVolley, interval, flipAngle);
        // all volleys done
        if (AI_Timer > interval * volleys) { Projectile.Kill(); }
    }

    protected void AI_MagicBombProjectile<T>(int damagePerProjectile, int ProjectileSpeedOnSpawn, int volleys = 6, int shotsPerVolley = 8, int interval = 30) where T : ModProjectile
    {
        VolleyProjectileAI<T>(damagePerProjectile, ProjectileSpeedOnSpawn, volleys, shotsPerVolley, interval, bundle: false);
    }

    protected void AI_MagicBombProjectileWithFlip<T>(int damagePerProjectile, int ProjectileSpeedOnSpawn, int volleys = 6, int shotsPerVolley = 8, int interval = 30) where T : ModProjectile
    {
        VolleyProjectileAI<T>(damagePerProjectile, ProjectileSpeedOnSpawn, volleys, shotsPerVolley, interval, bundle: false, flipAngle: MathHelper.PiOver4);
    }
    protected void AI_BundledProjectile<T>(int damagePerProjectile, int ProjectileSpeedOnSpawn, int volleys = 4, int shotsPerVolley = 5, int interval = 60) where T : ModProjectile
    {
        if (AI_Timer == 0)
        {
            Projectile.Center = Main.MouseWorld;
            Projectile.velocity = Vector2.Zero;
            Projectile.netUpdate = true;
        }

        VolleyProjectileAI<T>(damagePerProjectile, ProjectileSpeedOnSpawn, volleys, shotsPerVolley, interval, bundle: true, flipAngle: MathHelper.PiOver2);
    }

    protected void AI_TravelingBundleProjectile<T>(int damagePerProjectile, int ProjectileSpeedOnSpawn, int volleys = 4, int shotsPerVolley = 5, int interval = 60) where T : ModProjectile
    {
        SlowProjectile(slowPercentage: 0.97f, zeroThreshold: 0.5f);
        if (Projectile.velocity == Vector2.Zero)
        {
            VolleyProjectileAI<T>(damagePerProjectile, ProjectileSpeedOnSpawn, volleys, shotsPerVolley, interval, bundle: true, flipAngle: MathHelper.PiOver2);
        }
    }

    // AIS
}