using System;

using Microsoft.Xna.Framework;

using OmoriMod.Systems.EmotionSystem;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Buffs.Abstract;

public abstract class HappyEmotionBase : EmotionBuff
{
    // Player Configuration

    // ===== Movement Speed Increase =====
    private const float PLAYER_MOVEMENT_SPEED_INCREASE_MAX = 50.0f;
    private const float PLAYER_MOVEMENT_SPEED_INCREASE_RATE = 5.0f;
    private const float PLAYER_MOVEMENT_SPEED_INCREASE_STARTING_VALUE = 5.0f;

    // ===== Crit Chance Increase =====
    private const float PLAYER_EXTRA_CRIT_CHANCE_MAX = 60.0f;
    private const float PLAYER_EXTRA_CRIT_CHANCE_RATE = 08.0f;
    private const float PLAYER_EXTRA_CRIT_CHANCE_STARTING_VALUE = 3.5f;

    // ===== Miss Chance =====
    private const float PLAYER_MISS_CHANCE_MAX = 70.0f;
    private const float PLAYER_MISS_CHANCE_RATE = 09.5f;
    private const float PLAYER_MISS_CHANCE_STARTING_VALUE = 4.0f;



    // NPC Configuration

    // ===== Movement Speed Increase =====
    private const float NPC_MOVEMENT_SPEED_INCREASE_MAX = 35.0f;
    private const float NPC_MOVEMENT_SPEED_INCREASE_RATE = 3.5f;
    private const float NPC_MOVEMENT_SPEED_INCREASE_STARTING_VALUE = 4.5f;

    // ===== Crit Chance Increase =====
    private const float NPC_EXTRA_CRIT_CHANCE_MAX = 50.0f;
    private const float NPC_EXTRA_CRIT_CHANCE_RATE = 6.0f;
    private const float NPC_EXTRA_CRIT_CHANCE_STARTING_VALUE = 3.0f;

    // ===== Miss Chance =====
    private const float NPC_MISS_CHANCE_MAX = 55.0f;
    private const float NPC_MISS_CHANCE_RATE = 8.0f;
    private const float NPC_MISS_CHANCE_STARTING_VALUE = 4.0f;



    // movement speed
    public float PLAYER_MOVEMENT_SPEED_INCREASE_PERCENT => LinearPerLevel(
        max: PLAYER_MOVEMENT_SPEED_INCREASE_MAX,
        rate: PLAYER_MOVEMENT_SPEED_INCREASE_RATE,
        maxEmotionLevel: EmotionSystem.PLAYER_MAX_EMOTION_LEVEL,
        startingValue: PLAYER_MOVEMENT_SPEED_INCREASE_STARTING_VALUE
        );

    public float NPC_MOVEMENT_SPEED_INCREASE_PERCENT => LinearPerLevel(
        max: NPC_MOVEMENT_SPEED_INCREASE_MAX,
        rate: NPC_MOVEMENT_SPEED_INCREASE_RATE,
        maxEmotionLevel: EmotionSystem.NPC_MAX_EMOTION_LEVEL,
        startingValue: NPC_MOVEMENT_SPEED_INCREASE_STARTING_VALUE
        );


    // crit chance increase
    public float PLAYER_EXTRA_CRIT_CHANCE_PERCENT => LinearPerLevel(
        max: PLAYER_EXTRA_CRIT_CHANCE_MAX,
        rate: PLAYER_EXTRA_CRIT_CHANCE_RATE,
        maxEmotionLevel: EmotionSystem.PLAYER_MAX_EMOTION_LEVEL,
        startingValue: PLAYER_EXTRA_CRIT_CHANCE_STARTING_VALUE
        );
    public float NPC_EXTRA_CRIT_CHANCE_PERCENT => LinearPerLevel(
        max: NPC_EXTRA_CRIT_CHANCE_MAX,
        rate: NPC_EXTRA_CRIT_CHANCE_RATE,
        maxEmotionLevel: EmotionSystem.NPC_MAX_EMOTION_LEVEL,
        startingValue: NPC_EXTRA_CRIT_CHANCE_STARTING_VALUE
        );

    // miss chance
    public float PLAYER_MISS_CHANCE_PERCENT => LinearPerLevel(
        max: PLAYER_MISS_CHANCE_MAX,
        rate: PLAYER_MISS_CHANCE_RATE,
        maxEmotionLevel: EmotionSystem.PLAYER_MAX_EMOTION_LEVEL,
        startingValue: PLAYER_MISS_CHANCE_STARTING_VALUE
        );
    public float NPC_MISS_CHANCE_PERCENT => LinearPerLevel(
        max: NPC_MISS_CHANCE_MAX,
        rate: NPC_MISS_CHANCE_RATE,
        maxEmotionLevel: EmotionSystem.NPC_MAX_EMOTION_LEVEL,
        startingValue: NPC_MISS_CHANCE_STARTING_VALUE
        );

    public HappyEmotionBase()
    {
        Emotion = EmotionType.HAPPY;
        dustColor = Color.Yellow;
    }

    public override void UpdateEmotionBuff(Player player, ref int buffIndex)
    {
        EmotionSystem.RemoveIncompatibleEmotions<HappyEmotionBase>(player);
        ModifyPlayerMovement(player);
    }

    public override void UpdateEmotionBuff(NPC npc, ref int buffIndex)
    {
        EmotionSystem.RemoveIncompatibleEmotions<HappyEmotionBase>(npc);
        ModifyNPCMovement(npc);
    }

    public override void ModifyPlayerMovement(Player player)
    {
        player.moveSpeed *= 1 + PLAYER_MOVEMENT_SPEED_INCREASE_PERCENT;
    }

    public override void ModifyNPCMovement(NPC npc)
    {
        float modifier = NPC_MOVEMENT_SPEED_INCREASE_PERCENT;
        Vector2 change;
        if (npc.noGravity) { change = npc.velocity * modifier; }
        else { change = new Vector2(npc.velocity.X * modifier, 0); }
        Vector2 newPos = npc.position + change;

        // If the new speed collides with something, don't add it
        if (!Collision.SolidCollision(newPos, npc.width, npc.height))
        {
            npc.position = newPos;
        }
    }

    public override void ModifyPlayerHitNPC(ref NPC.HitModifiers modifiers)
    {
        // miss chance
        var noDMG = new StatModifier();
        noDMG *= 0;
        noDMG -= 1;
        modifiers.SourceDamage = Main.rand.NextFloat() < PLAYER_MISS_CHANCE_PERCENT ? noDMG : modifiers.SourceDamage;

        // extra crit chance
        if (Main.rand.NextFloat() < PLAYER_EXTRA_CRIT_CHANCE_PERCENT)
        {
            modifiers.SetCrit();
        }
    }

    public override void ModifyPlayerHitPlayer(ref Player.HurtModifiers modifiers)
    {
        // miss chance
        var noDMG = new StatModifier();
        noDMG *= 0;
        noDMG -= 1;
        modifiers.SourceDamage = Main.rand.NextFloat() < PLAYER_MISS_CHANCE_PERCENT ? noDMG : modifiers.SourceDamage;

        // extra crit chance
        if (Main.rand.NextFloat() < PLAYER_EXTRA_CRIT_CHANCE_PERCENT && modifiers.SourceDamage != noDMG)
        {
            modifiers.SourceDamage *= 1.5f;
        }
    }

    public virtual void HappyModifyBuffText(ref string buffName, ref string tip, ref int rare) { }
    public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
    {
        int speedUp = (int)MathF.Round(PLAYER_MOVEMENT_SPEED_INCREASE_PERCENT * 100);
        int extraCrit = (int)MathF.Round(PLAYER_EXTRA_CRIT_CHANCE_PERCENT * 100);
        int miss = (int)MathF.Round(PLAYER_MISS_CHANCE_PERCENT * 100);
        string buffTip = $"Speed up by {speedUp}%!" +
            $" Crit rate up by {extraCrit}%!" +
            $" Hit chance down by {miss}%!";
        tip = buffTip;
        HappyModifyBuffText(ref buffName, ref tip, ref rare);
    }
}