using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Systems.EmotionSystem;
using OmoriMod.Tests.TestDoubles;

namespace OmoriMod.Tests.Fixtures;

public sealed class EmotionRegistryFixture
{
    internal static class BuffTypes
    {
        internal const int AngryTierOne = 1001;
        internal const int AngryTierTwo = 1002;
        internal const int HappyTierOne = 2001;
        internal const int HappyTierTwo = 2002;
        internal const int SadTierOne = 3001;
        internal const int SadTierTwo = 3002;
        internal const int SadNoTime = 3101;
    }

    internal FakeEmotionRegistry Registry { get; }

    internal EmotionService Service { get; }

    public EmotionRegistryFixture()
    {
        Registry = new FakeEmotionRegistry()
            .Add(EmotionType.Angry, 1, EmotionBuffVariant.Standard, typeof(AngryEmotionBase), BuffTypes.AngryTierOne)
            .Add(EmotionType.Angry, 2, EmotionBuffVariant.Standard, typeof(AngryEmotionBase), BuffTypes.AngryTierTwo)
            .Add(EmotionType.Happy, 1, EmotionBuffVariant.Standard, typeof(HappyEmotionBase), BuffTypes.HappyTierOne)
            .Add(EmotionType.Happy, 2, EmotionBuffVariant.Standard, typeof(HappyEmotionBase), BuffTypes.HappyTierTwo)
            .Add(EmotionType.Sad, 1, EmotionBuffVariant.Standard, typeof(SadEmotionBase), BuffTypes.SadTierOne)
            .Add(EmotionType.Sad, 2, EmotionBuffVariant.Standard, typeof(SadEmotionBase), BuffTypes.SadTierTwo)
            .Add(EmotionType.Sad, 1, EmotionBuffVariant.NoTime, typeof(SadEmotionBase), BuffTypes.SadNoTime);

        Service = new EmotionService(Registry);
    }
}