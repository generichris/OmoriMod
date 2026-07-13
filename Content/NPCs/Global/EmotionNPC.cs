using Microsoft.Xna.Framework;

using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.Players;
using OmoriMod.Systems.EmotionSystem;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.NPCs.Global;

public class EmotionNPC : GlobalNPC, IEmotionEntity
{
    public override bool InstancePerEntity => true;
    public EmotionType Emotion { get; set; }
    public EmotionBuff ActiveEmotionBuff { get; set; }
    public bool ImmuneToEmotionChange { get; set; }

    public int colorTimer;

    public Color? original_color;

    public float happyBuffNPCSpeedUp = 0.3f;
    public float sadBuffNPCSlowDown = 0.3f;

    public override void SetDefaults(NPC entity)
    {
        // by default, all enemies are able to be effected by emotions
        ImmuneToEmotionChange = false;

        ModNPC modNPC = NPCLoader.GetNPC(entity.type);
        if (modNPC == null || modNPC is not OmoriModNPC)
        {
            // bosses are immune to emotions
            ImmuneToEmotionChange = entity.boss;
        }
    }

    public override void ResetEffects(NPC npc)
    {
        Emotion = EmotionType.NONE;
        ActiveEmotionBuff = null;
    }



    /// <summary>
    /// Changes the color of the <paramref name="npc"/> depending on its <see cref="Emotion"/>
    /// </summary>
    /// <param name="npc">The <see cref="NPC"/> effected</param>
    private void NPCColorChangeFromEmotion(NPC npc)
    {
        if (original_color == null)
        {
            original_color = npc.color;
        }
        colorTimer++;
        Color colorNeeded;
        if (Emotion != EmotionType.NONE)
        {
            if (Emotion == EmotionType.ANGRY)
            {
                colorNeeded = Color.Red;
            }
            else if (Emotion == EmotionType.SAD)
            {
                colorNeeded = Color.Blue;
            }
            else
            {
                colorNeeded = Color.Yellow;
            }
            // Flash emotion color and original color
            if (colorTimer > 60)
            {
                npc.color = Color.Lerp(npc.color, (Color)original_color, 0.1f);

                if (colorTimer > 90)
                {
                    colorTimer = 0;
                }
            }
            else
            {
                npc.color = Color.Lerp(npc.color, colorNeeded, 0.1f);
            }
        }
        else
        {
            // if we need to fix the color then do it, otherwise don't mess with the color
            if (npc.color != (Color)original_color) { npc.color = Color.Lerp(npc.color, (Color)original_color, 0.1f); }
        }
    }

    public override void PostAI(NPC npc)
    {
        // Color change happens regardless of what happens in PreAI or AI
        NPCColorChangeFromEmotion(npc);
    }

    private static void ApplyAdvantage(int advantage, ref NPC.HitModifiers modifiers)
    {
        if (advantage == 0) return;

        if (advantage > 0)
        {
            modifiers.SourceDamage += EmotionSystem.EMOTIONAL_ADVANTAGE_VALUE_PER_LEVEL * advantage;
            return;
        }

        if (advantage < 0)
        {
            modifiers.SourceDamage -= EmotionSystem.EMOTIONAL_ADVANTAGE_VALUE_PER_LEVEL * advantage;
            return;
        }
    }
    private static void ApplyAdvantage(int advantage, ref Player.HurtModifiers modifiers)
    {
        if (advantage == 0) return;

        if (advantage > 0)
        {
            // int index = advantage - 1; 
            modifiers.SourceDamage += EmotionSystem.EMOTIONAL_ADVANTAGE_VALUE_PER_LEVEL * advantage;
            return;
        }

        if (advantage < 0)
        {
            // int index = -advantage - 1;
            modifiers.SourceDamage -= EmotionSystem.EMOTIONAL_ADVANTAGE_VALUE_PER_LEVEL * advantage;
            return;
        }
    }


    private static void ApplyAdditionalEmotionModifiers(IEmotionEntity attacker, ref NPC.HitModifiers modifiers)
    {
        attacker.ActiveEmotionBuff?.ModifyPlayerOutgoingDamage(ref modifiers);
    }

    private static void ApplyAdditionalEmotionModifiers(IEmotionEntity attacker, IEmotionEntity defender, ref Player.HurtModifiers modifiers)
    {
        // Attacker (NPC or Player) hitting Player
        attacker.ActiveEmotionBuff?.ModifyNPCOutgoingDamage(ref modifiers);
        attacker.ActiveEmotionBuff?.ModifyPlayerHitPlayer(ref modifiers); // In case attacker is player (PVP)

        // Defender (Player) taking damage
        defender.ActiveEmotionBuff?.ModifyPlayerIncomingDamage(ref modifiers);
    }

    private static void EmotionalAdvantage(IEmotionEntity attacker, IEmotionEntity defender, ref NPC.HitModifiers modifiers)
    {
        int advantage = EmotionSystem.CalculateAdvantage(attacker, defender);
        ApplyAdvantage(advantage, ref modifiers);
        ApplyAdditionalEmotionModifiers(attacker, ref modifiers);

        // Happy vs NPC (if attacker is player) logic is now in ModifyPlayerOutgoingDamage/ModifyPlayerHitNPC?
        // Wait, HappyEmotionBase implementation:
        // ModifyPlayerHitNPC -> Miss/Crit
        // ModifyPlayerOutgoingDamage -> Not implemented in HappyBase? Ah, I missed moving `ModifyPlayerHitNPC` call to `ApplyAdditionalEmotionModifiers`?
        // Let's check `HappyEmotionBase`. It has `ModifyPlayerHitNPC`.
        // So `ApplyAdditionalEmotionModifiers` needs to call THAT too if attacker is Player.
        // But `ModifyPlayerOutgoingDamage` is for generic damage increase (Angry).

        attacker.ActiveEmotionBuff?.ModifyPlayerHitNPC(ref modifiers);
    }

    private static void EmotionalAdvantage(IEmotionEntity attacker, IEmotionEntity defender, ref Player.HurtModifiers modifiers)
    {
        int advantage = EmotionSystem.CalculateAdvantage(attacker, defender);
        ApplyAdvantage(advantage, ref modifiers);
        ApplyAdditionalEmotionModifiers(attacker, defender, ref modifiers);
    }


    public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        EmotionPlayer emotionPlayer = player.GetModPlayer<EmotionPlayer>();
        EmotionNPC emotionNPC = npc.GetGlobalNPC<EmotionNPC>();
        EmotionalAdvantage(
            attacker: emotionPlayer,
            defender: emotionNPC,
            modifiers: ref modifiers
            );
    }
    public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        Player player = Main.player[projectile.owner];
        EmotionPlayer emotionPlayer = player.GetModPlayer<EmotionPlayer>();
        EmotionNPC emotionNPC = npc.GetGlobalNPC<EmotionNPC>();
        EmotionalAdvantage(
            attacker: emotionPlayer,
            defender: emotionNPC,
            modifiers: ref modifiers
            );

    }
    public override void ModifyHitNPC(NPC npc, NPC target, ref NPC.HitModifiers modifiers)
    {
        EmotionNPC emotionAttacker = npc.GetGlobalNPC<EmotionNPC>();
        EmotionNPC emotionDefender = target.GetGlobalNPC<EmotionNPC>();
        EmotionalAdvantage(
            attacker: emotionAttacker,
            defender: emotionDefender,
            modifiers: ref modifiers
            );
    }
    public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
    {
        EmotionPlayer emotionPlayer = target.GetModPlayer<EmotionPlayer>();
        EmotionNPC emotionNPC = npc.GetGlobalNPC<EmotionNPC>();
        base.ModifyHitPlayer(npc, target, ref modifiers);
        EmotionalAdvantage(
            attacker: emotionNPC,
            defender: emotionPlayer,
            modifiers: ref modifiers
            );
    }


    // used for sad emotion mana conversion
    public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
    {
        EmotionPlayer emotionPlayer = target.GetModPlayer<EmotionPlayer>();
        emotionPlayer.ActiveEmotionBuff?.OnPlayerHurt(target, hurtInfo);
    }
}