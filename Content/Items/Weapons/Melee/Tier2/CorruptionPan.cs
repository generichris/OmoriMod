using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Weapons.Melee.Tier1;
using OmoriMod.Content.Projectiles.Friendly.Melee.Pan;
using OmoriMod.Systems.AbilitySystem.ItemAbilities.Registries;

using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Melee.Tier2;

public class CorruptionPan : HappyItem
{
    CorruptionPan()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        InnatePassiveAbilityID = (int)PassiveAbilityRegistry.PassiveAbilityID.SINGLE_PHANTOM_PAN;
        EmotionItemCloneWithDifferentProjectile<CorruptionBat>(ModContent.ProjectileType<PanProjectile>());
    }



    public override void AddRecipes()
    {
        MakeUpgradeRecipe(
            baseItemID: ModContent.ItemType<FryingPan>(),
            extraItemID: ItemID.DemoniteBar,
            extraItemAmount: 10,
            craftingStationID: TileID.Anvils
            );
    }
}