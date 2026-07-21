using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Summons.Summons.Buffs;
using OmoriMod.Content.Summons.Summons.Projectiles;

namespace OmoriMod.Content.Summons.Summons.Items
{
    public class SadGoober : OmoriModItem
    {
        public SadGoober()
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

            Item.damage = 12;
            Item.knockBack = 1f;
            Item.mana = 10;
            Item.width = 24;
            Item.height = 24;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item44;

            // Needed for a minion weapon
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<SadGooberBuff>();
            Item.shoot = ModContent.ProjectileType<SadGooberProjectile>();
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
            //Recipe recipe = CreateRecipe();
            //recipe.AddIngredient(ItemID.Mushroom, 15);
            //recipe.AddIngredient(ItemID.SpiderFang, 5);
            //recipe.AddTile(TileID.WorkBenches);
            //recipe.Register();
        }
    }
}