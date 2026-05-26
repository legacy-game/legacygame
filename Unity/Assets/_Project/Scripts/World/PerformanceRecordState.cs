using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class PerformanceRecordState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId CitizenId { get; }
        public WorldEntityId WorkplaceId { get; }
        public string Note { get; }
        public int Score { get; }
        public GameDateTime CreatedAt { get; }

        public PerformanceRecordState(WorldEntityId id, WorldEntityId citizenId, WorldEntityId workplaceId, string note, int score, GameDateTime createdAt)
        {
            Id = id;
            CitizenId = citizenId;
            WorkplaceId = workplaceId;
            Note = note ?? string.Empty;
            Score = score;
            CreatedAt = createdAt;
        }
    }
}
