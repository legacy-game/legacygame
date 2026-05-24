using System;
using System.Collections.Generic;
using Legacy.History;
using Legacy.Time;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class WorldRuntime
    {
        private const string GeneratedHistoryEventPrefix = "evt_";

        private HistoryLog _history;
        private WorldClock _clock;

        public WorldState State { get; private set; }
        public WorldClock Clock => _clock;

        public event Action<WorldCommandResult> CommandExecuted;
        public event Action<WorldState> StateReplaced;

        public WorldRuntime(WorldState initialState)
        {
            State = initialState ?? throw new ArgumentNullException(nameof(initialState));
            _history = CreateHistoryLogForState(State);
            _clock = new WorldClock(State.CurrentTime);
        }

        public WorldCommandResult Execute(IWorldCommand command)
        {
            if (command == null) {
                throw new ArgumentNullException(nameof(command));
            }

            _clock.SetTime(State.CurrentTime);
            var result = command.Execute(new WorldCommandContext(State, _history, _clock));
            if (result.Succeeded) {
                State.SetTime(_clock.Now);
                foreach (HistoryEvent historyEvent in result.HistoryEvents) {
                    State.AddHistoryEvent(historyEvent);
                }
            }

            CommandExecuted?.Invoke(result);
            return result;
        }

        public void ReplaceState(WorldState state)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
            _history = CreateHistoryLogForState(State);
            _clock = new WorldClock(State.CurrentTime);
            StateReplaced?.Invoke(State);
        }

        private static HistoryLog CreateHistoryLogForState(WorldState state)
        {
            return new HistoryLog(FindNextHistoryEventNumber(state));
        }

        private static long FindNextHistoryEventNumber(WorldState state)
        {
            long highestEventNumber = 0;
            IncludeHistoryEvents(state.HistoryArchive.Events, ref highestEventNumber);
            IncludeHistoryEvents(state.RecentHistory, ref highestEventNumber);
            return highestEventNumber + 1;
        }

        private static void IncludeHistoryEvents(IReadOnlyList<HistoryEvent> historyEvents, ref long highestEventNumber)
        {
            for (int i = 0; i < historyEvents.Count; i++) {
                IncludeHistoryEvent(historyEvents[i], ref highestEventNumber);
            }
        }

        private static void IncludeHistoryEvent(HistoryEvent historyEvent, ref long highestEventNumber)
        {
            string eventId = historyEvent.Id.Value;
            if (!eventId.StartsWith(GeneratedHistoryEventPrefix, StringComparison.Ordinal)) {
                return;
            }

            string eventNumberText = eventId.Substring(GeneratedHistoryEventPrefix.Length);
            if (long.TryParse(eventNumberText, out long eventNumber) && eventNumber > highestEventNumber) {
                highestEventNumber = eventNumber;
            }
        }
    }
}
