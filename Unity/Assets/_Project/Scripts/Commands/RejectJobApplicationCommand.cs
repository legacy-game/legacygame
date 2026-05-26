using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class RejectJobApplicationCommand : IWorldCommand
    {
        private readonly WorldEntityId _applicationId;
        private readonly WorldEntityId _employerCitizenId;

        public RejectJobApplicationCommand(WorldEntityId applicationId, WorldEntityId employerCitizenId)
        {
            _applicationId = applicationId;
            _employerCitizenId = employerCitizenId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetJobApplication(_applicationId, out JobApplicationState application)) {
                return WorldCommandResult.Failure($"Application not found: {_applicationId}");
            }

            if (!context.State.TryGetJobPosting(application.PostingId, out JobPostingState posting) || posting.EmployerCitizenId != _employerCitizenId) {
                return WorldCommandResult.Failure("Only the employer can reject this application.");
            }

            application.Reject();
            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.JobApplicationRejected,
                "A job application was rejected.",
                new[] { _employerCitizenId, application.ApplicantCitizenId },
                new[] { posting.WorkplaceId });

            return WorldCommandResult.Success("Application rejected.")
                .WithChangedEntity(application.Id)
                .WithHistoryEvent(history);
        }
    }
}
