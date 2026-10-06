using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Systems.EmotionSystem;
using OmoriMod.Tests.Fixtures;
using OmoriMod.Tests.TestDoubles;

namespace OmoriMod.Tests;

public sealed class EmotionServiceSelectionTests(EmotionRegistryFixture fixture)
    : IClassFixture<EmotionRegistryFixture>
{
    private readonly EmotionService _service = fixture.Service;

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void StandardVariantWinsRegardlessOfCandidateOrder(bool noTimeFirst)
    {
        int[] candidates = noTimeFirst
            ? [
                EmotionRegistryFixture.BuffTypes.SadNoTime,
                EmotionRegistryFixture.BuffTypes.SadTierOne
            ]
            : [
                EmotionRegistryFixture.BuffTypes.SadTierOne,
                EmotionRegistryFixture.BuffTypes.SadNoTime
            ];

        Assert.Equal(
            EmotionRegistryFixture.BuffTypes.SadTierOne,
            _service.GetPreferredEmotionBuffType(candidates));
    }

    [Fact]
    public void HighestStandardTierWins()
    {
        int[] candidates = [
            EmotionRegistryFixture.BuffTypes.HappyTierOne,
            EmotionRegistryFixture.BuffTypes.HappyTierTwo
        ];

        Assert.Equal(
            EmotionRegistryFixture.BuffTypes.HappyTierTwo,
            _service.GetPreferredEmotionBuffType(candidates));
    }

    [Fact]
    public void FirstCandidateWinsAnOtherwiseIdenticalTie()
    {
        const EmotionType testEmotion = (EmotionType)4;
        var registry = new FakeEmotionRegistry()
            .Add(
                EmotionType.Happy,
                1,
                EmotionBuffVariant.Standard,
                typeof(TestEmotionBuff),
                buffType: 4001)
            .Add(
                testEmotion,
                1,
                EmotionBuffVariant.Standard,
                typeof(OtherTestEmotionBuff),
                buffType: 4002);
        var service = new EmotionService(registry);

        Assert.Equal(4002, service.GetPreferredEmotionBuffType([4002, 4001]));
        Assert.Equal(4001, service.GetPreferredEmotionBuffType([4001, 4002]));
    }

    [Fact]
    public void UnknownCandidatesAreIgnored()
    {
        Assert.Equal(
            EmotionRegistryFixture.BuffTypes.AngryTierOne,
            _service.GetPreferredEmotionBuffType([
                -1,
                EmotionRegistryFixture.BuffTypes.AngryTierOne,
                9999
            ]));
        Assert.Null(_service.GetPreferredEmotionBuffType([-1, 9999]));
    }

    [Fact]
    public void RegisteredEmotionBuffsAreCollectedBeforeRemoval()
    {
        int[] liveBuffArray = [
            EmotionRegistryFixture.BuffTypes.AngryTierOne,
            EmotionRegistryFixture.BuffTypes.HappyTierOne,
            9999,
            EmotionRegistryFixture.BuffTypes.SadNoTime
        ];

        Assert.Equal(
            [
                EmotionRegistryFixture.BuffTypes.AngryTierOne,
                EmotionRegistryFixture.BuffTypes.HappyTierOne,
                EmotionRegistryFixture.BuffTypes.SadNoTime
            ],
            _service.GetRegisteredEmotionBuffTypes(liveBuffArray));
    }

    private sealed class TestEmotionBuff : EmotionBuff
    {
    }

    private sealed class OtherTestEmotionBuff : EmotionBuff
    {
    }
}