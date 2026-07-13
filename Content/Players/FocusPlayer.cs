using Terraria.ModLoader;

namespace OmoriMod.Content.Players;

public class FocusPlayer : ModPlayer
{

    public bool hasChargeItem;
    public bool reachedMaxCharge;
    public int currentCharge;
    public int maxCharge;

    public override void ResetEffects()
    {
        hasChargeItem = false;
        currentCharge = 0;
        maxCharge = 0;
    }
}