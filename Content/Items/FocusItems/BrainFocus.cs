using OmoriMod.Content.DamageClasses;
using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.BuffItems;
using OmoriMod.Content.Projectiles.Friendly.FocusProjectiles;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.FocusItems;

public class BrainFocus : FocusItem
{
    BrainFocus()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        InitFocusItem(
            maxCharge: 7,
            dpsIncrease: 2,
            ticksUntilChargeStarts: 40,
            ticksUntilDecayStarts: 20,
            tickDecayRate: 2
            );

        ItemDefaults(
            width: 26,
            height: 32,
            scale: 1f,
            buyPrice: Item.buyPrice(platinum: 0, gold: 1, silver: 0, copper: 0),
            stackSize: 1,
            consumable: false
            );

        DamageDefaults(
            damageType: ModContent.GetInstance<FocusDamage>(),
            damage: 3,
            knockback: 6f,
            crit: 4,
            noMelee: true
            );

        ProjectileDefaults(
            ammoID: AmmoID.None,
            projectileID: ModContent.ProjectileType<BrainBolt>(),
            shootSpeed: 15f
            );

        AnimationDefaults(
            useTime: 7,
            useStyleID: ItemUseStyleID.Shoot,
            useSound: SoundID.Item1,
            autoReuse: true,
            noUseAnimation: true
            );

        SetItemRarity(ItemRarityID.Blue);
    }

    public override void AddRecipes()
    {
        MakeUpgradeRecipe(
            baseItemID: ItemID.Book,
            extraItemID: ModContent.ItemType<RainCloud>(),
            extraItemAmount: 10,
            craftingStationID: TileID.Bookcases
            );
    }
}