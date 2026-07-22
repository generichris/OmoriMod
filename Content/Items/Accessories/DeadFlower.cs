using OmoriMod.Content.Buffs.SadBuff;
using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Systems.EmotionSystem;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Accessories;

public class DeadFlower : SadItem
{
    DeadFlower()
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
        player.AddBuff(ModContent.BuffType<SadNoTime>(), 2);
    }
}
