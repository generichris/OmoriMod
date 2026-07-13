using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Projectiles.Friendly.BossRelated.YeOldSprout;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.BossRelated.YeOldSproutWeapons;

public class SproutScythe : OmoriModItem
{
    SproutScythe()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        ItemDefaults(
            width: 32,
            height: 32,
            scale: 1,
            buyPrice: Item.buyPrice(platinum: 0, gold: 4, silver: 50, copper: 0),
            stackSize: 1,
            consumable: false
            );

        DamageDefaults(
            damageType: DamageClass.Magic,
            damage: 20,
            knockback: 1f,
            crit: 4,
            noMelee: true,
            mana: 10
            );

        ProjectileDefaults(
            ammoID: AmmoID.None,
            projectileID: ModContent.ProjectileType<SproutScytheProjectile>(),
            shootSpeed: 15f
            );

        AnimationDefaults(
            useTime: 25,
            useStyleID: ItemUseStyleID.Shoot,
            useSound: SoundID.Item1,
            autoReuse: true
            );

        SetItemRarity(ItemRarityID.Purple);
    }
}