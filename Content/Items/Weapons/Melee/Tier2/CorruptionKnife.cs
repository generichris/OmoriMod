using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Items.Weapons.Melee.Tier1;
using OmoriMod.Content.Projectiles.Friendly.Melee.Knife;
using OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.Registries;

using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Melee.Tier2;

public class CorruptionKnife : SadItem
{
    CorruptionKnife()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        InnatePassiveAbilityID = (int)PassiveAbilityRegistry.PassiveAbilityID.SINGLE_PHANTOM_KNIFE;
        EmotionItemCloneWithDifferentProjectile<CorruptionBat>(ModContent.ProjectileType<KnifeProjectile>());
    }



    public override void AddRecipes()
    {
        MakeUpgradeRecipe(
            baseItemID: ModContent.ItemType<Knife>(),
            extraItemID: ItemID.DemoniteBar,
            extraItemAmount: 10,
            craftingStationID: TileID.Anvils
            );
    }
}