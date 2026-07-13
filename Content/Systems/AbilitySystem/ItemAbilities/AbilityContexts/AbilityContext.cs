using Terraria;

namespace OmoriMod.Systems.AbilitySystem.ItemAbilities.AbilityContexts;

public class AbilityContext(Player player, Item item)
{
    public Player Player { get; set; } = player;
    public Item Item { get; set; } = item;
}