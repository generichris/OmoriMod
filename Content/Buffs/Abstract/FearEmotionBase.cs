using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using OmoriMod.Systems.EmotionSystem;

namespace OmoriMod.Content.Buffs.Abstract
{
    public abstract class FearEmotionBase : EmotionBuff
    {
        // Player Configuration

        // ===== Movement Speed Increase (small) =====
        private const float PLAYER_MOVEMENT_SPEED_INCREASE_MAX = 20.0f;
        private const float PLAYER_MOVEMENT_SPEED_INCREASE_RATE = 2.0f;
        private const float PLAYER_MOVEMENT_SPEED_INCREASE_STARTING_VALUE = 2.5f;

        // ===== Miss Chance =====
        private const float PLAYER_MISS_CHANCE_MAX = 90.0f;
        private const float PLAYER_MISS_CHANCE_RATE = 10.5f;
        private const float PLAYER_MISS_CHANCE_STARTING_VALUE = 4.0f;

        // ===== Damage Decrease =====
        private const float PLAYER_DAMAGE_DECREASE_MAX = 120.0f;
        private const float PLAYER_DAMAGE_DECREASE_RATE = 70.0f;
        private const float PLAYER_DAMAGE_DECREASE_STARTING_VALUE = 5.0f;

        // ===== Life Regen Increase =====
        // Values are in raw lifeRegen units
        private const float PLAYER_LIFE_REGEN_INCREASE_MAX = 40.0f;
        private const float PLAYER_LIFE_REGEN_INCREASE_RATE = 22.0f;
        private const float PLAYER_LIFE_REGEN_INCREASE_STARTING_VALUE = 1.0f;



        // NPC Configuration

        // ===== Movement Speed Increase (small) =====
        private const float NPC_MOVEMENT_SPEED_INCREASE_MAX = 15.0f;
        private const float NPC_MOVEMENT_SPEED_INCREASE_RATE = 1.5f;
        private const float NPC_MOVEMENT_SPEED_INCREASE_STARTING_VALUE = 2.0f;



        // ===== Miss Chance =====
        private const float NPC_MISS_CHANCE_MAX = 55.0f;
        private const float NPC_MISS_CHANCE_RATE = 8.0f;
        private const float NPC_MISS_CHANCE_STARTING_VALUE = 4.0f;

        // ===== Damage Decrease =====
        private const float NPC_DAMAGE_DECREASE_MAX = 35.0f;
        private const float NPC_DAMAGE_DECREASE_RATE = 4.0f;
        private const float NPC_DAMAGE_DECREASE_STARTING_VALUE = 4.0f;



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

        // damage decrease
        public float PLAYER_DAMAGE_DECREASE_PERCENT => LinearPerLevel(
            max: PLAYER_DAMAGE_DECREASE_MAX,
            rate: PLAYER_DAMAGE_DECREASE_RATE,
            maxEmotionLevel: EmotionSystem.PLAYER_MAX_EMOTION_LEVEL,
            startingValue: PLAYER_DAMAGE_DECREASE_STARTING_VALUE
            );
        public float NPC_DAMAGE_DECREASE_PERCENT => LinearPerLevel(
            max: NPC_DAMAGE_DECREASE_MAX,
            rate: NPC_DAMAGE_DECREASE_RATE,
            maxEmotionLevel: EmotionSystem.NPC_MAX_EMOTION_LEVEL,
            startingValue: NPC_DAMAGE_DECREASE_STARTING_VALUE
            );

        // life regen increase (raw units, not a percent)
        public float PLAYER_LIFE_REGEN_INCREASE_AMOUNT => LinearPerLevel(
            max: PLAYER_LIFE_REGEN_INCREASE_MAX,
            rate: PLAYER_LIFE_REGEN_INCREASE_RATE,
            maxEmotionLevel: EmotionSystem.PLAYER_MAX_EMOTION_LEVEL,
            startingValue: PLAYER_LIFE_REGEN_INCREASE_STARTING_VALUE
            ) * 100f;

        public FearEmotionBase()
        {
            Emotion = EmotionType.HAPPY;
            dustColor = Color.Gray;
        }

        public override void UpdateEmotionBuff(Player player, ref int buffIndex)
        {
            EmotionSystem.RemoveIncompatibleEmotions<FearEmotionBase>(player);
            ModifyPlayerMovement(player);
            ApplyPlayerLifeRegen(player);
        }

        public override void UpdateEmotionBuff(NPC npc, ref int buffIndex)
        {
            EmotionSystem.RemoveIncompatibleEmotions<FearEmotionBase>(npc);
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

        private void ApplyPlayerLifeRegen(Player player)
        {
            player.lifeRegen += (int)PLAYER_LIFE_REGEN_INCREASE_AMOUNT;
        }

        public override void ModifyPlayerOutgoingDamage(ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= 1 - PLAYER_DAMAGE_DECREASE_PERCENT;
        }

        public override void ModifyNPCOutgoingDamage(ref Player.HurtModifiers modifiers)
        {
            modifiers.SourceDamage *= 1 - NPC_DAMAGE_DECREASE_PERCENT;
        }

        public override void ModifyPlayerHitNPC(ref NPC.HitModifiers modifiers)
        {
            // miss chance
            var noDMG = new StatModifier();
            noDMG *= 0;
            noDMG -= 1;
            modifiers.SourceDamage = Main.rand.NextFloat() < PLAYER_MISS_CHANCE_PERCENT ? noDMG : modifiers.SourceDamage;

        }

        public override void ModifyPlayerHitPlayer(ref Player.HurtModifiers modifiers)
        {
            // miss chance
            var noDMG = new StatModifier();
            noDMG *= 0;
            noDMG -= 1;
            modifiers.SourceDamage = Main.rand.NextFloat() < PLAYER_MISS_CHANCE_PERCENT ? noDMG : modifiers.SourceDamage;
        }

        public virtual void FearModifyBuffText(ref string buffName, ref string tip, ref int rare) { }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            int speedUp = (int)MathF.Round(PLAYER_MOVEMENT_SPEED_INCREASE_PERCENT * 100);
            int miss = (int)MathF.Round(PLAYER_MISS_CHANCE_PERCENT * 100);
            int damageDown = (int)MathF.Round(PLAYER_DAMAGE_DECREASE_PERCENT * 100);
            string buffTip = $"Speed up by {speedUp}%!" +
                $" Hit chance down by {miss}%!" +
                $" Damage down by {damageDown}%!" +
                $" regenerating life!";
            tip = buffTip;
            FearModifyBuffText(ref buffName, ref tip, ref rare);
        }
    }
}