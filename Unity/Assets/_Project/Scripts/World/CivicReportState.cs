using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class CivicReportState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId ReporterCitizenId { get; }
        public WorldEntityId SubjectCitizenId { get; }
        public WorldEntityId RelatedPlaceId { get; }
        public string Summary { get; }
        public GameDateTime CreatedAt { get; }
        public CivicReportStatus Status { get; }

        public CivicReportState(
            WorldEntityId id,
            WorldEntityId reporterCitizenId,
            WorldEntityId subjectCitizenId,
            WorldEntityId relatedPlaceId,
            string summary,
            GameDateTime createdAt,
            CivicReportStatus status = CivicReportStatus.Filed)
        {
            Id = id;
            ReporterCitizenId = reporterCitizenId;
            SubjectCitizenId = subjectCitizenId;
            RelatedPlaceId = relatedPlaceId;
            Summary = summary ?? string.Empty;
            CreatedAt = createdAt;
            Status = status;
        }
    }
}
