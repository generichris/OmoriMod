using Terraria;
using Terraria.ModLoader;
using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Buffs.FearBuff;
using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Players;
using OmoriMod.Systems.EmotionSystem;

namespace OmoriMod.Content.Items.BuffItems
{
    public class Firecracker : EmotionBuffItem
    {
        Firecracker()
        {
            itemTypeForResearch = ItemTypeForResearch.BuffPotion;
        }
        public override void SetDefaults()
        {
            SetEmotionType(EmotionType.FEAR);
            EmotionItemClone<AirHorn>();
        }

        public override bool CanUseItemEmotionBuffItem(Player player)
        {
            EmotionPlayer emotionPlayer = player.GetModPlayer<EmotionPlayer>();

            if (emotionPlayer.Emotion == EmotionType.FEAR || emotionPlayer.Emotion == EmotionType.NONE) { return true; }
            return false;
        }

        public override bool? UseItemEmotionBuffItem(Player player)
        {
            EmotionSystem.ApplyOrPromoteBuff<FearEmotionBase>(
                player: player,
                baseBuffType: ModContent.BuffType<Fear>(),
                duration: EmotionSystem.EMOTION_TIME_IN_SECONDS * 60
                );

            return true;
        }
    }
}