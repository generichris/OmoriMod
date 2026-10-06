using Microsoft.Xna.Framework;

using OmoriMod.Content.Buffs.Abstract;
using OmoriMod.Content.NPCs.Classes;
using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem;
using OmoriMod.Content.Systems.EmotionSystem.Interfaces;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.NPCs.Global;

/// <summary>
/// Adds per-instance emotion state, visuals, immunity rules, and combat-hook integration to NPCs.
/// </summary>
/// <remarks>
/// Emotion buffs repopulate the transient state each tick. Vanilla bosses are immune by default,
/// while <see cref="OmoriModNPC"/> implementations may manage their own immunity behavior.
/// </remarks>
public class EmotionNPC : GlobalNPC, IEmotionEntity
{
    /// <summary>Gets whether each NPC receives an independent instance of this global component.</summary>
    public override bool InstancePerEntity => true;

    /// <summary>The preferred emotion buff type, resolved once per tick in <see cref="ResetEffects"/>.</summary>
    internal int? PreferredEmotionBuffType { get; private set; }

    /// <summary>Gets or sets the NPC's currently resolved emotion.</summary>
    public EmotionType Emotion { get; set; }

    /// <summary>Gets or sets the buff currently responsible for the NPC's emotion effects.</summary>
    public EmotionBuff ActiveEmotionBuff { get; set; }

    /// <summary>Gets or sets the registered tier used for NPC stat scaling.</summary>
    public int EmotionLevel { get; set; }

    /// <summary>Gets or sets whether attempts to apply a new emotion should be rejected.</summary>
    public bool ImmuneToEmotionChange { get; set; }

    /// <summary>Tracks the current frame within the emotion-color pulse.</summary>
    public int ColorTimer;

    /// <summary>Stores the NPC's original tint so it can be restored when no emotion is active.</summary>
    public Color? OriginalColor;

    /// <summary>Initializes the NPC's default emotion-change immunity.</summary>
    public override void SetDefaults(NPC entity)
    {
        // by default, all enemies are able to be effected by emotions
        ImmuneToEmotionChange = false;

        ModNPC modNpc = NPCLoader.GetNPC(entity.type);
        if (modNpc == null || modNpc is not OmoriModNPC)
        {
            // bosses are immune to emotions
            ImmuneToEmotionChange = entity.boss;
        }
    }

    /// <summary>Clears transient emotion state before active buffs repopulate it.</summary>
    public override void ResetEffects(NPC npc)
    {
        Emotion = EmotionType.None;
        ActiveEmotionBuff = null;
        EmotionLevel = 0;
        PreferredEmotionBuffType = EmotionSystem.GetEmotionType(npc);
    }



    /// <summary>
    /// Pulses an emotional NPC toward its emotion color and restores its original tint afterward.
    /// </summary>
    /// <param name="npc">The NPC whose tint should be updated.</param>
    private void NpcColorChangeFromEmotion(NPC npc)
    {
        OriginalColor ??= npc.color;
        Color original = OriginalColor.Value;

        if (Emotion == EmotionType.None)
        {
            // Nothing to do for the vast majority of NPCs.
            ColorTimer = 0;
            if (npc.color != original)
            {
                npc.color = StepToward(npc.color, original);
            }
            return;
        }

        ColorTimer++;
        Color colorNeeded = EmotionColors.Get(Emotion);
        // Flash emotion color and original color
        if (ColorTimer > 60)
        {
            npc.color = Color.Lerp(npc.color, original, 0.1f);

            if (ColorTimer > 90)
            {
                ColorTimer = 0;
            }
        }
        else
        {
            npc.color = Color.Lerp(npc.color, colorNeeded, 0.1f);
        }
    }

    /// <summary>
    /// Lerps a color toward a target and snaps to it once close. Lerping bytes by 10% truncates,
    /// so it would otherwise stall a few units short and never reach the target.
    /// </summary>
    private static Color StepToward(Color from, Color to)
    {
        const int snapDistance = 10;
        if (System.Math.Abs(from.R - to.R) <= snapDistance
            && System.Math.Abs(from.G - to.G) <= snapDistance
            && System.Math.Abs(from.B - to.B) <= snapDistance
            && System.Math.Abs(from.A - to.A) <= snapDistance)
        {
            return to;
        }

        return Color.Lerp(from, to, 0.1f);
    }

    /// <summary>Updates the emotion tint after the NPC's normal AI hooks complete.</summary>
    public override void PostAI(NPC npc)
    {
        // Color change happens regardless of what happens in PreAI or AI
        NpcColorChangeFromEmotion(npc);
    }

    /// <summary>Applies player and NPC emotion modifiers when this NPC is hit by an item.</summary>
    public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        EmotionPlayer emotionPlayer = player.GetModPlayer<EmotionPlayer>();
        EmotionNPC emotionNpc = npc.GetGlobalNPC<EmotionNPC>();
        EmotionSystem.ApplyCombatModifiers(
            attacker: emotionPlayer,
            defender: emotionNpc,
            modifiers: ref modifiers
            );
    }
    /// <summary>Applies the projectile owner's emotion modifiers when this NPC is hit by a projectile.</summary>
    public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        Player player = Main.player[projectile.owner];
        EmotionPlayer emotionPlayer = player.GetModPlayer<EmotionPlayer>();
        EmotionNPC emotionNpc = npc.GetGlobalNPC<EmotionNPC>();
        EmotionSystem.ApplyCombatModifiers(
            attacker: emotionPlayer,
            defender: emotionNpc,
            modifiers: ref modifiers
            );

    }
    /// <summary>Applies emotion modifiers when this NPC attacks another NPC.</summary>
    public override void ModifyHitNPC(NPC npc, NPC target, ref NPC.HitModifiers modifiers)
    {
        EmotionNPC emotionAttacker = npc.GetGlobalNPC<EmotionNPC>();
        EmotionNPC emotionDefender = target.GetGlobalNPC<EmotionNPC>();
        EmotionSystem.ApplyCombatModifiers(
            attacker: emotionAttacker,
            defender: emotionDefender,
            modifiers: ref modifiers
            );
    }
    /// <summary>Applies emotion modifiers when this NPC attacks a player.</summary>
    public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
    {
        EmotionPlayer emotionPlayer = target.GetModPlayer<EmotionPlayer>();
        EmotionNPC emotionNpc = npc.GetGlobalNPC<EmotionNPC>();
        EmotionSystem.ApplyCombatModifiers(
            attacker: emotionNpc,
            defender: emotionPlayer,
            modifiers: ref modifiers
            );
    }
}
