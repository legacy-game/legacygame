using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class LeaseContractState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId BuildingId { get; }
        public WorldEntityId LandlordCitizenId { get; }
        public WorldEntityId TenantCitizenId { get; }
        public int RentCents { get; }
        public int DueDayOfMonth { get; }
        public LeaseContractStatus Status { get; private set; }
        public GameDateTime StartedAt { get; }
        public GameDateTime LastPaidAt { get; private set; }
        public int PaymentsMade { get; private set; }

        public LeaseContractState(
            WorldEntityId id,
            WorldEntityId buildingId,
            WorldEntityId landlordCitizenId,
            WorldEntityId tenantCitizenId,
            int rentCents,
            int dueDayOfMonth,
            LeaseContractStatus status,
            GameDateTime startedAt,
            GameDateTime lastPaidAt,
            int paymentsMade)
        {
            if (rentCents <= 0) {
                throw new ArgumentOutOfRangeException(nameof(rentCents), "Rent must be positive.");
            }

            if (dueDayOfMonth < 1 || dueDayOfMonth > 31) {
                throw new ArgumentOutOfRangeException(nameof(dueDayOfMonth), "Due day must be 1-31.");
            }

            Id = id;
            BuildingId = buildingId;
            LandlordCitizenId = landlordCitizenId;
            TenantCitizenId = tenantCitizenId;
            RentCents = rentCents;
            DueDayOfMonth = dueDayOfMonth;
            Status = status;
            StartedAt = startedAt;
            LastPaidAt = lastPaidAt;
            PaymentsMade = paymentsMade;
        }

        public LeaseContractState(
            WorldEntityId id,
            WorldEntityId buildingId,
            WorldEntityId landlordCitizenId,
            WorldEntityId tenantCitizenId,
            int rentCents,
            int dueDayOfMonth,
            GameDateTime startedAt)
            : this(id, buildingId, landlordCitizenId, tenantCitizenId, rentCents, dueDayOfMonth, LeaseContractStatus.Active, startedAt, startedAt, 0)
        {
        }

        public void RecordPayment(GameDateTime paidAt)
        {
            LastPaidAt = paidAt;
            PaymentsMade++;
        }

        public void End(GameDateTime endedAt)
        {
            Status = LeaseContractStatus.Ended;
            LastPaidAt = endedAt;
        }
    }
}
