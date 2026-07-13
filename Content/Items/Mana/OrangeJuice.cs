using Terraria;
using Terraria.ID;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;

namespace OmoriMod.Content.Items.Mana
{
    public class OrangeJuice : OmoriModItem
    {
        OrangeJuice()
        {
            itemTypeForResearch = ItemTypeForResearch.RecoveryPotion;
        }
        public override void SetDefaults()
        {
            ItemDefaults(
                width: 32,
                height: 32,
                scale: 1f,
                buyPrice: Item.buyPrice(platinum: 0, gold: 0, silver: 0, copper: 6),
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
                healthHealed: 0,
                manaHealed: 20,
                isPotion: true
                );

            SetItemRarity(ItemRarityID.Blue);
        }
    }
}
