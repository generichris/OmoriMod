using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;

namespace OmoriMod.Content.Items.Health
{
    public class SproutMoleDish : OmoriModItem
    {
        SproutMoleDish()
        {
            itemTypeForResearch = ItemTypeForResearch.RecoveryPotion;
        }
        public override void SetDefaults()
        {
            ItemDefaults(
                width: 32,
                height: 32,
                scale: 1f,
                buyPrice: Item.buyPrice(platinum: 0, gold: 0, silver: 5, copper: 0),
                stackSize: 999,
                consumable: true
                );

            AnimationDefaults(
                useTime: 17,
                useStyleID: ItemUseStyleID.DrinkLiquid,
                useSound: SoundID.Item3,
                autoReuse: false,
                canTurnWhileUsing: true
                );

            PotionDefaults(
                healthHealed: 100,
                manaHealed: 0,
                isPotion: true,
                buffType: BuffID.WellFed,
                buffTimeInSeconds: 30 * 60
                );

            SetItemRarity(ItemRarityID.Green);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Tofu>(), 20);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();
        }
    }
}