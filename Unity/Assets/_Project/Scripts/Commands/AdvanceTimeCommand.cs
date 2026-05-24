using System;
using Legacy.History;
using Legacy.Time;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class AdvanceTimeCommand : IWorldCommand
    {
        private readonly int _minutes;

        public AdvanceTimeCommand(int minutes)
        {
            if (minutes <= 0) {
                throw new ArgumentOutOfRangeException(nameof(minutes), "Minutes must be positive.");
            }

            _minutes = minutes;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            GameDateTime previousTime = context.State.CurrentTime;
            context.Clock.AdvanceMinutes(_minutes);
            context.State.SetTime(context.Clock.Now);

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.TimeAdvanced,
                $"World time advanced by {_minutes} minutes.");

            WorldCommandResult result = WorldCommandResult
                .Success($"Advanced time to {context.State.CurrentTime}.")
                .WithHistoryEvent(historyEvent);

            foreach (HistoryEvent scheduleEvent in WorldScheduleSimulator.AdvanceSchedules(context.State, context.History, previousTime)) {
                result.WithHistoryEvent(scheduleEvent);
            }

            return result;
        }
    }
}
