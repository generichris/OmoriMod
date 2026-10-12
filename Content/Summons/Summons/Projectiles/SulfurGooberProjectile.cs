using Terraria.ModLoader;

using OmoriMod.Content.Summons.Summons.Buffs;

namespace OmoriMod.Content.Summons.Summons.Projectiles
{
    public class SulfurGooberProjectile : GooberSummonProjectile
    {
        protected override int FrameCount => 13;
        protected override int HitboxWidth => 31;
        protected override int HitboxHeight => 14;
        protected override int BuffType => ModContent.BuffType<SulfurGooberBuff>();
        protected override int BulletType => ModContent.ProjectileType<SulfurGooberBulletProjectile>();
    }
}
