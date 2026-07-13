using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Projectiles.Friendly.BossRelated.YeOldSprout;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.BossRelated.YeOldSproutWeapons;

public class SproutShotgun : OmoriModItem
{
    SproutShotgun()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        ItemDefaults(
            width: 32,
            height: 32,
            scale: 0.5f,
            buyPrice: Item.buyPrice(platinum: 0, gold: 4, silver: 50, copper: 0),
            stackSize: 1,
            consumable: false
            );

        DamageDefaults(
            damageType: DamageClass.Ranged,
            damage: 8,
            knockback: 6,
            crit: 4,
            noMelee: true
            );

        ProjectileDefaults(
            ammoID: AmmoID.None,
            shootSpeed: 8f,
            ammoUsedID: ModContent.ItemType<SproutBullet>()
            );

        AnimationDefaults(
            useTime: 20,
            useStyleID: ItemUseStyleID.Shoot,
            useSound: SoundID.Item1,
            autoReuse: true
            );

        SetItemRarity(ItemRarityID.Purple);
    }

    public override Vector2? HoldoutOffset()
    {
        var offset = new Vector2(-35, 0);
        return offset;
    }

    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        // arbitrary value here, just makes it look better when in the world
        scale = 0.4f;
        return true;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {

        int extraProjectiles = 4;
        int minAngle = -10;
        int maxAngle = 11;
        Vector2 startingVelocity = velocity;


        var rand = new Random();

        var randomAngles = new HashSet<int> { 0 };


        for (int i = 0; i < extraProjectiles; i++)
        {
            int randNumber = rand.Next(minAngle, maxAngle);
            while (!randomAngles.Add(randNumber))
            {
                randNumber = rand.Next(minAngle, maxAngle);
            }

        }

        foreach (int randomAngle in randomAngles)
        {
            if (randomAngle != 0)
            {
                Vector2 proj = startingVelocity;
                float angle = MathHelper.ToRadians(randomAngle);
                Matrix matrix = Matrix.CreateRotationZ(angle);

                proj = Vector2.Transform(proj, matrix);
                Projectile.NewProjectile(source, position, proj, ModContent.ProjectileType<SproutBulletProjectile>(), damage, knockback);
            }

        }

        return true;
    }


    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        MoveProjectileForward(ref position, ref velocity, ref type, 2.1f);
    }
}