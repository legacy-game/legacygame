using System;
using System.Collections.Generic;

namespace Legacy.History
{
    public static class HistoryQuery
    {
        public static IReadOnlyList<HistoryEvent> Last(IReadOnlyList<HistoryEvent> events, int count)
        {
            if (events == null) {
                throw new ArgumentNullException(nameof(events));
            }

            if (count <= 0 || events.Count == 0) {
                return Array.Empty<HistoryEvent>();
            }

            int start = Math.Max(0, events.Count - count);
            int length = events.Count - start;
            var result = new HistoryEvent[length];

            for (int i = 0; i < length; i++) {
                result[i] = events[start + i];
            }

            return result;
        }
    }
}
