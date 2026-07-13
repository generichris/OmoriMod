using OmoriMod.Content.Items.Accessories;
using OmoriMod.Content.Items.BuffItems;
using OmoriMod.Content.Summons.Pets.Items;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.NPCs.Global;

public class GlobalShopNPC : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.Merchant)
        {
            shop.Add<PartyPopper>();
            shop.Add<RainCloud>();
            shop.Add<AirHorn>();
            shop.Add<Something>();
            shop.Add<Firecracker>(Condition.Hardmode);
        }

        if (shop.NpcType == NPCID.Dryad)
        {
            shop.Add<Flower>();
            shop.Add<DeadFlower>();
            shop.Add<BloodyFlower>();
        }
    }

    public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
    {
        if (npc.type == NPCID.Merchant && Main.hardMode)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    Item item = ModContent.GetModItem(ModContent.ItemType<EmotionalAmplifier>()).Item;
                    items[i] = item;
                    break;
                }
            }
        }
    }
}