using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.HappyBuff;

public class Manic : HappyEmotionBase
{
    Manic()
    {
        emotionLevel = 3;
        dustSpawnFrequency = 3;
    }

    public override void SetStaticDefaults()
    {
        nextStageEmotionType = null;
    }
}