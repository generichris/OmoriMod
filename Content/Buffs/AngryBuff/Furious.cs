using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.AngryBuff;

public class Furious : AngryEmotionBase
{
    Furious()
    {
        emotionLevel = 3;
        dustSpawnFrequency = 3;
    }

    public override void SetStaticDefaults()
    {
        nextStageEmotionType = null;
    }
}