namespace OmoriMod.Systems.EmotionSystem;

/// <summary>
/// An enum to simplify emotion types. All I<see cref="IEmotionObject"/> communicate through this enum.
/// </summary>
public enum EmotionType
{
    NONE = 0,
    SAD = 1,
    ANGRY = 2,
    HAPPY = 3,
    FEAR = 4,
}

/// <summary>
/// An interface to consolidate emotion application.
/// </summary>
public interface IEmotionObject
{
    /// <summary>
    /// The emotion this object contains.
    /// </summary>
    public EmotionType Emotion { get; }
}