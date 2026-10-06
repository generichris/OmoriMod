using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem;
using OmoriMod.Tests.Fixtures;

namespace OmoriMod.Tests;

public sealed class EmotionScalingSyncTests
    : IClassFixture<EmotionRegistryFixture>, IDisposable
{
    private readonly EmotionService _service;

    public EmotionScalingSyncTests(EmotionRegistryFixture fixture)
    {
        _service = fixture.Service;
        EmotionSystem.InitializeRegistry(fixture.Registry);
    }

    public void Dispose()
    {
        EmotionSystem.ResetRegistry();
    }

    [Theory]
    [InlineData(EmotionType.None, 0, true)]
    [InlineData(EmotionType.None, 1, false)]
    [InlineData(EmotionType.Happy, 1, false)]
    [InlineData(EmotionType.Happy, 2, true)]
    [InlineData(EmotionType.Happy, 0, false)]
    [InlineData(EmotionType.Happy, EmotionStatTuning.PlayerMaxEmotionLevel + 1, false)]
    [InlineData((EmotionType)255, 1, false)]
    internal void ClientSnapshotValidationChecksStructure(
        EmotionType emotion,
        int level,
        bool expected)
    {
        Assert.Equal(
            expected,
            _service.IsValidScalingSync(
                emotion,
                level,
                EmotionType.None,
                currentLevel: 0,
                activeBuffType: null,
                EmotionScalingMode.Disabled,
                requireActiveBuff: false));
    }

    [Fact]
    public void ClientSnapshotRejectsEmotionWithoutFinalTierMetadata()
    {
        var service = new EmotionService(new TestDoubles.FakeEmotionRegistry());

        Assert.False(
            service.IsValidScalingSync(
                EmotionType.Happy,
                incomingLevel: 2,
                EmotionType.None,
                currentLevel: 0,
                activeBuffType: null,
                EmotionScalingMode.Disabled,
                requireActiveBuff: false));
    }

    [Theory]
    [InlineData(EmotionType.None, 0, 2, true)]
    [InlineData(EmotionType.None, 0, 3, true)]
    [InlineData(EmotionType.None, 0, 4, false)]
    [InlineData(EmotionType.Happy, 2, 2, true)]
    [InlineData(EmotionType.Happy, 2, 3, true)]
    [InlineData(EmotionType.Happy, 2, 4, false)]
    [InlineData(EmotionType.Happy, 3, 2, false)]
    [InlineData(EmotionType.Angry, 10, 2, true)]
    [InlineData(EmotionType.Angry, 10, 3, true)]
    [InlineData(
        EmotionType.Happy,
        EmotionStatTuning.PlayerMaxEmotionLevel,
        EmotionStatTuning.PlayerMaxEmotionLevel,
        true)]
    internal void ServerValidationAllowsInitializationOrOneLevelIncrease(
        EmotionType currentEmotion,
        int currentLevel,
        int incomingLevel,
        bool expected)
    {
        Assert.Equal(
            expected,
            _service.IsValidScalingSync(
                EmotionType.Happy,
                incomingLevel,
                currentEmotion,
                currentLevel,
                EmotionRegistryFixture.BuffTypes.HappyTierTwo,
                EmotionScalingMode.Capped,
                requireActiveBuff: true));
    }

    [Theory]
    [InlineData(null, EmotionScalingMode.Capped)]
    [InlineData(EmotionRegistryFixture.BuffTypes.AngryTierTwo, EmotionScalingMode.Capped)]
    [InlineData(EmotionRegistryFixture.BuffTypes.HappyTierOne, EmotionScalingMode.Capped)]
    [InlineData(EmotionRegistryFixture.BuffTypes.SadNoTime, EmotionScalingMode.Capped)]
    [InlineData(EmotionRegistryFixture.BuffTypes.HappyTierTwo, EmotionScalingMode.Disabled)]
    internal void ServerValidationRejectsInvalidActiveBuff(
        int? activeBuffType,
        EmotionScalingMode scalingMode)
    {
        Assert.False(
            _service.IsValidScalingSync(
                EmotionType.Happy,
                incomingLevel: 2,
                EmotionType.None,
                currentLevel: 0,
                activeBuffType,
                scalingMode,
                requireActiveBuff: true));
    }

    [Fact]
    public void ServerRejectsScalingResetWhileCappedFinalEmotionIsActive()
    {
        Assert.False(
            _service.IsValidScalingSync(
                EmotionType.None,
                incomingLevel: 0,
                EmotionType.Happy,
                currentLevel: 5,
                EmotionRegistryFixture.BuffTypes.HappyTierTwo,
                EmotionScalingMode.Capped,
                requireActiveBuff: true));
    }

    [Theory]
    [InlineData(null, EmotionScalingMode.Capped)]
    [InlineData(EmotionRegistryFixture.BuffTypes.HappyTierTwo, EmotionScalingMode.Disabled)]
    [InlineData(EmotionRegistryFixture.BuffTypes.SadNoTime, EmotionScalingMode.Capped)]
    public void ServerAllowsScalingResetWithoutActiveCappedFinalEmotion(
        int? activeBuffType,
        EmotionScalingMode scalingMode)
    {
        Assert.True(
            _service.IsValidScalingSync(
                EmotionType.None,
                incomingLevel: 0,
                EmotionType.Happy,
                currentLevel: 5,
                activeBuffType,
                scalingMode,
                requireActiveBuff: true));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void PreferredEmotionBuffIsIndependentOfBuffSlotOrder(bool noTimeFirst)
    {
        int standardBuffType = EmotionRegistryFixture.BuffTypes.SadTierTwo;
        int noTimeBuffType = EmotionRegistryFixture.BuffTypes.SadNoTime;
        var player = new Terraria.Player();
        player.buffType[0] = noTimeFirst ? noTimeBuffType : standardBuffType;
        player.buffType[1] = noTimeFirst ? standardBuffType : noTimeBuffType;

        Assert.True(EmotionSystem.IsPreferredEmotionBuff(player, standardBuffType));
        Assert.False(EmotionSystem.IsPreferredEmotionBuff(player, noTimeBuffType));
    }

    [Fact]
    public void ClientSnapshotUpdatesAndResetsRetainedPlayerState()
    {
        var player = new EmotionPlayer();

        Assert.True(player.TryApplySyncedScalingState(
            EmotionType.Happy,
            scalingLevel: 5,
            requireActiveBuff: false));
        Assert.Equal(EmotionType.Happy, player.ScalingEmotion);
        Assert.Equal(5, player.ScalingEmotionLevel);

        Assert.True(player.TryApplySyncedScalingState(
            EmotionType.None,
            scalingLevel: 0,
            requireActiveBuff: false));
        Assert.Equal(EmotionType.None, player.ScalingEmotion);
        Assert.Equal(0, player.ScalingEmotionLevel);
    }

    [Fact]
    public void InvalidClientSnapshotDoesNotChangeRetainedPlayerState()
    {
        var player = new EmotionPlayer
        {
            ScalingEmotion = EmotionType.Happy,
            ScalingEmotionLevel = 5
        };

        Assert.False(player.TryApplySyncedScalingState(
            (EmotionType)255,
            scalingLevel: 6,
            requireActiveBuff: false));
        Assert.Equal(EmotionType.Happy, player.ScalingEmotion);
        Assert.Equal(5, player.ScalingEmotionLevel);
    }

    [Fact]
    public void CopyClientStateCopiesRetainedScalingState()
    {
        var source = new EmotionPlayer
        {
            ScalingEmotion = EmotionType.Sad,
            ScalingEmotionLevel = 7
        };
        var copy = new EmotionPlayer();

        source.CopyClientState(copy);

        Assert.Equal(EmotionType.Sad, copy.ScalingEmotion);
        Assert.Equal(7, copy.ScalingEmotionLevel);
    }

}
