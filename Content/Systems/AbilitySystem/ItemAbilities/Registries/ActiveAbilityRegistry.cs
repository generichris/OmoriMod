using System.Collections.Generic;

namespace OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.Registries;

public static class ActiveAbilityRegistry
{
    private static readonly Dictionary<int, IItemAbility> _abilities = [];

    // ID Enum
    public enum ActiveAbilityID : int
    {
        None = 0
    }

    public static void Initialize()
    {
        _abilities.Clear();

        // Register Abilities
    }

    public static void Unload()
    {
        _abilities.Clear();
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
        return _abilities.TryGetValue(id, out IItemAbility ability) ? ability : null;
    }

    public static IItemAbility GetAbility(ActiveAbilityID id)
    {
        return GetAbility((int)id);
    }
}