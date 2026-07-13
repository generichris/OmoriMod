using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Weapons.Magic.Tier2;
using OmoriMod.Content.Projectiles.Friendly.Magic.Tier3;

using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Magic.Tier3;

public class HappyBomb : HappyItem
{
    HappyBomb()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        EmotionItemCloneWithDifferentProjectile<AngryBomb>(ModContent.ProjectileType<HappyBombProjectile>());
    }

    public override void AddRecipes()
    {
        MakeUpgradeRecipe(
            baseItemID: ModContent.ItemType<HappyBundle>(),
            extraItemID: ItemID.HallowedBar,
            extraItemAmount: 20,
            craftingStationID: TileID.Bookcases
            );
    }
}