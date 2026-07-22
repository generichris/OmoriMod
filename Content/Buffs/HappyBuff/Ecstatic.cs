using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.HappyBuff;

public class Ecstatic : HappyEmotionBase
{
    Ecstatic()
    {
        EmotionTier = 2;
        _dustSpawnFrequency = 2;
    }
}
