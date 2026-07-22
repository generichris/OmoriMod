using OmoriMod.Content.Buffs.Abstract;

namespace OmoriMod.Content.Systems.EmotionSystem.Interfaces;

/// <summary>
/// Exposes the current emotion state required by shared player and NPC emotion logic.
/// </summary>
public interface IEmotionEntity : IEmotionObject
{
    /// <summary>Gets whether the entity rejects attempts to apply a new emotion.</summary>
    public bool ImmuneToEmotionChange { get; }

    /// <summary>Gets or sets the emotion buff currently supplying the entity's effects.</summary>
    public EmotionBuff ActiveEmotionBuff { get; set; }

    /// <summary>
    /// Gets or sets the level used to scale the active emotion's stats.
    /// This may exceed the registered buff tier for a player at the final tier.
    /// </summary>
    public int EmotionLevel { get; set; }
}
