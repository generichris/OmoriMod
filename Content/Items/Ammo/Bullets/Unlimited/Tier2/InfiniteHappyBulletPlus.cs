using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Items.Ammo.Bullets.Regular.Tier2;
using OmoriMod.Content.Items.Ammo.Bullets.Unlimited.Tier1;
using OmoriMod.Content.Projectiles.Friendly.Bullets.Tier2;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Ammo.Bullets.Unlimited.Tier2;

public class InfiniteHappyBulletPlus : HappyItem
{
    InfiniteHappyBulletPlus()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        EmotionItemCloneWithDifferentProjectile<InfiniteAngryBulletPlus>(ModContent.ProjectileType<HappyBulletPlusProjectile>());
        Item.damage = ModContent.GetModItem(ModContent.ItemType<InfiniteHappyBullet>()).Item.damage;
    }

    public override void AddRecipes()
    {
        MakeEndlessAmmoRecipe(ModContent.ItemType<HappyBulletPlus>());
    }
}