using OmoriMod.Content.Systems.EmotionSystem;

namespace OmoriMod.Content.Projectiles.Abstract_Classes;

/// <summary>
/// Automatically sets <see cref="EmotionProjectile.Emotion"/> to <see cref="EmotionType.Sad"/>.
/// </summary>
public abstract class SadProjectile : EmotionProjectile
{
    public SadProjectile()
    {
        SetEmotionType(EmotionType.Sad);
    }
}
