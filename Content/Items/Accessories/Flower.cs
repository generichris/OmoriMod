using OmoriMod.Content.Buffs.HappyBuff;
using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Systems.EmotionSystem;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Accessories;

public class Flower : HappyItem
{
    Flower()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        EmotionItemClone<BloodyFlower>();
    }

    public override void UpdateEquip(Player player)
    {
        EmotionSystem.ClearAllEmotions(player);
        player.AddBuff(ModContent.BuffType<HappyNoTime>(), 2);
    }
}