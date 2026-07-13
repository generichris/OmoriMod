using System.Collections.Generic;

using Microsoft.Xna.Framework;

using OmoriMod.Systems.AbilitySystem.AbilityMenuUI;
using OmoriMod.Systems.ChargeBar;

using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace OmoriMod.Systems;

public class OmoriModUISystem : ModSystem
{

    private UserInterface chargeBarInterface;
    private ChargeBarUI chargeBar;

    private UserInterface abilityMenuInterface;
    private AbilityMenu abilityMenu;

    public override void Load()
    {
        // A server doesn't need UI lol
        if (!Main.dedServ)
        {
            chargeBar = new ChargeBarUI();
            chargeBarInterface = new UserInterface();
            chargeBarInterface.SetState(chargeBar);

            abilityMenu = new AbilityMenu();
            abilityMenuInterface = new UserInterface();
            abilityMenuInterface.SetState(abilityMenu);
        }
    }

    public override void UpdateUI(GameTime gameTime)
    {
        chargeBarInterface?.Update(gameTime);
        abilityMenuInterface?.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        // choose which layer to put our UI. This layer works for us
        int generalUIIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Interface Logic 2"));
        if (generalUIIndex != -1)
        {
            layers.Insert(generalUIIndex, new LegacyGameInterfaceLayer(
                OmoriMod.MOD_NAME + "AbilityMenu",
                delegate
                {
                    abilityMenuInterface.Draw(Main.spriteBatch, new GameTime());
                    return true;
                },
                InterfaceScaleType.UI));

            layers.Insert(generalUIIndex, new LegacyGameInterfaceLayer(
                OmoriMod.MOD_NAME + "ChargeBar",
                delegate
                {
                    chargeBarInterface.Draw(Main.spriteBatch, new GameTime());
                    return true;
                },
                InterfaceScaleType.UI));


        }
    }
}