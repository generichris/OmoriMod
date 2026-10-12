using Terraria.ModLoader;

using OmoriMod.Content.Summons.Summons.Buffs;

namespace OmoriMod.Content.Summons.Summons.Projectiles
{
    public class FearGooberProjectile : GooberSummonProjectile
    {
        protected override int FrameCount => 4;
        protected override int HitboxWidth => 32;
        protected override int HitboxHeight => 32;
        protected override int BuffType => ModContent.BuffType<FearGooberBuff>();
        protected override int BulletType => ModContent.ProjectileType<FearGooberBulletProjectile>();
    }
}
