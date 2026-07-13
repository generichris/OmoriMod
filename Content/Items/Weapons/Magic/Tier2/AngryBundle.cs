using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Weapons.Magic.Tier1;
using OmoriMod.Content.Projectiles.Friendly.Magic.Tier2;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Magic.Tier2;

public class AngryBundle : AngryItem
{
    AngryBundle()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        ItemDefaults(
            width: 32,
            height: 26,
            scale: 1f,
            buyPrice: Item.buyPrice(0, 3, 0, 0),
            stackSize: 1,
            consumable: false
            );

        DamageDefaults(
            damageType: DamageClass.Magic,
            damage: 32,
            knockback: 6f,
            crit: 4,
            noMelee: true,
            mana: 16
            );

        ProjectileDefaults(
            ammoID: AmmoID.None,
            projectileID: ModContent.ProjectileType<AngryBundleProjectile>(),
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
            baseItemID: ModContent.ItemType<AngryBolt>(),
            extraItemID: ItemID.HellstoneBar,
            extraItemAmount: 15,
            craftingStationID: TileID.Bookcases
            );
    }
}