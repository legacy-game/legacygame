using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class QuitJobCommand : IWorldCommand
    {
        private readonly WorldEntityId _contractId;
        private readonly WorldEntityId _workerCitizenId;

        public QuitJobCommand(WorldEntityId contractId, WorldEntityId workerCitizenId)
        {
            _contractId = contractId;
            _workerCitizenId = workerCitizenId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetEmploymentContract(_contractId, out EmploymentContractState contract)) {
                return WorldCommandResult.Failure($"Contract not found: {_contractId}");
            }

            if (contract.WorkerCitizenId != _workerCitizenId) {
                return WorldCommandResult.Failure("Only the worker can quit this job.");
            }

            contract.Quit();
            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.EmploymentContractEnded,
                "A worker quit their job.",
                new[] { contract.WorkerCitizenId, contract.EmployerCitizenId },
                new[] { contract.WorkplaceId });

            return WorldCommandResult.Success("Job quit.")
                .WithChangedEntity(contract.Id)
                .WithHistoryEvent(history);
        }
    }
}
