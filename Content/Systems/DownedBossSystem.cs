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

namespace OmoriMod.Systems;

public class DownedBossSystem : ModSystem
{
    // Keep a single set of IDs for all downed bosses
    private static readonly HashSet<string> _downed = new(System.StringComparer.OrdinalIgnoreCase);

    // --- Public API ---
    public static bool IsDowned(string id) => _downed.Contains(id);

    /// <summary>
    /// Marks a boss as downed. Returns true if this was the first time.
    /// Also triggers a world-data sync on server so clients update.
    /// </summary>
    public static bool MarkDowned(string id)
    {
        if (!_downed.Add(id))
            return false;

        if (Main.netMode == NetmodeID.Server)
            NetMessage.SendData(MessageID.WorldData); // will invoke NetSend on server

        return true;
    }

    public static void ClearDowned(string id) => _downed.Remove(id);

    // --- Lifecycle ---
    public override void ClearWorld() => _downed.Clear(); // correct place to reset world state

    public override void SaveWorldData(TagCompound tag)
    {
        if (_downed.Count > 0)
            tag["downedBosses"] = _downed.ToList();
    }

    public override void LoadWorldData(TagCompound tag)
    {
        _downed.Clear();
        foreach (var s in tag.GetList<string>("downedBosses"))
            _downed.Add(s);
    }

    // --- Multiplayer sync ---
    public override void NetSend(BinaryWriter writer)
    {
        writer.Write(_downed.Count);
        foreach (var id in _downed)
            writer.Write(id);
    }

    public override void NetReceive(BinaryReader reader)
    {
        _downed.Clear();
        int n = reader.ReadInt32();
        for (int i = 0; i < n; i++)
            _downed.Add(reader.ReadString());
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