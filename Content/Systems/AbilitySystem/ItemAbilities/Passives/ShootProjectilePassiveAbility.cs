using System;

using Microsoft.Xna.Framework;

using OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.AbilityContexts;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.Passives;

public class ShootProjectilePassiveAbility : IItemPassiveAbility
{
    private int _projectileType;
    public int ProjectileType { get => _projectileType; set => _projectileType = value; }

    public ShootProjectilePassiveAbility(int projectileType)
    {
        ProjectileType = projectileType;
    }

    public bool IsUnlocked(Item item, Player player)
    {
        return false;
    }

    public bool IsEquippable(Item item, Player player)
    {
        return false;
    }

    private bool MoveProjectileForward(ref Vector2 position, ref Vector2 velocity, float ticks)
    {
        Projectile projectile = ProjectileLoader.GetProjectile(_projectileType).Projectile;

        int actingTicks = (int)MathF.Floor(ticks);
        Vector2 actingVelocity = velocity;

        while (ticks % 1 != 0)
        {
            actingTicks *= 10;
            ticks *= 10;
            actingVelocity /= 10;
        }

        for (int i = 0; i < actingTicks; i++)
        {
            // compute next canidate position
            Vector2 nextPos = position + actingVelocity;
            var hitbox = new Rectangle(
                (int)nextPos.X,
                (int)nextPos.Y,
                projectile.width,
                projectile.height
            );

            // If that spot collides with solid tiles, abort early
            if (Collision.SolidCollision(hitbox.TopLeft(), hitbox.Width, hitbox.Height))
            {
                return false;
            }

            position = nextPos;
        }

        return true;
    }

    /// <summary>
    /// Call inside of <see cref="ModItem.Shoot(Player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo, Vector2, Vector2, int, int, float)"/>.
    /// This method expects a signature like 
    /// <see cref="ModItem.ModifyShootStats(Player, ref Vector2, ref Vector2, ref int, ref int, ref float)"/> with an <see cref="Item"/> object at the end.
    /// </summary>
    /// <param name="context">
    /// The <see cref="AbilityContext"/> containing all necessary data.
    /// </param>
    /// <returns>false</returns>
    public bool PerformAbility(AbilityContext context)
    {
        if (context is not PassiveAbilityShootContext shootContext)
        {
            return false;
        }

        Player player = shootContext.Player;
        Vector2 position = shootContext.Position;
        Vector2 velocity = shootContext.Velocity;
        int type = ProjectileType;
        int damage = shootContext.Damage;
        float knockback = shootContext.Knockback;
        Item item = shootContext.Item;

        float ticksToMoveFoward = 0f;
        if (shootContext.TicksToMoveForward.HasValue) { ticksToMoveFoward = shootContext.TicksToMoveForward.Value; }

        MoveProjectileForward(ref position, ref velocity, ticksToMoveFoward);

        return ShootProjectile(player, ref position, ref velocity, ref type, ref damage, ref knockback, item);
    }
    private static bool ShootProjectile(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, Item item)
    {
        if (Main.myPlayer == player.whoAmI)
        {
            string context = "ShootProjectileActiveAbility:ShootProjectile";
            Projectile.NewProjectile(player.GetSource_ItemUse(item, context), position, velocity, type, damage, knockback, player.whoAmI);
        }
        return true;
    }
}