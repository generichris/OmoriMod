using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.HappyBuff;

public class Hysterical : HappyEmotionBase
{
    Hysterical()
    {
        EmotionTier = 4;
        _dustSpawnFrequency = 4;
    }
}
