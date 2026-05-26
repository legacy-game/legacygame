using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class CreateJobTaskCommand : IWorldCommand
    {
        private readonly WorldEntityId _taskId;
        private readonly string _definitionId;
        private readonly WorldEntityId _workplaceId;
        private readonly WorldEntityId _targetEntityId;

        public CreateJobTaskCommand(WorldEntityId taskId, string definitionId, WorldEntityId workplaceId, WorldEntityId targetEntityId = default)
        {
            _taskId = taskId;
            _definitionId = definitionId;
            _workplaceId = workplaceId;
            _targetEntityId = targetEntityId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetWorkplace(_workplaceId, out WorkplaceState workplace)) {
                return WorldCommandResult.Failure($"Workplace not found: {_workplaceId}");
            }

            if (!JobTaskCatalog.TryGet(_definitionId, out JobTaskDefinition definition)) {
                return WorldCommandResult.Failure($"Task definition not found: {_definitionId}");
            }

            var task = new JobTaskState(_taskId, definition.Id, workplace.Id, default, default, _targetEntityId, JobTaskStatus.Queued, 0, context.State.CurrentTime, context.State.CurrentTime, context.State.CurrentTime);
            context.State.AddJobTask(task);

            HistoryEvent history = context.History.Create(context.State.CurrentTime, HistoryEventKind.JobTaskCreated, $"Job task created: {definition.DisplayName}.", System.Array.Empty<WorldEntityId>(), new[] { workplace.PlaceId });
            return WorldCommandResult.Success($"Created task: {definition.DisplayName}.").WithChangedEntity(task.Id).WithHistoryEvent(history);
        }
    }
}
