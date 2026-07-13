using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Ammo.Arrows.Regular.Tier1;
using OmoriMod.Content.Items.Ammo.Arrows.Unlimited.Tier1;
using OmoriMod.Content.Projectiles.Friendly.Arrows.Tier2.CanDrop;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Ammo.Arrows.Regular.Tier2;

public class AngryArrowPlus : AngryItem
{
    AngryArrowPlus()
    {
        itemTypeForResearch = ItemTypeForResearch.Ammo_Explosives;
    }
    public override void SetDefaults()
    {
        ItemDefaults(
            width: 16,
            height: 16,
            scale: 1,
            buyPrice: Item.buyPrice(0, 0, 5, 0),
            stackSize: 9999,
            consumable: true
            );

        ProjectileDefaults(
            ammoID: AmmoID.Arrow,
            projectileID: ModContent.ProjectileType<AngryArrowPlusProjectile>(),
            shootSpeed: 8.5f
            );

        DamageDefaults(
            damageType: DamageClass.Ranged,
            damage: 24,
            knockback: 1f,
            crit: 4,
            noMelee: true
            );
    }

    public override void AddRecipes()
    {
        // Create recipes
        MakeAmmoRecipes(
            resultAmount: 100,

            baseIngredientID: ItemID.HallowedBar,
            baseAmount: 1,

            nonEndlessIngredientID: ModContent.ItemType<AngryArrow>(),
            nonEndlessAmount: 100,

            endlessIngredientID: ModContent.ItemType<InfiniteAngryArrow>()
            );
    }
}