using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Items.Weapons.Melee.Tier1;
using OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.Registries;

using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Melee.Tier2;

public class CrimsonBat : AngryItem
{
    CrimsonBat()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        InnatePassiveAbilityID = (int)PassiveAbilityRegistry.PassiveAbilityID.SINGLE_PHANTOM_BAT;
        EmotionItemClone<CorruptionBat>();
    }



    public override void AddRecipes()
    {
        MakeUpgradeRecipe(
            baseItemID: ModContent.ItemType<Bat>(),
            extraItemID: ItemID.CrimtaneBar,
            extraItemAmount: 10,
            craftingStationID: TileID.Anvils
            );
    }
}