using Terraria;

namespace OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.AbilityContexts;

public class ActiveAbilityContext(Player player, Item item) : AbilityContext(player, item)
{
    // Add specific active ability properties here if needed.
    // For now, Player and Item are sufficient.
    // Maybe "IsChanneling"? or "Duration"?

}