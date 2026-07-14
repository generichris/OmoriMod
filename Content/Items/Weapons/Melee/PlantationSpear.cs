using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;

namespace OmoriMod.Content.Items.Weapons.Melee
{
    // Not craftable - obtain this some other way (e.g. an NPC drop or added to a chest/loot pool).
    public class PlantationSpear : OmoriModItem
    {
        PlantationSpear()
        {
            itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.scale = 2f;

            Item.damage = 25;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 4f;

            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(silver: 60);
            Item.rare = ItemRarityID.Green;
        }
    }
}