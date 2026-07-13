using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;

namespace OmoriMod.Content.Items.BossRelated.RabbitWeapons
{
    public class RabbitGun : OmoriModItem
    {
        RabbitGun()
        {
            itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
        }
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42;
            Item.height = 26;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(gold: 10);
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.useAmmo = AmmoID.Bullet;
            SetItemRarity(ItemRarityID.Purple);
        }

        public override Vector2? HoldoutOffset()
        {
            var offset = new Vector2(-35, 0);
            return offset;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            scale = 0.4f;
            return true;
        }
    }
}
