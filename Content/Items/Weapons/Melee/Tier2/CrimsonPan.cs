using Microsoft.Xna.Framework;

using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Items.Weapons.Melee.Tier1;
using OmoriMod.Content.Projectiles.Friendly.Melee.Pan;
using OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.Registries;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Melee.Tier2;

public class CrimsonPan : HappyItem
{
    CrimsonPan()
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
            extraItemID: ItemID.CrimtaneBar,
            extraItemAmount: 10,
            craftingStationID: TileID.Anvils
            );
    }
}