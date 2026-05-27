using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class TenantRecordState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId LeaseContractId { get; }
        public WorldEntityId BuildingId { get; }
        public WorldEntityId TenantCitizenId { get; }
        public TenantRecordStatus Status { get; private set; }
        public GameDateTime StartedAt { get; }
        public GameDateTime EndedAt { get; private set; }

        public TenantRecordState(
            WorldEntityId id,
            WorldEntityId leaseContractId,
            WorldEntityId buildingId,
            WorldEntityId tenantCitizenId,
            TenantRecordStatus status,
            GameDateTime startedAt,
            GameDateTime endedAt)
        {
            Id = id;
            LeaseContractId = leaseContractId;
            BuildingId = buildingId;
            TenantCitizenId = tenantCitizenId;
            Status = status;
            StartedAt = startedAt;
            EndedAt = endedAt;
        }

        public TenantRecordState(
            WorldEntityId id,
            WorldEntityId leaseContractId,
            WorldEntityId buildingId,
            WorldEntityId tenantCitizenId,
            GameDateTime startedAt)
            : this(id, leaseContractId, buildingId, tenantCitizenId, TenantRecordStatus.Active, startedAt, startedAt)
        {
        }

        public void End(GameDateTime endedAt)
        {
            Status = TenantRecordStatus.Ended;
            EndedAt = endedAt;
        }
    }
}
