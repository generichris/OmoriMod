using Microsoft.Xna.Framework;

using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;

using Terraria;
using Terraria.ID;

namespace OmoriMod.Content.Summons.Abstract_Classes;

public abstract class ModPetItem : OmoriModItem
{
    public ModPetItem()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }

    /// <summary>
    /// Use this to hook into <see cref="SetDefaults"/>
    /// </summary>
    public virtual void PetSetDefaults() { }

    public virtual void PetUseStyle() { }

    public override void SetDefaults()
    {
        // Pet summoning style
        Item.useStyle = ItemUseStyleID.Swing;
        Item.UseSound = SoundID.Item2;

        Item.useTime = 20;
        Item.useAnimation = Item.useTime;

        // Cost for pets
        Item.value = Item.buyPrice(platinum: 1, gold: 0, silver: 0, copper: 0);
        PetSetDefaults();
    }

    public override void UseStyle(Player player, Rectangle heldItemFrame)
    {
        if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
        {
            player.AddBuff(Item.buffType, 69);
        }
        PetUseStyle();
    }
}