using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Tests.TestDoubles;

namespace OmoriMod.Tests;

public sealed class EmotionStatScalingTests
{
    [Theory]
    [InlineData(1, 0.09f)]
    [InlineData(3, 0.23f)]
    [InlineData(4, 0.23975f)]
    [InlineData(43, 0.60f)]
    [InlineData(100, 0.60f)]
    public void AngryPlayerDamageCurveUsesLinearInterpolationAndCap(
        int emotionLevel,
        float expected)
    {
        Assert.Equal(
            expected,
            AngryEmotionBase.GetPlayerDamageIncreasePercent(emotionLevel),
            precision: 5);
    }

    [Fact]
    public void PlayerMovementModifiersUseTheirFamilyCurves()
    {
        var happyPlayer = TerrariaEntityTestDoubles.CreateHealthyPlayer();
        happyPlayer.moveSpeed = 2f;
        var sadPlayer = TerrariaEntityTestDoubles.CreateHealthyPlayer();
        sadPlayer.moveSpeed = 2f;

        new TestHappyEmotion().ModifyPlayerMovement(happyPlayer, emotionLevel: 1);
        new TestSadEmotion().ModifyPlayerMovement(sadPlayer, emotionLevel: 1);

        Assert.Equal(2.2f, happyPlayer.moveSpeed, precision: 5);
        Assert.Equal(1.78f, sadPlayer.moveSpeed, precision: 5);
    }

    [Fact]
    public void NpcDefenseModifiersUseNpcSpecificTuning()
    {
        var angryNpc = TerrariaEntityTestDoubles.CreateRegularNpc();
        angryNpc.defDefense = 100;
        var sadNpc = TerrariaEntityTestDoubles.CreateRegularNpc();
        sadNpc.defDefense = 100;

        new TestAngryEmotion().ModifyNpcDefense(angryNpc, emotionLevel: 1);
        new TestSadEmotion().ModifyNpcDefense(sadNpc, emotionLevel: 1);

        Assert.Equal(88, angryNpc.defense);
        Assert.Equal(112, sadNpc.defense);
    }

    [Fact]
    public void NpcCurvesClampAtConfiguredMaximumLevel()
    {
        Assert.Equal(
            AngryEmotionBase.GetNpcDamageIncreasePercent(1),
            AngryEmotionBase.GetNpcDamageIncreasePercent(4));
        Assert.Equal(
            AngryEmotionBase.GetNpcDefenseDecreasePercent(1),
            AngryEmotionBase.GetNpcDefenseDecreasePercent(4));
        Assert.Equal(
            HappyEmotionBase.GetNpcMovementSpeedIncreasePercent(1),
            HappyEmotionBase.GetNpcMovementSpeedIncreasePercent(4));
        Assert.Equal(
            HappyEmotionBase.GetNpcExtraCritChancePercent(1),
            HappyEmotionBase.GetNpcExtraCritChancePercent(4));
        Assert.Equal(
            HappyEmotionBase.GetNpcMissChancePercent(1),
            HappyEmotionBase.GetNpcMissChancePercent(4));
        Assert.Equal(
            SadEmotionBase.GetNpcDefenseIncreasePercent(1),
            SadEmotionBase.GetNpcDefenseIncreasePercent(4));
        Assert.Equal(
            SadEmotionBase.GetNpcMovementSpeedDecreasePercent(1),
            SadEmotionBase.GetNpcMovementSpeedDecreasePercent(4));
    }

    [Fact]
    public void CurvesClampNegativeLevelsToZero()
    {
        Assert.Equal(
            AngryEmotionBase.GetPlayerDamageIncreasePercent(0),
            AngryEmotionBase.GetPlayerDamageIncreasePercent(-1));
        Assert.Equal(
            SadEmotionBase.GetNpcDefenseIncreasePercent(0),
            SadEmotionBase.GetNpcDefenseIncreasePercent(-1));
    }

    private sealed class TestAngryEmotion : AngryEmotionBase
    {
    }

    private sealed class TestHappyEmotion : HappyEmotionBase
    {
    }

    private sealed class TestSadEmotion : SadEmotionBase
    {
    }
}
