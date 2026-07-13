using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Systems.EmotionSystem;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Abstract_Classes;

/// <summary>
/// An abstract class for items that inflict emotions.
/// Use <see cref="AngryItem"/>, <see cref="HappyItem"/>, or <see cref="SadItem"/> 
/// to set emotions. If <see cref="Emotion"/> is not set, it will default to <see cref="EmotionType.NONE"/>.
/// </summary>
public abstract class EmotionItem : AbilityItem, IOnHitEmotionObject
{
    public EmotionType Emotion { get; protected set; }

    public float meleeWeaponProjectileMoveTime = 0.2f;

    /// <summary>
    /// Used to set the <see cref="Emotion"/>
    /// </summary>
    /// <param name="emotion">The emotion to be set.</param>
    protected void SetEmotionType(EmotionType emotion)
    {
        Emotion = emotion;
    }

    /// <summary>
    /// A hook method that allows emotion items to call <see cref="OnHitNPC(Player, NPC, NPC.HitInfo, int)"/> without breaking the emotion system.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <param name="target">The target.</param>
    /// <param name="hit">The damage.</param>
    /// <param name="damageDone">The actual damage dealt to/taken by the NPC.</param>
    public virtual void OnHitNPCEmotion(Player player, NPC target, NPC.HitInfo hit, int damageDone) { }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        ((IOnHitEmotionObject)this).InflictEmotion(target);
        OnHitNPCEmotion(player, target, hit, damageDone);
    }

    protected override void SetRarity()
    {
        switch (Emotion)
        {
            case EmotionType.NONE:
                break;
            case EmotionType.SAD:
                Item.rare = ItemRarityID.Blue;
                break;
            case EmotionType.ANGRY:
                Item.rare = ItemRarityID.Red;
                break;
            case EmotionType.HAPPY:
                Item.rare = ItemRarityID.Yellow;
                break;
            case EmotionType.FEAR:
                Item.rare = ItemRarityID.Purple;
                break;
        }
    }

    /// <summary>
    /// Clones the defaults of a <see cref="ModItem"/> inlcuding the research unlock count. 
    /// Preserves <see cref="Item.rare"/> and <see cref="Emotion"/>
    /// </summary>
    /// <typeparam name="T">The <see cref="Item"/> to be cloned.</typeparam>
    public void EmotionItemClone<T>() where T : ModItem
    {
        ModItemClone<T>();
        SetRarity();
    }

    /// <summary>
    /// Clones the defaults of a <see cref="ModItem"/> inlcuding the research unlock count. 
    /// Preserves <see cref="Item.rare"/> and <see cref="Emotion"/>. Changes projectile shot to
    /// the type of <paramref name="projType"/>
    /// </summary>
    /// <typeparam name="T">The <see cref="ModItem"/> to be cloned.</typeparam>
    /// <param name="projType">The type of the <see cref="Projectile"/> shot.</param>
    public void EmotionItemCloneWithDifferentProjectile<T>(int newProjectileType) where T : ModItem
    {
        ModItemClone<T>();
        Item.shoot = newProjectileType;
        SetRarity();
    }

    /// <summary>
    /// Clones the defaults of a <see cref="ModItem"/> inlcuding the research unlock count. 
    /// Preserves <see cref="Item.rare"/> and <see cref="Emotion"/>. Changes buff applied to
    /// tge type of <paramref name="newBuffType"/>
    /// </summary>
    /// <typeparam name="T">The <see cref="ModItem"/> to be cloned.</typeparam>
    /// <param name="projType">The type of the <see cref="Projectile"/> shot.</param>
    public void EmotionItemCloneWithDifferentBuff<T>(int newBuffType) where T : ModItem
    {
        ModItemClone<T>();
        Item.buffType = newBuffType;
        SetRarity();
    }
}