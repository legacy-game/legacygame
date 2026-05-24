using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class TransferPlotOwnershipCommand : IWorldCommand
    {
        private readonly WorldEntityId _plotId;
        private readonly WorldEntityId _newOwnerId;
        private readonly WorldEntityId _actorId;

        public TransferPlotOwnershipCommand(WorldEntityId plotId, WorldEntityId newOwnerId, WorldEntityId actorId)
        {
            _plotId = plotId;
            _newOwnerId = newOwnerId;
            _actorId = actorId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetPlot(_plotId, out PlotState plot)) {
                return WorldCommandResult.Failure($"Plot not found: {_plotId}");
            }

            if (!context.State.TryGetCitizen(_newOwnerId, out CitizenState newOwner)) {
                return WorldCommandResult.Failure($"Citizen not found: {_newOwnerId}");
            }

            WorldEntityId previousOwnerId = plot.OwnerCitizenId;
            plot.TransferOwnership(_newOwnerId);

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.PlotOwnershipTransferred,
                $"{plot.DisplayName} plot ownership transferred from {previousOwnerId} to {newOwner.DisplayName}.",
                new[] { _actorId, _newOwnerId },
                new[] { plot.Id });

            return WorldCommandResult
                .Success($"{plot.DisplayName} is now owned by {newOwner.DisplayName}.")
                .WithChangedEntity(plot.Id)
                .WithHistoryEvent(historyEvent);
        }
    }
}
