using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Ammo.Bullets.Regular.Tier2;
using OmoriMod.Content.Projectiles.Friendly.Bullets.Tier2;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Ammo.Bullets.Unlimited.Tier2;

public class InfiniteSadBulletPlus : SadItem
{
    InfiniteSadBulletPlus()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        EmotionItemCloneWithDifferentProjectile<InfiniteAngryBulletPlus>(ModContent.ProjectileType<SadBulletPlusProjectile>());
        Item.damage = 50;
        Item.shootSpeed = 30f;
    }

    public override void AddRecipes()
    {
        MakeEndlessAmmoRecipe(ModContent.ItemType<SadBulletPlus>());
    }
}