using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class AcceptJobOfferCommand : IWorldCommand
    {
        private readonly WorldEntityId _contractId;
        private readonly WorldEntityId _applicationId;
        private readonly WorldEntityId _workerCitizenId;

        public AcceptJobOfferCommand(WorldEntityId contractId, WorldEntityId applicationId, WorldEntityId workerCitizenId)
        {
            _contractId = contractId;
            _applicationId = applicationId;
            _workerCitizenId = workerCitizenId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetJobApplication(_applicationId, out JobApplicationState application)) {
                return WorldCommandResult.Failure($"Application not found: {_applicationId}");
            }

            if (application.ApplicantCitizenId != _workerCitizenId) {
                return WorldCommandResult.Failure("Only the applicant can accept this offer.");
            }

            if (application.Status != JobApplicationStatus.Offered && application.Status != JobApplicationStatus.Pending) {
                return WorldCommandResult.Failure("Application is not offerable.");
            }

            if (!context.State.TryGetJobPosting(application.PostingId, out JobPostingState posting)) {
                return WorldCommandResult.Failure($"Posting not found: {application.PostingId}");
            }

            application.Accept();
            posting.FillSlot();
            var contract = new EmploymentContractState(
                _contractId,
                posting.Id,
                posting.EmployerCitizenId,
                application.ApplicantCitizenId,
                posting.WorkplaceId,
                posting.JobDefinitionId,
                posting.RoleId,
                posting.PayModel,
                posting.PayCents,
                EmploymentContractStatus.Active,
                context.State.CurrentTime);
            context.State.AddEmploymentContract(contract);

            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.EmploymentContractSigned,
                "An employment contract was signed.",
                new[] { posting.EmployerCitizenId, application.ApplicantCitizenId },
                new[] { posting.WorkplaceId });

            return WorldCommandResult.Success("Employment contract signed.")
                .WithChangedEntity(application.Id)
                .WithChangedEntity(posting.Id)
                .WithChangedEntity(contract.Id)
                .WithHistoryEvent(history);
        }
    }
}
