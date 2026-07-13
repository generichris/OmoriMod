using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Weapons.Melee.Tier3;
using OmoriMod.Content.Projectiles.Friendly.Melee.Bat;
using OmoriMod.Systems.AbilitySystem.ItemAbilities.Registries;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Melee.Tier4;

public class HallowBat : AngryItem
{
    HallowBat()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        InnatePassiveAbilityID = (int)PassiveAbilityRegistry.PassiveAbilityID.QUINTUPLE_PHANTOM_BAT;
        ItemDefaults(
            width: 32,
            height: 32,
            scale: 1.5f,
            buyPrice: Item.buyPrice(0, 8, 0, 0),
            stackSize: 1,
            consumable: false
            );

        DamageDefaults(
            damageType: DamageClass.Melee,
            damage: 50,
            knockback: 6f,
            crit: 4,
            noMelee: false
            );

        ProjectileDefaults(
            ammoID: AmmoID.None,
            projectileID: ModContent.ProjectileType<BatProjectileFive>(),
            shootSpeed: 8f
            );

        AnimationDefaults(
            useTime: 14,
            useStyleID: ItemUseStyleID.Swing,
            useSound: SoundID.Item1,
            autoReuse: true
            );
    }



    public override void AddRecipes()
    {
        MakeUpgradeRecipe(
            baseItemID: ModContent.ItemType<HellBat>(),
            extraItemID: ItemID.HallowedBar,
            extraItemAmount: 20,
            craftingStationID: TileID.MythrilAnvil
            );
    }
}