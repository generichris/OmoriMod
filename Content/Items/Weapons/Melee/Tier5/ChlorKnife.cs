using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Abstract_Classes.Emotion_Classes;
using OmoriMod.Content.Items.Weapons.Melee.Tier4;
using OmoriMod.Content.Projectiles.Friendly.Melee.Knife;
using OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.Registries;

using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.Weapons.Melee.Tier5;

public class ChlorKnife : SadItem
{
    ChlorKnife()
    {
        itemTypeForResearch = ItemTypeForResearch.Weapons_Tools_Armor_Accessory;
    }
    public override void SetDefaults()
    {
        InnatePassiveAbilityID = (int)PassiveAbilityRegistry.PassiveAbilityID.QUINTUPLE_SEEKING_PHANTOM_KNIFE;
        EmotionItemCloneWithDifferentProjectile<ChlorBat>(ModContent.ProjectileType<KnifeProjectileFiveSeeking>());
    }



    public override void AddRecipes()
    {
        MakeUpgradeRecipe(
            baseItemID: ModContent.ItemType<HallowKnife>(),
            extraItemID: ItemID.ChlorophyteBar,
            extraItemAmount: 25,
            craftingStationID: TileID.MythrilAnvil
            );
    }
}