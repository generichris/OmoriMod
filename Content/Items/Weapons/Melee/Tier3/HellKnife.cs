using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Weapons.Melee.Tier2;
using OmoriMod.Content.Projectiles.Friendly.Melee.Knife;
using OmoriMod.Systems.AbilitySystem.ItemAbilities.Registries;

using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Melee.Tier3;

public class HellKnife : SadItem
{
    HellKnife()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        InnatePassiveAbilityID = (int)PassiveAbilityRegistry.PassiveAbilityID.TRIPLE_PHANTOM_KNIFE;
        EmotionItemCloneWithDifferentProjectile<HellBat>(ModContent.ProjectileType<KnifeProjectileTriple>());
    }



    public override void AddRecipes()
    {
        MakeUpgradeRecipe(
            baseItemID: ModContent.ItemType<CorruptionKnife>(),
            extraItemID: ItemID.HellstoneBar,
            extraItemAmount: 15,
            craftingStationID: TileID.Anvils
            );
        MakeUpgradeRecipe(
            baseItemID: ModContent.ItemType<CrimsonKnife>(),
            extraItemID: ItemID.HellstoneBar,
            extraItemAmount: 15,
            craftingStationID: TileID.Anvils
            );
    }
}