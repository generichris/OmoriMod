using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Items.Ammo.Arrows.Regular.Tier1;
using OmoriMod.Content.Items.Ammo.Arrows.Unlimited.Tier1;
using OmoriMod.Content.Projectiles.Friendly.Arrows.Tier2.CanDrop;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Ammo.Arrows.Regular.Tier2;

public class SadArrowPlus : SadItem
{
    SadArrowPlus()
    {
        itemTypeForResearch = ItemTypeForResearch.Ammo_Explosives;
    }
    public override void SetDefaults()
    {
        EmotionItemCloneWithDifferentProjectile<AngryArrowPlus>(ModContent.ProjectileType<SadArrowPlusProjectile>());
        Item.damage = 50;
        Item.shootSpeed = 30f;
    }

    public override void AddRecipes()
    {
        // Create recipes
        MakeAmmoRecipes(
            resultAmount: 100,

            baseIngredientID: ItemID.HallowedBar,
            baseAmount: 1,

            nonEndlessIngredientID: ModContent.ItemType<SadArrow>(),
            nonEndlessAmount: 100,

            endlessIngredientID: ModContent.ItemType<InfiniteSadArrow>()
            );
    }
}