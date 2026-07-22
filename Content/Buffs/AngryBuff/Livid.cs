using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.AngryBuff;

public class Livid : AngryEmotionBase
{
    Livid()
    {
        EmotionTier = 4;
        _dustSpawnFrequency = 4;
    }
}
