using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.BuffItems;
using OmoriMod.Content.Projectiles.Friendly.Magic.Tier1;

using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Magic.Tier1;

public class SadBolt : SadItem
{
    SadBolt()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        EmotionItemCloneWithDifferentProjectile<AngryBolt>(ModContent.ProjectileType<SadBoltProjectile>());
    }

    public override void AddRecipes()
    {
        MakeUpgradeRecipe(
            baseItemID: ItemID.Book,
            extraItemID: ModContent.ItemType<RainCloud>(),
            extraItemAmount: 10,
            craftingStationID: TileID.Bookcases
            );
    }
}