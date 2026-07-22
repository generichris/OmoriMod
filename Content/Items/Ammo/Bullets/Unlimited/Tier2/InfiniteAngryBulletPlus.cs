using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Items.Ammo.Bullets.Regular.Tier2;
using OmoriMod.Content.Projectiles.Friendly.Bullets.Tier2;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Ammo.Bullets.Unlimited.Tier2;

public class InfiniteAngryBulletPlus : AngryItem
{
    InfiniteAngryBulletPlus()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        ItemDefaults(
            width: 16,
            height: 16,
            scale: 1,
            buyPrice: Item.buyPrice(0, 15, 0, 0),
            stackSize: 1,
            consumable: false
            );

        ProjectileDefaults(
            ammoID: AmmoID.Bullet,
            projectileID: ModContent.ProjectileType<AngryBulletPlusProjectile>(),
            shootSpeed: 20.5f
            );

        DamageDefaults(
            damageType: DamageClass.Ranged,
            damage: 17,
            knockback: 1f,
            crit: 4,
            noMelee: true
            );
    }

    public override void AddRecipes()
    {
        MakeEndlessAmmoRecipe(ModContent.ItemType<AngryBulletPlus>());
    }
}