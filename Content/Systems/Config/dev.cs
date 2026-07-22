using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OmoriMod.Content.Systems.Config;

public class Dev : ModSystem
{
    public override void AddRecipes()
    {
        int maxItems = ItemLoader.ItemCount;

        for (int i = 0; i < maxItems; i++)
        {
            var item = ItemLoader.GetItem(i);
            if (item != null && item.Mod is OmoriMod)
            {
                Recipe recipe = Recipe.Create(i, 1);

                // Add a dynamic condition using a lambda
                LocalizedText conditionText = Language.GetText(OmoriMod.MOD_NAME + "RecipeCondition:DevMode");
                recipe.AddCondition(
                    conditionText,
                    () => ModContent.GetInstance<OmoriModConfig>().EnableDevMode
                );

                recipe.Register();
            }
        }
    }
}