using Terraria.ModLoader;

using OmoriMod.Content.Summons.Summons.Buffs;

namespace OmoriMod.Content.Summons.Summons.Projectiles
{
    public class PearGooberProjectile : GooberSummonProjectile
    {
        protected override int FrameCount => 6;
        protected override int HitboxWidth => 20;
        protected override int HitboxHeight => 20;
        protected override int BuffType => ModContent.BuffType<PearGooberBuff>();
        protected override int BulletType => ModContent.ProjectileType<PearGooberBulletProjectile>();
    }
}
