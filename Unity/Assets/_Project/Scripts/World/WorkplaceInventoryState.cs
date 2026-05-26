using System;
using System.Collections.Generic;

namespace Legacy.World
{
    [Serializable]
    public sealed class WorkplaceInventoryState
    {
        private readonly List<InventoryStackState> _stacks = new();

        public WorldEntityId WorkplaceId { get; }
        public IReadOnlyList<InventoryStackState> Stacks => _stacks;

        public WorkplaceInventoryState(WorldEntityId workplaceId)
        {
            WorkplaceId = workplaceId;
        }

        public void Add(string itemId, int count)
        {
            if (count <= 0) {
                return;
            }

            InventoryStackState stack = GetOrCreate(itemId);
            stack.Add(count);
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
