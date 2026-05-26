using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class TransactionState
    {
        public WorldEntityId Id { get; }
        public GameDateTime Timestamp { get; }
        public WorldEntityId PayerAccountId { get; }
        public WorldEntityId PayeeAccountId { get; }
        public int AmountCents { get; }
        public string Reason { get; }
        public WorldEntityId RelatedPlaceId { get; }
        public WorldActionKind ActionKind { get; }

        public TransactionState(
            WorldEntityId id,
            GameDateTime timestamp,
            WorldEntityId payerAccountId,
            WorldEntityId payeeAccountId,
            int amountCents,
            string reason,
            WorldEntityId relatedPlaceId,
            WorldActionKind actionKind)
        {
            if (amountCents <= 0) {
                throw new ArgumentOutOfRangeException(nameof(amountCents), "Transaction amount must be positive.");
            }

            if (string.IsNullOrWhiteSpace(reason)) {
                throw new ArgumentException("Reason must not be empty.", nameof(reason));
            }

            Id = id;
            Timestamp = timestamp;
            PayerAccountId = payerAccountId;
            PayeeAccountId = payeeAccountId;
            AmountCents = amountCents;
            Reason = reason;
            RelatedPlaceId = relatedPlaceId;
            ActionKind = actionKind;
        }
    }
}
