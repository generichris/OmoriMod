using Terraria.ModLoader;

namespace OmoriMod.Content.Players;


/// <summary>
/// Keeps track of ability related things. Such as whether the ability menu is up or not
/// </summary>
public class AbilityPlayer : ModPlayer
{
    public bool abilityMenuActive;

    public AbilityPlayer()
    {
        abilityMenuActive = false;
    }
}