using OmoriMod.Content.Systems.AbilitySystem.ItemAbilities.Registries;

using Terraria.ModLoader;

namespace OmoriMod.Content.Systems.AbilitySystem;

public class AbilitySystem : ModSystem
{
    public override void PostSetupContent()
    {
        PassiveAbilityRegistry.Initialize();
        ActiveAbilityRegistry.Initialize();
    }
}