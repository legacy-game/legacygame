using System;
using System.Collections.Generic;
using Legacy.Time;
using Legacy.World;

namespace Legacy.History
{
    public sealed class HistoryLog
    {
        private long _nextEventNumber;

        public HistoryLog(long nextEventNumber = 1)
        {
            if (nextEventNumber < 1) {
                throw new ArgumentOutOfRangeException(nameof(nextEventNumber), "Event number must start at 1.");
            }

            _nextEventNumber = nextEventNumber;
        }

        public HistoryEvent Create(
            GameDateTime timestamp,
            HistoryEventKind kind,
            string description,
            IEnumerable<WorldEntityId> actorIds = null,
            IEnumerable<WorldEntityId> placeIds = null)
        {
            var id = new WorldEntityId($"evt_{_nextEventNumber:000000}");
            _nextEventNumber++;
            return new HistoryEvent(id, timestamp, kind, description, actorIds, placeIds);
        }
    }
}
