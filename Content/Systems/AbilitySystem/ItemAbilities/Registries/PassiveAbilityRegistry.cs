using System.Collections.Generic;

using OmoriMod.Content.Projectiles.Friendly.Melee.Bat;
using OmoriMod.Content.Projectiles.Friendly.Melee.Knife;
using OmoriMod.Content.Projectiles.Friendly.Melee.Pan;
using OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.Passives;

using Terraria.ModLoader;

namespace OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.Registries;

public static class PassiveAbilityRegistry
{
    private static readonly Dictionary<int, IItemAbility> _abilities = [];

    // ID Enum
    public enum PassiveAbilityID : int
    {
        None = 0,
        SINGLE_PHANTOM_BAT = 1,
        TRIPLE_PHANTOM_BAT = 2,
        QUINTUPLE_PHANTOM_BAT = 3,
        QUINTUPLE_SEEKING_PHANTOM_BAT = 4,

        SINGLE_PHANTOM_PAN = 5,
        TRIPLE_PHANTOM_PAN = 6,
        QUINTUPLE_PHANTOM_PAN = 7,
        QUINTUPLE_SEEKING_PHANTOM_PAN = 8,

        SINGLE_PHANTOM_KNIFE = 9,
        TRIPLE_PHANTOM_KNIFE = 10,
        QUINTUPLE_PHANTOM_KNIFE = 11,
        QUINTUPLE_SEEKING_PHANTOM_KNIFE = 12,
    }

    public static void Initialize()
    {
        _abilities.Clear();

        // Register Specific Abilities

        // Bat Abilities
        Register((int)PassiveAbilityID.SINGLE_PHANTOM_BAT, new ShootProjectilePassiveAbility(ModContent.ProjectileType<BatProjectile>()));
        Register((int)PassiveAbilityID.TRIPLE_PHANTOM_BAT, new ShootProjectilePassiveAbility(ModContent.ProjectileType<BatProjectileTriple>()));
        Register((int)PassiveAbilityID.QUINTUPLE_PHANTOM_BAT, new ShootProjectilePassiveAbility(ModContent.ProjectileType<BatProjectileFive>()));
        Register((int)PassiveAbilityID.QUINTUPLE_SEEKING_PHANTOM_BAT, new ShootProjectilePassiveAbility(ModContent.ProjectileType<BatProjectileFiveSeeking>()));

        // Pan Abilities
        Register((int)PassiveAbilityID.SINGLE_PHANTOM_PAN, new ShootProjectilePassiveAbility(ModContent.ProjectileType<PanProjectile>()));
        Register((int)PassiveAbilityID.TRIPLE_PHANTOM_PAN, new ShootProjectilePassiveAbility(ModContent.ProjectileType<PanProjectileTriple>()));
        Register((int)PassiveAbilityID.QUINTUPLE_PHANTOM_PAN, new ShootProjectilePassiveAbility(ModContent.ProjectileType<PanProjectileFive>()));
        Register((int)PassiveAbilityID.QUINTUPLE_SEEKING_PHANTOM_PAN, new ShootProjectilePassiveAbility(ModContent.ProjectileType<PanProjectileFiveSeeking>()));

        // Knife Abilities
        Register((int)PassiveAbilityID.SINGLE_PHANTOM_KNIFE, new ShootProjectilePassiveAbility(ModContent.ProjectileType<KnifeProjectile>()));
        Register((int)PassiveAbilityID.TRIPLE_PHANTOM_KNIFE, new ShootProjectilePassiveAbility(ModContent.ProjectileType<KnifeProjectileTriple>()));
        Register((int)PassiveAbilityID.QUINTUPLE_PHANTOM_KNIFE, new ShootProjectilePassiveAbility(ModContent.ProjectileType<KnifeProjectileFive>()));
        Register((int)PassiveAbilityID.QUINTUPLE_SEEKING_PHANTOM_KNIFE, new ShootProjectilePassiveAbility(ModContent.ProjectileType<KnifeProjectileFiveSeeking>()));
    }

    public static void Register(int id, IItemAbility ability)
    {
        _abilities.TryAdd(id, ability);
    }

    public static IItemAbility GetAbility(int id)
    {
        return _abilities.TryGetValue(id, out IItemAbility ability) ? ability : null;
    }

    public static IItemAbility GetAbility(PassiveAbilityID id)
    {
        return GetAbility((int)id);
    }
}