using OmoriMod.Content.Buffs.Abstract;

using Terraria.ModLoader;

namespace OmoriMod.Content.Buffs.AngryBuff;

public class Enraged : AngryEmotionBase
{
    Enraged()
    {
        emotionLevel = 2;
        dustSpawnFrequency = 2;
    }

    public override void SetStaticDefaults()
    {
        nextStageEmotionType = ModContent.BuffType<Furious>();
    }
}