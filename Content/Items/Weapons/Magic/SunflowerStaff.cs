using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using OmoriMod.Content.Projectiles.Friendly.Magic;

namespace OmoriMod.Content.Items.Weapons.Magic
{
    public class SunflowerStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Magic;
            Item.width = 44;
            Item.height = 44;
            Item.useTime = 25;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SunflowerProjectile>();
            Item.shootSpeed = 18f;
        }
    }
}