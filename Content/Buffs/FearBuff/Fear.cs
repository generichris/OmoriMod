using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.FearBuff;

public class Fear : FearEmotionBase
{
    Fear()
    {
        EmotionTier = 1;
        _dustSpawnFrequency = 1;
    }
}
