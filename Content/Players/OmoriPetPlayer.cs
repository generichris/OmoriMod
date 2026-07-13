using Terraria.ModLoader;

namespace OmoriMod.Content.Players;

public class OmoriPetPlayer : ModPlayer
{
    public bool SomethingPet;

    public override void ResetEffects()
    {
        SomethingPet = false;
    }
}