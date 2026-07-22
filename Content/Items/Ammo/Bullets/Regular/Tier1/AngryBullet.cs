using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Items.BuffItems;
using OmoriMod.Content.Projectiles.Friendly.Bullets.Tier1;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Ammo.Bullets.Regular.Tier1;

public class AngryBullet : AngryItem
{
    AngryBullet()
    {
        itemTypeForResearch = ItemTypeForResearch.Ammo_Explosives;
    }
    public override void SetDefaults()
    {
        ItemDefaults(
            width: 16,
            height: 16,
            scale: 1,
            buyPrice: Item.buyPrice(0, 0, 1, 0),
            stackSize: 9999,
            consumable: true
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
        // Create recipes
        MakeAmmoRecipes(
            resultAmount: 100,

            baseIngredientID: ModContent.ItemType<AirHorn>(),
            baseAmount: 1,

            nonEndlessIngredientID: ItemID.MusketBall,
            nonEndlessAmount: 100,

            endlessIngredientID: ItemID.EndlessMusketPouch
            );
    }
}