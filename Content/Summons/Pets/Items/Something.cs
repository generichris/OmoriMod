using OmoriMod.Content.Summons.Abstract_Classes;
using OmoriMod.Content.Summons.Pets.Buffs;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Summons.Pets.Items;

public class Something : ModPetItem
{
    public override void PetSetDefaults()
    {
        // The projectile will come from the buff
        Item.buffType = ModContent.BuffType<SomethingBuff>();
    }
}