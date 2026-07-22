using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Systems.EmotionSystem;

using Terraria;
using Terraria.ID;

namespace OmoriMod.Content.Items.BuffItems;

public class EmotionalAmplifier : EmotionBuffItem
{
    EmotionalAmplifier()
    {
        _cooldownTicks = 5;
        itemTypeForResearch = ItemTypeForResearch.BuffPotion;
    }
    public override void SetDefaults()
    {
        EmotionItemClone<AirHorn>();
        SetItemRarity(ItemRarityID.Purple);
    }

    public override bool CanUseItemEmotionBuffItem(Player player)
    {
        return EmotionSystem.CanApplyFinalTierEmotion(player);
    }

    public override bool? UseItemEmotionBuffItem(Player player)
    {
        return EmotionSystem.ApplyFinalTierEmotion(
            player: player,
            duration: EmotionStatTuning.EmotionTimeInSeconds * 60
        );
    }
}
