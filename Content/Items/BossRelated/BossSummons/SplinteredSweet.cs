using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Items.Health;
using OmoriMod.Content.NPCs.Enemies.Bosses.SweetHeart;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Items.BossRelated.BossSummons;

public class SplinteredSweet : OmoriModItem
{

    SplinteredSweet()
    {
        itemTypeForResearch = ItemTypeForResearch.TreasureBag_BossSummons_Dye;
    }

    public override void SetDefaults()
    {
        ItemDefaults(
            width: 16,
            height: 16,
            scale: 1,
            buyPrice: Item.buyPrice(platinum: 0, gold: 0, silver: 15, copper: 0),
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
        return !NPC.AnyNPCs(ModContent.NPCType<SweetHeart>());
    }

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            // If the player using the item is the client
            // (explicitely excluded serverside here)
            SoundEngine.PlaySound(SoundID.Roar, player.position);

            int type = ModContent.NPCType<SweetHeart>();

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                // If the player is not in multiplayer, spawn directly
                NPC.SpawnOnPlayer(player.whoAmI, type);
            }
            else
            {
                // If the player is in multiplayer, request a spawn
                // This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in MinionBossBody
                NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
            }
        }

        return true;
    }

    public override void AddRecipes()
    {
        MakeRegularRecipe(ModContent.ItemType<Sweets>(), 10, TileID.Anvils);
    }
}