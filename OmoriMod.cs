using OmoriMod.Content.Items.BossRelated.BossSummons;
using OmoriMod.Content.NPCs.Enemies.Bosses.SweetHeart;
using OmoriMod.Content.NPCs.Enemies.Bosses.YeOldSprout;

using Terraria.ModLoader;

namespace OmoriMod;


public class OmoriMod : Mod
{
    private static Mod s_modInstance;
    public const string ModName = "OmoriMod";

    public static Mod Mod { get => s_modInstance; }

    public OmoriMod()
    {
        s_modInstance = this;
    }

    public override void PostSetupContent()
    {
        if (ModLoader.TryGetMod("dementiaMod", out Mod dementiaMod))
        {
            dementiaMod.Call("AddBossSummon", ModContent.ItemType<MegaTofu>(), new[] { ModContent.NPCType<YeOldSprout>() });
            dementiaMod.Call("AddBossSummon", ModContent.ItemType<SplinteredSweet>(), new[] { ModContent.NPCType<SweetHeart>() });
        }
    }
}