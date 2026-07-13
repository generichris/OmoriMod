using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Ammo.Bullets.Regular.Tier1;
using OmoriMod.Content.Projectiles.Friendly.Bullets.Tier1;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Ammo.Bullets.Unlimited.Tier1;

public class InfiniteAngryBullet : AngryItem
{
    InfiniteAngryBullet()
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
            ammoID: AmmoID.Bullet,
            projectileID: ModContent.ProjectileType<AngryBulletProjectile>(),
            shootSpeed: 20.5f
            );

        DamageDefaults(
            damageType: DamageClass.Ranged,
            damage: 12,
            knockback: 1f,
            crit: 4,
            noMelee: true
            );
    }

    public override void AddRecipes()
    {
        MakeEndlessAmmoRecipe(ModContent.ItemType<AngryBullet>());
    }
}