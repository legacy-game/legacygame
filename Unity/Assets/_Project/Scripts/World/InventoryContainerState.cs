using System;
using System.Collections.Generic;

namespace Legacy.World
{
    [Serializable]
    public sealed class InventoryContainerState
    {
        private readonly List<InventoryStackState> _stacks = new();

        public WorldEntityId Id { get; }
        public WorldEntityId OwnerEntityId { get; }
        public InventoryContainerKind Kind { get; }
        public string DisplayName { get; }
        public IReadOnlyList<InventoryStackState> Stacks => _stacks;

        public InventoryContainerState(WorldEntityId id, WorldEntityId ownerEntityId, InventoryContainerKind kind, string displayName)
        {
            Id = id;
            OwnerEntityId = ownerEntityId;
            Kind = kind;
            DisplayName = displayName ?? string.Empty;
        }

        public bool TryAdd(string itemId, int count)
        {
            if (count <= 0 || !ItemCatalog.TryGet(itemId, out ItemDefinition item)) {
                return false;
            }

            InventoryStackState stack = GetOrCreate(itemId);
            if (item.StackLimit > 0 && stack.Count + count > item.StackLimit) {
                return false;
            }

            stack.Add(count);
            return true;
        }

        public bool TryRemove(string itemId, int count)
        {
            if (count <= 0) {
                return true;
            }

            InventoryStackState stack = Find(itemId);
            return stack != null && stack.TryRemove(count);
        }

        public int CountOf(string itemId)
        {
            InventoryStackState stack = Find(itemId);
            return stack == null ? 0 : stack.Count;
        }

        private InventoryStackState GetOrCreate(string itemId)
        {
            InventoryStackState stack = Find(itemId);
            if (stack != null) {
                return stack;
            }

            stack = new InventoryStackState(itemId, 0);
            _stacks.Add(stack);
            return stack;
        }

        private InventoryStackState Find(string itemId)
        {
            for (int i = 0; i < _stacks.Count; i++) {
                if (_stacks[i].ItemId == itemId) {
                    return _stacks[i];
                }
            }

            return null;
        }
    }
}
