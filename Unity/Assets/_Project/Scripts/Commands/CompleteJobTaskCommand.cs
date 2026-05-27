using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class CompleteJobTaskCommand : IWorldCommand
    {
        private readonly WorldEntityId _taskId;
        private readonly WorldEntityId _workerCitizenId;

        public CompleteJobTaskCommand(WorldEntityId taskId, WorldEntityId workerCitizenId)
        {
            _taskId = taskId;
            _workerCitizenId = workerCitizenId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetJobTask(_taskId, out JobTaskState task)) {
                return WorldCommandResult.Failure($"Task not found: {_taskId}");
            }

            if (task.Status == JobTaskStatus.Completed) {
                return WorldCommandResult.Failure("Task is already complete.");
            }

            if (task.AssignedWorkerId != _workerCitizenId || task.Status != JobTaskStatus.ResultSubmitted) {
                return WorldCommandResult.Failure("Task needs a submitted mini-game result from this worker.");
            }

            if (!context.State.TryGetShift(task.ShiftId, out ShiftState shift) || shift.Status != ShiftStatus.Active) {
                return WorldCommandResult.Failure("Active shift not found.");
            }

            if (!context.State.TryGetWorkplace(task.WorkplaceId, out WorkplaceState workplace)) {
                return WorldCommandResult.Failure("Workplace not found.");
            }

            if (!JobTaskCatalog.TryGet(task.DefinitionId, out JobTaskDefinition definition)) {
                return WorldCommandResult.Failure($"Task definition not found: {task.DefinitionId}");
            }

            if (!context.State.TryGetWorkplaceInventory(workplace.Id, out WorkplaceInventoryState inventory)) {
                return WorldCommandResult.Failure("Workplace inventory not found.");
            }

            if (!context.State.TryGetMoneyAccount(workplace.BusinessAccountId, out MoneyAccountState businessAccount)) {
                return WorldCommandResult.Failure("Business account not found.");
            }

            int earnedCents = definition.BasePayCents;
            if (businessAccount.BalanceCents < earnedCents) {
                return WorldCommandResult.Failure("Business cannot pay this task.");
            }

            if (!context.State.TryGetMoneyAccountForOwner(_workerCitizenId, out MoneyAccountState workerAccount)) {
                return WorldCommandResult.Failure("Worker account not found.");
            }

            if (!string.IsNullOrEmpty(definition.InputItemId) && inventory.CountOf(definition.InputItemId) < definition.InputItemCount) {
                return WorldCommandResult.Failure($"Missing inventory: needs {definition.InputItemCount} {definition.InputItemId}.");
            }

            var economy = new EconomySystem(context.State);
            EconomyTransferResult wage = economy.Transfer(workplace.BusinessAccountId, workerAccount.Id, earnedCents, definition.DisplayName, workplace.PlaceId, definition.ActionKind, context.State.CurrentTime);
            if (!wage.Succeeded) {
                return WorldCommandResult.Failure(wage.Message);
            }

            if (!string.IsNullOrEmpty(definition.InputItemId)) {
                inventory.TryRemove(definition.InputItemId, definition.InputItemCount);
            }

            if (!string.IsNullOrEmpty(definition.OutputItemId)) {
                inventory.Add(definition.OutputItemId, definition.OutputItemCount);
            }

            shift.CompleteTask(task.Id, earnedCents);
            task.Complete(context.State.CurrentTime);
            workplace.RemoveTask(task.Id);
            context.State.Morning?.AddCompletedTask(earnedCents);

            VisitState visit = null;
            if (context.State.TryGetVisitForTask(task.Id, out VisitState linkedVisit)) {
                linkedVisit.MarkServed(context.State.CurrentTime);
                visit = linkedVisit;
            }

            if (context.State.TryGetEmploymentContract(shift.ContractId, out EmploymentContractState contract) &&
                JobCatalog.TryGet(contract.JobDefinitionId, out JobDefinition job)) {
                context.State.GetOrCreateSkill(_workerCitizenId, job.PrimarySkill).AddExperience(task.Quality);
            }

            HistoryEvent payment = context.History.Create(context.State.CurrentTime, HistoryEventKind.PaymentRecorded, $"{EconomySystem.FormatMoney(earnedCents)} paid for {definition.DisplayName}.", new[] { _workerCitizenId, workplace.OwnerCitizenId }, new[] { workplace.PlaceId });
            HistoryEvent completed = context.History.Create(context.State.CurrentTime, HistoryEventKind.JobTaskCompleted, $"{definition.DisplayName} completed with quality {task.Quality}.", new[] { _workerCitizenId }, new[] { workplace.PlaceId });
            HistoryEvent skill = context.History.Create(context.State.CurrentTime, HistoryEventKind.SkillImproved, "A worker gained job experience.", new[] { _workerCitizenId }, new[] { workplace.PlaceId });
            HistoryEvent visitCompleted = visit == null
                ? null
                : context.History.Create(context.State.CurrentTime, HistoryEventKind.VisitCompleted, $"{visit.CompletionLine}", new[] { visit.VisitorCitizenId, _workerCitizenId }, new[] { workplace.PlaceId });

            WorldCommandResult result = WorldCommandResult.Success(visit == null
                    ? $"{definition.DisplayName} complete. Money, inventory, skill, and history updated."
                    : $"{definition.DisplayName} complete. {visit.CompletionLine}")
                .WithChangedEntity(task.Id)
                .WithChangedEntity(shift.Id)
                .WithChangedEntity(workplace.Id)
                .WithChangedEntity(workerAccount.Id)
                .WithHistoryEvent(payment)
                .WithHistoryEvent(completed)
                .WithHistoryEvent(skill);
            if (visitCompleted != null) {
                result.WithChangedEntity(visit.Id).WithHistoryEvent(visitCompleted);
            }

            return result;
        }
    }
}
