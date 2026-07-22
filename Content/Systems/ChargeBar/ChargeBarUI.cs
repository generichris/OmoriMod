using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using OmoriMod.Content.Items.Abstract_Classes;
using OmoriMod.Content.Items.Abstract_Classes.BaseClasses;
using OmoriMod.Content.Players;

using ReLogic.Content;

using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace OmoriMod.Content.Systems.ChargeBar;

public class ChargeBarUI : UIState
{

    private UIElement area;

    private UIImage barFrame;

    private static Texture2D barTexture;

    private static Texture2D barTextureFull;
    public override void OnInitialize()
    {
        area = new UIElement();
        area.Top.Set(-25, 0f);
        area.Left.Set(64, 0f);
        area.Width.Set(182, 0f);
        area.Height.Set(60, 0f);
        area.HAlign = area.VAlign = 0.5f;


        barFrame = new UIImage(ModContent.Request<Texture2D>("OmoriMod/Content/Systems/ChargeBar/ChargeBarFrame"));

        barTexture = ModContent.Request<Texture2D>("OmoriMod/Content/Systems/ChargeBar/ChargeBarMiddle", AssetRequestMode.ImmediateLoad).Value;
        barTextureFull = ModContent.Request<Texture2D>("OmoriMod/Content/Systems/ChargeBar/ChargeBarFull", AssetRequestMode.ImmediateLoad).Value;

        barFrame.Left.Set(0, 0f);
        barFrame.Top.Set(0, 0f);
        barFrame.Width.Set(138, 0f);
        barFrame.Height.Set(34, 0f);

        area.Append(barFrame);
        Append(area);

    }

    public override void Draw(SpriteBatch spriteBatch)
    {

        if (Main.LocalPlayer.HeldItem.ModItem is not FocusItem)
        {
            return;
        }

        // typically base functions are empty, but not this one
        base.Draw(spriteBatch);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        DrawChargeBar(spriteBatch);
    }

    private void DrawChargeBar(SpriteBatch spriteBatch)
    {
        FocusPlayer modPlayer = Main.LocalPlayer.GetModPlayer<FocusPlayer>();

        // Calculate percent full
        float percentage = (float)((float)(modPlayer.currentCharge) / (float)(modPlayer.maxCharge));

        // make sure percent doesn't go above 1
        percentage = Utils.Clamp(percentage, 0f, 1f);

        // Here we get the screen dimensions of the barFrame element,
        // then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient.
        Rectangle innerBar = barFrame.GetInnerDimensions().ToRectangle();
        innerBar.X += 4;
        innerBar.Width -= 94;
        innerBar.Y += 3;
        innerBar.Height -= 21;

        if (percentage >= 1f)
        {
            spriteBatch.Draw(barTextureFull, innerBar, Color.White);
            return;
        }

        int currentWidth = (int)(innerBar.Width * percentage);


        Rectangle sourceRect = new Rectangle(0, 0, currentWidth, barTexture.Height);

        // Adjust destination rectangle width to match percentage
        Rectangle destRect = new Rectangle(innerBar.X, innerBar.Y, (int)(innerBar.Width * percentage), innerBar.Height);
        spriteBatch.Draw(barTexture, destRect, sourceRect, Color.White);
    }

    public override void Update(GameTime gameTime)
    {
        if (Main.LocalPlayer.HeldItem.ModItem is not FocusItem)
        {
            return;
        }

        // typically base functions are empty, but not this one
        base.Update(gameTime);
    }
}