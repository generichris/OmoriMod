using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem;

namespace OmoriMod.Tests;

public sealed class EmotionPlayerScalingTests
{
    private const EmotionType TestEmotion = (EmotionType)4;
    private const EmotionType OtherTestEmotion = (EmotionType)5;

    [Fact]
    public void AmplificationInitializesAtFinalTierAndIncreasesOneLevel()
    {
        var player = new EmotionPlayer();

        bool increased = player.TryAmplifyEmotion(TestEmotion, finalTier: 1);

        Assert.True(increased);
        Assert.Equal(TestEmotion, player.ScalingEmotion);
        Assert.Equal(2, player.ScalingEmotionLevel);
        Assert.Equal(2, player.EmotionLevel);
    }

    [Fact]
    public void AmplificationSwitchesFamiliesBeforeIncreasing()
    {
        var player = new EmotionPlayer
        {
            ScalingEmotion = TestEmotion,
            ScalingEmotionLevel = 10,
            EmotionLevel = 10
        };

        bool increased = player.TryAmplifyEmotion(OtherTestEmotion, finalTier: 3);

        Assert.True(increased);
        Assert.Equal(OtherTestEmotion, player.ScalingEmotion);
        Assert.Equal(4, player.ScalingEmotionLevel);
        Assert.Equal(4, player.EmotionLevel);
    }

    [Fact]
    public void AmplificationDoesNotExceedMaximumLevel()
    {
        var player = new EmotionPlayer
        {
            ScalingEmotion = TestEmotion,
            ScalingEmotionLevel = EmotionStatTuning.PlayerMaxEmotionLevel,
            EmotionLevel = EmotionStatTuning.PlayerMaxEmotionLevel
        };

        bool increased = player.TryAmplifyEmotion(TestEmotion, finalTier: 1);

        Assert.False(increased);
        Assert.Equal(EmotionStatTuning.PlayerMaxEmotionLevel, player.ScalingEmotionLevel);
        Assert.Equal(EmotionStatTuning.PlayerMaxEmotionLevel, player.EmotionLevel);
    }

    [Fact]
    public void EnsureScalingEmotionDoesNotReduceExistingLevelForSameFamily()
    {
        var player = new EmotionPlayer
        {
            ScalingEmotion = TestEmotion,
            ScalingEmotionLevel = 10
        };

        player.EnsureScalingEmotion(TestEmotion, finalTier: 4);

        Assert.Equal(TestEmotion, player.ScalingEmotion);
        Assert.Equal(10, player.ScalingEmotionLevel);
    }

    [Fact]
    public void EnsureScalingEmotionRepairsLevelBelowFinalTier()
    {
        var player = new EmotionPlayer
        {
            ScalingEmotion = TestEmotion,
            ScalingEmotionLevel = 2
        };

        player.EnsureScalingEmotion(TestEmotion, finalTier: 4);

        Assert.Equal(TestEmotion, player.ScalingEmotion);
        Assert.Equal(4, player.ScalingEmotionLevel);
    }

    [Fact]
    public void AmplificationRepairsRetainedLevelAboveMaximum()
    {
        var player = new EmotionPlayer
        {
            ScalingEmotion = TestEmotion,
            ScalingEmotionLevel = int.MaxValue,
            EmotionLevel = int.MaxValue
        };

        bool increased = player.TryAmplifyEmotion(TestEmotion, finalTier: 4);

        Assert.False(increased);
        Assert.Equal(EmotionStatTuning.PlayerMaxEmotionLevel, player.ScalingEmotionLevel);
        Assert.Equal(EmotionStatTuning.PlayerMaxEmotionLevel, player.EmotionLevel);
    }
}
