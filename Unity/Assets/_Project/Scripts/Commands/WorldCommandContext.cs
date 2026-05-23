using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class WorldCommandContext
    {
        public WorldState State { get; }
        public HistoryLog History { get; }

        public WorldCommandContext(WorldState state, HistoryLog history)
        {
            State = state;
            History = history;
        }
    }
}
