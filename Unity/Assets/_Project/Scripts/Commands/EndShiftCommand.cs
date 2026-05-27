using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class EndShiftCommand : IWorldCommand
    {
        private readonly WorldEntityId _shiftId;
        private readonly WorldEntityId _workerCitizenId;

        public EndShiftCommand(WorldEntityId shiftId, WorldEntityId workerCitizenId)
        {
            _shiftId = shiftId;
            _workerCitizenId = workerCitizenId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetShift(_shiftId, out ShiftState shift)) {
                return WorldCommandResult.Failure($"Shift not found: {_shiftId}");
            }

            if (shift.WorkerCitizenId != _workerCitizenId) {
                return WorldCommandResult.Failure("Only the worker can end this shift.");
            }

            if (shift.Status == ShiftStatus.Ended) {
                return WorldCommandResult.Failure("Shift is already ended.");
            }

            if (shift.Status != ShiftStatus.Active && shift.Status != ShiftStatus.Paused) {
                return WorldCommandResult.Failure("Shift is not active.");
            }

            foreach (JobTaskState task in context.State.JobTasksById.Values) {
                if (task.ShiftId == shift.Id &&
                    task.Status != JobTaskStatus.Completed &&
                    task.Status != JobTaskStatus.Cancelled) {
                    return WorldCommandResult.Failure("Finish or cancel the active task before ending the shift.");
                }
            }

            shift.End(context.State.CurrentTime);
            if (context.State.TryGetWorkplace(shift.WorkplaceId, out WorkplaceState workplace)) {
                workplace.RemoveActiveShift(shift.Id);
            }

            var summary = new ShiftSummaryState(
                shift.Id,
                shift.WorkerCitizenId,
                shift.WorkplaceId,
                shift.StartedAt,
                context.State.CurrentTime,
                shift.CompletedTaskIds.Count,
                shift.EarnedCents,
                context.State.RecentHistory.Count);
            context.State.AddShiftSummary(summary);

            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.ShiftEnded,
                $"A work shift ended with {shift.CompletedTaskIds.Count} tasks complete and {EconomySystem.FormatMoney(shift.EarnedCents)} earned.",
                new[] { shift.WorkerCitizenId },
                new[] { shift.WorkplaceId });
            return WorldCommandResult.Success($"Shift ended: {shift.CompletedTaskIds.Count} tasks, {EconomySystem.FormatMoney(shift.EarnedCents)} earned.")
                .WithChangedEntity(shift.Id)
                .WithHistoryEvent(history);
        }
    }
}
