using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;

using OmoriMod.Content.Items.BossRelated.BossSummons;
using OmoriMod.Content.NPCs.Enemies.Bosses.Rabbit;
using OmoriMod.Content.NPCs.Enemies.Bosses.SweetHeart;
using OmoriMod.Content.NPCs.Enemies.Bosses.YeOldSprout;
using OmoriMod.Util;

namespace OmoriMod.Content.Systems;

public class DownedBossSystem : ModSystem
{
    // Keep a single set of IDs for all downed bosses
    private static readonly HashSet<string> Downed = new(System.StringComparer.OrdinalIgnoreCase);

    // --- Public API ---
    public static bool IsDowned(string id) => Downed.Contains(id);

    /// <summary>
    /// Marks a boss as downed. Returns true if this was the first time.
    /// Also triggers a world-data sync on server so clients update.
    /// </summary>
    public static bool MarkDowned(string id)
    {
        if (!Downed.Add(id))
            return false;

        if (Main.netMode == NetmodeID.Server)
            NetMessage.SendData(MessageID.WorldData); // will invoke NetSend on server

        return true;
    }

    public static void ClearDowned(string id) => Downed.Remove(id);

    // --- Lifecycle ---
    public override void ClearWorld() => Downed.Clear(); // correct place to reset world state

    public override void SaveWorldData(TagCompound tag)
    {
        if (Downed.Count > 0)
            tag["downedBosses"] = Downed.ToList();
    }

    public override void LoadWorldData(TagCompound tag)
    {
        Downed.Clear();
        foreach (var s in tag.GetList<string>("downedBosses"))
            Downed.Add(s);
    }

    // --- Multiplayer sync ---
    public override void NetSend(BinaryWriter writer)
    {
        writer.Write(Downed.Count);
        foreach (var id in Downed)
            writer.Write(id);
    }

    public override void NetReceive(BinaryReader reader)
    {
        Downed.Clear();
        int n = reader.ReadInt32();
        for (int i = 0; i < n; i++)
            Downed.Add(reader.ReadString());
    }

    public override void PostSetupContent()
    {
        // BossChecklist integration
        if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
        {
            bossChecklist.Call(
                "LogBoss",
                Mod,
                nameof(YeOldSprout),
                4.0f,
                (Func<bool>)(() => IsDowned("YeOldSprout".OmoriModString())),
                ModContent.NPCType<YeOldSprout>(),
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<MegaTofu>(),
                    ["spawnInfo"] = Language.GetOrRegister("Mods.OmoriMod.BossChecklist.YeOldSprout.SpawnInfo", () => "Use a Mega Tofu to summon Ye Old Sprout."),
                }
            );

            bossChecklist.Call(
                "LogBoss",
                Mod,
                nameof(SweetHeart),
                7.0f,
                (Func<bool>)(() => IsDowned("SweetHeart".OmoriModString())),
                ModContent.NPCType<SweetHeart>(),
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<SplinteredSweet>(),
                    ["spawnInfo"] = Language.GetOrRegister("Mods.OmoriMod.BossChecklist.SweetHeart.SpawnInfo", () => "Use a Splintered Sweet to summon Sweetheart."),
                }
            );

            bossChecklist.Call(
                "LogBoss",
                Mod,
                nameof(Rabbit),
                8.0f,
                (Func<bool>)(() => IsDowned("Rabbit".OmoriModString())),
                ModContent.NPCType<Rabbit>(),
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<RabbitsFootKeychain>(),
                    ["spawnInfo"] = Language.GetOrRegister("Mods.OmoriMod.BossChecklist.Rabbit.SpawnInfo", () => "Use a Rabbit's Foot Keychain to summon the Rabbit."),
                }
            );
        }
    }
}