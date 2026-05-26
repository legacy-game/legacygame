using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class JobTaskState
    {
        public WorldEntityId Id { get; }
        public string DefinitionId { get; }
        public WorldEntityId WorkplaceId { get; }
        public WorldEntityId AssignedWorkerId { get; private set; }
        public WorldEntityId ShiftId { get; private set; }
        public WorldEntityId TargetEntityId { get; }
        public JobTaskStatus Status { get; private set; }
        public int Quality { get; private set; }
        public GameDateTime CreatedAt { get; }
        public GameDateTime StartedAt { get; private set; }
        public GameDateTime CompletedAt { get; private set; }
        public MiniGameResultState MiniGameResult { get; private set; }

        public JobTaskState(
            WorldEntityId id,
            string definitionId,
            WorldEntityId workplaceId,
            WorldEntityId assignedWorkerId,
            WorldEntityId shiftId,
            WorldEntityId targetEntityId,
            JobTaskStatus status,
            int quality,
            GameDateTime createdAt,
            GameDateTime startedAt,
            GameDateTime completedAt,
            MiniGameResultState miniGameResult = null)
        {
            Id = id;
            DefinitionId = definitionId;
            WorkplaceId = workplaceId;
            AssignedWorkerId = assignedWorkerId;
            ShiftId = shiftId;
            TargetEntityId = targetEntityId;
            Status = status;
            Quality = quality;
            CreatedAt = createdAt;
            StartedAt = startedAt;
            CompletedAt = completedAt;
            MiniGameResult = miniGameResult;
        }

        public void Start(WorldEntityId workerId, WorldEntityId shiftId, GameDateTime startedAt)
        {
            AssignedWorkerId = workerId;
            ShiftId = shiftId;
            StartedAt = startedAt;
            Status = JobTaskStatus.Active;
        }

        public void SubmitResult(MiniGameResultState result)
        {
            MiniGameResult = result;
            Quality = result.Quality;
            Status = JobTaskStatus.ResultSubmitted;
        }

        public void Complete(GameDateTime completedAt)
        {
            CompletedAt = completedAt;
            Status = JobTaskStatus.Completed;
        }

        public void Cancel()
        {
            Status = JobTaskStatus.Cancelled;
        }
    }
}
