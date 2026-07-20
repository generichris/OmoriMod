using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Health;
using OmoriMod.Content.Projectiles.Friendly.Bullets;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Ammo.Bullets.Regular;

public class Leaf : OmoriModItem
{
    Leaf()
    {
        itemTypeForResearch = ItemTypeForResearch.Ammo_Explosives;
    }
    public override void SetDefaults()
    {
        ItemDefaults(
            width: 16,
            height: 16,
            scale: 2f,
            buyPrice: Item.buyPrice(platinum: 0, gold: 0, silver: 0, copper: 3),
            stackSize: 9999,
            consumable: true
            );

        DamageDefaults(
            damageType: DamageClass.Ranged,
            damage: 3,
            knockback: 11f,
            crit: 4,
            noMelee: true
            );

        ProjectileDefaults(
            ammoID: Item.type,
            projectileID: ModContent.ProjectileType<LeafProjectile>(),
            shootSpeed: 16f
            );

        SetItemRarity(ItemRarityID.Purple);
    }
}