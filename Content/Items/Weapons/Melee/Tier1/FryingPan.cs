using System.Collections.Generic;

using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;

using Terraria.ID;

namespace OmoriMod.Content.Items.Weapons.Melee.Tier1;

public class FryingPan : HappyItem
{
    FryingPan()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        EmotionItemClone<Bat>();
    }

    public override void AddRecipes()
    {
        var recipes = new List<(int, int)> {
            (ItemID.CopperBar, 6),
            (ItemID.TinBar, 6)
        };
        MakeRegularRecipes(
            ingredients: recipes,
            craftingStationID: TileID.Anvils
            );
    }
}