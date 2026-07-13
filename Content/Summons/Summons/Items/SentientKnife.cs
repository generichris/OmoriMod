using Microsoft.Xna.Framework;

using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.BuffItems;
using OmoriMod.Content.Summons.Summons.Buffs;
using OmoriMod.Content.Summons.Summons.Projectiles;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Summons.Summons.Items;

public class SentientKnife : OmoriModItem
{
    SentientKnife()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }

    public override void OmoriModItemSetStaticDefaults()
    {
        ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
        ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
    }

    public override void SetDefaults()
    {
        Item.ResearchUnlockCount = 1;

        Item.damage = 10;
        Item.knockBack = 3f;
        Item.mana = 10; // mana cost
        Item.width = 32;
        Item.height = 32;
        Item.useTime = 36;
        Item.useAnimation = 36;
        Item.useStyle = ItemUseStyleID.Swing; // how the player's arm moves when using the item
        Item.value = Item.sellPrice(gold: 30);
        Item.rare = ItemRarityID.Cyan;
        Item.UseSound = SoundID.Item44; // What sound should play when using the item

        // These below are needed for a minion weapon
        Item.noMelee = true; // this item doesn't do any melee damage
        Item.DamageType = DamageClass.Summon; // Makes the damage register as summon. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type
        Item.buffType = ModContent.BuffType<SentientKnifeBuff>();

        // No buffTime because otherwise the item tooltip would say something like "1 minute duration"
        Item.shoot = ModContent.ProjectileType<SentientKnifeProjectile>(); // This item creates the minion projectile
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        // Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position
        position = Main.MouseWorld;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
        player.AddBuff(Item.buffType, 2);

        // Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
        var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
        projectile.originalDamage = Item.damage;

        // Since we spawned the projectile manually already, we do not need the game to spawn it for ourselves anymore, so return false
        return false;
    }

    public override void AddRecipes()
    {
        Recipe recipe1 = CreateRecipe();
        recipe1.AddIngredient(ModContent.ItemType<RainCloud>(), 100);
        recipe1.AddIngredient(ItemID.IronBar, 10);
        recipe1.AddTile(TileID.Anvils);
        recipe1.Register();

        Recipe recipe2 = CreateRecipe();
        recipe2.AddIngredient(ModContent.ItemType<RainCloud>(), 100);
        recipe2.AddIngredient(ItemID.LeadBar, 10);
        recipe2.AddTile(TileID.Anvils);
        recipe2.Register();
    }
}