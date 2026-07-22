using System.Collections.Generic;

using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;

using Terraria.ID;

namespace OmoriMod.Content.Items.Weapons.Melee.Tier1;

public class Knife : SadItem
{
    Knife()
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
            (ItemID.IronBar, 6),
            (ItemID.LeadBar, 6)
        };
        MakeRegularRecipes(
            ingredients: recipes,
            craftingStationID: TileID.Anvils
            );
    }
}