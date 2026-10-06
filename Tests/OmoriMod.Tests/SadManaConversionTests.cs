using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Tests;

public sealed class SadManaConversionTests
{
    [Theory]
    [InlineData(100, 0.75f, 100, 75)]
    [InlineData(100, 0.75f, 50, 50)]
    [InlineData(100, 0.75f, 0, 0)]
    [InlineData(1, 0.75f, 100, 0)]
    [InlineData(2, 0.75f, 100, 1)]
    [InlineData(3, 0.50f, 100, 2)]
    [InlineData(100, 0f, 100, 0)]
    public void ConversionRespectsRoundingManaAndMinimumHealthDamage(
        int healthDamage,
        float conversionPercent,
        int availableMana,
        int expectedManaDamage)
    {
        Assert.Equal(
            expectedManaDamage,
            SadEmotionBase.CalculateManaDamage(
                healthDamage,
                conversionPercent,
                availableMana));
    }

    [Fact]
    public void ConversionBoundsExtremeDamageBeforeIntegerConversion()
    {
        Assert.Equal(
            int.MaxValue - 1,
            SadEmotionBase.CalculateManaDamage(
                int.MaxValue,
                conversionPercent: 1f,
                availableMana: int.MaxValue));
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void ConversionRejectsNonFinitePercentages(float conversionPercent)
    {
        Assert.Equal(
            0,
            SadEmotionBase.CalculateManaDamage(
                healthDamage: 100,
                conversionPercent,
                availableMana: 100));
    }
}
