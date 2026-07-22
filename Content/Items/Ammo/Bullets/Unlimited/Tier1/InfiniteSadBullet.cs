using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Items.Ammo.Bullets.Regular.Tier1;
using OmoriMod.Content.Projectiles.Friendly.Bullets.Tier1;

using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Ammo.Bullets.Unlimited.Tier1;

public class InfiniteSadBullet : SadItem
{
    InfiniteSadBullet()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        EmotionItemCloneWithDifferentProjectile<InfiniteAngryBullet>(ModContent.ProjectileType<SadBulletProjectile>());
    }

    public override void AddRecipes()
    {
        MakeEndlessAmmoRecipe(ModContent.ItemType<SadBullet>());
    }
}