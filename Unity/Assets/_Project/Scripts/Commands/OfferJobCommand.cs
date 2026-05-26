using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class OfferJobCommand : IWorldCommand
    {
        private readonly WorldEntityId _applicationId;
        private readonly WorldEntityId _employerCitizenId;

        public OfferJobCommand(WorldEntityId applicationId, WorldEntityId employerCitizenId)
        {
            _applicationId = applicationId;
            _employerCitizenId = employerCitizenId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetJobApplication(_applicationId, out JobApplicationState application)) {
                return WorldCommandResult.Failure($"Application not found: {_applicationId}");
            }

            if (!context.State.TryGetJobPosting(application.PostingId, out JobPostingState posting)) {
                return WorldCommandResult.Failure($"Posting not found: {application.PostingId}");
            }

            if (posting.EmployerCitizenId != _employerCitizenId) {
                return WorldCommandResult.Failure("Only the employer can offer this job.");
            }

            application.Offer();
            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.JobOffered,
                "A job offer was made.",
                new[] { _employerCitizenId, application.ApplicantCitizenId },
                new[] { posting.WorkplaceId });

            return WorldCommandResult.Success("Job offer made.")
                .WithChangedEntity(application.Id)
                .WithHistoryEvent(history);
        }
    }
}
