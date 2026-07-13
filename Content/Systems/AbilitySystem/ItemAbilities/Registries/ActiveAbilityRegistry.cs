using System.Collections.Generic;

namespace OmoriMod.Systems.AbilitySystem.ItemAbilities.Registries;

public static class ActiveAbilityRegistry
{
    private static readonly Dictionary<int, IItemAbility> _abilities = new Dictionary<int, IItemAbility>();

    // ID Enum
    public enum ActiveAbilityID : int
    {
        NONE = 0
    }

    public static void Initialize()
    {
        _abilities.Clear();

        // Register Abilities
    }

    public static void Register(int id, IItemAbility ability)
    {
        if (!_abilities.ContainsKey(id))
        {
            _abilities.Add(id, ability);
        }
    }

    public static IItemAbility GetAbility(int id)
    {
        if (_abilities.TryGetValue(id, out IItemAbility ability))
        {
            return ability;
        }
        return null;
    }

    public static IItemAbility GetAbility(ActiveAbilityID id)
    {
        return GetAbility((int)id);
    }
}