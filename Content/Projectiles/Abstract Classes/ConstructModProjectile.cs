using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Projectiles.Abstract_Classes;

/// <summary>
/// Stage 1 of <see cref="OmoriModProjectile"/> Contains set methods and base method overrides.
/// Goes into <see cref="HelperMethodModProjectile"/>.
/// </summary>
public abstract class ConstructModProjectile : ModProjectile
{
    /// <summary>
    /// The first value in the <see cref="Projectile.ai"/> array. Standardized for timers.
    /// </summary>
    protected float AI_Timer
    {
        get => Projectile.ai[0];
        set => Projectile.ai[0] = value;
    }


    // DEFAULTS

    /// <summary>
    /// Set defaults for friendly projectiles.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="scale"></param>
    /// <param name="damageType"></param>
    /// <param name="penetration"></param>
    /// <param name="tileCollide"></param>
    /// <param name="timeLeft"></param>
    /// <param name="alpha"></param>
    private void FriendlyDefaults(int width, int height, float scale, DamageClass damageType, int penetration, bool tileCollide, int timeLeft, int alpha)
    {
        // Friendly so it hurts enemies
        Projectile.friendly = true;
        Projectile.hostile = false;

        // Dimensions
        Projectile.scale = scale;
        Projectile.width = (int)(width * Projectile.scale);
        Projectile.height = (int)(height * Projectile.scale);



        // Ranged damage
        Projectile.DamageType = damageType;
        Projectile.penetrate = penetration;

        Projectile.tileCollide = tileCollide;

        Projectile.timeLeft = timeLeft;

        Projectile.alpha = alpha;
    }

    /// <summary>
    /// Sets the defaults for friendly modded arrows
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="scale"></param>
    /// <param name="penetration"></param>
    /// <param name="tileCollide"></param>
    /// <param name="timeLeft"></param>
    /// <param name="alpha"></param>
    protected void SetArrowDefaults(int width = 8, int height = 12, float scale = 1, int penetration = 1, bool tileCollide = true, int timeLeft = 3600, int alpha = 0)
    {
        // Copy the ai style of an arrow
        Projectile.aiStyle = ProjAIStyleID.Arrow;

        // Is an arrow
        Projectile.arrow = true;

        // Ranged damage type
        DamageClass damageType = DamageClass.Ranged;


        FriendlyDefaults(width, height, scale, damageType, penetration, tileCollide, timeLeft, alpha);
    }

    /// <summary>
    /// Sets the defaults for friendly modded bullets
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="scale"></param>
    /// <param name="penetration"></param>
    /// <param name="tileCollide"></param>
    /// <param name="timeLeft"></param>
    /// <param name="alpha"></param>
    protected void SetBulletDefaults(int width = 6, int height = 6, float scale = 1, int penetration = 1, bool tileCollide = true, int timeLeft = 3600, int alpha = 0)
    {
        // Copy the ai style of a bullet
        Projectile.aiStyle = 0;

        // Is not an arrow
        Projectile.arrow = false;

        // Ranged damage type
        DamageClass damageType = DamageClass.Ranged;

        FriendlyDefaults(width, height, scale, damageType, penetration, tileCollide, timeLeft, alpha);
    }

    /// <summary>
    /// Set defaults for other friendly projectiles. Defaults to bullet AI.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="damageType"></param>
    /// <param name="aiStyle"></param>
    /// <param name="penetration"></param>
    /// <param name="scale"></param>
    /// <param name="tileCollide"></param>
    /// <param name="timeLeft"></param>
    /// <param name="alpha"></param>
    protected void SetOtherDefaults(int width, int height, DamageClass damageType, int aiStyle, int penetration = 1, float scale = 1, bool tileCollide = true, int timeLeft = 3600, int alpha = 0)
    {
        // Set AI style
        Projectile.aiStyle = aiStyle;

        // Is not an arrow
        Projectile.arrow = false;

        FriendlyDefaults(width, height, scale, damageType, penetration, tileCollide, timeLeft, alpha);
    }

    // DEFAULTS




    // DROP CHANCE

    /// <summary>
    /// Creates the <see cref="ModItem"/> corresponding with the <see cref="EmotionProjectile"/> with a certain chance.
    /// </summary>
    /// <typeparam name="T">The <see cref="ModItem"/> for the corresponding <see cref="EmotionProjectile"/></typeparam>
    /// <param name="chance">The chance for the <see cref="ModItem"/> to drop. <paramref name="chance"/> = 5 means a 1 in 5 chance.</param>
    private void DropChance<T>(int chance = 5) where T : ModItem
    {
        if (Projectile.owner == Main.myPlayer)
        {
            //has a chance to drop arrow for pickup
            int item = Main.rand.NextBool(chance) ? Item.NewItem(Entity.GetSource_Death(), Projectile.getRect(), ModContent.ItemType<T>()) : 0;
        }
    }

    /// <summary>
    /// The <see cref="ModProjectile.OnKill(int)"/> method for modded projectiles that can drop an item.
    /// </summary>
    /// <typeparam name="T">The <see cref="ModItem"/> for the corresponding <see cref="EmotionProjectile"/></typeparam>
    /// <param name="timeLeft">How much time a projectile has left.</param>
    /// <param name="noSound">If no sound should be played.</param>
    protected void OnKillWithDrop<T>(int timeLeft = 1, bool noSound = false) where T : ModItem
    {
        if (timeLeft > 0)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            if (!noSound) { SoundEngine.PlaySound(SoundID.Item10, Projectile.position); }
        }
        DropChance<T>();
    }

    /// <summary>
    /// The <see cref="ModProjectile.OnKill(int)"/> method for modded projectiles that can not drop an item.
    /// </summary>
    /// <param name="timeLeft">How much time a projectile has left.</param>
    /// <param name="noSound">If no sound should be played.</param>
    protected void OnKillNoDrop(int timeLeft = 1, bool noSound = false)
    {
        if (timeLeft > 0)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            if (!noSound) { SoundEngine.PlaySound(SoundID.Item10, Projectile.position); }
        }
    }

    // DROP CHANCE
}