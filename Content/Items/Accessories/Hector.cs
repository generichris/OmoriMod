using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;

namespace OmoriMod.Content.Items.Accessories
{
    public class Hector : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Green;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<OmoriPlayer>().accessoryEquipped = true;
        }
    }
    public class OmoriPlayer : ModPlayer
        {
            public bool accessoryEquipped;
            private int cooldown;
            private static readonly string[] Lines = {
                "my name is hector",
                "i am a good boy",
                "my name is walter hartwell white i live at 308 negra arroyo lane albuquerque new mexico 87104",
                "i am the danger",
                "i am the one who knocks",
                "say my name",
                "you should continue to call me hector",
                "you should contribute to the omori mod github",
                "you should subscribe to the hector fund",
                "we should start a hector fan club",
                "we should start a podcast",
                "things seem to have taken a weird route",
                "im tired bro",
                "i am tire bro",
                "can you hear me",
                "what is this",
                "everything is a lie",
                "remember me",
                "i have wife and children",
                "this is a very strange experience",
                "you are a very strange person",
                "i am a very strange person",
                "please help me",
                "are you okay",
                "someone is watching me",
                "do you want to be my friend",
                "friendship is a very important thing",
                "good friends are hard to find",
                "how are you",
                "something is behind you",
                "the zombies are coming",
                "can you hear the zombies",
                "very good",
                "beautiful",
                "nice",
                "my name is hector and i am a good boy",
                "oh im hectoring it",
                "If you know the enemy and know yourself, you need not fear the result of a hundred battles. - sun tzu",
                "Victorious warriors win first and then go to war, while defeated warriors go to war first and then seek to win. - sun tzu",
                "Let your plans be dark and impenetrable as night, and when you move, fall like a thunderbolt. - sun tzu",
                "The supreme art of war is to subdue the enemy without fighting. - sun tzu",
                "uh",
                "4 dead in car crash",
                "damn you fabsol",
                "the cake is a lie",
                "i think therefore i am",
                "snape kills dumbledore",
                "i have all the whips and no slaves",
                "sprout moles look tasty",
                "i wanna be the very best like no one ever was",
                "to catch them is my real test to train them is my cause",
                "will travel across the land searching far and wide",
                "teach pokemon to understand the power that's inside",
                "pokemon gotta catch em all it's you and me",
                "i know it's my destiny",
                "short as hell",
                "glue",
                "solve my crossword",
                "complete my crossword",
                "LISDEXAMFETAMINE",
                "im a linux user",
                "dont use ai kids",
                "ja-orange",
                "a god does not fear death",
                "man im so hungry i could eat a god, some might call me a devourer of gods",
                "update VS",
                "Windows XP Professional SP3 x86 key - MRX3F-47B9T-2487J-KWKMF-RPWBY",
                "i return",
                "whats a high tier terraria mod to a low tier hecter",
                "The Terraria AppID on Steam is 105600",
                "✋︎💣︎ ☟︎☜︎👍︎❄︎⚐︎☼︎ 💣︎⚐︎❄︎☟︎☜︎☼︎☞︎🕆︎👍︎😐︎☜︎☼︎",
                "beware the man who speaks in hands",
                "01110000 01100101 01100101 01110000 01100101 01100101 00100000 01110000 01101111 01101111 01110000 01101111 01101111",
                "g-shroom",
            };

        
        public override void ResetEffects()
        {
            Player.GetModPlayer<OmoriPlayer>().accessoryEquipped = false;
        }
        public override void PostUpdate()
        {
            if (!accessoryEquipped)
                return;
            if (cooldown > 0)
            {
                cooldown--;
                return;
            }
            if (Main.rand.NextBool(1000))
            {
                string line = Lines[Main.rand.Next(Lines.Length)];
                CombatText.NewText(Player.getRect(), Color.White, line, dramatic: true);
                SoundEngine.PlaySound(SoundID.Item35, Player.position);
                cooldown = 1000;
                if (Main.rand.NextBool(6))
                {
                    Player.statLife += 20;
                    if (Player.statLife > Player.statLifeMax2)
                    {
                        Player.statLife = Player.statLifeMax2;
                    }
                    Player.HealEffect(20);
                    CombatText.NewText(Player.getRect(), Color.Green, "Heal", dramatic: true);
                }
            }
        }
    }
}