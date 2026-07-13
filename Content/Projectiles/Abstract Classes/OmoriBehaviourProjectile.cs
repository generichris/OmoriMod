using OmoriMod.Systems.State_Management.Projectiles;

namespace OmoriMod.Content.Projectiles.Abstract_Classes;

/// <summary>
	/// Base class for projectiles that utilize the <see cref="ProjectileBehaviourManager"/> system for AI behaviour and animation control.
	/// <para>
	/// Inherits from <see cref="EmotionProjectile"/> and provides a protected <see cref="ProjectileBehaviourManager"/> instance, 
	/// allowing derived projectiles to define, manage, and execute multiple behaviours, background behaviours, and animations.
	/// </para>
	/// <para>
	/// Derived classes can use <see cref="behaviourManager"/> to integrate AI logic and animation updates into the projectile's update loop.
	/// </para>
	/// </summary>
public abstract class OmoriBehaviourProjectile : EmotionProjectile
{
    /// <summary>
		/// The behaviour manager for this <see cref="OmoriBehaviourProjectile"/>
		/// </summary>
    protected ProjectileBehaviourManager behaviourManager;
}