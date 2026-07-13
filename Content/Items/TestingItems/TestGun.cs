using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.TestingItems;

public class TestGun : OmoriModItem
{
    TestGun()
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
        Item.DefaultToRangedWeapon(ProjectileID.Bullet, AmmoID.Bullet, 4, 8f);
        Item.useAmmo = AmmoID.Bullet;
    }
}