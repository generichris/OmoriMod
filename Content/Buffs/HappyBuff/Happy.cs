using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.HappyBuff;

public class Happy : HappyEmotionBase
{
    Happy()
    {
        EmotionTier = 1;
        _dustSpawnFrequency = 1;
    }
}
