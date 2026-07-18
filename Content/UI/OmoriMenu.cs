using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OmoriMod.Content.UI
{
    public class OmoriMenu : ModMenu
    {
        private Asset<Texture2D> _background;

        public override string DisplayName => "Omori Mod";
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/Menu");
        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("OmoriMod/Content/UI/OmoriMenu");

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color colorp)
        {
            _background ??= ModContent.Request<Texture2D>("OmoriMod/Content/UI/OmoriMenuBackground");
            Rectangle destination = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
            spriteBatch.Draw(_background.Value, destination, Color.White);

            return true;
        }
    }
}