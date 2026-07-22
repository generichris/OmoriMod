using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.SadBuff;

public class Miserable : SadEmotionBase
{
    Miserable()
    {
        EmotionTier = 3;
        _dustSpawnFrequency = 3;
    }
}
