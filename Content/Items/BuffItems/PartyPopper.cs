using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Buffs.HappyBuff;
using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.BuffItems;

public class PartyPopper : EmotionBuffItem
{
    PartyPopper()
    {
        itemTypeForResearch = ItemTypeForResearch.BuffPotion;
    }
    public override void SetDefaults()
    {
        SetEmotionType(EmotionType.Happy);
        EmotionItemClone<AirHorn>();
    }

    public override bool CanUseItemEmotionBuffItem(Player player)
    {
        return EmotionSystem.CanApplyOrPromoteEmotion<HappyEmotionBase>(player);
    }

    public override bool? UseItemEmotionBuffItem(Player player)
    {
        return EmotionSystem.ApplyOrPromoteEmotion<HappyEmotionBase>(
            player: player,
            duration: EmotionStatTuning.EmotionTimeInSeconds * 60
        );
    }
}
