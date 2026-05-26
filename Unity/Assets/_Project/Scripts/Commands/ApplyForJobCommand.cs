using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class ApplyForJobCommand : IWorldCommand
    {
        private readonly WorldEntityId _applicationId;
        private readonly WorldEntityId _postingId;
        private readonly WorldEntityId _applicantCitizenId;

        public ApplyForJobCommand(WorldEntityId applicationId, WorldEntityId postingId, WorldEntityId applicantCitizenId)
        {
            _applicationId = applicationId;
            _postingId = postingId;
            _applicantCitizenId = applicantCitizenId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetJobPosting(_postingId, out JobPostingState posting)) {
                return WorldCommandResult.Failure($"Posting not found: {_postingId}");
            }

            if (posting.Status != JobPostingStatus.Open || posting.OpenSlots <= 0) {
                return WorldCommandResult.Failure("Job posting is not open.");
            }

            if (!context.State.TryGetCitizen(_applicantCitizenId, out CitizenState applicant)) {
                return WorldCommandResult.Failure($"Applicant not found: {_applicantCitizenId}");
            }

            var application = new JobApplicationState(_applicationId, posting.Id, applicant.Id, JobApplicationStatus.Pending, context.State.CurrentTime);
            context.State.AddJobApplication(application);

            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.JobApplied,
                $"{applicant.DisplayName} applied for a job.",
                new[] { applicant.Id },
                new[] { posting.WorkplaceId });

            return WorldCommandResult.Success($"{applicant.DisplayName} applied for the job.")
                .WithChangedEntity(application.Id)
                .WithChangedEntity(applicant.Id)
                .WithHistoryEvent(history);
        }
    }
}
