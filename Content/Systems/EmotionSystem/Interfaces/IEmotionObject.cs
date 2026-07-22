namespace OmoriMod.Content.Systems.EmotionSystem.Interfaces;

/// <summary>
/// Represents any gameplay object associated with an emotion.
/// </summary>
public interface IEmotionObject
{
    /// <summary>Gets the emotion represented by this object.</summary>
    EmotionType Emotion { get; }
}
