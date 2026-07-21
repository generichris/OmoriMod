using Terraria;
using Terraria.ModLoader;
using OmoriMod.Content.Summons.Summons.Projectiles;

namespace OmoriMod.Content.Summons.Summons.Buffs
{
    public class HappyGooberBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<HappyGooberProjectile>()] > 0)
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
}