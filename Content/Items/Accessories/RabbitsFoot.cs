using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;

namespace OmoriMod.Content.Items.Accessories
{
    public class RabbitsFoot : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.jumpSpeedBoost += 3.0f; //Frog Leg is 2.4f so this is fine prob if i could test but noooooooooooooooooooo i cant bumb error vro
            player.autoJump = true;

            player.moveSpeed += 0.50f;
        }

    }
}
