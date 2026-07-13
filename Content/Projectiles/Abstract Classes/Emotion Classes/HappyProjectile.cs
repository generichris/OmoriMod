using OmoriMod.Systems.EmotionSystem;

namespace OmoriMod.Content.Projectiles.Abstract_Classes;

/// <summary>
/// Automatically sets <see cref="EmotionProjectile.Emotion"/> to <see cref="EmotionType.HAPPY"/>.
/// </summary>
public abstract class HappyProjectile : EmotionProjectile
{
    public HappyProjectile()
    {
        SetEmotionType(EmotionType.HAPPY);
    }
}