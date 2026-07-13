using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Ammo.Arrows.Regular.Tier1;
using OmoriMod.Content.Items.Ammo.Arrows.Regular.Tier2;
using OmoriMod.Content.Projectiles.Friendly.Arrows.Tier2.NoDrops;

using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Ammo.Arrows.Unlimited.Tier2;

public class InfiniteHappyArrowPlus : HappyItem
{
    InfiniteHappyArrowPlus()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        EmotionItemCloneWithDifferentProjectile<InfiniteAngryArrowPlus>(ModContent.ProjectileType<HappyArrowPlusProjectileNoDrop>());
        Item.damage = ModContent.GetModItem(ModContent.ItemType<HappyArrow>()).Item.damage;
    }

    public override void AddRecipes()
    {
        MakeEndlessAmmoRecipe(ModContent.ItemType<HappyArrowPlus>());
    }
}