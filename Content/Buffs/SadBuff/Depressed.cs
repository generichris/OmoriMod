using OmoriMod.Content.Buffs.Abstract;

using Terraria.ModLoader;

namespace OmoriMod.Content.Buffs.SadBuff;

public class Depressed : SadEmotionBase
{
    Depressed()
    {
        emotionLevel = 2;
        dustSpawnFrequency = 2;
    }

    public override void SetStaticDefaults()
    {
        nextStageEmotionType = ModContent.BuffType<Miserable>();
    }
}