using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.BossRelated.RabbitWeapons;
using OmoriMod.Content.Items.BossRelated.YeOldSproutWeapons;
using OmoriMod.Content.Items.Health;
using OmoriMod.Content.NPCs.Enemies.Bosses.Rabbit;

namespace OmoriMod.Content.Items.BossRelated.BossBags
{
    public class RabbitBossBag : OmoriModItem
    {
        RabbitBossBag()
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

            itemLoot.Add(ItemDropRule.Common(ItemID.LesserHealingPotion, 1, 8, 12));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<RabbitGun>()));

            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<Rabbit>()));

        }
    }
}
