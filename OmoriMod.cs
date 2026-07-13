using OmoriMod.Content.Items.BossRelated.BossSummons;
using OmoriMod.Content.NPCs.Enemies.Bosses.SweetHeart;
using OmoriMod.Content.NPCs.Enemies.Bosses.YeOldSprout;

using Terraria.ModLoader;

namespace OmoriMod;


public class OmoriMod : Mod
{
    private static Mod _modInstance;
    public const string MOD_NAME = "OmoriMod";

    public static Mod Mod { get => _modInstance; }

    public OmoriMod()
    {
        _modInstance = this;
    }

    public override void PostSetupContent()
    {
        if (ModLoader.TryGetMod("dementiaMod", out Mod dementiaMod))
        {
            dementiaMod.Call("AddBossSummon", ModContent.ItemType<MegaTofu>(), new int[] { ModContent.NPCType<YeOldSprout>() });
            dementiaMod.Call("AddBossSummon", ModContent.ItemType<SplinteredSweet>(), new int[] { ModContent.NPCType<SweetHeart>() });
        }
    }
}