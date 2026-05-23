using System;
using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class WorldRuntime
    {
        private HistoryLog _history;

        public WorldState State { get; private set; }

        public event Action<WorldCommandResult> CommandExecuted;
        public event Action<WorldState> StateReplaced;

        public WorldRuntime(WorldState initialState)
        {
            State = initialState ?? throw new ArgumentNullException(nameof(initialState));
            _history = new HistoryLog(initialState.RecentHistory.Count + 1);
        }

        public WorldCommandResult Execute(IWorldCommand command)
        {
            if (command == null) {
                throw new ArgumentNullException(nameof(command));
            }

            var result = command.Execute(new WorldCommandContext(State, _history));
            if (result.Succeeded) {
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
            _history = new HistoryLog(State.RecentHistory.Count + 1);
            StateReplaced?.Invoke(State);
        }
    }
}
