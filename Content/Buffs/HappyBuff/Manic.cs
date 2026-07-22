using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.HappyBuff;

public class Manic : HappyEmotionBase
{
    Manic()
    {
        EmotionTier = 3;
        _dustSpawnFrequency = 3;
    }
}
