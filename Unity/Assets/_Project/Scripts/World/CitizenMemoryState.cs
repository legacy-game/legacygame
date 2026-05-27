using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class CitizenMemoryState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId CitizenId { get; }
        public WorldEntityId SubjectCitizenId { get; }
        public string Kind { get; }
        public string Summary { get; }
        public GameDateTime CreatedAt { get; }
        public WorldEntityId SourceHistoryEventId { get; }
        public int Salience { get; }

        public CitizenMemoryState(
            WorldEntityId id,
            WorldEntityId citizenId,
            WorldEntityId subjectCitizenId,
            string kind,
            string summary,
            GameDateTime createdAt,
            WorldEntityId sourceHistoryEventId,
            int salience = 1)
        {
            if (string.IsNullOrWhiteSpace(kind)) {
                throw new ArgumentException("Memory kind must not be empty.", nameof(kind));
            }

            if (string.IsNullOrWhiteSpace(summary)) {
                throw new ArgumentException("Memory summary must not be empty.", nameof(summary));
            }

            Id = id;
            CitizenId = citizenId;
            SubjectCitizenId = subjectCitizenId;
            Kind = kind;
            Summary = summary;
            CreatedAt = createdAt;
            SourceHistoryEventId = sourceHistoryEventId;
            Salience = salience;
        }
    }
}
