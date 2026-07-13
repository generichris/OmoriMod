using OmoriMod.Systems.AbilitySystem.ItemAbilities.Registries;

using Terraria.ModLoader;

namespace OmoriMod.Systems.AbilitySystem;

public class AbilitySystem : ModSystem
{
    public override void PostSetupContent()
    {
        PassiveAbilityRegistry.Initialize();
        ActiveAbilityRegistry.Initialize();
    }
}