using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Health;
using OmoriMod.Content.Summons.Summons.Buffs;
using OmoriMod.Content.Summons.Summons.Projectiles;

namespace OmoriMod.Content.Summons.Summons.Items
{
    public class FlyingSproutMole : OmoriModItem
    {
        public FlyingSproutMole()
        {
            itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
        }

        public override void OmoriModItemSetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.ResearchUnlockCount = 1;

            Item.damage = 8;
            Item.knockBack = 1f;
            Item.mana = 10;
            Item.width = 34;
            Item.height = 34;
            Item.scale = 0.2f; // source art is 170x170px, scale down to ~34x34
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item44;

            // Needed for a minion weapon
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<FlyingSproutMoleBuff>();
            Item.shoot = ModContent.ProjectileType<FlyingSproutMoleProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Tofu>(), 15);
            recipe.AddIngredient(ItemID.Feather, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}