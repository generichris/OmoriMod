using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;

using OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.AbilityContexts;
using OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.Registries;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace OmoriMod.Content.Items.Abstract_Classes.BaseClasses;

public abstract class AbilityItem : OmoriModItem
{
    public int CurrentPassiveAbilityID = (int)PassiveAbilityRegistry.PassiveAbilityID.None;
    public int InnatePassiveAbilityID = (int)PassiveAbilityRegistry.PassiveAbilityID.None;

    public int CurrentActiveAbilityID = (int)ActiveAbilityRegistry.ActiveAbilityID.None;
    public int InnateActiveAbilityID = (int)ActiveAbilityRegistry.ActiveAbilityID.None;

    public override void SaveData(TagCompound tag)
    {
        base.SaveData(tag);
        tag["CurrentPassiveAbilityID"] = CurrentPassiveAbilityID;
        tag["CurrentActiveAbilityID"] = CurrentActiveAbilityID;
    }

    public override void LoadData(TagCompound tag)
    {
        base.LoadData(tag);
        if (tag.ContainsKey("CurrentPassiveAbilityID"))
        {
            CurrentPassiveAbilityID = tag.GetInt("CurrentPassiveAbilityID");
        }
        if (tag.ContainsKey("CurrentActiveAbilityID"))
        {
            CurrentActiveAbilityID = tag.GetInt("CurrentActiveAbilityID");
        }
    }

    public override void NetSend(BinaryWriter writer)
    {
        base.NetSend(writer);
        writer.Write(CurrentPassiveAbilityID);
        writer.Write(CurrentActiveAbilityID);
    }

    public override void NetReceive(BinaryReader reader)
    {
        base.NetReceive(reader);
        CurrentPassiveAbilityID = reader.ReadInt32();
        CurrentActiveAbilityID = reader.ReadInt32();
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (CanUsePassiveAbility())
        {
            var ability = PassiveAbilityRegistry.GetAbility(EffectivePassiveAbilityID);
            if (ability != null)
            {
                // Create Shoot Context
                AbilityContext context = new PassiveAbilityShootContext(player, Item, source, position, velocity, type, damage, knockback);
                bool result = ability.PerformAbility(context);
                // prevent vanilla code from running if ability runs
                if (result) return false;
            }
        }
        return base.Shoot(player, source, position, velocity, type, damage, knockback);
    }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(player, target, hit, damageDone);

        if (CanUsePassiveAbility())
        {
            var ability = PassiveAbilityRegistry.GetAbility(EffectivePassiveAbilityID);
            if (ability != null)
            {
                AbilityContext context = new PassiveAbilityOnHitContext(player, Item, target, damageDone, hit.Knockback, hit.Crit);
                ability.PerformAbility(context);
            }
        }
    }

    public override bool AltFunctionUse(Player player)
    {
        return CanUseActiveAbility();
    }

    public override bool? UseItem(Player player)
    {
        if (player.altFunctionUse == 2)
        {
            if (CanUseActiveAbility())
            {
                var ability = ActiveAbilityRegistry.GetAbility(EffectiveActiveAbilityID);
                if (ability != null)
                {
                    AbilityContext context = new ActiveAbilityContext(player, Item);
                    bool result = ability.PerformAbility(context);
                    if (result) return true;
                }
            }
        }
        return base.UseItem(player);
    }
    // Use this property to get the actual ability to perform. 
    public int EffectivePassiveAbilityID => CurrentPassiveAbilityID != (int)PassiveAbilityRegistry.PassiveAbilityID.None ? CurrentPassiveAbilityID : InnatePassiveAbilityID;
    public int EffectiveActiveAbilityID => CurrentActiveAbilityID != (int)ActiveAbilityRegistry.ActiveAbilityID.None ? CurrentActiveAbilityID : InnateActiveAbilityID;

    public virtual bool CanUsePassiveAbility()
    {
        return EffectivePassiveAbilityID != (int)PassiveAbilityRegistry.PassiveAbilityID.None;
    }

    public virtual bool CanUseActiveAbility()
    {
        return EffectiveActiveAbilityID != (int)ActiveAbilityRegistry.ActiveAbilityID.None;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        base.ModifyTooltips(tooltips);
        if (EffectivePassiveAbilityID != (int)PassiveAbilityRegistry.PassiveAbilityID.None)
        {
            // Optionally add tooltip about current ability
            // tooltips.Add(new TooltipLine(Mod, "Ability", $"Ability: {EffectivePassiveAbilityID}"));
        }
    }
}