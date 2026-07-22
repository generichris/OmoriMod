using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Items.BuffItems;
using OmoriMod.Content.Projectiles.Friendly.Arrows.Tier1.CanDrop;

using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Ammo.Arrows.Regular.Tier1;

public class SadArrow : SadItem
{
    SadArrow()
    {
        itemTypeForResearch = ItemTypeForResearch.Ammo_Explosives;
    }
    public override void SetDefaults()
    {
        EmotionItemCloneWithDifferentProjectile<AngryArrow>(ModContent.ProjectileType<SadArrowProjectile>());
    }

    public override void AddRecipes()
    {
        // Create recipes
        MakeAmmoRecipes(
            resultAmount: 50,

            baseIngredientID: ModContent.ItemType<RainCloud>(),
            baseAmount: 1,

            nonEndlessIngredientID: ItemID.WoodenArrow,
            nonEndlessAmount: 50,

            endlessIngredientID: ItemID.EndlessQuiver
            );
    }
}