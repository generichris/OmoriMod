using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Buffs.SadBuff;
using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Players;
using OmoriMod.Systems.EmotionSystem;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.BuffItems;

public class RainCloud : EmotionBuffItem
{
    RainCloud()
    {
        itemTypeForResearch = ItemTypeForResearch.BuffPotion;
    }
    public override void SetDefaults()
    {
        SetEmotionType(EmotionType.SAD);
        EmotionItemClone<AirHorn>();
    }

    public override bool CanUseItemEmotionBuffItem(Player player)
    {
        EmotionPlayer emotionPlayer = player.GetModPlayer<EmotionPlayer>();

        if (emotionPlayer.Emotion == EmotionType.SAD || emotionPlayer.Emotion == EmotionType.NONE) { return true; }
        return false;
    }

    public override bool? UseItemEmotionBuffItem(Player player)
    {
        EmotionSystem.ApplyOrPromoteBuff<SadEmotionBase>(
            player: player,
            baseBuffType: ModContent.BuffType<Sad>(),
            duration: EmotionSystem.EMOTION_TIME_IN_SECONDS * 60
            );

        return true;
    }
}