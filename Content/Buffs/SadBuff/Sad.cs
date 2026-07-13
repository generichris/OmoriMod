using OmoriMod.Content.Buffs.Abstract;

using Terraria.ModLoader;

namespace OmoriMod.Content.Buffs.SadBuff;

public class Sad : SadEmotionBase
{
    Sad()
    {
        emotionLevel = 1;
        dustSpawnFrequency = 1;
    }

    public override void SetStaticDefaults()
    {
        nextStageEmotionType = ModContent.BuffType<Depressed>();
    }
}