using System.Collections.Generic;

using Microsoft.Xna.Framework;

using OmoriMod.Content.Systems.AbilitySystem.AbilityMenuUI;
using OmoriMod.Content.Systems.ChargeBar;

using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace OmoriMod.Content.Systems;

public class OmoriModUiSystem : ModSystem
{

    private UserInterface _chargeBarInterface;
    private ChargeBarUI _chargeBar;

    private UserInterface _abilityMenuInterface;
    private AbilityMenu _abilityMenu;

    public override void Load()
    {
        // A server doesn't need UI lol
        if (!Main.dedServ)
        {
            _chargeBar = new ChargeBarUI();
            _chargeBarInterface = new UserInterface();
            _chargeBarInterface.SetState(_chargeBar);

            _abilityMenu = new AbilityMenu();
            _abilityMenuInterface = new UserInterface();
            _abilityMenuInterface.SetState(_abilityMenu);
        }
    }

    public override void UpdateUI(GameTime gameTime)
    {
        _chargeBarInterface?.Update(gameTime);
        _abilityMenuInterface?.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        // choose which layer to put our UI. This layer works for us
        int generalUiIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Interface Logic 2"));
        if (generalUiIndex == -1) { return; }

        layers.Insert(generalUiIndex, new LegacyGameInterfaceLayer(
            OmoriMod.MOD_NAME + "AbilityMenu",
            delegate
            {
                _abilityMenuInterface.Draw(Main.spriteBatch, new GameTime());
                return true;
            },
            InterfaceScaleType.UI));

        layers.Insert(generalUiIndex, new LegacyGameInterfaceLayer(
            OmoriMod.MOD_NAME + "ChargeBar",
            delegate
            {
                _chargeBarInterface.Draw(Main.spriteBatch, new GameTime());
                return true;
            },
            InterfaceScaleType.UI));
    }
}