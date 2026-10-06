using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Systems.EmotionSystem;
using OmoriMod.Tests.TestDoubles;

namespace OmoriMod.Tests;

public sealed class EmotionApplicationDecisionTests
{
    private const EmotionType TestEmotion = (EmotionType)4;
    private const int TierOneBuffType = 4001;
    private const int TierTwoBuffType = 4002;
    private const int TierThreeBuffType = 4003;

    [Fact]
    public void RegularItemAppliesTierOneWhenEmotionIsNotActive()
    {
        EmotionService service = CreateService(tierCount: 1);

        EmotionApplicationDecision decision = service.GetApplicationDecision(
            EmotionApplicationRequest.RegularItem,
            typeof(TestEmotionBuff),
            currentBuffType: null,
            EmotionScalingMode.Capped,
            scalingLevel: 0);

        AssertDecision(decision, EmotionApplicationAction.ApplyTierOne, TierOneBuffType);
    }

    [Fact]
    public void AmplifierIsBlockedWhenEmotionIsNotActive()
    {
        EmotionService service = CreateService(tierCount: 1);

        EmotionApplicationDecision decision = service.GetApplicationDecision(
            EmotionApplicationRequest.Amplifier,
            typeof(TestEmotionBuff),
            currentBuffType: null,
            EmotionScalingMode.Capped,
            scalingLevel: 0);

        AssertDecision(decision, EmotionApplicationAction.Block);
    }

    [Fact]
    public void SingleTierRegularItemRefreshesBeforeAmplification()
    {
        EmotionService service = CreateService(tierCount: 1);

        EmotionApplicationDecision decision = service.GetApplicationDecision(
            EmotionApplicationRequest.RegularItem,
            typeof(TestEmotionBuff),
            TierOneBuffType,
            EmotionScalingMode.Capped,
            scalingLevel: 1);

        AssertDecision(decision, EmotionApplicationAction.RefreshCurrent, TierOneBuffType);
    }

    [Fact]
    public void SingleTierAmplifierStartsScaling()
    {
        EmotionService service = CreateService(tierCount: 1);

        EmotionApplicationDecision decision = service.GetApplicationDecision(
            EmotionApplicationRequest.Amplifier,
            typeof(TestEmotionBuff),
            TierOneBuffType,
            EmotionScalingMode.Capped,
            scalingLevel: 1);

        AssertDecision(decision, EmotionApplicationAction.AmplifyCurrent, TierOneBuffType);
    }

    [Fact]
    public void SingleTierRegularItemIsBlockedAfterAmplification()
    {
        EmotionService service = CreateService(tierCount: 1);

        EmotionApplicationDecision decision = service.GetApplicationDecision(
            EmotionApplicationRequest.RegularItem,
            typeof(TestEmotionBuff),
            TierOneBuffType,
            EmotionScalingMode.Capped,
            scalingLevel: 2);

        AssertDecision(decision, EmotionApplicationAction.Block);
    }

    [Fact]
    public void AmplifierRefreshesAtMaximumScalingLevel()
    {
        EmotionService service = CreateService(tierCount: 1);

        EmotionApplicationDecision decision = service.GetApplicationDecision(
            EmotionApplicationRequest.Amplifier,
            typeof(TestEmotionBuff),
            TierOneBuffType,
            EmotionScalingMode.Capped,
            EmotionStatTuning.PlayerMaxEmotionLevel);

        AssertDecision(decision, EmotionApplicationAction.RefreshCurrent, TierOneBuffType);
    }

    [Theory]
    [InlineData(EmotionApplicationRequest.RegularItem, EmotionApplicationAction.RefreshCurrent)]
    [InlineData(EmotionApplicationRequest.Amplifier, EmotionApplicationAction.Block)]
    internal void DisabledScalingOnlyAllowsRegularRefresh(
        EmotionApplicationRequest request,
        EmotionApplicationAction expectedAction)
    {
        EmotionService service = CreateService(tierCount: 1);

        EmotionApplicationDecision decision = service.GetApplicationDecision(
            request,
            typeof(TestEmotionBuff),
            TierOneBuffType,
            EmotionScalingMode.Disabled,
            scalingLevel: 1);

        AssertDecision(
            decision,
            expectedAction,
            expectedAction == EmotionApplicationAction.Block ? null : TierOneBuffType);
    }

    [Theory]
    [InlineData(
        EmotionApplicationRequest.RegularItem,
        TierOneBuffType,
        EmotionApplicationAction.PromoteNextTier,
        TierTwoBuffType)]
    [InlineData(
        EmotionApplicationRequest.RegularItem,
        TierTwoBuffType,
        EmotionApplicationAction.PromoteNextTier,
        TierThreeBuffType)]
    [InlineData(
        EmotionApplicationRequest.RegularItem,
        TierThreeBuffType,
        EmotionApplicationAction.RefreshCurrent,
        TierThreeBuffType)]
    [InlineData(
        EmotionApplicationRequest.Amplifier,
        TierOneBuffType,
        EmotionApplicationAction.Block,
        null)]
    [InlineData(
        EmotionApplicationRequest.Amplifier,
        TierTwoBuffType,
        EmotionApplicationAction.Block,
        null)]
    [InlineData(
        EmotionApplicationRequest.Amplifier,
        TierThreeBuffType,
        EmotionApplicationAction.Block,
        null)]
    internal void DisabledMultiTierEmotionUsesRegularItemsForAllProgression(
        EmotionApplicationRequest request,
        int currentBuffType,
        EmotionApplicationAction expectedAction,
        int? expectedBuffType)
    {
        EmotionService service = CreateService(tierCount: 3);

        EmotionApplicationDecision decision = service.GetApplicationDecision(
            request,
            typeof(TestEmotionBuff),
            currentBuffType,
            EmotionScalingMode.Disabled,
            scalingLevel: 3);

        AssertDecision(decision, expectedAction, expectedBuffType);
    }

    [Theory]
    [InlineData(
        EmotionApplicationRequest.RegularItem,
        TierOneBuffType,
        EmotionApplicationAction.PromoteNextTier,
        TierTwoBuffType)]
    [InlineData(
        EmotionApplicationRequest.Amplifier,
        TierOneBuffType,
        EmotionApplicationAction.Block,
        null)]
    [InlineData(
        EmotionApplicationRequest.RegularItem,
        TierTwoBuffType,
        EmotionApplicationAction.RefreshCurrent,
        TierTwoBuffType)]
    [InlineData(
        EmotionApplicationRequest.Amplifier,
        TierTwoBuffType,
        EmotionApplicationAction.PromoteNextTier,
        TierThreeBuffType)]
    [InlineData(
        EmotionApplicationRequest.RegularItem,
        TierThreeBuffType,
        EmotionApplicationAction.Block,
        null)]
    [InlineData(
        EmotionApplicationRequest.Amplifier,
        TierThreeBuffType,
        EmotionApplicationAction.AmplifyCurrent,
        TierThreeBuffType)]
    internal void ResolvesMultiTierProgressionActions(
        EmotionApplicationRequest request,
        int currentBuffType,
        EmotionApplicationAction expectedAction,
        int? expectedBuffType)
    {
        EmotionService service = CreateService(tierCount: 3);

        EmotionApplicationDecision decision = service.GetApplicationDecision(
            request,
            typeof(TestEmotionBuff),
            currentBuffType,
            EmotionScalingMode.Capped,
            scalingLevel: 3);

        AssertDecision(decision, expectedAction, expectedBuffType);
    }

    [Fact]
    public void NonStandardEmotionIsBlocked()
    {
        var registry = new FakeEmotionRegistry()
            .Add(
                TestEmotion,
                1,
                EmotionBuffVariant.NoTime,
                typeof(TestEmotionBuff),
                TierOneBuffType);
        var service = new EmotionService(registry);

        EmotionApplicationDecision decision = service.GetApplicationDecision(
            EmotionApplicationRequest.RegularItem,
            typeof(TestEmotionBuff),
            TierOneBuffType,
            EmotionScalingMode.Capped,
            scalingLevel: 1);

        AssertDecision(decision, EmotionApplicationAction.Block);
    }

    [Theory]
    [InlineData(EmotionScalingMode.Capped)]
    [InlineData(EmotionScalingMode.Disabled)]
    internal void MissingNextTierRegistrationIsBlocked(EmotionScalingMode scalingMode)
    {
        var registry = new FakeEmotionRegistry()
            .Add(
                TestEmotion,
                1,
                EmotionBuffVariant.Standard,
                typeof(TestEmotionBuff),
                TierOneBuffType)
            .Add(
                TestEmotion,
                3,
                EmotionBuffVariant.Standard,
                typeof(TestEmotionBuff),
                TierThreeBuffType);
        var service = new EmotionService(registry);

        EmotionApplicationDecision decision = service.GetApplicationDecision(
            EmotionApplicationRequest.RegularItem,
            typeof(TestEmotionBuff),
            TierOneBuffType,
            scalingMode,
            scalingLevel: 1);

        AssertDecision(decision, EmotionApplicationAction.Block);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(9999)]
    public void MissingRegistrationIsBlocked(int? currentBuffType)
    {
        var service = new EmotionService(new FakeEmotionRegistry());

        EmotionApplicationDecision decision = service.GetApplicationDecision(
            EmotionApplicationRequest.RegularItem,
            typeof(TestEmotionBuff),
            currentBuffType,
            EmotionScalingMode.Capped,
            scalingLevel: 0);

        AssertDecision(decision, EmotionApplicationAction.Block);
    }

    private static EmotionService CreateService(int tierCount)
    {
        var registry = new FakeEmotionRegistry()
            .Add(
                TestEmotion,
                1,
                EmotionBuffVariant.Standard,
                typeof(TestEmotionBuff),
                TierOneBuffType);

        if (tierCount >= 2)
        {
            registry.Add(
                TestEmotion,
                2,
                EmotionBuffVariant.Standard,
                typeof(TestEmotionBuff),
                TierTwoBuffType);
        }

        if (tierCount >= 3)
        {
            registry.Add(
                TestEmotion,
                3,
                EmotionBuffVariant.Standard,
                typeof(TestEmotionBuff),
                TierThreeBuffType);
        }

        return new EmotionService(registry);
    }

    private static void AssertDecision(
        EmotionApplicationDecision decision,
        EmotionApplicationAction expectedAction,
        int? expectedBuffType = null)
    {
        Assert.Equal(expectedAction, decision.Action);
        Assert.Equal(expectedBuffType, decision.BuffType);
        Assert.Equal(expectedAction != EmotionApplicationAction.Block, decision.CanApply);
    }

    private sealed class TestEmotionBuff : EmotionBuff
    {
    }
}