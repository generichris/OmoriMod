using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Buffs.AngryBuff;
using OmoriMod.Content.Buffs.HappyBuff;
using OmoriMod.Content.Buffs.SadBuff;
using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Systems.EmotionSystem;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.BuffItems;

public class EmotionalAmplifier : EmotionBuffItem
{
    EmotionalAmplifier()
    {
        cooldownTicks = 5;
        itemTypeForResearch = ItemTypeForResearch.BuffPotion;
    }
    public override void SetDefaults()
    {
        EmotionItemClone<AirHorn>();
        SetItemRarity(ItemRarityID.Purple);
    }

    public override bool CanUseItemEmotionBuffItem(Player player)
    {
        int? buffType = EmotionSystem.GetEmotionType(player);
        if (!buffType.HasValue) return false;
        if (EmotionSystem.TIER3_EMOTION_TYPES.Contains(buffType.Value) || EmotionSystem.TIER4_EMOTION_TYPES.Contains(buffType.Value)) return true;
        return false;
    }

    public override bool? UseItemEmotionBuffItem(Player player)
    {
        int? buffType = EmotionSystem.GetEmotionType(player);
        ModBuff buff = ModContent.GetModBuff(buffType.Value);
        if (buff is AngryEmotionBase)
        {
            EmotionSystem.ApplyTier4Emotion<AngryEmotionBase>(
                player: player,
                baseBuffType: ModContent.BuffType<Livid>(),
                duration: EmotionSystem.EMOTION_TIME_IN_SECONDS * 60
            );
        }
        if (buff is HappyEmotionBase)
        {
            EmotionSystem.ApplyTier4Emotion<HappyEmotionBase>(
                player: player,
                baseBuffType: ModContent.BuffType<Hysterical>(),
                duration: EmotionSystem.EMOTION_TIME_IN_SECONDS * 60
            );
        }
        if (buff is SadEmotionBase)
        {
            EmotionSystem.ApplyTier4Emotion<SadEmotionBase>(
                player: player,
                baseBuffType: ModContent.BuffType<Despondent>(),
                duration: EmotionSystem.EMOTION_TIME_IN_SECONDS * 60
            );
        }

        return true;
    }
}