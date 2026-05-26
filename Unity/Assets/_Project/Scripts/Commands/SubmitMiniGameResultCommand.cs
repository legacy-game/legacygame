using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class SubmitMiniGameResultCommand : IWorldCommand
    {
        private readonly WorldEntityId _taskId;
        private readonly WorldEntityId _workerCitizenId;
        private readonly int _score;
        private readonly int _maxScore;
        private readonly int _durationSeconds;
        private readonly int _mistakes;

        public SubmitMiniGameResultCommand(WorldEntityId taskId, WorldEntityId workerCitizenId, int score, int maxScore, int durationSeconds, int mistakes)
        {
            _taskId = taskId;
            _workerCitizenId = workerCitizenId;
            _score = score;
            _maxScore = maxScore;
            _durationSeconds = durationSeconds;
            _mistakes = mistakes;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetJobTask(_taskId, out JobTaskState task)) {
                return WorldCommandResult.Failure($"Task not found: {_taskId}");
            }

            if (task.AssignedWorkerId != _workerCitizenId || task.Status != JobTaskStatus.Active) {
                return WorldCommandResult.Failure("Task is not active for this worker.");
            }

            int maxScore = _maxScore <= 0 ? 1 : _maxScore;
            int rawQuality = (_score * 100) / maxScore;
            int quality = rawQuality < 0 ? 0 : rawQuality > 100 ? 100 : rawQuality;
            task.SubmitResult(new MiniGameResultState(_score, maxScore, quality, _durationSeconds, _mistakes));

            HistoryEvent history = context.History.Create(context.State.CurrentTime, HistoryEventKind.MiniGameResultSubmitted, $"Mini-game result submitted with quality {quality}.", new[] { _workerCitizenId }, new[] { task.WorkplaceId });
            return WorldCommandResult.Success("Mini-game result submitted.").WithChangedEntity(task.Id).WithHistoryEvent(history);
        }
    }
}
