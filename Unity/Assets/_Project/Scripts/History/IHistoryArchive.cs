using System.Collections.Generic;
using Legacy.Time;
using Legacy.World;

namespace Legacy.History
{
    public interface IHistoryArchive
    {
        int Count { get; }
        IReadOnlyList<HistoryEvent> Events { get; }

        void Add(HistoryEvent historyEvent);
        IReadOnlyList<HistoryEvent> GetForActor(WorldEntityId actorId);
        IReadOnlyList<HistoryEvent> GetForPlace(WorldEntityId placeId);
        IReadOnlyList<HistoryEvent> GetByKind(HistoryEventKind kind);
        List<HistoryEvent> GetBetween(GameDateTime startInclusive, GameDateTime endInclusive);
    }
}
