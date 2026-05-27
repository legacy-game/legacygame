using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class ItemDefinition
    {
        public string Id { get; }
        public string DisplayName { get; }
        public ItemKind Kind { get; }
        public int BaseValueCents { get; }
        public int StackLimit { get; }

        public ItemDefinition(string id, string displayName, ItemKind kind, int baseValueCents, int stackLimit)
        {
            Id = id ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            Kind = kind;
            BaseValueCents = baseValueCents;
            StackLimit = stackLimit;
        }
    }
}
