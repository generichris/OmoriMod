using OmoriMod.Systems.EmotionSystem;

namespace OmoriMod.Content.Projectiles.Abstract_Classes;

/// <summary>
/// Automatically sets <see cref="EmotionProjectile.Emotion"/> to <see cref="EmotionType.SAD"/>.
/// </summary>
public abstract class SadProjectile : EmotionProjectile
{
    public SadProjectile()
    {
        SetEmotionType(EmotionType.SAD);
    }
}