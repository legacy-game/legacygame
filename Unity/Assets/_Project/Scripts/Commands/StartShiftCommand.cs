using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class StartShiftCommand : IWorldCommand
    {
        private readonly WorldEntityId _shiftId;
        private readonly WorldEntityId _contractId;
        private readonly int _expectedDurationMinutes;

        public StartShiftCommand(WorldEntityId shiftId, WorldEntityId contractId, int expectedDurationMinutes)
        {
            _shiftId = shiftId;
            _contractId = contractId;
            _expectedDurationMinutes = expectedDurationMinutes;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetEmploymentContract(_contractId, out EmploymentContractState contract)) {
                return WorldCommandResult.Failure($"Contract not found: {_contractId}");
            }

            if (contract.Status != EmploymentContractStatus.Active) {
                return WorldCommandResult.Failure("Contract is not active.");
            }

            if (context.State.TryGetActiveShift(contract.WorkerCitizenId, contract.WorkplaceId, out ShiftState _)) {
                return WorldCommandResult.Failure("Worker already has an active shift here.");
            }

            var shift = new ShiftState(
                _shiftId,
                contract.Id,
                contract.WorkerCitizenId,
                contract.WorkplaceId,
                context.State.CurrentTime,
                context.State.CurrentTime.AddMinutes(_expectedDurationMinutes),
                context.State.CurrentTime,
                ShiftStatus.Active,
                0);
            context.State.AddShift(shift);

            HistoryEvent history = context.History.Create(context.State.CurrentTime, HistoryEventKind.ShiftStarted, "A work shift started.", new[] { contract.WorkerCitizenId }, new[] { contract.WorkplaceId });
            return WorldCommandResult.Success("Shift started.").WithChangedEntity(shift.Id).WithHistoryEvent(history);
        }
    }
}
