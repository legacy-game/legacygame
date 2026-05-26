using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class RecordPerformanceNoteCommand : IWorldCommand
    {
        private readonly WorldEntityId _recordId;
        private readonly WorldEntityId _citizenId;
        private readonly WorldEntityId _workplaceId;
        private readonly string _note;
        private readonly int _score;
        private readonly WorldEntityId _actorId;

        public RecordPerformanceNoteCommand(WorldEntityId recordId, WorldEntityId citizenId, WorldEntityId workplaceId, string note, int score, WorldEntityId actorId)
        {
            _recordId = recordId;
            _citizenId = citizenId;
            _workplaceId = workplaceId;
            _note = note;
            _score = score;
            _actorId = actorId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetWorkplace(_workplaceId, out WorkplaceState workplace)) {
                return WorldCommandResult.Failure("Workplace not found.");
            }

            if (workplace.OwnerCitizenId != _actorId) {
                return WorldCommandResult.Failure("Only the employer can record performance.");
            }

            var record = new PerformanceRecordState(_recordId, _citizenId, workplace.Id, _note, _score, context.State.CurrentTime);
            context.State.AddPerformanceRecord(record);
            HistoryEvent history = context.History.Create(context.State.CurrentTime, HistoryEventKind.PerformanceRecorded, $"Performance note recorded: {_note}", new[] { _actorId, _citizenId }, new[] { workplace.PlaceId });
            return WorldCommandResult.Success("Performance note recorded.").WithChangedEntity(record.Id).WithHistoryEvent(history);
        }
    }
}
