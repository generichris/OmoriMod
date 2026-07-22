using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Melee.Tier1;

public class Bat : AngryItem
{
    Bat()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        ItemDefaults(
            width: 32,
            height: 32,
            scale: 1.5f,
            buyPrice: Item.buyPrice(0, 1, 0, 0),
            stackSize: 1,
            consumable: false
            );

        DamageDefaults(
            damageType: DamageClass.Melee,
            damage: 8,
            knockback: 6f,
            crit: 4,
            noMelee: false
            );

        AnimationDefaults(
            useTime: 20,
            useStyleID: ItemUseStyleID.Swing,
            useSound: SoundID.Item1,
            autoReuse: true
            );
    }

    public override void AddRecipes()
    {
        MakeRegularRecipe(
            ingredientID: ItemID.Wood,
            amount: 15,
            craftingStationID: TileID.WorkBenches
            );
    }
}