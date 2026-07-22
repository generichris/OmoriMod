using OmoriMod.Content.Buffs.Abstract;

using Terraria;

namespace OmoriMod.Content.Buffs.AngryBuff;

public class AngryNoTime : AngryEmotionBase
{
    AngryNoTime()
    {
        EmotionTier = 1;
        _dustSpawnFrequency = 1;
    }
    public override void SetStaticDefaults()
    {
        Main.buffNoTimeDisplay[Type] = true;
    }
}
