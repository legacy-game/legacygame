using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class EndMorningCommand : IWorldCommand
    {
        private readonly WorldEntityId _actorId;

        public EndMorningCommand(WorldEntityId actorId)
        {
            _actorId = actorId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (context.State.Morning == null) {
                return WorldCommandResult.Failure("Morning state not found.");
            }

            if (context.State.Morning.Status == MorningStatus.Complete) {
                return WorldCommandResult.Failure("Morning is already complete.");
            }

            foreach (ShiftState shift in context.State.ShiftsById.Values) {
                if (shift.Status == ShiftStatus.Active || shift.Status == ShiftStatus.Paused) {
                    return WorldCommandResult.Failure("End active shifts before ending the morning.");
                }
            }

            int visitorsLeft = 0;
            foreach (VisitState visit in context.State.VisitsById.Values) {
                if (visit.Status == VisitStatus.Scheduled ||
                    visit.Status == VisitStatus.Arrived ||
                    visit.Status == VisitStatus.WaitingForTask) {
                    visit.MarkLeft(context.State.CurrentTime);
                    visitorsLeft++;
                }
            }

            int tasksCancelled = 0;
            foreach (JobTaskState task in context.State.JobTasksById.Values) {
                if (task.Status != JobTaskStatus.Queued) {
                    continue;
                }

                task.Cancel();
                if (context.State.TryGetWorkplace(task.WorkplaceId, out WorkplaceState workplace)) {
                    workplace.RemoveTask(task.Id);
                }

                tasksCancelled++;
            }

            context.State.Morning.Complete(context.State.CurrentTime);
            string cleanup = visitorsLeft > 0 || tasksCancelled > 0
                ? $" {visitorsLeft} visitors left; {tasksCancelled} queued tasks cleared."
                : string.Empty;
            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.MorningCompleted,
                $"Morning ended with {context.State.Morning.TasksCompleted} tasks completed and {EconomySystem.FormatMoney(context.State.Morning.MoneyEarnedCents)} earned.{cleanup}",
                new[] { _actorId },
                System.Array.Empty<WorldEntityId>());

            return WorldCommandResult.Success($"Morning complete: {context.State.Morning.TasksCompleted} tasks, {EconomySystem.FormatMoney(context.State.Morning.MoneyEarnedCents)} earned.{cleanup}")
                .WithChangedEntity(_actorId)
                .WithHistoryEvent(history);
        }
    }
}
