using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.AngryBuff;

public class Furious : AngryEmotionBase
{
    Furious()
    {
        EmotionTier = 3;
        _dustSpawnFrequency = 3;
    }
}
