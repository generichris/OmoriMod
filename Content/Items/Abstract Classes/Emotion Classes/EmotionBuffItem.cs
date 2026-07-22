using OmoriMod.Content.Buffs.Abstract;

using Terraria;

namespace OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;

/// <summary>
/// Base class for items that grant or promote <see cref="EmotionBuff"/> instances and need
/// a short use cooldown independent of the item's normal use animation.
/// </summary>
public abstract class EmotionBuffItem : EmotionItem
{
    /// <summary>
    /// The minimum number of ticks between emotion-buff application attempts.
    /// </summary>
    protected int _cooldownTicks = 10;

    /// <summary>
    /// The remaining internal cooldown in ticks.
    /// </summary>
    private int _timer;

    /// <summary>
    /// Provides an extension hook for inventory updates without bypassing cooldown handling.
    /// </summary>
    /// <param name="player">The <see cref="Player"/></param>
    public virtual void UpdateInventoryEmotionBuffItem(Player player) { }

    /// <summary>
    /// Provides an extension hook for item use after the cooldown has been started.
    /// </summary>
    /// <param name="player">The <see cref="Player"/></param>
    public virtual bool? UseItemEmotionBuffItem(Player player) { return null; }

    /// <summary>
    /// Provides an extension hook for item-specific eligibility in addition to the cooldown check.
    /// </summary>
    /// <param name="player">The <see cref="Player"/></param>
    public virtual bool CanUseItemEmotionBuffItem(Player player) { return true; }



    public override void UpdateInventory(Player player)
    {
        if (_timer > 0) { _timer--; }
        UpdateInventoryEmotionBuffItem(player);
    }

    public override bool? UseItem(Player player)
    {
        _timer = _cooldownTicks;
        return UseItemEmotionBuffItem(player);
    }

    public override bool CanUseItem(Player player)
    {
        bool timerBool = _timer == 0;
        return timerBool && CanUseItemEmotionBuffItem(player);
    }
}
