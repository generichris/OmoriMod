using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace OmoriMod.Content.Buffs.Abstract;

/// <summary>
/// A buff that does nothing. Used for custom buff logic (such as emotions).
/// </summary>
public class DummyBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        player.DelBuff(buffIndex);
        buffIndex -= 1;
    }

    public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
    {
        buffName = "";
        tip = "";
    }
    public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
    {
        return false;
    }
}