using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Buffs.AngryBuff;
using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem;

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
        SetEmotionType(EmotionType.Angry);

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
        return EmotionSystem.CanApplyOrPromoteEmotion<AngryEmotionBase>(player);
    }

    public override bool? UseItemEmotionBuffItem(Player player)
    {
        return EmotionSystem.ApplyOrPromoteEmotion<AngryEmotionBase>(
            player: player,
            duration: EmotionStatTuning.EmotionTimeInSeconds * 60
        );
    }
}
