using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;

namespace OmoriMod.Systems.AbilitySystem.ItemAbilities.AbilityContexts;

public class PassiveAbilityShootContext(
    Player player,
    Item item,
    EntitySource_ItemUse_WithAmmo source,
    Vector2 position,
    Vector2 velocity,
    int type,
    int damage,
    float knockback,
    float? ticksToMoveForward = null
    ) : AbilityContext(player, item)
{
    public EntitySource_ItemUse_WithAmmo Source { get; set; } = source;
    public Vector2 Position { get; set; } = position;
    public Vector2 Velocity { get; set; } = velocity;
    public int Type { get; set; } = type;
    public int Damage { get; set; } = damage;
    public float Knockback { get; set; } = knockback;
    public float? TicksToMoveForward { get; set; } = ticksToMoveForward;
}