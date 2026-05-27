using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class CivicRegistryEntryState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId CitizenId { get; }
        public CivicRegistryEntryKind Kind { get; }
        public string Summary { get; }
        public GameDateTime CreatedAt { get; }
        public WorldEntityId SourceReportId { get; }
        public WorldEntityId FiledByCitizenId { get; }
        public WorldEntityId RelatedPlaceId { get; }

        public CivicRegistryEntryState(
            WorldEntityId id,
            WorldEntityId citizenId,
            CivicRegistryEntryKind kind,
            string summary,
            GameDateTime createdAt,
            WorldEntityId sourceReportId,
            WorldEntityId filedByCitizenId,
            WorldEntityId relatedPlaceId)
        {
            Id = id;
            CitizenId = citizenId;
            Kind = kind;
            Summary = summary ?? string.Empty;
            CreatedAt = createdAt;
            SourceReportId = sourceReportId;
            FiledByCitizenId = filedByCitizenId;
            RelatedPlaceId = relatedPlaceId;
        }
    }
}
