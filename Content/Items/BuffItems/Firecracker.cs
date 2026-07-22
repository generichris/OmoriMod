using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Items.BuffItems;
using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem;

using Terraria;

namespace OmoriMod.Content.Items.BuffItems;

public class Firecracker : EmotionBuffItem
{
    Firecracker()
    {
        itemTypeForResearch = ItemTypeForResearch.BuffPotion;
    }
    public override void SetDefaults()
    {
        SetEmotionType(EmotionType.Fear);
        EmotionItemClone<AirHorn>();
    }

    public override bool CanUseItemEmotionBuffItem(Player player)
    {
        return EmotionSystem.CanApplyOrPromoteEmotion<FearEmotionBase>(player);
    }

    public override bool? UseItemEmotionBuffItem(Player player)
    {
        return EmotionSystem.ApplyOrPromoteEmotion<FearEmotionBase>(
            player: player,
            duration: EmotionStatTuning.EmotionTimeInSeconds * 60
        );
    }
}
