using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class FireWorkerCommand : IWorldCommand
    {
        private readonly WorldEntityId _contractId;
        private readonly WorldEntityId _employerCitizenId;

        public FireWorkerCommand(WorldEntityId contractId, WorldEntityId employerCitizenId)
        {
            _contractId = contractId;
            _employerCitizenId = employerCitizenId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetEmploymentContract(_contractId, out EmploymentContractState contract)) {
                return WorldCommandResult.Failure($"Contract not found: {_contractId}");
            }

            if (contract.EmployerCitizenId != _employerCitizenId) {
                return WorldCommandResult.Failure("Only the employer can fire this worker.");
            }

            contract.Fire();
            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.EmploymentContractEnded,
                "A worker was fired.",
                new[] { contract.WorkerCitizenId, contract.EmployerCitizenId },
                new[] { contract.WorkplaceId });

            return WorldCommandResult.Success("Worker fired.")
                .WithChangedEntity(contract.Id)
                .WithHistoryEvent(history);
        }
    }
}
