using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.NPCs.Global;
using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem;

using Terraria;

namespace OmoriMod.Tests.TestDoubles;

/// <summary>
/// Creates fresh Terraria entities with useful unit-test scenarios.
/// </summary>
/// <remarks>
/// These raw entities are not attached to tModLoader's content loaders. Unit tests must not call
/// <see cref="Player.GetModPlayer{T}"/> or <see cref="NPC.GetGlobalNPC{T}"/> on them.
/// Use the emotion-state factories for tests that consume emotion components directly.
/// </remarks>
internal static class TerrariaEntityTestDoubles
{
    internal static Player CreateHealthyPlayer()
    {
        return CreatePlayer(currentMana: 100);
    }

    internal static Player CreateLowManaPlayer()
    {
        return CreatePlayer(currentMana: 10);
    }

    internal static NPC CreateRegularNpc()
    {
        return CreateNpc(life: 100, damage: 20, defense: 10, isBoss: false);
    }

    internal static NPC CreateBossNpc()
    {
        return CreateNpc(life: 1000, damage: 50, defense: 20, isBoss: true);
    }

    internal static EmotionPlayer CreateEmotionPlayer(EmotionType emotion, int tier)
    {
        return new EmotionPlayer
        {
            Emotion = emotion,
            EmotionLevel = tier,
            ActiveEmotionBuff = new TestEmotionBuff(emotion, tier)
        };
    }

    internal static EmotionNPC CreateEmotionNpc(EmotionType emotion, int tier)
    {
        return new EmotionNPC
        {
            Emotion = emotion,
            EmotionLevel = tier,
            ActiveEmotionBuff = new TestEmotionBuff(emotion, tier)
        };
    }

    private static Player CreatePlayer(int currentMana)
    {
        return new Player
        {
            active = true,
            statLife = 100,
            statLifeMax = 100,
            statLifeMax2 = 100,
            statMana = currentMana,
            statManaMax = 100,
            statManaMax2 = 100
        };
    }

    private static NPC CreateNpc(int life, int damage, int defense, bool isBoss)
    {
        return new NPC
        {
            active = true,
            life = life,
            lifeMax = life,
            damage = damage,
            defDamage = damage,
            defense = defense,
            defDefense = defense,
            boss = isBoss
        };
    }

    private sealed class TestEmotionBuff : EmotionBuff
    {
        internal TestEmotionBuff(EmotionType emotion, int tier)
        {
            Emotion = emotion;
            EmotionTier = tier;
        }
    }
}