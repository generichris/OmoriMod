using System.IO;

using OmoriMod.Content.Players;
using OmoriMod.Content.Items.BossRelated.BossSummons;
using OmoriMod.Content.NPCs.Enemies.Bosses.SweetHeart;
using OmoriMod.Content.NPCs.Enemies.Bosses.YeOldSprout;
using OmoriMod.Content.Systems.EmotionSystem;

using Terraria;
using Terraria.ID;
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

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        OmoriModMessageType messageType = (OmoriModMessageType)reader.ReadByte();
        switch (messageType)
        {
            case OmoriModMessageType.SyncEmotionPlayer:
                HandleEmotionPlayerSync(reader, whoAmI);
                break;
            default:
                Logger.Warn($"Received unknown OmoriMod packet type '{messageType}'.");
                break;
        }
    }

    private static void HandleEmotionPlayerSync(BinaryReader reader, int whoAmI)
    {
        int transmittedPlayerIndex = reader.ReadByte();
        EmotionType emotion = (EmotionType)reader.ReadByte();
        int scalingLevel = reader.ReadByte();
        int playerIndex = Main.netMode == NetmodeID.Server
            ? whoAmI
            : transmittedPlayerIndex;

        if (playerIndex < 0 || playerIndex >= Main.maxPlayers)
        {
            return;
        }

        EmotionPlayer emotionPlayer = Main.player[playerIndex].GetModPlayer<EmotionPlayer>();
        bool accepted = emotionPlayer.TryApplySyncedScalingState(
            emotion,
            scalingLevel,
            requireActiveBuff: Main.netMode == NetmodeID.Server);
        if (Main.netMode != NetmodeID.Server)
        {
            return;
        }

        if (accepted)
        {
            emotionPlayer.SyncPlayer(toWho: -1, fromWho: whoAmI, newPlayer: false);
        }
        else
        {
            emotionPlayer.SyncPlayer(toWho: whoAmI, fromWho: -1, newPlayer: false);
        }
    }
}