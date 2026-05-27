namespace Legacy.World
{
    public static class CafeRecipeSystem
    {
        public static bool CanPrepare(InventoryContainerState inventory, CafeRecipeDefinition recipe, out string reason)
        {
            if (inventory == null) {
                reason = "Inventory container not found.";
                return false;
            }

            for (int i = 0; i < recipe.Ingredients.Count; i++) {
                RecipeIngredientDefinition ingredient = recipe.Ingredients[i];
                if (inventory.CountOf(ingredient.ItemId) < ingredient.Count) {
                    reason = $"Missing inventory: needs {ingredient.Count} {ingredient.ItemId}.";
                    return false;
                }
            }

            reason = string.Empty;
            return true;
        }

        public static bool TryPrepare(InventoryContainerState inventory, CafeRecipeDefinition recipe, out string reason)
        {
            if (!CanPrepare(inventory, recipe, out reason)) {
                return false;
            }

            for (int i = 0; i < recipe.Ingredients.Count; i++) {
                RecipeIngredientDefinition ingredient = recipe.Ingredients[i];
                inventory.TryRemove(ingredient.ItemId, ingredient.Count);
            }

            if (!string.IsNullOrEmpty(recipe.OutputItemId) && recipe.OutputCount > 0) {
                inventory.TryAdd(recipe.OutputItemId, recipe.OutputCount);
            }

            reason = string.Empty;
            return true;
        }
    }
}
