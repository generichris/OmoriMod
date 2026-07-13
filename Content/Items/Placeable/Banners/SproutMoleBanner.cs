using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OmoriMod.Content.Tiles.Banners;

namespace OmoriMod.Content.Items.Placeable.Banners
{
    public class SproutMoleBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SproutMoleBannerTile>());
            Item.width = 10;
            Item.height = 24;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 10);

        }
    }
}