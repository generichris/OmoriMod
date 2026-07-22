using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Abstract_Classes.BaseClasses;


public enum ItemTypeForResearch
{
    Weapons_Tools_Armor_Accessory = 1,
    QuestFish_TombStone_HerbBag = 2,
    TreasureBag_BossSummons_Dye = 3,
    Mechanism_Fruit_RareCraftingMaterial_Food_BiomeCrate = 5,
    Crates_LifeManaCrystal_LifeFruit = 10,
    Gems = 15,
    BuffPotion = 20,
    Ingot_Herb_Seed_CraftingMaterial = 25,
    RecoveryPotion = 30,
    Acorn_FallingStar_Beam = 50,
    Ammo_Explosives = 99,
    Ore_Block_Torch_Rope_EmptyBullet_Coin = 100,
    Platform_ExtractinatableBlock = 200,
    Wall = 400,
    Unobtainable = 0
}

/// <summary>
/// Contains useful methods for a variety of mod items. Useful for when an item doesn't require being an <see cref="EmotionItem"/>
/// </summary>
public abstract class OmoriModItem : ModItem
{

    public ItemTypeForResearch itemTypeForResearch;

    /// <summary>
    /// Use this to call <see cref="SetStaticDefaults"/>
    /// </summary>
    public virtual void OmoriModItemSetStaticDefaults() { }

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = (int)itemTypeForResearch;
        OmoriModItemSetStaticDefaults();
    }




    // Rarity Setting

    /// <summary>
    /// Used for <see cref="EmotionItem"/> that requires a hook method to work
    /// </summary>
    protected virtual void SetRarity() { }

    /// <summary>
    /// Manually sets <see cref="Item.rare"/>. Must be called after <see cref="ItemDefaults(int, int, float, int, int, int, bool)."/>
    /// </summary>
    /// <param name="itemRarity">The rarity of the item.</param>
    public virtual void SetItemRarity(int itemRarity)
    {
        Item.rare = itemRarity;
    }

    // Rarity Setting




    // Item Cloning

    /// <summary>
    /// Clones the defaults of a <see cref="ModItem"/> inlcuding the research unlock count
    /// </summary>
    /// <typeparam name="T">The <see cref="ModItem"/> to be cloned</typeparam>
    public void ModItemClone<T>() where T : ModItem
    {
        int itemType = ModContent.ItemType<T>();
        Item itemToClone = ModContent.GetModItem(itemType).Item;
        Item.CloneDefaults(itemType);
        Item.ResearchUnlockCount = itemToClone.ResearchUnlockCount;
    }

    // Item Cloning




    // Defaults

    /// <summary>
    /// Sets the defaults that every item shares
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="scale"></param>
    /// <param name="buyPrice"></param>
    /// <param name="stackSize"></param>
    /// <param name="researchCount"></param>
    public void ItemDefaults(int width, int height, float scale, int buyPrice, int stackSize, bool consumable)
    {
        Item.scale = scale;
        Item.width = (int)(width * Item.scale);
        Item.height = (int)(height * Item.scale);
        Item.value = buyPrice;
        Item.maxStack = stackSize;
        Item.consumable = consumable;
        SetRarity();
    }

    /// <summary>
    /// Sets the defaults for any item that does damage
    /// </summary>
    /// <param name="damageType"></param>
    /// <param name="damage"></param>
    /// <param name="knockback"></param>
    /// <param name="crit"></param>
    /// <param name="noMelee"></param>
    public void DamageDefaults(DamageClass damageType, int damage, float knockback, int crit, bool noMelee, int mana = 0)
    {
        Item.damage = damage;
        Item.knockBack = knockback;
        Item.DamageType = damageType;
        Item.crit = crit;
        Item.noMelee = noMelee;
        Item.mana = mana;
    }

    /// <summary> 
    /// Sets the defaults for any item that has an animation
    /// </summary>
    /// <param name="useTime"></param>
    /// <param name="useStyleID"></param>
    /// <param name="useSound"></param>
    /// <param name="autoReuse"></param>
    public void AnimationDefaults(int useTime, int useStyleID, SoundStyle useSound, bool autoReuse, int animationTime = -1, bool canTurnWhileUsing = false, bool noUseAnimation = false)
    {
        Item.useTime = useTime;
        if (animationTime == -1) { animationTime = useTime; }
        Item.useAnimation = animationTime;
        Item.useStyle = useStyleID;
        Item.UseSound = useSound;
        Item.autoReuse = autoReuse;
        Item.useTurn = canTurnWhileUsing;
        Item.noUseGraphic = noUseAnimation;
    }


    /// <summary>
    /// Sets the defaults for any item that shoots projectiles
    /// </summary>
    /// <param name="projectileID">The id of the projectile being shot. Defaults to 10 (used for items that use Ammo)</param>
    /// <param name="shootSpeed">How fast the projectile gets shot</param>
    /// <param name="ammoUsedID">The type of ammo this item will CONSUME on use. AKA for guns</param>
    /// <param name="ammoID">The type of ammo this item counts as. AKA for ammo</param>
    public void ProjectileDefaults(float shootSpeed, int projectileID = 10, int ammoUsedID = 0, int ammoID = 0)
    {
        Item.ammo = ammoID;
        Item.useAmmo = ammoUsedID;
        Item.shootSpeed = shootSpeed;
        Item.shoot = projectileID;
    }

    /// <summary>
    /// Sets the defaults for any potion-like item
    /// </summary>
    /// <param name="healthHealed"></param>
    /// <param name="manaHealed"></param>
    /// <param name="isPotion"></param>
    /// <param name="buffType">Defaults to 0 (no buff type)</param>
    /// <param name="buffTimeInSeconds">Defaults to 0 (no buff time)</param>
    public void PotionDefaults(int healthHealed, int manaHealed, bool isPotion, int buffType = 0, float buffTimeInSeconds = 0)
    {
        Item.healLife = healthHealed;
        Item.healMana = manaHealed;
        Item.potion = isPotion;
        Item.buffType = buffType;
        Item.buffTime = (int)(buffTimeInSeconds * 60);
    }

    /// <summary>
    /// Special Method for setting Accessory Defaults
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="buyPrice"></param>
    public void SetAccessoryDefaults(int width, int height, int buyPrice)
    {
        Item.accessory = true;
        ItemDefaults(
            width: width,
            height: height,
            scale: 1,
            buyPrice: buyPrice,
            stackSize: 1,
            consumable: false
        );
    }

    // Defaults




    // Recipes

    /// <summary>
    /// Helps make ammo recipes.
    /// </summary>
    /// <param name="resultAmount">The amount of ammo being created from this recipe.</param>
    /// <param name="baseIngredientID">The ID of the ingredient that is used as the base for this item.</param>
    /// <param name="nonEndlessIngredientID">The ID of the ingredient used for non-endless crafting.</param>
    /// <param name="endlessIngredientID">The ID of the ingredient used for endless crafting.</param>
    /// <param name="baseAmount">The amount of the base ingredient needed.</param>
    /// <param name="nonEndlessAmount">The amount of the non-endless ingredient needed.</param>
    public void MakeAmmoRecipes(int resultAmount, int baseIngredientID, int nonEndlessIngredientID, int endlessIngredientID, int baseAmount = 1, int nonEndlessAmount = 1)
    {
        Recipe recipe1 = CreateRecipe(resultAmount);
        recipe1.AddIngredient(baseIngredientID, baseAmount);
        recipe1.AddIngredient(nonEndlessIngredientID, nonEndlessAmount);
        recipe1.Register();

        Recipe recipe2 = CreateRecipe(resultAmount);
        recipe2.AddIngredient(baseIngredientID, baseAmount);
        recipe2.AddCondition(Condition.PlayerCarriesItem(endlessIngredientID));
        recipe2.Register();
    }

    /// <summary>
    /// Helps make endless ammo recipes.
    /// </summary>
    /// <param name="ingredientID">The non-endless ammo varient ID.</param>
    /// <param name="ammoNeeded">The amount of non-endless ammo needed.</param>
    /// <param name="craftingStationID">The ID of the crafting station used.</param>
    public void MakeEndlessAmmoRecipe(int ingredientID, int ammoNeeded = 3996, int craftingStationID = TileID.CrystalBall)
    {
        Recipe recipe1 = CreateRecipe();
        recipe1.AddIngredient(ingredientID, ammoNeeded);
        recipe1.AddTile(craftingStationID);
        recipe1.Register();
    }
    /// <summary>
    /// Makes regular 1-ingredient recipes
    /// </summary>
    /// <param name="ingredientID">The ingredient's ID.</param>
    /// <param name="amount">The amount of the ingredient needed.</param>
    /// <param name="craftingStationID">The ID of the crafting station used.</param>
    public void MakeRegularRecipe(int ingredientID, int amount, int craftingStationID)
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ingredientID, amount);
        recipe.AddTile(craftingStationID);
        recipe.Register();
    }

    /// <summary>
    /// Makes a bunch of 1-ingredient recipes
    /// </summary>
    /// <param name="ingredients">A list where you have each value be (ingredientID, amount).</param>
    /// <param name="craftingStationID">The ID of the crafting station used.</param>
    public void MakeRegularRecipes(List<(int, int)> ingredients, int craftingStationID)
    {
        foreach (var ingredient in ingredients)
        {
            int ing = ingredient.Item1;
            int amount = ingredient.Item2;
            MakeRegularRecipe(ing, amount, craftingStationID);
        }
    }

    /// <summary>
    /// Makes an 'upgrade' recipe
    /// </summary>
    /// <param name="baseItemID">The item to be 'upgraded'.</param>
    /// <param name="extraItemID">The ID of the upgrade material.</param>
    /// <param name="extraItemAmount">The amount of upgrade material needed.</param>
    /// <param name="craftingStationID">The ID of the crafting station used.</param>
    public void MakeUpgradeRecipe(int baseItemID, int extraItemID, int extraItemAmount, int craftingStationID)
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(baseItemID, 1);
        recipe.AddIngredient(extraItemID, extraItemAmount);
        recipe.AddTile(craftingStationID);
        recipe.Register();
    }

    // Recipes




    // Helpful Methods

    /// <summary>
    /// Moves projectile forward for spawning purposes.
    /// </summary>
    /// <param name="position">The current position of the projectile.</param>
    /// <param name="velocity">The current velocity of the projectile.</param>
    /// <param name="ticks">How many ticks to simulate this projectile moving for.</param>
    /// <returns><c>True</c> if no collision occured.</returns>
    public virtual bool MoveProjectileForward(ref Vector2 position, ref Vector2 velocity, ref int type, float ticks = 2)
    {
        Projectile projectile = ModContent.GetModProjectile(type).Projectile;

        int actingTicks = (int)MathF.Floor(ticks);
        Vector2 actingVelocity = velocity;

        while (ticks % 1 != 0)
        {
            actingTicks *= 10;
            ticks *= 10;
            actingVelocity /= 10;
        }

        for (int i = 0; i < actingTicks; i++)
        {
            // compute next canidate position
            Vector2 nextPos = position + actingVelocity;
            var hitbox = new Rectangle(
                (int)nextPos.X,
                (int)nextPos.Y,
                projectile.width,
                projectile.height
            );

            // If that spot collides with solid tiles, abort early
            if (Collision.SolidCollision(hitbox.TopLeft(), hitbox.Width, hitbox.Height))
            {
                return false;
            }

            position = nextPos;
        }

        return true;
    }

    // Helpful Methods
}