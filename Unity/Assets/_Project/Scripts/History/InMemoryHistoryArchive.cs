using System;
using System.Collections.Generic;
using Legacy.Time;
using Legacy.World;

namespace Legacy.History
{
    public sealed class InMemoryHistoryArchive : IHistoryArchive
    {
        private readonly List<HistoryEvent> _events = new();
        private readonly Dictionary<WorldEntityId, List<HistoryEvent>> _historyByActorId = new();
        private readonly Dictionary<WorldEntityId, List<HistoryEvent>> _historyByPlaceId = new();
        private readonly Dictionary<HistoryEventKind, List<HistoryEvent>> _historyByKind = new();
        private readonly SortedDictionary<long, List<HistoryEvent>> _historyByAbsoluteMinute = new();

        public int Count => _events.Count;
        public IReadOnlyList<HistoryEvent> Events => _events;

        public void Add(HistoryEvent historyEvent)
        {
            _events.Add(historyEvent);
            AddToHistoryIndex(_historyByKind, historyEvent.Kind, historyEvent);

            for (int i = 0; i < historyEvent.ActorIds.Count; i++) {
                AddToHistoryIndex(_historyByActorId, historyEvent.ActorIds[i], historyEvent);
            }

            for (int i = 0; i < historyEvent.PlaceIds.Count; i++) {
                AddToHistoryIndex(_historyByPlaceId, historyEvent.PlaceIds[i], historyEvent);
            }

            long minute = ToAbsoluteMinute(historyEvent.Timestamp);
            if (!_historyByAbsoluteMinute.TryGetValue(minute, out List<HistoryEvent> eventsAtMinute)) {
                eventsAtMinute = new List<HistoryEvent>();
                _historyByAbsoluteMinute.Add(minute, eventsAtMinute);
            }

            eventsAtMinute.Add(historyEvent);
        }

        public IReadOnlyList<HistoryEvent> GetForActor(WorldEntityId actorId)
        {
            return _historyByActorId.TryGetValue(actorId, out List<HistoryEvent> events)
                ? events
                : Array.Empty<HistoryEvent>();
        }

        public IReadOnlyList<HistoryEvent> GetForPlace(WorldEntityId placeId)
        {
            return _historyByPlaceId.TryGetValue(placeId, out List<HistoryEvent> events)
                ? events
                : Array.Empty<HistoryEvent>();
        }

        public IReadOnlyList<HistoryEvent> GetByKind(HistoryEventKind kind)
        {
            return _historyByKind.TryGetValue(kind, out List<HistoryEvent> events)
                ? events
                : Array.Empty<HistoryEvent>();
        }

        public List<HistoryEvent> GetBetween(GameDateTime startInclusive, GameDateTime endInclusive)
        {
            long start = ToAbsoluteMinute(startInclusive);
            long end = ToAbsoluteMinute(endInclusive);
            var result = new List<HistoryEvent>();

            foreach (KeyValuePair<long, List<HistoryEvent>> entry in _historyByAbsoluteMinute) {
                if (entry.Key < start) {
                    continue;
                }

                if (entry.Key > end) {
                    break;
                }

                result.AddRange(entry.Value);
            }

            return result;
        }

        private static void AddToHistoryIndex<TKey>(Dictionary<TKey, List<HistoryEvent>> index, TKey key, HistoryEvent historyEvent)
        {
            if (!index.TryGetValue(key, out List<HistoryEvent> events)) {
                events = new List<HistoryEvent>();
                index.Add(key, events);
            }

            events.Add(historyEvent);
        }

        private static long ToAbsoluteMinute(GameDateTime dateTime)
        {
            return (long)dateTime.Date.AbsoluteDay * 1440L + dateTime.Time.TotalMinutes;
        }
    }
}
