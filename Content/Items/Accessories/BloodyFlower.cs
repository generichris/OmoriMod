using OmoriMod.Content.Buffs.AngryBuff;
using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Systems.EmotionSystem;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Accessories;

public class BloodyFlower : AngryItem
{
    BloodyFlower()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        SetAccessoryDefaults(
            width: 20,
            height: 30,
            buyPrice: Item.buyPrice(1, 0, 0, 0)
            );
    }

    public override void UpdateEquip(Player player)
    {
        EmotionSystem.ClearAllEmotions(player);
        player.AddBuff(ModContent.BuffType<AngryNoTime>(), 2);
    }
}