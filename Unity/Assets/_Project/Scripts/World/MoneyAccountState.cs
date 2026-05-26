using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class MoneyAccountState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId OwnerEntityId { get; }
        public string DisplayName { get; private set; }
        public int BalanceCents { get; private set; }

        public MoneyAccountState(WorldEntityId id, WorldEntityId ownerEntityId, string displayName, int balanceCents)
        {
            if (string.IsNullOrWhiteSpace(displayName)) {
                throw new ArgumentException("Display name must not be empty.", nameof(displayName));
            }

            Id = id;
            OwnerEntityId = ownerEntityId;
            DisplayName = displayName;
            BalanceCents = balanceCents;
        }

        public void Credit(int cents)
        {
            if (cents < 0) {
                throw new ArgumentOutOfRangeException(nameof(cents), "Credit amount must be non-negative.");
            }

            BalanceCents += cents;
        }

        public void Debit(int cents)
        {
            if (cents < 0) {
                throw new ArgumentOutOfRangeException(nameof(cents), "Debit amount must be non-negative.");
            }

            BalanceCents -= cents;
        }
    }
}
