using OmoriMod.Content.Players;
using OmoriMod.Content.Summons.Pets.Buffs;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.Summons.Pets.Projectiles;

public class SomethingProjectile : ModProjectile
{
    public override void SetStaticDefaults()
    {
        Main.projPet[Projectile.type] = true;
    }

    public override void SetDefaults()
    {
        // clone pet behavior
        Projectile.CloneDefaults(ProjectileID.DD2PetGato);
        AIType = ProjectileID.DD2PetGato;

        // size
        Projectile.height = 55;
        Projectile.width = 22;
    }

    public override bool PreAI()
    {
        // copying gato ai
        Player player = Main.player[Projectile.owner];
        player.petFlagDD2Gato = false;
        player.GetModPlayer<OmoriPetPlayer>().SomethingPet = true;
        return true;
    }

    public override void AI()
    {
        Player player = Main.player[Projectile.owner];

        if (!player.dead && player.HasBuff<SomethingBuff>())
        {
            Projectile.timeLeft = 2;
        }

    }
}