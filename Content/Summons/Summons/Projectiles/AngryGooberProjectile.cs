using Terraria.ModLoader;

using OmoriMod.Content.Summons.Summons.Buffs;

namespace OmoriMod.Content.Summons.Summons.Projectiles
{
    public class AngryGooberProjectile : GooberSummonProjectile
    {
        protected override int FrameCount => 4;
        protected override int HitboxWidth => 32;
        protected override int HitboxHeight => 32;
        protected override int BuffType => ModContent.BuffType<AngryGooberBuff>();
        protected override int BulletType => ModContent.ProjectileType<AngryGooberBulletProjectile>();
    }
}
