using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class NpcBackfillJobCommand : IWorldCommand
    {
        private readonly WorldEntityId _contractId;
        private readonly WorldEntityId _postingId;

        public NpcBackfillJobCommand(WorldEntityId contractId, WorldEntityId postingId)
        {
            _contractId = contractId;
            _postingId = postingId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetJobPosting(_postingId, out JobPostingState posting)) {
                return WorldCommandResult.Failure("Posting not found.");
            }

            if (!NpcJobMatcher.TryFindWorkerForPosting(context.State, posting, out CitizenState worker)) {
                return WorldCommandResult.Failure("No NPC worker available.");
            }

            var contract = new EmploymentContractState(
                _contractId,
                posting.Id,
                posting.EmployerCitizenId,
                worker.Id,
                posting.WorkplaceId,
                posting.JobDefinitionId,
                posting.RoleId,
                posting.PayModel,
                posting.PayCents,
                EmploymentContractStatus.Active,
                context.State.CurrentTime);
            context.State.AddEmploymentContract(contract);
            posting.FillSlot();

            HistoryEvent history = context.History.Create(context.State.CurrentTime, HistoryEventKind.EmploymentContractSigned, $"{worker.DisplayName} filled an open job.", new[] { worker.Id, posting.EmployerCitizenId }, new[] { posting.WorkplaceId });
            return WorldCommandResult.Success($"{worker.DisplayName} filled the job.").WithChangedEntity(contract.Id).WithHistoryEvent(history);
        }
    }
}
