using Legacy.World;

namespace Legacy.Commands
{
    public sealed class SuspendWorkerCommand : IWorldCommand
    {
        private readonly WorldEntityId _contractId;
        private readonly WorldEntityId _employerCitizenId;

        public SuspendWorkerCommand(WorldEntityId contractId, WorldEntityId employerCitizenId)
        {
            _contractId = contractId;
            _employerCitizenId = employerCitizenId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetEmploymentContract(_contractId, out EmploymentContractState contract)) {
                return WorldCommandResult.Failure("Contract not found.");
            }

            if (contract.EmployerCitizenId != _employerCitizenId) {
                return WorldCommandResult.Failure("Only the employer can suspend this worker.");
            }

            contract.Suspend();
            return WorldCommandResult.Success("Worker suspended.").WithChangedEntity(contract.Id);
        }
    }
}
