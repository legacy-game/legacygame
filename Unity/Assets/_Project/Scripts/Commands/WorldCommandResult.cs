using System.Collections.Generic;
using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class WorldCommandResult
    {
        private readonly List<WorldEntityId> _changedEntityIds = new();
        private readonly List<HistoryEvent> _historyEvents = new();

        public bool Succeeded { get; }
        public string Message { get; private set; }
        public IReadOnlyList<WorldEntityId> ChangedEntityIds => _changedEntityIds;
        public IReadOnlyList<HistoryEvent> HistoryEvents => _historyEvents;

        private WorldCommandResult(bool succeeded, string message)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public static WorldCommandResult Success(string message = "")
        {
            return new WorldCommandResult(true, message);
        }

        public static WorldCommandResult Failure(string message)
        {
            return new WorldCommandResult(false, message);
        }

        public WorldCommandResult WithChangedEntity(WorldEntityId entityId)
        {
            _changedEntityIds.Add(entityId);
            return this;
        }

        public WorldCommandResult WithHistoryEvent(HistoryEvent historyEvent)
        {
            _historyEvents.Add(historyEvent);
            return this;
        }

        public WorldCommandResult WithMessage(string message)
        {
            Message = message;
            return this;
        }
    }
}
