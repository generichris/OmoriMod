using System.Collections.Generic;

using OmoriMod.Content.Items.Starter;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Players;

public class OmoriStartingItemsPlayer : ModPlayer
{

    public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
    {
        return
        [
            new Item(ModContent.ItemType<Note>())
        ];
    }
}