namespace OmoriMod.Content.Systems.EmotionSystem;

/// <summary>
/// Defines the percentage inputs used to scale one emotion stat by emotion level.
/// </summary>
/// <param name="MaximumPercent">The upper bound of the scaled stat, expressed as a percentage.</param>
/// <param name="RatePercent">The percentage gained per level during the initial linear phase.</param>
/// <param name="StartingPercent">The flat percentage added after level-based scaling.</param>
public readonly record struct EmotionStatScaling(
    float MaximumPercent,
    float RatePercent,
    float StartingPercent);

/// <summary>
/// Provides the authoritative tuning values for emotion duration, level limits,
/// emotional advantage, and player- and NPC-specific stat scaling.
/// </summary>
/// <remarks>
/// This type contains balance data only. The emotion buff families interpret these
/// values and implement the corresponding gameplay behavior.
/// </remarks>
internal static class EmotionStatTuning
{
    /// <summary>
    /// The default duration, in seconds, applied by items that grant emotion buffs.
    /// </summary>
    public const int EmotionTimeInSeconds = 60;

    /// <summary>
    /// The source-damage adjustment applied for each signed level of emotional advantage.
    /// </summary>
    public const float EmotionalAdvantageValuePerLevel = 0.07f;

    /// <summary>
    /// The highest scaling level a player can reach by repeatedly applying a final-tier emotion.
    /// </summary>
    public const int PlayerMaxEmotionLevel = 43;

    /// <summary>
    /// The highest emotion level used when scaling NPC emotion stats.
    /// </summary>
    public const int NpcMaxEmotionLevel = 1;
    

    /// <summary>
    /// Contains tuning values for Angry stat effects.
    /// </summary>
    internal static class Angry
    {
        /// <summary>
        /// Contains Angry stat scaling used by players.
        /// </summary>
        internal static class PlayerStats
        {
            /// <summary>The player's outgoing-damage increase.</summary>
            internal static readonly EmotionStatScaling DamageIncrease = new(60f, 7f, 2f);
            /// <summary>The player's defense reduction.</summary>
            internal static readonly EmotionStatScaling DefenseDecrease = new(99.9f, 12.5f, 12.5f);
        }

        /// <summary>
        /// Contains Angry stat scaling used by NPCs.
        /// </summary>
        internal static class NpcStats
        {
            /// <summary>The NPC's outgoing-damage increase.</summary>
            internal static readonly EmotionStatScaling DamageIncrease = new(50f, 5f, 7f);
            /// <summary>The NPC's defense reduction.</summary>
            internal static readonly EmotionStatScaling DefenseDecrease = new(40f, 8.5f, 3.5f);
        }
    }

    /// <summary>
    /// Contains tuning values for Happy stat effects.
    /// </summary>
    internal static class Happy
    {
        /// <summary>
        /// Contains Happy stat scaling used by players.
        /// </summary>
        internal static class PlayerStats
        {
            /// <summary>The player's movement-speed increase.</summary>
            internal static readonly EmotionStatScaling MovementSpeedIncrease = new(50f, 5f, 5f);
            /// <summary>The player's additional critical-hit chance.</summary>
            internal static readonly EmotionStatScaling ExtraCritChance = new(60f, 8f, 3.5f);
            /// <summary>The player's chance for an attack to deal no damage.</summary>
            internal static readonly EmotionStatScaling MissChance = new(70f, 9.5f, 4f);
        }

        /// <summary>
        /// Contains Happy stat scaling used by NPCs.
        /// </summary>
        internal static class NpcStats
        {
            /// <summary>The NPC's movement-speed increase.</summary>
            internal static readonly EmotionStatScaling MovementSpeedIncrease = new(35f, 3.5f, 4.5f);
            /// <summary>The NPC's additional critical-hit chance.</summary>
            internal static readonly EmotionStatScaling ExtraCritChance = new(50f, 6f, 3f);
            /// <summary>The NPC's chance for an attack to deal no damage.</summary>
            internal static readonly EmotionStatScaling MissChance = new(55f, 8f, 4f);
        }
    }

    /// <summary>
    /// Contains tuning values for Fear stat effects.
    /// </summary>
    internal static class Fear
    {
        /// <summary>
        /// Contains Fear stat scaling used by players.
        /// </summary>
        internal static class PlayerStats
        {
            /// <summary>The player's movement-speed increase.</summary>
            internal static readonly EmotionStatScaling MovementSpeedIncrease = new(20f, 2f, 2.5f);
            /// <summary>The player's chance for an attack to deal no damage.</summary>
            internal static readonly EmotionStatScaling MissChance = new(90f, 10.5f, 4f);
            /// <summary>The player's outgoing-damage decrease.</summary>
            internal static readonly EmotionStatScaling DamageDecrease = new(120f, 70f, 5f);
            /// <summary>The player's life-regen increase, in raw lifeRegen units.</summary>
            internal static readonly EmotionStatScaling LifeRegenIncrease = new(40f, 15f, 1f);
        }

        /// <summary>
        /// Contains Fear stat scaling used by NPCs.
        /// </summary>
        internal static class NpcStats
        {
            /// <summary>The NPC's movement-speed increase.</summary>
            internal static readonly EmotionStatScaling MovementSpeedIncrease = new(15f, 1.5f, 2f);
            /// <summary>The NPC's chance for an attack to deal no damage.</summary>
            internal static readonly EmotionStatScaling MissChance = new(55f, 8f, 4f);
            /// <summary>The NPC's outgoing-damage decrease.</summary>
            internal static readonly EmotionStatScaling DamageDecrease = new(35f, 4f, 4f);
        }
    }

    /// <summary>
    /// Contains tuning values for Sad stat effects.
    /// </summary>
    internal static class Sad
    {
        /// <summary>
        /// Contains Sad stat scaling used by players.
        /// </summary>
        internal static class PlayerStats
        {
            /// <summary>The player's movement-speed reduction.</summary>
            internal static readonly EmotionStatScaling MovementSpeedDecrease = new(80f, 5f, 6f);
            /// <summary>The player's defense increase.</summary>
            internal static readonly EmotionStatScaling DefenseIncrease = new(60f, 6f, 3.5f);
            /// <summary>The share of incoming health damage redirected to mana.</summary>
            internal static readonly EmotionStatScaling HealthDamageToManaConversion = new(75f, 6.5f, 6f);
        }

        /// <summary>
        /// Contains Sad stat scaling used by NPCs.
        /// </summary>
        internal static class NpcStats
        {
            /// <summary>The NPC's movement-speed reduction.</summary>
            internal static readonly EmotionStatScaling MovementSpeedDecrease = new(60f, 4f, 7f);
            /// <summary>The NPC's defense increase.</summary>
            internal static readonly EmotionStatScaling DefenseIncrease = new(50f, 3.5f, 8.5f);
        }
    }
}
