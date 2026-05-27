using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class RecipeIngredientDefinition
    {
        public string ItemId { get; }
        public int Count { get; }

        public RecipeIngredientDefinition(string itemId, int count)
        {
            ItemId = itemId ?? string.Empty;
            Count = count;
        }
    }
}
