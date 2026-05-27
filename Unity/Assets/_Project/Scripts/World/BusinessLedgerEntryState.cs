using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class BusinessLedgerEntryState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId WorkplaceId { get; }
        public WorldEntityId AccountId { get; }
        public BusinessLedgerEntryKind Kind { get; }
        public int AmountCents { get; }
        public string Reason { get; }
        public GameDateTime Timestamp { get; }
        public WorldEntityId RelatedEntityId { get; }

        public BusinessLedgerEntryState(
            WorldEntityId id,
            WorldEntityId workplaceId,
            WorldEntityId accountId,
            BusinessLedgerEntryKind kind,
            int amountCents,
            string reason,
            GameDateTime timestamp,
            WorldEntityId relatedEntityId)
        {
            if (amountCents <= 0) {
                throw new ArgumentOutOfRangeException(nameof(amountCents), "Ledger amount must be positive.");
            }

            if (string.IsNullOrWhiteSpace(reason)) {
                throw new ArgumentException("Reason must not be empty.", nameof(reason));
            }

            Id = id;
            WorkplaceId = workplaceId;
            AccountId = accountId;
            Kind = kind;
            AmountCents = amountCents;
            Reason = reason;
            Timestamp = timestamp;
            RelatedEntityId = relatedEntityId;
        }
    }
}
