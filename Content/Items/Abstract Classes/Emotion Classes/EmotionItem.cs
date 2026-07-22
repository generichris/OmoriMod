using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Players;
using OmoriMod.Content.Systems.EmotionSystem;
using OmoriMod.Content.Systems.EmotionSystem.Interfaces;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;

/// <summary>
/// Base class for emotion-aware items that apply an emotion to NPCs on hit and participate
/// in emotional-advantage calculations during player-versus-player hits.
/// </summary>
/// <remarks>
/// Derive from <see cref="AngryItem"/>, <see cref="HappyItem"/>, or <see cref="SadItem"/>
/// when the item's emotion is fixed. Direct subclasses default to <see cref="EmotionType.None"/>.
/// </remarks>
public abstract class EmotionItem : AbilityItem, IOnHitEmotionObject
{
    /// <summary>Gets the emotion applied or represented by this item.</summary>
    public EmotionType Emotion { get; protected set; }

    /// <summary>The travel-time value used by melee weapons that create emotion projectiles.</summary>
    public float MeleeWeaponProjectileMoveTime = 0.2f;

    /// <summary>
    /// Sets the emotion represented by this item.
    /// </summary>
    /// <param name="emotion">The emotion to be set.</param>
    protected void SetEmotionType(EmotionType emotion)
    {
        Emotion = emotion;
    }

    /// <summary>
    /// Provides an extension hook that runs after the base class applies its emotion to an NPC.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <param name="target">The target.</param>
    /// <param name="hit">The damage.</param>
    /// <param name="damageDone">The actual damage dealt to/taken by the NPC.</param>
    public virtual void OnHitNPCEmotion(Player player, NPC target, NPC.HitInfo hit, int damageDone) { }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        EmotionSystem.ApplyEmotion(target, Emotion);
        OnHitNPCEmotion(player, target, hit, damageDone);
    }

    public override void ModifyHitPvp(Player player, Player target, ref Player.HurtModifiers modifiers)
    {
        EmotionSystem.ApplyCombatModifiers(
            player.GetModPlayer<EmotionPlayer>(),
            target.GetModPlayer<EmotionPlayer>(),
            ref modifiers);
    }

    protected override void SetRarity()
    {
        switch (Emotion)
        {
            case EmotionType.Sad:
                Item.rare = ItemRarityID.Blue;
                break;
            case EmotionType.Angry:
                Item.rare = ItemRarityID.Red;
                break;
            case EmotionType.Happy:
                Item.rare = ItemRarityID.Yellow;
                break;
            case EmotionType.Fear:
                Item.rare = ItemRarityID.Purple;
                break;
            case EmotionType.None:
            default:
                break;
        }
    }

    /// <summary>
    /// Clones another mod item's defaults and research count, then restores emotion-based rarity.
    /// </summary>
    /// <typeparam name="T">The <see cref="Item"/> to be cloned.</typeparam>
    public void EmotionItemClone<T>() where T : ModItem
    {
        ModItemClone<T>();
        SetRarity();
    }

    /// <summary>
    /// Clones another mod item's defaults and research count, replaces its projectile,
    /// then restores emotion-based rarity.
    /// </summary>
    /// <typeparam name="T">The <see cref="ModItem"/> to be cloned.</typeparam>
    /// <param name="newProjectileType">The type of the <see cref="Projectile"/> shot.</param>
    public void EmotionItemCloneWithDifferentProjectile<T>(int newProjectileType) where T : ModItem
    {
        ModItemClone<T>();
        Item.shoot = newProjectileType;
        SetRarity();
    }

    /// <summary>
    /// Clones another mod item's defaults and research count, replaces its applied buff,
    /// then restores emotion-based rarity.
    /// </summary>
    /// <typeparam name="T">The <see cref="ModItem"/> to be cloned.</typeparam>
    /// <param name="newBuffType">The type of the <see cref="ModBuff"/> provided.</param>
    public void EmotionItemCloneWithDifferentBuff<T>(int newBuffType) where T : ModItem
    {
        ModItemClone<T>();
        Item.buffType = newBuffType;
        SetRarity();
    }
}
