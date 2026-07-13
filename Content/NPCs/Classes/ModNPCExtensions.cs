using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OmoriMod.Content.NPCs.Classes;

public static class ModNPCExtensions
{

    /// <summary>
    /// Moves the <see cref="NPC"/> horizontally
    /// </summary>
    /// <param name="speed">The speed of the movement</param>
    /// <param name="inertia">How much inertia the <see cref="NPC"/> should have. This value should be greater than 1</param>
    /// <param name="xDirection">What direction the movement is going</param>
    public static void MoveHorizontal(this ModNPC npc, float speed, float inertia, int xDirection)
    {
        if (xDirection == 0) { return; }
        NPC n = npc.NPC;
        var direction = new Vector2(xDirection, 0);
        direction.Normalize();
        n.velocity = (n.velocity * (inertia - 1) + direction * speed) / inertia;
    }

    /// <summary>
    /// Creates a set of unit vectors evenly spaced above the horizontal line, within ±maxAngle.
    /// </summary>
    /// <param name="npc">The NPC this extension is applied to (unused, just for extension).</param>
    /// <param name="amountOfVectors">Number of vectors to generate.</param>
    /// <param name="maxAngle">Maximum angle in degrees from horizontal.</param>
    /// <returns>A HashSet of Vector2 representing the unit vectors.</returns>
    public static HashSet<Vector2> CreateVectors(this ModNPC npc, int amountOfVectors, float maxAngle)
    {
        HashSet<Vector2> vectors = [];

        if (amountOfVectors <= 0) return vectors;

        // Calculate step angle between vectors
        float step = amountOfVectors == 1 ? 0f : maxAngle * 2f / (amountOfVectors - 1);

        for (int i = 0; i < amountOfVectors; i++)
        {
            // Compute the angle for this vector
            float angleDeg = -maxAngle + step * i; // -maxAngle is leftmost, +maxAngle is rightmost
            float angleRad = MathHelper.ToRadians(angleDeg) + 3 * (MathHelper.Pi / 2); // convert to radians

            // Create unit vector
            Vector2 v = new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));
            vectors.Add(Vector2.Normalize(v));
        }

        return vectors;
    }


    /// <summary>
    /// Returns the direction to the <paramref name="npc"/>'s target
    /// </summary>
    /// <param name="npc"></param>
    /// <returns></returns>
    public static int DirectionToTarget(this ModNPC npc)
    {
        if (npc.NPC.HasValidTarget)
        {
            if (float.IsNaN(npc.NPC.velocity.X)) { return 0; }
            return Math.Sign(Main.player[npc.NPC.target].Center.X - npc.NPC.Center.X);
        }
        return 0;
    }

    /// <summary>
    /// Gets this <see cref="NPC"/> to send something into the chat
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="message"></param>
    public static void SpeakNPC(this ModNPC npc, string message)
    {
        if (Main.netMode == NetmodeID.SinglePlayer)
        {
            Main.NewText(message);
            return;
        }
        else if (Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server)
        {
            ChatHelper.BroadcastChatMessage(NetworkText.FromKey(message, []), Color.White, -1);
        }

    }

    /// <summary>
    /// Finds the distance between 2 sets of coordinates
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="x1"></param>
    /// <param name="x2"></param>
    /// <param name="y1"></param>
    /// <param name="y2"></param>
    /// <returns></returns>
    public static double FindDistance(this ModNPC npc, double x1, double x2, double y1, double y2)
    {
        return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
    }
}