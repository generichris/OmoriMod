using OmoriMod.Content.Buffs.Abstract;

using Terraria;

namespace OmoriMod.Content.Items.Abstract_Classes;

/// <summary>
/// Parent class for any item that grants an <see cref="EmotionBuff"/>
/// </summary>
public abstract class EmotionBuffItem : EmotionItem
{
    /// <summary>
    /// The amount of ticks needed between <see cref="EmotionBuff"/> applications.
    /// </summary>
    protected int cooldownTicks;

    /// <summary>
    /// The timer to keep track of the amount of ticks since the last <see cref="EmotionBuff"/> application.
    /// </summary>
    private int timer;

    public EmotionBuffItem()
    {
        cooldownTicks = 10;
        timer = 0;
    }

    /// <summary>
    /// Use this instead of <see cref="UpdateInventory(Player)"/>
    /// </summary>
    /// <param name="player">The <see cref="Player"/></param>
    public virtual void UpdateInventoryEmotionBuffItem(Player player) { }

    /// <summary>
    /// Use this instead of <see cref="UseItem(Player)(Player)"/>
    /// </summary>
    /// <param name="player">The <see cref="Player"/></param>
    public virtual bool? UseItemEmotionBuffItem(Player player) { return null; }

    /// <summary>
    /// Use this instead of <see cref="CanUseItem(Player)(Player)"/>
    /// </summary>
    /// <param name="player">The <see cref="Player"/></param>
    public virtual bool CanUseItemEmotionBuffItem(Player player) { return true; }



    public override void UpdateInventory(Player player)
    {
        if (timer > 0) { timer--; }
        UpdateInventoryEmotionBuffItem(player);
    }

    public override bool? UseItem(Player player)
    {
        timer = cooldownTicks;
        return UseItemEmotionBuffItem(player);
    }

    public override bool CanUseItem(Player player)
    {
        bool timerBool = false;
        if (timer == 0) timerBool = true;
        return timerBool && CanUseItemEmotionBuffItem(player);
    }
}