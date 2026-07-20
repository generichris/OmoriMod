using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Projectiles.Friendly.Bullets;
using OmoriMod.Content.Items.Ammo.Bullets.Regular;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Gun;

public class PlantationGun : OmoriModItem
{
    PlantationGun()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        ItemDefaults(
            width: 29,
            height: 13,
            scale: 1f,
            buyPrice: Item.buyPrice(platinum: 0, gold: 4, silver: 50, copper: 0),
            stackSize: 1,
            consumable: false
            );

        DamageDefaults(
            damageType: DamageClass.Ranged,
            damage: 11,
            knockback: 15,
            crit: 2,
            noMelee: true
            );

        ProjectileDefaults(
            ammoID: AmmoID.None,
            shootSpeed: 12f,
            ammoUsedID: ModContent.ItemType<Leaf>()
            );

        AnimationDefaults(
            useTime: 20,
            useStyleID: ItemUseStyleID.Shoot,
            useSound: SoundID.Item1,
            autoReuse: true
            );

        SetItemRarity(ItemRarityID.Purple);
    }

    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        // arbitrary value here, just makes it look better when in the world
        scale = 0.4f;
        return true;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {

        int extraProjectiles = 1;
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
                Projectile.NewProjectile(source, position, proj, ModContent.ProjectileType<LeafProjectile>(), damage, knockback);
            }

        }

        return true;
    }


    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        MoveProjectileForward(ref position, ref velocity, ref type, 2.1f);
    }
}