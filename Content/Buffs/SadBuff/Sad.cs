using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.SadBuff;

public class Sad : SadEmotionBase
{
    Sad()
    {
        EmotionTier = 1;
        _dustSpawnFrequency = 1;
    }
}
