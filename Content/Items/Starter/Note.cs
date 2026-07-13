using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;

using Terraria;
using Terraria.ID;

namespace OmoriMod.Content.Items.Starter;

public class Note : OmoriModItem
{
    Note()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        ItemDefaults(
            width: 32,
            height: 32,
            scale: 1f,
            buyPrice: Item.buyPrice(0, 0, 0, 0),
            stackSize: 1,
            consumable: false
            );

        SetItemRarity(ItemRarityID.Green);
    }

    public override bool CanRightClick()
    {
        return true;
    }

    public override bool ConsumeItem(Player player)
    {
        return false;
    }

    public override void RightClick(Player player)
    {
        Utils.OpenToURL("https://sites.google.com/view/omorimodwiki?usp=sharing");
    }

    public override void AddRecipes()
    {
        Recipe test = CreateRecipe();
        test.Register();
    }
}