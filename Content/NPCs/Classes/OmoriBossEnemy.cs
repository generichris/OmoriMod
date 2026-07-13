using Terraria;
using Terraria.ID;

namespace OmoriMod.Content.NPCs.Classes;

public abstract class OmoriBossEnemy : OmoriBehaviourNPC
{
    protected string bossName;

    /// <summary>
    /// Allows you to set all your NPC's properties, such as width, damage, aiStyle, lifeMax, etc. <br/>
    /// <see cref="NPC.boss"/> is already set to <c>true</c>
    /// </summary>
    public virtual void SetDefaultsBossEnemy() { }
    public override void SetDefaults()
    {
        NPC.boss = true;
        NPCID.Sets.MPAllowedEnemies[Type] = true;
        // Automatically group with other bosses
        NPCID.Sets.BossBestiaryPriority.Add(Type);

        SetDefaultsBossEnemy();
    }
}