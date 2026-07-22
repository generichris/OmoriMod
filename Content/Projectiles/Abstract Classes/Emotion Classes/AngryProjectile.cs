using OmoriMod.Content.Systems.EmotionSystem;

namespace OmoriMod.Content.Projectiles.Abstract_Classes;

/// <summary>
/// Automatically sets <see cref="EmotionProjectile.Emotion"/> to <see cref="EmotionType.Angry"/>.
/// </summary>
public abstract class AngryProjectile : EmotionProjectile
{
    public AngryProjectile()
    {
        SetEmotionType(EmotionType.Angry);
    }
}
