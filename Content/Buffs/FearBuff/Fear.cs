using Terraria.ModLoader;
using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Buffs.FearBuff
{
    public class Fear : FearEmotionBase
    {
        Fear()
        {
            emotionLevel = 1;
            dustSpawnFrequency = 1;
        }

        public override void SetStaticDefaults()
        {
            nextStageEmotionType = null;
        }
    }
}
