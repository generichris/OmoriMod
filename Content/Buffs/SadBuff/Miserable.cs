using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.SadBuff;

public class Miserable : SadEmotionBase
{
    Miserable()
    {
        emotionLevel = 3;
        dustSpawnFrequency = 3;
    }

    public override void SetStaticDefaults()
    {
        nextStageEmotionType = null;
    }
}