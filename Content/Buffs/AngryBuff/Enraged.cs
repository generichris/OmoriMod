using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.AngryBuff;

public class Enraged : AngryEmotionBase
{
    Enraged()
    {
        EmotionTier = 2;
        _dustSpawnFrequency = 2;
    }
}
