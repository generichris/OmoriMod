using Terraria.ModLoader;

using OmoriMod.Content.Summons.Summons.Buffs;

namespace OmoriMod.Content.Summons.Summons.Projectiles
{
    public class SadGooberProjectile : GooberSummonProjectile
    {
        protected override int FrameCount => 6;
        protected override int HitboxWidth => 20;
        protected override int HitboxHeight => 20;
        protected override int BuffType => ModContent.BuffType<SadGooberBuff>();
        protected override int BulletType => ModContent.ProjectileType<SadGooberBulletProjectile>();
    }
}
