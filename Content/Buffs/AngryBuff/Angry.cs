using OmoriMod.Content.Buffs.Abstract;

using Terraria.ModLoader;

namespace OmoriMod.Content.Buffs.AngryBuff;

public class Angry : AngryEmotionBase
{
    Angry()
    {
        emotionLevel = 1;
        dustSpawnFrequency = 1;
    }

    public override void SetStaticDefaults()
    {
        nextStageEmotionType = ModContent.BuffType<Enraged>();
    }
}