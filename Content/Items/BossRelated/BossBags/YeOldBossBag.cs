using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.BossRelated.YeOldSproutWeapons;
using OmoriMod.Content.Items.Health;
using OmoriMod.Content.NPCs.Enemies.Bosses.YeOldSprout;

using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.BossRelated.BossBags;

public class YeOldBossBag : OmoriModItem
{
    YeOldBossBag()
    {
        itemTypeForResearch = ItemTypeForResearch.TreasureBag_BossSummons_Dye;
    }

    public override void OmoriModItemSetStaticDefaults()
    {
        // boss bag
        ItemID.Sets.BossBag[Type] = true;

        // pre-hardmode boss bag
        ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;
    }
    public override void SetDefaults()
    {
        ItemDefaults(
            width: 32,
            height: 32,
            scale: 1f,
            buyPrice: 0,
            stackSize: 9999,
            consumable: true
            );

        // expert mode only
        Item.expert = true;
    }

    public override bool CanRightClick()
    {
        return true;
    }

    public override void ModifyItemLoot(ItemLoot itemLoot)
    {
        int[] weaponOptions = [
            ModContent.ItemType<SproutShotgun>(),
            ModContent.ItemType<SproutScythe>(),
            ];
        itemLoot.Add(ItemDropRule.OneFromOptions(1, weaponOptions));


        itemLoot.Add(ItemDropRule.Common(ItemID.LesserHealingPotion, 1, 8, 12));
        itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tofu>(), 1, 30, 50));
        itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<YeOldSprout>()));

    }
}