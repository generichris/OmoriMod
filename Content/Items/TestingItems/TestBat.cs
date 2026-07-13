using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Systems.AbilitySystem.ItemAbilities.Registries;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.TestingItems;

public class TestBat : AngryItem
{
    TestBat()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }

    public override void SetDefaults()
    {
        InnatePassiveAbilityID = (int)PassiveAbilityRegistry.PassiveAbilityID.QUINTUPLE_SEEKING_PHANTOM_BAT;

        ItemDefaults(
            width: 32,
            height: 32,
            scale: 1.5f,
            buyPrice: Item.buyPrice(0, 2, 0, 0),
            stackSize: 1,
            consumable: false
            );

        DamageDefaults(
            damageType: DamageClass.Melee,
            damage: 20,
            knockback: 6f,
            crit: 4,
            noMelee: false
            );

        ProjectileDefaults(
            ammoID: AmmoID.None,
            shootSpeed: 8f
            );

        AnimationDefaults(
            useTime: 17,
            useStyleID: ItemUseStyleID.Swing,
            useSound: SoundID.Item1,
            autoReuse: true
            );
    }
}