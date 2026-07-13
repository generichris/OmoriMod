using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.NPCs.Classes;

/// <summary>
/// Base class for all <see cref="ModNPC"/> in my mod.
/// Defines <see cref="NPC.ai"/>[0] as <see cref="AI_Timer"/>
/// </summary>
public abstract class OmoriModNPC : ModNPC
{
    /// <summary>
    /// A timer available to use for AI.
    /// </summary>
    public float AI_Timer
    {
        get => NPC.ai[0];
        set => NPC.ai[0] = value;
    }
}