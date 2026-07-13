using OmoriMod.Content.Summons.Summons.Projectiles;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Summons.Summons.Buffs;

public class SentientKnifeBuff : ModBuff
{
    //props to Lynx on youtube for providing the following code
    public override void SetStaticDefaults()
    {
        Main.buffNoTimeDisplay[Type] = true;
        Main.buffNoSave[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        // If the minions exist reset the buff time, otherwise remove the buff from the player
        if (player.ownedProjectileCounts[ModContent.ProjectileType<SentientKnifeProjectile>()] > 0)
        {
            player.buffTime[buffIndex] = 18000;
        }
        else
        {
            player.DelBuff(buffIndex);
            buffIndex--;
        }
    }
}