using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Weapons.Melee.Tier3;
using OmoriMod.Content.Projectiles.Friendly.Melee.Pan;
using OmoriMod.Systems.AbilitySystem.ItemAbilities.Registries;

using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Melee.Tier4;

public class HallowPan : HappyItem
{
    HallowPan()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        InnatePassiveAbilityID = (int)PassiveAbilityRegistry.PassiveAbilityID.QUINTUPLE_PHANTOM_PAN;
        EmotionItemCloneWithDifferentProjectile<HallowBat>(ModContent.ProjectileType<PanProjectileFive>());
    }



    public override void AddRecipes()
    {
        MakeUpgradeRecipe(
            baseItemID: ModContent.ItemType<HellPan>(),
            extraItemID: ItemID.HallowedBar,
            extraItemAmount: 20,
            craftingStationID: TileID.MythrilAnvil
            );
    }
}