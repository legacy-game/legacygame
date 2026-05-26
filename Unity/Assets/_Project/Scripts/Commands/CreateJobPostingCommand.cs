using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class CreateJobPostingCommand : IWorldCommand
    {
        private readonly WorldEntityId _postingId;
        private readonly string _jobDefinitionId;
        private readonly WorldEntityId _employerCitizenId;
        private readonly WorldEntityId _workplaceId;
        private readonly int _openSlots;

        public CreateJobPostingCommand(WorldEntityId postingId, string jobDefinitionId, WorldEntityId employerCitizenId, WorldEntityId workplaceId, int openSlots)
        {
            _postingId = postingId;
            _jobDefinitionId = jobDefinitionId;
            _employerCitizenId = employerCitizenId;
            _workplaceId = workplaceId;
            _openSlots = openSlots;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetCitizen(_employerCitizenId, out CitizenState employer)) {
                return WorldCommandResult.Failure($"Employer not found: {_employerCitizenId}");
            }

            if (!context.State.TryGetWorkplace(_workplaceId, out WorkplaceState workplace)) {
                return WorldCommandResult.Failure($"Workplace not found: {_workplaceId}");
            }

            if (workplace.OwnerCitizenId != employer.Id) {
                return WorldCommandResult.Failure($"{employer.DisplayName} cannot post jobs for this workplace.");
            }

            if (!JobCatalog.TryGet(_jobDefinitionId, out JobDefinition job)) {
                return WorldCommandResult.Failure($"Job definition not found: {_jobDefinitionId}");
            }

            var posting = new JobPostingState(
                _postingId,
                job.Id,
                employer.Id,
                workplace.Id,
                job.RoleId,
                job.DefaultPayModel,
                job.DefaultPayCents,
                _openSlots,
                JobPostingStatus.Open,
                context.State.CurrentTime);
            context.State.AddJobPosting(posting);

            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.JobPostingCreated,
                $"{employer.DisplayName} posted {job.DisplayName} work.",
                new[] { employer.Id },
                new[] { workplace.PlaceId });

            return WorldCommandResult.Success($"Posted job: {job.DisplayName}.")
                .WithChangedEntity(posting.Id)
                .WithChangedEntity(workplace.Id)
                .WithHistoryEvent(history);
        }
    }
}
