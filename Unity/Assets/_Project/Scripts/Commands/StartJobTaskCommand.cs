using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class StartJobTaskCommand : IWorldCommand
    {
        private readonly WorldEntityId _taskId;
        private readonly WorldEntityId _shiftId;
        private readonly WorldEntityId _workerCitizenId;

        public StartJobTaskCommand(WorldEntityId taskId, WorldEntityId shiftId, WorldEntityId workerCitizenId)
        {
            _taskId = taskId;
            _shiftId = shiftId;
            _workerCitizenId = workerCitizenId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetJobTask(_taskId, out JobTaskState task)) {
                return WorldCommandResult.Failure($"Task not found: {_taskId}");
            }

            if (task.Status != JobTaskStatus.Queued) {
                return WorldCommandResult.Failure($"Task is {task.Status}; only queued tasks can be started.");
            }

            if (!context.State.TryGetShift(_shiftId, out ShiftState shift) || shift.Status != ShiftStatus.Active) {
                return WorldCommandResult.Failure("Active shift not found.");
            }

            if (shift.WorkerCitizenId != _workerCitizenId || shift.WorkplaceId != task.WorkplaceId) {
                return WorldCommandResult.Failure("Shift does not match this task.");
            }

            if (!JobTaskCatalog.TryGet(task.DefinitionId, out JobTaskDefinition definition)) {
                return WorldCommandResult.Failure($"Task definition not found: {task.DefinitionId}");
            }

            if (!context.State.TryGetEmploymentContract(shift.ContractId, out EmploymentContractState contract) ||
                contract.Status != EmploymentContractStatus.Active ||
                contract.RoleId != definition.RequiredRoleId) {
                return WorldCommandResult.Failure("Worker is not contracted for this task.");
            }

            task.Start(_workerCitizenId, shift.Id, context.State.CurrentTime);
            if (context.State.TryGetWorkplace(task.WorkplaceId, out WorkplaceState workplace)) {
                workplace.RemoveTask(task.Id);
            }

            HistoryEvent history = context.History.Create(context.State.CurrentTime, HistoryEventKind.JobTaskStarted, $"Job task started: {definition.DisplayName}.", new[] { _workerCitizenId }, new[] { task.WorkplaceId });
            return WorldCommandResult.Success($"Started task: {definition.DisplayName}.").WithChangedEntity(task.Id).WithChangedEntity(task.WorkplaceId).WithHistoryEvent(history);
        }
    }
}
