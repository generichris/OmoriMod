using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Accessories;
using OmoriMod.Content.Items.Health;
using OmoriMod.Content.NPCs.Enemies.Bosses.Rabbit;

namespace OmoriMod.Content.Items.BossRelated.BossSummons
{
    public class RabbitsFootKeychain : OmoriModItem
    {
        RabbitsFootKeychain()
        {
            itemTypeForResearch = ItemTypeForResearch.TreasureBag_BossSummons_Dye;
        }
        public override void SetDefaults()
        {
            ItemDefaults(
                width: 16,
                height: 16,
                scale: 1,
                buyPrice: Item.buyPrice(platinum: 0, gold: 2, silver: 50, copper: 0),
                stackSize: 1,
                consumable: false
                );

            AnimationDefaults(
                useTime: 20,
                useStyleID: ItemUseStyleID.HoldUp,
                useSound: SoundID.Roar,
                autoReuse: false
                );

            SetItemRarity(ItemRarityID.Purple);
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Rabbit>());
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {

                SoundEngine.PlaySound(SoundID.Roar, player.position);

                int type = ModContent.NPCType<Rabbit>();

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
                }
            }

            return true;
        }

        public override void AddRecipes()
        {
            Recipe r1 = CreateRecipe();
            r1.AddIngredient<RabbitsFoot>(30);
            r1.AddIngredient(ItemID.HellstoneBar, 1);
            r1.Register();
        }

    }
}
