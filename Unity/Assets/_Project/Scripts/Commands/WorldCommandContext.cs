using Legacy.History;
using Legacy.Time;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class WorldCommandContext
    {
        public WorldState State { get; }
        public HistoryLog History { get; }
        public WorldClock Clock { get; }

        public WorldCommandContext(WorldState state, HistoryLog history, WorldClock clock)
        {
            State = state;
            History = history;
            Clock = clock;
        }
    }
}
