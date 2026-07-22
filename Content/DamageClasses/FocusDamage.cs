using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.DamageClasses;

public class FocusDamage : DamageClass
{


    // This class uses standard crit calculations
    public override bool UseStandardCritCalcs => true;

    public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
    {

        return damageClass == Generic
            ? StatInheritanceData.Full
            : new StatInheritanceData(
            damageInheritance: 0f,
            critChanceInheritance: 0f,
            attackSpeedInheritance: 0f,
            armorPenInheritance: 0f,
            knockbackInheritance: 0f
            );
        /*
        if (damageClass == DamageClass.Ranged)
            return new StatInheritanceData(
                damageInheritance: 1f,
                critChanceInheritance: -1f,
                attackSpeedInheritance: 0.4f,
                armorPenInheritance: 2.5f,
                knockbackInheritance: 0f
            );
        */
        // This would allow our custom class to benefit from the following ranged stat bonuses:
        // - Damage, at 100% effectiveness
        // - Attack speed, at 40% effectiveness
        // - Crit chance, at -100% effectiveness (this means anything that raises ranged crit chance specifically will lower the crit chance of our custom class by the same amount)
        // - Armor penetration, at 250% effectiveness

        // CAUTION: There is no hardcap on what you can set these to. Please be aware and advised that whatever you set them to may have unintended consequences,
        // and that we are NOT responsible for any temporary or permanent damage caused to you, your character, or your world as a result of your morbid curiosity.
        // To refer to a non-vanilla damage class for these sorts of things, use "ModContent.GetInstance<TargetDamageClassHere>()" instead of "DamageClass.XYZ".
    }

    public override bool GetEffectInheritance(DamageClass damageClass)
    {
        // For now we will use ranged and magic effects
        // This method allows you to make your damage class benefit from and be able to activate other classes' effects (e.g. Spectre bolts, Magma Stone) based on what returns true.
        // Note that unlike our stat inheritance methods up above, you do not need to account for universal bonuses in this method.
        // For this example, we'll make our class able to activate melee- and magic-specifically effects.
        if (damageClass == Ranged)
            return true;
        return damageClass == Magic;
    }

    public override bool ShowStatTooltipLine(Player player, string lineName)
    {
        /*if (lineName == "Speed" || lineName == "CritChance")
        {
            return false;
        }
        */

        return true;
    }

    /*
    public override void SetDefaultStats(Player player)
    {
        // This method lets you set default statistical modifiers for your example damage class.
        // Here, we'll make our example damage class have more critical strike chance and armor penetration than normal.
        player.GetCritChance<ExampleDamageClass>() += 4;
        player.GetArmorPenetration<ExampleDamageClass>() += 10;
        // These sorts of modifiers also exist for damage (GetDamage), knockback (GetKnockback), and attack speed (GetAttackSpeed).
        // You'll see these used all around in referencce to vanilla classes and our example class here. Familiarize yourself with them.
    }

    */
}