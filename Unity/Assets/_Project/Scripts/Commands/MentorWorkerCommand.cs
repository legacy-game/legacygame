using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class MentorWorkerCommand : IWorldCommand
    {
        private readonly WorldEntityId _mentorId;
        private readonly WorldEntityId _workerId;
        private readonly SkillKind _skill;
        private readonly int _experience;

        public MentorWorkerCommand(WorldEntityId mentorId, WorldEntityId workerId, SkillKind skill, int experience)
        {
            _mentorId = mentorId;
            _workerId = workerId;
            _skill = skill;
            _experience = experience;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetCitizen(_mentorId, out CitizenState mentor)) {
                return WorldCommandResult.Failure("Mentor not found.");
            }

            if (!context.State.TryGetCitizen(_workerId, out CitizenState worker)) {
                return WorldCommandResult.Failure("Worker not found.");
            }

            context.State.GetOrCreateSkill(worker.Id, _skill).AddExperience(_experience);
            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.SkillImproved,
                $"{mentor.DisplayName} mentored {worker.DisplayName} in {_skill}.",
                new[] { mentor.Id, worker.Id },
                System.Array.Empty<WorldEntityId>());

            return WorldCommandResult.Success("Mentorship completed.")
                .WithChangedEntity(worker.Id)
                .WithHistoryEvent(history);
        }
    }
}
