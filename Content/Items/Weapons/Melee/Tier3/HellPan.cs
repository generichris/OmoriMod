using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Items.Weapons.Melee.Tier2;
using OmoriMod.Content.Projectiles.Friendly.Melee.Pan;
using OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.Registries;

using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Melee.Tier3;

public class HellPan : HappyItem
{
    HellPan()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        InnatePassiveAbilityID = (int)PassiveAbilityRegistry.PassiveAbilityID.TRIPLE_PHANTOM_PAN;
        EmotionItemCloneWithDifferentProjectile<HellBat>(ModContent.ProjectileType<PanProjectileTriple>());
    }



    public override void AddRecipes()
    {
        MakeUpgradeRecipe(
            baseItemID: ModContent.ItemType<CorruptionPan>(),
            extraItemID: ItemID.HellstoneBar,
            extraItemAmount: 15,
            craftingStationID: TileID.Anvils
            );
        MakeUpgradeRecipe(
            baseItemID: ModContent.ItemType<CrimsonPan>(),
            extraItemID: ItemID.HellstoneBar,
            extraItemAmount: 15,
            craftingStationID: TileID.Anvils
            );
    }
}