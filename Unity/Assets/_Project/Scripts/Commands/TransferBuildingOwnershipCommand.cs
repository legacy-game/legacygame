using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class TransferBuildingOwnershipCommand : IWorldCommand
    {
        private readonly WorldEntityId _buildingId;
        private readonly WorldEntityId _newOwnerId;
        private readonly WorldEntityId _actorId;

        public TransferBuildingOwnershipCommand(WorldEntityId buildingId, WorldEntityId newOwnerId, WorldEntityId actorId)
        {
            _buildingId = buildingId;
            _newOwnerId = newOwnerId;
            _actorId = actorId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetBuilding(_buildingId, out BuildingState building)) {
                return WorldCommandResult.Failure($"Building not found: {_buildingId}");
            }

            if (!context.State.TryGetCitizen(_newOwnerId, out CitizenState newOwner)) {
                return WorldCommandResult.Failure($"Citizen not found: {_newOwnerId}");
            }

            WorldEntityId previousOwnerId = building.OwnerCitizenId;
            building.TransferOwnership(_newOwnerId);

            if (context.State.TryGetPlot(building.PlotId, out PlotState plot)) {
                plot.TransferOwnership(_newOwnerId);
            }

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.BuildingOwnershipTransferred,
                $"{building.DisplayName} ownership transferred from {previousOwnerId} to {newOwner.DisplayName}.",
                new[] { _actorId, _newOwnerId },
                new[] { building.Id });

            return WorldCommandResult
                .Success($"{building.DisplayName} is now owned by {newOwner.DisplayName}.")
                .WithChangedEntity(building.Id)
                .WithHistoryEvent(historyEvent);
        }
    }
}
