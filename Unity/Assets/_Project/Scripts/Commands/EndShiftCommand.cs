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

            shift.End(context.State.CurrentTime);
            if (context.State.TryGetWorkplace(shift.WorkplaceId, out WorkplaceState workplace)) {
                workplace.RemoveActiveShift(shift.Id);
            }

            HistoryEvent history = context.History.Create(context.State.CurrentTime, HistoryEventKind.ShiftEnded, $"A work shift ended with {EconomySystem.FormatMoney(shift.EarnedCents)} earned.", new[] { shift.WorkerCitizenId }, new[] { shift.WorkplaceId });
            return WorldCommandResult.Success("Shift ended.").WithChangedEntity(shift.Id).WithHistoryEvent(history);
        }
    }
}
