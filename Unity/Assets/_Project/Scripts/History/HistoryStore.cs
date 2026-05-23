using System.Collections.Generic;
using Legacy.Time;
using Legacy.World;

namespace Legacy.History
{
    public sealed class HistoryStore
    {
        public const int MaxRecentHistoryEvents = 100;

        private readonly List<HistoryEvent> _recentHistory = new();
        private readonly Dictionary<WorldEntityId, List<HistoryEvent>> _historyByActorId = new();
        private readonly Dictionary<WorldEntityId, List<HistoryEvent>> _historyByPlaceId = new();
        private readonly Dictionary<HistoryEventKind, List<HistoryEvent>> _historyByKind = new();
        private readonly SortedDictionary<long, List<HistoryEvent>> _historyByAbsoluteMinute = new();
        private readonly IHistoryArchive _archive;

        public IReadOnlyList<HistoryEvent> RecentHistory => _recentHistory;
        public IHistoryArchive Archive => _archive;

        public HistoryStore(IHistoryArchive archive = null)
        {
            _archive = archive ?? new InMemoryHistoryArchive();
        }

        public void Add(HistoryEvent historyEvent)
        {
            _recentHistory.Add(historyEvent);
            IndexHistoryEvent(historyEvent);

            while (_recentHistory.Count > MaxRecentHistoryEvents) {
                HistoryEvent archivedEvent = _recentHistory[0];
                _recentHistory.RemoveAt(0);
                _archive.Add(archivedEvent);
            }
        }

        public IReadOnlyList<HistoryEvent> GetForActor(WorldEntityId actorId)
        {
            var result = new List<HistoryEvent>();
            result.AddRange(_archive.GetForActor(actorId));

            if (_historyByActorId.TryGetValue(actorId, out List<HistoryEvent> events)) {
                result.AddRange(events);
            }

            return result;
        }

        public IReadOnlyList<HistoryEvent> GetForPlace(WorldEntityId placeId)
        {
            var result = new List<HistoryEvent>();
            result.AddRange(_archive.GetForPlace(placeId));

            if (_historyByPlaceId.TryGetValue(placeId, out List<HistoryEvent> events)) {
                result.AddRange(events);
            }

            return result;
        }

        public IReadOnlyList<HistoryEvent> GetByKind(HistoryEventKind kind)
        {
            var result = new List<HistoryEvent>();
            result.AddRange(_archive.GetByKind(kind));

            if (_historyByKind.TryGetValue(kind, out List<HistoryEvent> events)) {
                result.AddRange(events);
            }

            return result;
        }

        public List<HistoryEvent> GetBetween(GameDateTime startInclusive, GameDateTime endInclusive)
        {
            long start = ToAbsoluteMinute(startInclusive);
            long end = ToAbsoluteMinute(endInclusive);
            List<HistoryEvent> result = _archive.GetBetween(startInclusive, endInclusive);

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

        private void IndexHistoryEvent(HistoryEvent historyEvent)
        {
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
