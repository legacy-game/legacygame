using System;
using System.Collections.Generic;

namespace Legacy.World
{
    [Serializable]
    public sealed class CashDrawerState
    {
        private readonly List<CashDenominationStackState> _stacks = new();

        public WorldEntityId Id { get; }
        public WorldEntityId OwnerEntityId { get; }
        public CashContainerKind Kind { get; }
        public string DisplayName { get; }
        public IReadOnlyList<CashDenominationStackState> Stacks => _stacks;
        public int TotalCents {
            get {
                int total = 0;
                for (int i = 0; i < _stacks.Count; i++) {
                    total += _stacks[i].TotalCents;
                }

                return total;
            }
        }

        public CashDrawerState(WorldEntityId id, WorldEntityId ownerEntityId, CashContainerKind kind, string displayName)
        {
            Id = id;
            OwnerEntityId = ownerEntityId;
            Kind = kind;
            DisplayName = displayName ?? string.Empty;
        }

        public void Add(CashDenomination denomination, int count)
        {
            if (count <= 0) {
                return;
            }

            GetOrCreate(denomination).Add(count);
        }

        public bool TryRemove(CashDenomination denomination, int count)
        {
            if (count <= 0) {
                return true;
            }

            CashDenominationStackState stack = Find(denomination);
            return stack != null && stack.TryRemove(count);
        }

        public int CountOf(CashDenomination denomination)
        {
            CashDenominationStackState stack = Find(denomination);
            return stack == null ? 0 : stack.Count;
        }

        private CashDenominationStackState GetOrCreate(CashDenomination denomination)
        {
            CashDenominationStackState stack = Find(denomination);
            if (stack != null) {
                return stack;
            }

            stack = new CashDenominationStackState(denomination, 0);
            _stacks.Add(stack);
            return stack;
        }

        private CashDenominationStackState Find(CashDenomination denomination)
        {
            for (int i = 0; i < _stacks.Count; i++) {
                if (_stacks[i].Denomination == denomination) {
                    return _stacks[i];
                }
            }

            return null;
        }
    }
}
