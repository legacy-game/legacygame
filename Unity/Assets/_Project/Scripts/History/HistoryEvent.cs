using System;
using System.Collections.Generic;
using Legacy.Time;
using Legacy.World;

namespace Legacy.History
{
    [Serializable]
    public sealed class HistoryEvent
    {
        private readonly List<WorldEntityId> _actorIds;
        private readonly List<WorldEntityId> _placeIds;

        public WorldEntityId Id { get; }
        public GameDateTime Timestamp { get; }
        public HistoryEventKind Kind { get; }
        public string Description { get; }
        public IReadOnlyList<WorldEntityId> ActorIds => _actorIds;
        public IReadOnlyList<WorldEntityId> PlaceIds => _placeIds;

        public HistoryEvent(
            WorldEntityId id,
            GameDateTime timestamp,
            HistoryEventKind kind,
            string description,
            IEnumerable<WorldEntityId> actorIds,
            IEnumerable<WorldEntityId> placeIds)
        {
            if (string.IsNullOrWhiteSpace(description)) {
                throw new ArgumentException("Description must not be empty.", nameof(description));
            }

            Id = id;
            Timestamp = timestamp;
            Kind = kind;
            Description = description;
            _actorIds = new List<WorldEntityId>(actorIds ?? Array.Empty<WorldEntityId>());
            _placeIds = new List<WorldEntityId>(placeIds ?? Array.Empty<WorldEntityId>());
        }
    }
}
