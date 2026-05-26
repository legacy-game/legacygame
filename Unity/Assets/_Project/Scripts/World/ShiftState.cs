using System;
using System.Collections.Generic;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class ShiftState
    {
        private readonly List<WorldEntityId> _completedTaskIds = new();

        public WorldEntityId Id { get; }
        public WorldEntityId ContractId { get; }
        public WorldEntityId WorkerCitizenId { get; }
        public WorldEntityId WorkplaceId { get; }
        public GameDateTime StartedAt { get; }
        public GameDateTime ExpectedEndAt { get; }
        public GameDateTime EndedAt { get; private set; }
        public ShiftStatus Status { get; private set; }
        public int EarnedCents { get; private set; }
        public IReadOnlyList<WorldEntityId> CompletedTaskIds => _completedTaskIds;

        public ShiftState(
            WorldEntityId id,
            WorldEntityId contractId,
            WorldEntityId workerCitizenId,
            WorldEntityId workplaceId,
            GameDateTime startedAt,
            GameDateTime expectedEndAt,
            GameDateTime endedAt,
            ShiftStatus status,
            int earnedCents)
        {
            Id = id;
            ContractId = contractId;
            WorkerCitizenId = workerCitizenId;
            WorkplaceId = workplaceId;
            StartedAt = startedAt;
            ExpectedEndAt = expectedEndAt;
            EndedAt = endedAt;
            Status = status;
            EarnedCents = earnedCents;
        }

        public void AddEarnings(int cents)
        {
            EarnedCents += cents;
        }

        public void CompleteTask(WorldEntityId taskId, int earnedCents)
        {
            if (!_completedTaskIds.Contains(taskId)) {
                _completedTaskIds.Add(taskId);
            }

            AddEarnings(earnedCents);
        }

        public void Pause()
        {
            Status = ShiftStatus.Paused;
        }

        public void End(GameDateTime endedAt)
        {
            Status = ShiftStatus.Ended;
            EndedAt = endedAt;
        }
    }
}
