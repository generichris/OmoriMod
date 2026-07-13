using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;

using Terraria;
using Terraria.ID;

namespace OmoriMod.Content.Items.Health;

public class Sweets : OmoriModItem
{
    Sweets()
    {
        itemTypeForResearch = ItemTypeForResearch.RecoveryPotion;
    }
    public override void SetDefaults()
    {
        ItemDefaults(
            width: 32,
            height: 32,
            scale: 1f,
            buyPrice: Item.buyPrice(platinum: 0, gold: 0, silver: 2, copper: 0),
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
            healthHealed: 50,
            manaHealed: 0,
            isPotion: true
            );

        SetItemRarity(ItemRarityID.Blue);
    }
}