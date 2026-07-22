using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.AngryBuff;

public class Angry : AngryEmotionBase
{
    Angry()
    {
        EmotionTier = 1;
        _dustSpawnFrequency = 1;
    }
}
