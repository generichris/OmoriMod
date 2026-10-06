using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Systems.EmotionSystem;
using OmoriMod.Tests.Fixtures;
using OmoriMod.Tests.TestDoubles;

namespace OmoriMod.Tests;

public sealed class EmotionServiceTests(EmotionRegistryFixture fixture) : IClassFixture<EmotionRegistryFixture>
{
    private readonly EmotionService _service = fixture.Service;

    [Fact]
    public void ThrowsWhenGivenNullEmotionRegistry()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new EmotionService(null!));

        Assert.Equal("registry", exception.ParamName);
    }

    [Fact]
    public void EmptyServiceReturnsSafeDefaults()
    {
        EmotionService service = EmotionService.Empty;
        var entity = TerrariaEntityTestDoubles.CreateEmotionPlayer(EmotionType.Happy, tier: 3);

        Assert.Null(service.GetEmotionBuffType(EmotionType.Happy, 1));
        Assert.Null(service.GetEmotionBuffType<HappyEmotionBase>(1));
        Assert.Null(service.GetNextTierEmotionType(EmotionType.Happy, 1));
        Assert.Null(service.GetNextTierEmotionType(entity.ActiveEmotionBuff));
        Assert.Null(service.GetEmotionTier(entity.ActiveEmotionBuff.Type));
        Assert.Null(service.GetMaxEmotionTier(EmotionType.Happy));
        Assert.False(service.IsFinalEmotionTier(entity.ActiveEmotionBuff.Type));
        Assert.Null(service.GetEmotionVariant(entity.ActiveEmotionBuff.Type));
        Assert.Equal(3, service.GetEmotionTier(entity));
    }

    [Fact]
    public void ResolvesStandardAndNoTimeRegistrations()
    {
        Assert.Equal(
            EmotionRegistryFixture.BuffTypes.SadTierOne,
            _service.GetEmotionBuffType(EmotionType.Sad, 1));
        Assert.Equal(
            EmotionRegistryFixture.BuffTypes.SadNoTime,
            _service.GetEmotionBuffType(EmotionType.Sad, 1, EmotionBuffVariant.NoTime));
    }

    [Fact]
    public void ResolvesRegistrationsByEmotionFamily()
    {
        Assert.Equal(
            EmotionRegistryFixture.BuffTypes.AngryTierTwo,
            _service.GetEmotionBuffType<AngryEmotionBase>(2));
    }

    [Fact]
    public void ReportsProgressionAndFinalTierMetadata()
    {
        Assert.Equal(
            EmotionRegistryFixture.BuffTypes.HappyTierTwo,
            _service.GetNextTierEmotionType(EmotionType.Happy, 1));
        Assert.Equal(2, _service.GetMaxEmotionTier(EmotionType.Happy));
        Assert.True(_service.IsFinalEmotionTier(EmotionRegistryFixture.BuffTypes.HappyTierTwo));
        Assert.False(_service.IsFinalEmotionTier(EmotionRegistryFixture.BuffTypes.HappyTierOne));
        Assert.Equal(
            EmotionBuffVariant.Standard,
            _service.GetEmotionVariant(EmotionRegistryFixture.BuffTypes.HappyTierOne));
    }

    [Fact]
    public void TreatsSingleTierEmotionAsBothStartingAndFinalTier()
    {
        const EmotionType testEmotion = (EmotionType)4;
        const int testEmotionBuffType = 4001;
        var registry = new FakeEmotionRegistry()
            .Add(
                testEmotion,
                1,
                EmotionBuffVariant.Standard,
                typeof(TestEmotionBuff),
                testEmotionBuffType);
        var service = new EmotionService(registry);

        Assert.Equal(testEmotionBuffType, service.GetEmotionBuffType(testEmotion, 1));
        Assert.Equal(1, service.GetMaxEmotionTier(testEmotion));
        Assert.True(service.IsFinalEmotionTier(testEmotionBuffType));
        Assert.Null(service.GetNextTierEmotionType(testEmotion, 1));
    }

    [Fact]
    public void ResolvesNextTierFromRegisteredEmotionBuff()
    {
        var registry = new FakeEmotionRegistry()
            .Add(EmotionType.Happy, 1, EmotionBuffVariant.Standard, typeof(HappyEmotionBase), buffType: 0)
            .Add(EmotionType.Happy, 2, EmotionBuffVariant.Standard, typeof(HappyEmotionBase), buffType: 1);
        var service = new EmotionService(registry);
        var entity = TerrariaEntityTestDoubles.CreateEmotionPlayer(EmotionType.Happy, tier: 7);

        Assert.Equal(1, service.GetNextTierEmotionType(entity.ActiveEmotionBuff));
        Assert.Equal(1, service.GetEmotionTier(entity));
    }

    [Fact]
    public void ReturnsNullWhenCurrentEmotionBuffIsAtFinalTier()
    {
        var registry = new FakeEmotionRegistry()
            .Add(EmotionType.Happy, 2, EmotionBuffVariant.Standard, typeof(HappyEmotionBase), buffType: 0);
        var service = new EmotionService(registry);
        var entity = TerrariaEntityTestDoubles.CreateEmotionPlayer(EmotionType.Happy, tier: 2);

        Assert.Null(service.GetNextTierEmotionType(entity.ActiveEmotionBuff));
    }

    [Fact]
    public void ReturnsNullWhenCurrentBuffHasTierButNoRegisteredEmotion()
    {
        var registry = new FakeEmotionRegistry()
            .Add(EmotionType.Happy, 1, EmotionBuffVariant.Standard, typeof(HappyEmotionBase), buffType: 0);
        var service = new EmotionService(new RegistryWithoutEmotionMetadata(registry));
        var entity = TerrariaEntityTestDoubles.CreateEmotionPlayer(EmotionType.Happy, tier: 1);

        Assert.Null(service.GetNextTierEmotionType(entity.ActiveEmotionBuff));
    }

    [Fact]
    public void ReturnsZeroTierWhenEntityHasNoActiveEmotion()
    {
        var entity = TerrariaEntityTestDoubles.CreateEmotionPlayer(EmotionType.None, tier: 0);
        entity.ActiveEmotionBuff = null!;

        Assert.Equal(0, _service.GetEmotionTier(entity));
    }

    [Fact]
    public void ReturnsNullForMissingRegistrations()
    {
        Assert.Null(_service.GetEmotionBuffType(EmotionType.None, 1));
        Assert.Null(_service.GetNextTierEmotionType(EmotionType.Angry, 2));
        Assert.Null(_service.GetEmotionTier(-1));
        Assert.Null(_service.GetMaxEmotionTier(EmotionType.None));
        Assert.Null(_service.GetEmotionVariant(-1));
    }

    [Fact]
    public void CalculatesPlayerAdvantageFromEmotionTriangleAndTierDistance()
    {
        var happyPlayer = TerrariaEntityTestDoubles.CreateEmotionPlayer(EmotionType.Happy, tier: 1);
        var angryNpc = TerrariaEntityTestDoubles.CreateEmotionNpc(EmotionType.Angry, tier: 2);

        Assert.Equal(2, _service.CalculateAdvantage(happyPlayer, angryNpc));
    }

    [Fact]
    public void CalculatesNpcDisadvantageFromEmotionTriangleAndTierDistance()
    {
        var sadNpc = TerrariaEntityTestDoubles.CreateEmotionNpc(EmotionType.Sad, tier: 2);
        var angryPlayer = TerrariaEntityTestDoubles.CreateEmotionPlayer(EmotionType.Angry, tier: 1);

        Assert.Equal(-2, _service.CalculateAdvantage(sadNpc, angryPlayer));
    }

    [Fact]
    public void ReturnsZeroAdvantageForMatchingEmotions()
    {
        var attacker = TerrariaEntityTestDoubles.CreateEmotionPlayer(EmotionType.Happy, tier: 1);
        var defender = TerrariaEntityTestDoubles.CreateEmotionNpc(EmotionType.Happy, tier: 2);

        Assert.Equal(0, _service.CalculateAdvantage(attacker, defender));
    }

    [Fact]
    public void AdvantageUsesRegisteredTierInsteadOfScaledPlayerLevel()
    {
        var attacker = TerrariaEntityTestDoubles.CreateEmotionPlayer(EmotionType.Happy, tier: 2);
        attacker.EmotionLevel = EmotionStatTuning.PlayerMaxEmotionLevel;
        var defender = TerrariaEntityTestDoubles.CreateEmotionNpc(EmotionType.Angry, tier: 2);

        Assert.Equal(1, _service.CalculateAdvantage(attacker, defender));
    }

    [Theory]
    [InlineData(EmotionType.Sad, EmotionType.Happy, 1)]
    [InlineData(EmotionType.Sad, EmotionType.Angry, -1)]
    [InlineData(EmotionType.Angry, EmotionType.Sad, 1)]
    [InlineData(EmotionType.Angry, EmotionType.Happy, -1)]
    [InlineData(EmotionType.Happy, EmotionType.Angry, 1)]
    [InlineData(EmotionType.Happy, EmotionType.Sad, -1)]
    [InlineData(EmotionType.None, EmotionType.Angry, 0)]
    public void CalculatesEveryEmotionTriangleOutcome(
        EmotionType attackerEmotion,
        EmotionType defenderEmotion,
        int expectedAdvantage)
    {
        var attacker = TerrariaEntityTestDoubles.CreateEmotionPlayer(attackerEmotion, tier: 1);
        var defender = TerrariaEntityTestDoubles.CreateEmotionNpc(defenderEmotion, tier: 1);

        Assert.Equal(expectedAdvantage, _service.CalculateAdvantage(attacker, defender));
    }

    private sealed class RegistryWithoutEmotionMetadata(IEmotionRegistry registry) : IEmotionRegistry
    {
        public int? GetEmotionBuffType(
            EmotionType emotion,
            int emotionLevel,
            EmotionBuffVariant variant = EmotionBuffVariant.Standard)
        {
            return registry.GetEmotionBuffType(emotion, emotionLevel, variant);
        }

        public int? GetEmotionBuffType(
            Type familyType,
            int emotionLevel,
            EmotionBuffVariant variant = EmotionBuffVariant.Standard)
        {
            return registry.GetEmotionBuffType(familyType, emotionLevel, variant);
        }

        public EmotionType? GetEmotion(int buffType) => null;

        public int? GetEmotionTier(int buffType) => registry.GetEmotionTier(buffType);

        public int? GetMaxEmotionTier(EmotionType emotion) => registry.GetMaxEmotionTier(emotion);

        public bool IsFinalEmotionTier(int buffType) => registry.IsFinalEmotionTier(buffType);

        public EmotionBuffVariant? GetEmotionVariant(int buffType) => registry.GetEmotionVariant(buffType);
    }

    private sealed class TestEmotionBuff : EmotionBuff
    {
    }
}
