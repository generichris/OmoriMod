using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Projectiles.Abstract_Classes;

namespace OmoriMod.Content.Systems.EmotionSystem.Interfaces;


/// <summary>
/// Marks an emotion-aware item or projectile that can apply its emotion when it hits a target.
/// </summary>
/// <remarks>
/// Shared by <see cref="EmotionProjectile"/> and <see cref="EmotionItem"/> so callers can
/// inspect their emotion without depending on either inheritance hierarchy.
/// </remarks>
public interface IOnHitEmotionObject : IEmotionObject
{
}
