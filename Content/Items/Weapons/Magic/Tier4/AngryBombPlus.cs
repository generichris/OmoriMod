using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Weapons.Magic.Tier3;
using OmoriMod.Content.Projectiles.Friendly.Magic.Tier4;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Magic.Tier4;

public class AngryBombPlus : AngryItem
{
    AngryBombPlus()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        ItemDefaults(
            width: 32,
            height: 26,
            scale: 1f,
            buyPrice: Item.buyPrice(0, 12, 0, 0),
            stackSize: 1,
            consumable: false
            );

        DamageDefaults(
            damageType: DamageClass.Magic,
            damage: 60,
            knockback: 6f,
            crit: 4,
            noMelee: true,
            mana: 48
            );

        ProjectileDefaults(
            ammoID: AmmoID.None,
            projectileID: ModContent.ProjectileType<AngryBombPlusProjectile>(),
            shootSpeed: 3f
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
            baseItemID: ModContent.ItemType<AngryBomb>(),
            extraItemID: ItemID.ChlorophyteBar,
            extraItemAmount: 25,
            craftingStationID: TileID.Bookcases
            );
    }
}