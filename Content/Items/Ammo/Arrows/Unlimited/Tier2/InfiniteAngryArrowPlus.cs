using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Items.Ammo.Arrows.Regular.Tier2;
using OmoriMod.Content.Projectiles.Friendly.Arrows.Tier2.NoDrops;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Ammo.Arrows.Unlimited.Tier2;

public class InfiniteAngryArrowPlus : AngryItem
{
    InfiniteAngryArrowPlus()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        ItemDefaults(
            width: 16,
            height: 16,
            scale: 1,
            buyPrice: Item.buyPrice(0, 5, 0, 0),
            stackSize: 1,
            consumable: false
            );

        ProjectileDefaults(
            ammoID: AmmoID.Arrow,
            projectileID: ModContent.ProjectileType<AngryArrowPlusProjectileNoDrop>(),
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
        MakeEndlessAmmoRecipe(ModContent.ItemType<AngryArrowPlus>());
    }
}