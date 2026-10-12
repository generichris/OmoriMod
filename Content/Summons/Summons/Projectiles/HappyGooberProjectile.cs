using Terraria.ModLoader;

using OmoriMod.Content.Summons.Summons.Buffs;

namespace OmoriMod.Content.Summons.Summons.Projectiles
{
    public class HappyGooberProjectile : GooberSummonProjectile
    {
        protected override int FrameCount => 5;
        protected override int HitboxWidth => 32;
        protected override int HitboxHeight => 32;
        protected override int BuffType => ModContent.BuffType<HappyGooberBuff>();
        protected override int BulletType => ModContent.ProjectileType<HappyGooberBulletProjectile>();
    }
}
