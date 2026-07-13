using OmoriMod.Content.Buffs.Abstract;

using Terraria.ModLoader;

namespace OmoriMod.Content.Buffs.HappyBuff;

public class Happy : HappyEmotionBase
{
    Happy()
    {
        emotionLevel = 1;
        dustSpawnFrequency = 1;
    }

    public override void SetStaticDefaults()
    {
        nextStageEmotionType = ModContent.BuffType<Ecstatic>();
    }
}