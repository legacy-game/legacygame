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
            var propertySystem = new PropertySystem(context.State);
            if (!propertySystem.TryGetBuildingSummary(_buildingId, out BuildingPropertySummary summary)) {
                return WorldCommandResult.Failure($"Building not found: {_buildingId}");
            }

            PropertyAccessResult access = propertySystem.CheckBuildingAccess(_buildingId, _inspectorId);

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.BuildingInspected,
                $"{summary.DisplayName} was inspected.",
                new[] { _inspectorId },
                new[] { summary.BuildingId, summary.PlotId, summary.InteriorPlaceId });

            return WorldCommandResult
                .Success($"{summary.DisplayName}\nOwned by {summary.OwnerName}\nPlot: {summary.PlotName}\nPlace: {summary.InteriorPlaceName}\nAccess: {summary.AccessRule} ({access.Reason})")
                .WithChangedEntity(summary.BuildingId)
                .WithHistoryEvent(historyEvent);
        }
    }
}
