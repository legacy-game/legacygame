using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class InventoryStackState
    {
        public string ItemId { get; }
        public int Count { get; private set; }

        public InventoryStackState(string itemId, int count)
        {
            ItemId = itemId ?? string.Empty;
            Count = count;
        }

        public void Add(int count)
        {
            Count += count;
        }

        public bool TryRemove(int count)
        {
            if (Count < count) {
                return false;
            }

            Count -= count;
            return true;
        }
    }
}
