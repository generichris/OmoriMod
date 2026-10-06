using Microsoft.Xna.Framework;

namespace OmoriMod.Content.Systems.EmotionSystem;

/// <summary>Single source of truth for the color that represents each emotion.</summary>
public static class EmotionColors
{
    /// <summary>Gets the display color used for dust and tints of an emotion.</summary>
    public static Color Get(EmotionType emotion) => emotion switch
    {
        EmotionType.Sad => Color.Blue,
        EmotionType.Angry => Color.Red,
        EmotionType.Happy => Color.Yellow,
        EmotionType.Fear => Color.Gray,
        _ => Color.White
    };
}
