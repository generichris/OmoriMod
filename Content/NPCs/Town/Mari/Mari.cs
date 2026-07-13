using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using OmoriMod.Systems;
using OmoriMod.Util;

namespace OmoriMod.Content.NPCs.Town.Mari
{
    [AutoloadHead]
    public class Mari : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 26;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return DownedBossSystem.IsDowned("YeOldSprout".OmoriModString());
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 25;
            NPC.scale = 2f;
            NPC.aiStyle = NPCAIStyleID.Passive;
            AnimationType = NPCID.Guide;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 250;
            NPC.knockBackResist = 0.5f;
            NPC.friendly = true;
            NPC.townNPC = true;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
            }
        }

        public override ITownNPCProfile TownNPCProfile()
        {
            return new Terraria.GameContent.Profiles.DefaultNPCProfile("OmoriMod/Content/NPCs/Town/Mari/Mari", NPCHeadLoader.GetHeadSlot(HeadTexture));
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                shop = ShopName;
            }
        }

        public const string ShopName = "Shop";

        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, ShopName);

            npcShop.Add<Content.Items.Weapons.Melee.Tier1.Bat>();
            npcShop.Add<Content.Items.Weapons.Melee.Tier1.FryingPan>();
            npcShop.Add<Content.Items.Weapons.Melee.Tier1.Knife>();

            npcShop.Add<Content.Items.Weapons.Melee.Tier2.CorruptionBat>(Condition.DownedEowOrBoc);
            npcShop.Add<Content.Items.Weapons.Melee.Tier2.CorruptionKnife>(Condition.DownedEowOrBoc);
            npcShop.Add<Content.Items.Weapons.Melee.Tier2.CorruptionPan>(Condition.DownedEowOrBoc);
            npcShop.Add<Content.Items.Weapons.Melee.Tier2.CrimsonBat>(Condition.DownedEowOrBoc);
            npcShop.Add<Content.Items.Weapons.Melee.Tier2.CrimsonKnife>(Condition.DownedEowOrBoc);
            npcShop.Add<Content.Items.Weapons.Melee.Tier2.CrimsonPan>(Condition.DownedEowOrBoc);

            npcShop.Add<Content.Items.Weapons.Melee.Tier3.HellBat>(Condition.Hardmode);
            npcShop.Add<Content.Items.Weapons.Melee.Tier3.HellKnife>(Condition.Hardmode);
            npcShop.Add<Content.Items.Weapons.Melee.Tier3.HellPan>(Condition.Hardmode);

            npcShop.Add<Content.Items.Weapons.Melee.Tier4.HallowBat>(Condition.DownedMechBossAny);
            npcShop.Add<Content.Items.Weapons.Melee.Tier4.HallowKnife>(Condition.DownedMechBossAny);
            npcShop.Add<Content.Items.Weapons.Melee.Tier4.HallowPan>(Condition.DownedMechBossAny);

            npcShop.Add<Content.Items.Weapons.Melee.Tier5.ChlorBat>(Condition.DownedPlantera);
            npcShop.Add<Content.Items.Weapons.Melee.Tier5.ChlorKnife>(Condition.DownedPlantera);
            npcShop.Add<Content.Items.Weapons.Melee.Tier5.ChlorPan>(Condition.DownedPlantera);

            npcShop.Add<Content.Items.Weapons.Magic.Tier1.AngryBolt>();
            npcShop.Add<Content.Items.Weapons.Magic.Tier1.HappyBolt>();
            npcShop.Add<Content.Items.Weapons.Magic.Tier1.SadBolt>();

            npcShop.Add<Content.Items.Weapons.Magic.Tier2.AngryBundle>(Condition.DownedEowOrBoc);
            npcShop.Add<Content.Items.Weapons.Magic.Tier2.HappyBundle>(Condition.DownedEowOrBoc);
            npcShop.Add<Content.Items.Weapons.Magic.Tier2.SadBundle>(Condition.DownedEowOrBoc);

            npcShop.Add<Content.Items.Weapons.Magic.Tier3.AngryBomb>(Condition.Hardmode);
            npcShop.Add<Content.Items.Weapons.Magic.Tier3.HappyBomb>(Condition.Hardmode);
            npcShop.Add<Content.Items.Weapons.Magic.Tier3.SadBomb>(Condition.Hardmode);

            npcShop.Add<Content.Items.Weapons.Magic.Tier4.AngryBombPlus>(Condition.DownedMechBossAny);
            npcShop.Add<Content.Items.Weapons.Magic.Tier4.HappyBombPlus>(Condition.DownedMechBossAny);
            npcShop.Add<Content.Items.Weapons.Magic.Tier4.SadBombPlus>(Condition.DownedMechBossAny);

            npcShop.Add<Content.Items.Ammo.Arrows.Regular.Tier1.AngryArrow>();
            npcShop.Add<Content.Items.Ammo.Arrows.Regular.Tier1.HappyArrow>();
            npcShop.Add<Content.Items.Ammo.Arrows.Regular.Tier1.SadArrow>();
            npcShop.Add<Content.Items.Ammo.Arrows.Regular.Tier2.AngryArrowPlus>(Condition.Hardmode);
            npcShop.Add<Content.Items.Ammo.Arrows.Regular.Tier2.HappyArrowPlus>(Condition.Hardmode);
            npcShop.Add<Content.Items.Ammo.Arrows.Regular.Tier2.SadArrowPlus>(Condition.Hardmode);

            npcShop.Add<Content.Items.Ammo.Bullets.Regular.Tier1.AngryBullet>();
            npcShop.Add<Content.Items.Ammo.Bullets.Regular.Tier1.HappyBullet>();
            npcShop.Add<Content.Items.Ammo.Bullets.Regular.Tier1.SadBullet>();
            npcShop.Add<Content.Items.Ammo.Bullets.Regular.Tier2.AngryBulletPlus>(Condition.Hardmode);
            npcShop.Add<Content.Items.Ammo.Bullets.Regular.Tier2.HappyBulletPlus>(Condition.Hardmode);
            npcShop.Add<Content.Items.Ammo.Bullets.Regular.Tier2.SadBulletPlus>(Condition.Hardmode);

            npcShop.Add<Content.Items.BossRelated.YeOldSproutWeapons.SproutBullet>();

            npcShop.Add<Content.Items.Accessories.Flower>();
            npcShop.Add<Content.Items.Accessories.DeadFlower>();
            npcShop.Add<Content.Items.Accessories.BloodyFlower>();
            npcShop.Add<Content.Items.Accessories.RabbitsFoot>();

            npcShop.Add<Content.Items.BuffItems.PartyPopper>();
            npcShop.Add<Content.Items.BuffItems.RainCloud>();
            npcShop.Add<Content.Items.BuffItems.AirHorn>();
            npcShop.Add<Content.Items.BuffItems.Firecracker>(Condition.Hardmode);
            npcShop.Add<Content.Items.BuffItems.EmotionalAmplifier>(Condition.Hardmode);

            npcShop.Add<Content.Items.Health.Sweets>();
            npcShop.Add<Content.Items.Health.Tofu>();
            npcShop.Add<Content.Items.Mana.AppleJuice>();
            npcShop.Add<Content.Items.Mana.OrangeJuice>();
            npcShop.Add<Content.Items.FocusItems.BrainFocus>();

            npcShop.Add<Content.Summons.Pets.Items.Something>();
            npcShop.Add<Content.Summons.Summons.Items.GShroober>();
            npcShop.Add<Content.Summons.Summons.Items.SentientKnife>();

            npcShop.Register();
        }

        public override string GetChat()
        {
            string[] lines =
            {
                "The garden's looking nice today, isn't it?",
                "I like taking pictures of the flowers around here.",
                "Sunny said he'd stop by later."
            };

            return lines[Main.rand.Next(lines.Length)];
        }

        public override bool CanChat()
        {
            return true;
        }
    }
}