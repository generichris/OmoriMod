using OmoriMod.Systems.EmotionSystem;

using Terraria;
using Terraria.ModLoader;

namespace OmoriMod.Content.Dusts;

/// <summary>
/// Dust used for <see cref="EmotionType"/> effects.
/// </summary>
public class EmotionDust : ModDust
{
    public override void OnSpawn(Dust dust)
    {
        dust.noGravity = true;
        dust.noLight = true;
        dust.scale = 1f;
    }

}