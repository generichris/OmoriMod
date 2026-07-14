using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Health;
using OmoriMod.Content.Projectiles.Friendly.SproutMole;

namespace OmoriMod.Content.Items.Weapons.SproutMole
{
    public class SproutMoleMissileRemote : OmoriModItem
    {
        SproutMoleMissileRemote()
        {
            itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
        }

        public override void SetDefaults()
        {
            Item.width = 33;
            Item.height = 43;
            Item.scale = 1f;

            Item.damage = 100;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 6f;

            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item42;
            Item.noMelee = true;
            Item.noUseGraphic = false;

            Item.shoot = ModContent.ProjectileType<SproutMoleMissileProjectile>();
            Item.shootSpeed = 1f;

            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 target = Main.MouseWorld;
            Vector2 spawnPosition = new Vector2(target.X, target.Y - 1600f);
            Vector2 missileVelocity = new Vector2(0f, 6f);

            Projectile.NewProjectile(source, spawnPosition, missileVelocity, type, damage, knockback, player.whoAmI);

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Tofu>(), 10);
            recipe.AddIngredient(ItemID.RocketI, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}