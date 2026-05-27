using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class ShiftSummaryState
    {
        public WorldEntityId ShiftId { get; }
        public WorldEntityId WorkerCitizenId { get; }
        public WorldEntityId WorkplaceId { get; }
        public GameDateTime StartedAt { get; }
        public GameDateTime EndedAt { get; }
        public int TasksCompleted { get; }
        public int EarnedCents { get; }
        public int HistoryEventsAtEnd { get; }

        public ShiftSummaryState(
            WorldEntityId shiftId,
            WorldEntityId workerCitizenId,
            WorldEntityId workplaceId,
            GameDateTime startedAt,
            GameDateTime endedAt,
            int tasksCompleted,
            int earnedCents,
            int historyEventsAtEnd)
        {
            ShiftId = shiftId;
            WorkerCitizenId = workerCitizenId;
            WorkplaceId = workplaceId;
            StartedAt = startedAt;
            EndedAt = endedAt;
            TasksCompleted = tasksCompleted;
            EarnedCents = earnedCents;
            HistoryEventsAtEnd = historyEventsAtEnd;
        }
    }
}
