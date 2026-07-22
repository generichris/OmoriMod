using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.SadBuff;

public class Despondent : SadEmotionBase
{
    Despondent()
    {
        EmotionTier = 4;
        _dustSpawnFrequency = 4;
    }
}
