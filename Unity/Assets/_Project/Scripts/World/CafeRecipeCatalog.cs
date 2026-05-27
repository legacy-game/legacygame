namespace Legacy.World
{
    public static class CafeRecipeCatalog
    {
        public const string HouseCoffee = "recipe_house_coffee";

        private static readonly CafeRecipeDefinition[] Definitions = {
            new CafeRecipeDefinition(
                HouseCoffee,
                "House coffee",
                ItemCatalog.PreparedCoffee,
                1,
                275,
                new RecipeIngredientDefinition(ItemCatalog.CoffeeBeans, 1),
                new RecipeIngredientDefinition(ItemCatalog.Milk, 1),
                new RecipeIngredientDefinition(ItemCatalog.PaperCup, 1))
        };

        public static bool TryGet(string id, out CafeRecipeDefinition definition)
        {
            for (int i = 0; i < Definitions.Length; i++) {
                if (Definitions[i].Id == id) {
                    definition = Definitions[i];
                    return true;
                }
            }

            definition = null;
            return false;
        }
    }
}
