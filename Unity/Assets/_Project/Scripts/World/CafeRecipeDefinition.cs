using System;
using System.Collections.Generic;

namespace Legacy.World
{
    [Serializable]
    public sealed class CafeRecipeDefinition
    {
        private readonly RecipeIngredientDefinition[] _ingredients;

        public string Id { get; }
        public string DisplayName { get; }
        public string OutputItemId { get; }
        public int OutputCount { get; }
        public int PriceCents { get; }
        public IReadOnlyList<RecipeIngredientDefinition> Ingredients => _ingredients;

        public CafeRecipeDefinition(
            string id,
            string displayName,
            string outputItemId,
            int outputCount,
            int priceCents,
            params RecipeIngredientDefinition[] ingredients)
        {
            Id = id ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            OutputItemId = outputItemId ?? string.Empty;
            OutputCount = outputCount;
            PriceCents = priceCents;
            _ingredients = ingredients ?? Array.Empty<RecipeIngredientDefinition>();
        }
    }
}
