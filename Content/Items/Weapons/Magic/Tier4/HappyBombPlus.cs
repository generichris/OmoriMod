using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Items.Weapons.Magic.Tier3;
using OmoriMod.Content.Projectiles.Friendly.Magic.Tier4;

using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Magic.Tier4;

public class HappyBombPlus : HappyItem
{
    HappyBombPlus()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        EmotionItemCloneWithDifferentProjectile<AngryBombPlus>(ModContent.ProjectileType<HappyBombPlusProjectile>());
    }

    public override void AddRecipes()
    {
        MakeUpgradeRecipe(
            baseItemID: ModContent.ItemType<HappyBomb>(),
            extraItemID: ItemID.ChlorophyteBar,
            extraItemAmount: 25,
            craftingStationID: TileID.Bookcases
            );
    }
}