using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Items.Weapons.Melee.Tier2;
using OmoriMod.Content.Projectiles.Friendly.Melee.Bat;
using OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.Registries;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Melee.Tier3;

public class HellBat : AngryItem
{
    HellBat()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        InnatePassiveAbilityID = (int)PassiveAbilityRegistry.PassiveAbilityID.TRIPLE_PHANTOM_BAT;
        ItemDefaults(
            width: 32,
            height: 32,
            scale: 1.5f,
            buyPrice: Item.buyPrice(0, 4, 0, 0),
            stackSize: 1,
            consumable: false
            );

        DamageDefaults(
            damageType: DamageClass.Melee,
            damage: 35,
            knockback: 6f,
            crit: 4,
            noMelee: false
            );

        ProjectileDefaults(
            ammoID: AmmoID.None,
            projectileID: ModContent.ProjectileType<BatProjectileTriple>(),
            shootSpeed: 8f
            );

        AnimationDefaults(
            useTime: 15,
            useStyleID: ItemUseStyleID.Swing,
            useSound: SoundID.Item1,
            autoReuse: true
            );
    }



    public override void AddRecipes()
    {
        MakeUpgradeRecipe(
            baseItemID: ModContent.ItemType<CorruptionBat>(),
            extraItemID: ItemID.HellstoneBar,
            extraItemAmount: 15,
            craftingStationID: TileID.Anvils
            );
        MakeUpgradeRecipe(
            baseItemID: ModContent.ItemType<CrimsonBat>(),
            extraItemID: ItemID.HellstoneBar,
            extraItemAmount: 15,
            craftingStationID: TileID.Anvils
            );
    }
}