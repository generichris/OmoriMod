using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Buffs.AngryBuff;
using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Players;
using OmoriMod.Systems.EmotionSystem;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.BuffItems;

public class AirHorn : EmotionBuffItem
{
    AirHorn()
    {
        itemTypeForResearch = ItemTypeForResearch.BuffPotion;
    }
    public override void SetDefaults()
    {
        SetEmotionType(EmotionType.ANGRY);

        ItemDefaults(
            width: 16,
            height: 16,
            scale: 1f,
            buyPrice: Item.buyPrice(0, 0, 2, 0),
            stackSize: 999,
            consumable: true
            );

        AnimationDefaults(
            useTime: 20,
            useStyleID: ItemUseStyleID.HoldUp,
            useSound: SoundID.Item1,
            autoReuse: false
            );

        PotionDefaults(
            healthHealed: 0,
            manaHealed: 0,
            isPotion: false,
            buffType: ModContent.BuffType<DummyBuff>(),
            buffTimeInSeconds: 60f
            );
    }

    public override bool CanUseItemEmotionBuffItem(Player player)
    {
        EmotionPlayer emotionPlayer = player.GetModPlayer<EmotionPlayer>();

        if (emotionPlayer.Emotion == EmotionType.ANGRY || emotionPlayer.Emotion == EmotionType.NONE) { return true; }
        return false;
    }

    public override bool? UseItemEmotionBuffItem(Player player)
    {

        EmotionSystem.ApplyOrPromoteBuff<AngryEmotionBase>(
            player: player,
            baseBuffType: ModContent.BuffType<Angry>(),
            duration: EmotionSystem.EMOTION_TIME_IN_SECONDS * 60
            );

        return true;
    }
}