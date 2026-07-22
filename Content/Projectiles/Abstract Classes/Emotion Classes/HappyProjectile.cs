using OmoriMod.Content.Systems.EmotionSystem;

namespace OmoriMod.Content.Projectiles.Abstract_Classes;

/// <summary>
/// Automatically sets <see cref="EmotionProjectile.Emotion"/> to <see cref="EmotionType.Happy"/>.
/// </summary>
public abstract class HappyProjectile : EmotionProjectile
{
    public HappyProjectile()
    {
        SetEmotionType(EmotionType.Happy);
    }
}
