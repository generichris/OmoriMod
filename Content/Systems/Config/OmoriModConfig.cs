using System.ComponentModel;

using Terraria.ModLoader.Config;

namespace OmoriMod.Systems.Config;

public class OmoriModConfig : ModConfig
{
    // Where the config is saved (client vs server)
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [DefaultValue(false)]
    public bool EnableDevMode { get; set; }
}