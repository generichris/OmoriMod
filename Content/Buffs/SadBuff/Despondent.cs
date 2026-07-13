using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Players;
using OmoriMod.Systems.EmotionSystem;

using Terraria;

namespace OmoriMod.Content.Buffs.SadBuff;

public class Despondent : SadEmotionBase
{
    Despondent()
    {
        emotionLevel = 4;
        dustSpawnFrequency = 4;
    }

    public override void UpdateTier4EmotionBuff(Player player, ref int buffIndex)
    {
        emotionLevel = player.GetModPlayer<EmotionPlayer>().tier4EmotionLevel;
    }

    public override bool ReApply(Player player, int time, int buffIndex)
    {
        if (player.GetModPlayer<EmotionPlayer>().tier4EmotionLevel < EmotionSystem.PLAYER_MAX_EMOTION_LEVEL) player.GetModPlayer<EmotionPlayer>().tier4EmotionLevel++;
        return false;
    }
    public override void SadModifyBuffText(ref string buffName, ref string tip, ref int rare)
    {
        Tier4ModifyBuffText(ref buffName, ref tip, ref rare);
    }

    public override void SetStaticDefaults()
    {
        nextStageEmotionType = null;
    }
}