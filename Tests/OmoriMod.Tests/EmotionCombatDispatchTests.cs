using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem;
using OmoriMod.Tests.TestDoubles;

using Terraria;

namespace OmoriMod.Tests;

public sealed class EmotionCombatDispatchTests
{
    [Fact]
    public void PlayerToNpcDispatchesPlayerOutgoingAndPlayerHitNpcHooks()
    {
        var buff = new SpyEmotionBuff();
        EmotionPlayer attacker = CreatePlayerAttacker(buff);
        var defender = TerrariaEntityTestDoubles.CreateEmotionNpc(EmotionType.Happy, tier: 1);
        NPC.HitModifiers modifiers = default;

        EmotionSystem.ApplyCombatModifiers(attacker, defender, ref modifiers);

        Assert.Equal(1, buff.PlayerOutgoingNpcCalls);
        Assert.Equal(1, buff.PlayerHitNpcCalls);
        Assert.Equal(0, buff.NpcHitNpcCalls);
    }

    [Fact]
    public void PlayerToPlayerDispatchesPlayerOutgoingAndPlayerHitPlayerHooks()
    {
        var buff = new SpyEmotionBuff();
        EmotionPlayer attacker = CreatePlayerAttacker(buff);
        var defender = TerrariaEntityTestDoubles.CreateEmotionPlayer(EmotionType.Happy, tier: 1);
        Player.HurtModifiers modifiers = default;

        EmotionSystem.ApplyCombatModifiers(attacker, defender, ref modifiers);

        Assert.Equal(1, buff.PlayerOutgoingPlayerCalls);
        Assert.Equal(1, buff.PlayerHitPlayerCalls);
        Assert.Equal(0, buff.NpcOutgoingPlayerCalls);
    }

    [Fact]
    public void NpcToNpcDispatchesOnlyNpcHitNpcHook()
    {
        var buff = new SpyEmotionBuff();
        var attacker = TerrariaEntityTestDoubles.CreateEmotionNpc(EmotionType.Happy, tier: 1);
        attacker.ActiveEmotionBuff = buff;
        var defender = TerrariaEntityTestDoubles.CreateEmotionNpc(EmotionType.Happy, tier: 1);
        NPC.HitModifiers modifiers = default;

        EmotionSystem.ApplyCombatModifiers(attacker, defender, ref modifiers);

        Assert.Equal(1, buff.NpcHitNpcCalls);
        Assert.Equal(0, buff.PlayerOutgoingNpcCalls);
        Assert.Equal(0, buff.PlayerHitNpcCalls);
    }

    [Fact]
    public void NpcToPlayerDispatchesOnlyNpcOutgoingHook()
    {
        var buff = new SpyEmotionBuff();
        var attacker = TerrariaEntityTestDoubles.CreateEmotionNpc(EmotionType.Happy, tier: 1);
        attacker.ActiveEmotionBuff = buff;
        var defender = TerrariaEntityTestDoubles.CreateEmotionPlayer(EmotionType.Happy, tier: 1);
        Player.HurtModifiers modifiers = default;

        EmotionSystem.ApplyCombatModifiers(attacker, defender, ref modifiers);

        Assert.Equal(1, buff.NpcOutgoingPlayerCalls);
        Assert.Equal(0, buff.PlayerOutgoingPlayerCalls);
        Assert.Equal(0, buff.PlayerHitPlayerCalls);
    }

    private static EmotionPlayer CreatePlayerAttacker(SpyEmotionBuff buff)
    {
        EmotionPlayer attacker = TerrariaEntityTestDoubles.CreateEmotionPlayer(
            EmotionType.Happy,
            tier: 1);
        attacker.ActiveEmotionBuff = buff;
        return attacker;
    }

    private sealed class SpyEmotionBuff : EmotionBuff
    {
        internal int PlayerOutgoingNpcCalls { get; private set; }
        internal int PlayerHitNpcCalls { get; private set; }
        internal int PlayerOutgoingPlayerCalls { get; private set; }
        internal int PlayerHitPlayerCalls { get; private set; }
        internal int NpcHitNpcCalls { get; private set; }
        internal int NpcOutgoingPlayerCalls { get; private set; }

        internal SpyEmotionBuff()
        {
            Emotion = EmotionType.Happy;
            EmotionTier = 1;
        }

        public override void ModifyPlayerOutgoingDamage(
            int emotionLevel,
            ref NPC.HitModifiers modifiers)
        {
            PlayerOutgoingNpcCalls++;
        }

        public override void ModifyPlayerHitNpc(
            int emotionLevel,
            ref NPC.HitModifiers modifiers)
        {
            PlayerHitNpcCalls++;
        }

        public override void ModifyPlayerOutgoingDamage(
            int emotionLevel,
            ref Player.HurtModifiers modifiers)
        {
            PlayerOutgoingPlayerCalls++;
        }

        public override void ModifyPlayerHitPlayer(
            int emotionLevel,
            ref Player.HurtModifiers modifiers)
        {
            PlayerHitPlayerCalls++;
        }

        public override void ModifyNpcHitNpc(
            int emotionLevel,
            ref NPC.HitModifiers modifiers)
        {
            NpcHitNpcCalls++;
        }

        public override void ModifyNpcOutgoingDamage(
            int emotionLevel,
            ref Player.HurtModifiers modifiers)
        {
            NpcOutgoingPlayerCalls++;
        }
    }
}
