using Terraria;

namespace OmoriMod.Systems.AbilitySystem.ItemAbilities.AbilityContexts;

public class PassiveAbilityOnHitContext(
    Player player,
    Item item,
    NPC target,
    int damageDone,
    float knockback,
    bool crit
    ) : AbilityContext(player, item)
{
    public NPC Target { get; set; } = target;
    public int DamageDone { get; set; } = damageDone;
    public float Knockback { get; set; } = knockback;
    public bool Crit { get; set; } = crit;
}