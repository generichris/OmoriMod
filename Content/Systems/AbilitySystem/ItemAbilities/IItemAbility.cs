using OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.AbilityContexts;

using Terraria;

namespace OmoriMod.Content.Systems.AbilitySystem.ItemAbilities;

public interface IItemAbility
{
    public bool IsUnlocked(Item item, Player player);
    public bool IsEquippable(Item item, Player player);
    public bool PerformAbility(AbilityContext context);
}