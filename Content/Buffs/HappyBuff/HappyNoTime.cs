using OmoriMod.Content.Buffs.Abstract;

using Terraria;

namespace OmoriMod.Content.Buffs.HappyBuff;

public class HappyNoTime : HappyEmotionBase
{
    HappyNoTime()
    {
        EmotionTier = 1;
        _dustSpawnFrequency = 1;
    }
    public override void SetStaticDefaults()
    {
        Main.buffNoTimeDisplay[Type] = true;
    }
}
