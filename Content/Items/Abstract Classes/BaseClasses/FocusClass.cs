using System;
using System.IO;

using Microsoft.Xna.Framework;

using OmoriMod.Content.Dusts;
using OmoriMod.Content.Players;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Abstract_Classes;


public enum FocusState
{
    IdleToDecay,
    IdleToCharge,
    Charging,
    Decaying
}
public abstract class FocusItem : EmotionItem
{


    /// <summary>
    /// The current state of the item
    /// </summary>
    private FocusState currentState;

    /// <summary>
    /// The amount of ticks that have passed since the last state change
    /// </summary>
    private int stateTickTimer;

    /// <summary>
    /// The current charge of the item
    /// </summary>
    private int charge;



    /// <summary>
    /// The maximum charge of this <see cref="FocusItem"/>
    /// </summary>
    public int maxCharge;

    /// <summary>
    /// The amount that the dps of this <see cref="FocusItem"/> increases each second
    /// </summary>
    public int dpsIncrease;

    /// <summary>
    /// The amount of ticks that pass before the <see cref="FocusItem"/>s charge starts increasing
    /// </summary>
    public int ticksUntilChargeStarts;

    /// <summary>
    /// The amount of ticks that pass before the <see cref="FocusItem"/>s charge starts decaying
    /// </summary>
    public int ticksUntilDecayStarts;

    /// <summary>
    /// How fast in ticks the charge of this <see cref="FocusItem"/> should decay. A <paramref name="tickDecayRate"/> of 2 means charge will be lost every 2 ticks
    /// </summary>
    public int tickDecayRate;


    /// <summary>
    /// Initalizes the focus item.
    /// </summary>
    /// <param name="maxCharge">The maximum charge of this <see cref="FocusItem"/></param>
    /// <param name="dpsIncrease">The amount that the dps of this <see cref="FocusItem"/> increases each second</param>
    /// <param name="ticksUntilChargeStarts">The amount of ticks that pass before the <see cref="FocusItem"/>s charge starts increasing</param>
    /// <param name="ticksUntilDecayStarts">The amount of ticks that pass before the <see cref="FocusItem"/>s charge starts decaying</param>
    /// <param name="tickDecayRate">How fast in ticks the charge of this <see cref="FocusItem"/> should decay. A <paramref name="tickDecayRate"/> of 2 means charge will be lost every 2 ticks</param>
    public void InitFocusItem(int maxCharge, int dpsIncrease, int ticksUntilChargeStarts, int ticksUntilDecayStarts, int tickDecayRate)
    {
        currentState = FocusState.IdleToCharge;
        stateTickTimer = 0;
        charge = 0;

        this.maxCharge = maxCharge * 60;
        this.dpsIncrease = dpsIncrease;
        this.ticksUntilChargeStarts = ticksUntilChargeStarts;
        this.ticksUntilDecayStarts = ticksUntilDecayStarts;
        this.tickDecayRate = tickDecayRate;
    }


    /// <summary>
    /// A hook method that allows focus items to use <see cref="HoldItem(Player)"/> without breaking Focus logic
    /// </summary>
    /// <param name="player">The player for <see cref="HoldItem(Player)"/></param>
    public virtual void HoldItemFocus(Player player) { }

    /// <summary>
    /// A hook method that allows focus items to use <see cref="UpdateInventory(Player)"/> without breaking Focus logic
    /// </summary>
    /// <param name="player">The player for <see cref="UpdateInventory(Player)"/></param>
    public virtual void UpdateInventoryFocus(Player player) { }

    /// <summary>
    /// A hook method that allows focus items to use <see cref="NetSend(BinaryWriter)"/> without breaking Focus logic
    /// </summary>
    /// <param name="writer">The writer for <see cref="NetSend(BinaryWriter)"/></param>
    public virtual void NetSendFocus(BinaryWriter writer) { }

    /// <summary>
    /// A hook method that allows focus items to use <see cref="NetReceive(BinaryReader)"/> without breaking Focus logic
    /// </summary>
    /// <param name="reader">The reader for <see cref="NetReceive(BinaryReader)"/></param>
    public virtual void NetReceiveFocus(BinaryReader reader) { }

    /// <summary>
    /// A hook method that allows focus items to use <see cref="ModifyWeaponDamage(Player, ref StatModifier)"/> without breaking Focus logic
    /// </summary>
    /// <param name="player">The player for <see cref="ModifyWeaponDamage(Player, ref StatModifier)"/></param>
    /// <param name="damage">The <see cref="StatModifier"/> for <see cref="ModifyWeaponDamage(Player, ref StatModifier)"/></param>
    public virtual void ModifyWeaponDamageFocus(Player player, ref StatModifier damage) { }





    // LOGIC FUNCTIONS
    private void FocusHoldLogic(Player player)
    {

        // set FocusPlayer variables to the current held item
        var modPlayer = player.GetModPlayer<FocusPlayer>();
        modPlayer.hasChargeItem = true;
        modPlayer.currentCharge = charge;
        modPlayer.maxCharge = maxCharge;

        bool reachedMaxCharge = modPlayer.reachedMaxCharge;

        if (charge == maxCharge && !reachedMaxCharge)
        {

            DustHandler(
                player,
                amtOfDust: 40,
                pOff: 6,
                SpOff: 2,
                ScOff: 2
                );
            modPlayer.reachedMaxCharge = true;
        }
        else if (charge != maxCharge)
        {

            modPlayer.reachedMaxCharge = false;
        }


        if (!Main.mouseLeft)
        {
            // mouse is not held
            switch (currentState)
            {
                case FocusState.IdleToCharge:
                    if (++stateTickTimer >= ticksUntilChargeStarts)
                    {
                        currentState = FocusState.Charging;
                        stateTickTimer = 0;
                    }
                    break;

                case FocusState.Charging:
                    if (charge < maxCharge)
                        charge++;
                    break;

                case FocusState.Decaying:
                    currentState = FocusState.IdleToCharge;
                    stateTickTimer = 0;
                    break;
            }
        }
        else
        {
            // mouse is held: reset to idle and allow decay timer to start
            if (currentState != FocusState.Decaying && currentState != FocusState.IdleToDecay)
            {
                currentState = FocusState.IdleToDecay;
                stateTickTimer = 0;
            }
        }
    }

    private void FocusUpdateInventoryLogic(Player player)
    {

        if (currentState == FocusState.IdleToDecay)
        {
            if (++stateTickTimer >= ticksUntilDecayStarts)
            {

                currentState = FocusState.Decaying;
                stateTickTimer = 0;
            }
        }
        else if (currentState == FocusState.Decaying)
        {
            if (++stateTickTimer >= tickDecayRate && charge > 0)
            {

                charge--;
                stateTickTimer = 0;
            }
        }
    }

    private void FocusNetSendLogic(BinaryWriter writer)
    {
        // Cast enum to int for transmission
        writer.Write((int)currentState);
        writer.Write(charge);
        writer.Write(stateTickTimer);
    }

    private void FocusNetReceiveLogic(BinaryReader reader)
    {
        // Cast back to FocusState
        currentState = (FocusState)reader.ReadInt32();
        charge = reader.ReadInt32();
        stateTickTimer = reader.ReadInt32();
    }

    private void FocusModifyWeaponDamageLogic(Player player, ref StatModifier damage)
    {
        damage.Base = Item.damage + ((charge * dpsIncrease) / 60);
    }

    // LOGIC FUNCTIONS




    // DO NOT TOUCH THESE FUNCTIONS
    public override void HoldItem(Player player)
    {
        FocusHoldLogic(player);
        HoldItemFocus(player);
    }

    public override void UpdateInventory(Player player)
    {
        FocusUpdateInventoryLogic(player);
        UpdateInventoryFocus(player);
    }

    public override void NetSend(BinaryWriter writer)
    {
        FocusNetSendLogic(writer);
        NetSendFocus(writer);
    }

    public override void NetReceive(BinaryReader reader)
    {
        FocusNetReceiveLogic(reader);
        NetReceiveFocus(reader);
    }

    public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
    {
        FocusModifyWeaponDamageLogic(player, ref damage);
        ModifyWeaponDamageFocus(player, ref damage);
    }

    // DO NOT TOUCH THESE FUNCTIONS



    // OTHER FUNCTIONS

    private static void DustHandler(Player player, int amtOfDust, int pOff, int SpOff, int ScOff)
    {
        var rand = new Random();

        int SpOffUse = SpOff * 2;

        for (int i = 0; i < amtOfDust; i++)
        {
            float xSpeed = SpOffUse * (rand.NextSingle() - 0.5f);
            float ySpeed = SpOffUse * (rand.NextSingle() - 0.5f);
            float scale = ScOff * rand.NextSingle();

            int xOffset = rand.Next(-pOff, pOff);
            int yOffset = rand.Next(-pOff, pOff);
            var position = new Vector2(player.Center.X + xOffset, player.Center.Y + yOffset);

            Dust.NewDust(position, 2, 2, ModContent.DustType<EmotionDust>(), xSpeed, ySpeed, 0, Color.LightGoldenrodYellow, scale);
        }
    }
}