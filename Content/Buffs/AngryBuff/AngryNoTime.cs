using OmoriMod.Content.Buffs.Abstract;

using Terraria;

namespace OmoriMod.Content.Buffs.AngryBuff;

public class AngryNoTime : AngryEmotionBase
{
    AngryNoTime()
    {
        emotionLevel = 1;
        dustSpawnFrequency = 1;
    }
    public override void SetStaticDefaults()
    {
        Main.buffNoTimeDisplay[Type] = true;
    }
}