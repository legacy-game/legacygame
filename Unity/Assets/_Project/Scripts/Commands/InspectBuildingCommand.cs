using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class InspectBuildingCommand : IWorldCommand
    {
        private readonly WorldEntityId _buildingId;
        private readonly WorldEntityId _inspectorId;

        public InspectBuildingCommand(WorldEntityId buildingId, WorldEntityId inspectorId)
        {
            _buildingId = buildingId;
            _inspectorId = inspectorId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetBuilding(_buildingId, out BuildingState building)) {
                return WorldCommandResult.Failure($"Building not found: {_buildingId}");
            }

            string ownerName = context.State.TryGetCitizen(building.OwnerCitizenId, out CitizenState owner)
                ? owner.DisplayName
                : building.OwnerCitizenId.ToString();

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.BuildingInspected,
                $"{building.DisplayName} was inspected.",
                new[] { _inspectorId },
                new[] { building.Id });

            return WorldCommandResult
                .Success($"{building.DisplayName}\nOwned by {ownerName}")
                .WithChangedEntity(building.Id)
                .WithHistoryEvent(historyEvent);
        }
    }
}
