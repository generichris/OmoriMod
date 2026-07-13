using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.BuffItems;
using OmoriMod.Content.Projectiles.Friendly.Magic.Tier1;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Magic.Tier1;

public class AngryBolt : AngryItem
{
    AngryBolt()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        ItemDefaults(
            width: 32,
            height: 26,
            scale: 1f,
            buyPrice: Item.buyPrice(0, 1, 50, 0),
            stackSize: 1,
            consumable: false
            );

        DamageDefaults(
            damageType: DamageClass.Magic,
            damage: 18,
            knockback: 6f,
            crit: 4,
            noMelee: true,
            mana: 8
            );

        ProjectileDefaults(
            ammoID: AmmoID.None,
            projectileID: ModContent.ProjectileType<AngryBoltProjectile>(),
            shootSpeed: 15f
            );

        AnimationDefaults(
            useTime: 20,
            useStyleID: ItemUseStyleID.Shoot,
            useSound: SoundID.Item1,
            autoReuse: true
            );
    }

    public override void AddRecipes()
    {
        MakeUpgradeRecipe(
            baseItemID: ItemID.Book,
            extraItemID: ModContent.ItemType<AirHorn>(),
            extraItemAmount: 10,
            craftingStationID: TileID.Bookcases
            );
    }
}