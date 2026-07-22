using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.SadBuff;

public class Depressed : SadEmotionBase
{
    Depressed()
    {
        EmotionTier = 2;
        _dustSpawnFrequency = 2;
    }
}
