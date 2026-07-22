using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Systems;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.TestingItems;

public class TestBow : OmoriModItem
{
    TestBow()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        Item.damage = 5;
        Item.DamageType = DamageClass.Ranged;
        Item.useTime = 20;
        Item.useAnimation = Item.useTime;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 6;
        Item.value = 00000;
        Item.rare = ItemRarityID.Green;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;

        Item.DefaultToBow(4, 4f);
        Item.useAmmo = AmmoID.Arrow;
        Item.shootSpeed = 3f;
    }
    public override bool CanRightClick()
    {
        return true;
    }
    public override void RightClick(Player player)
    {
        DownedBossSystem.ClearDowned("OmoriMod:YeOldSprout");
        Main.NewText("Now downedSprout should be false");
    }
}