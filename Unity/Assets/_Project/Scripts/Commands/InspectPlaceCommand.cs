using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class InspectPlaceCommand : IWorldCommand
    {
        private readonly WorldEntityId _placeId;
        private readonly WorldEntityId _inspectorId;

        public InspectPlaceCommand(WorldEntityId placeId, WorldEntityId inspectorId)
        {
            _placeId = placeId;
            _inspectorId = inspectorId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            var propertySystem = new PropertySystem(context.State);
            if (!propertySystem.TryGetPlaceSummary(_placeId, out PlacePropertySummary summary)) {
                return WorldCommandResult.Failure($"Place not found: {_placeId}");
            }

            PropertyAccessResult access = propertySystem.CheckPlaceAccess(_placeId, _inspectorId);

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.PlaceInspected,
                $"{summary.DisplayName} place was inspected.",
                new[] { _inspectorId },
                summary.HasBuilding
                    ? new[] { summary.PlaceId, summary.BuildingId }
                    : new[] { summary.PlaceId });

            return WorldCommandResult
                .Success($"{summary.DisplayName}\nKind: {summary.Kind}\nOwner: {summary.OwnerName}\nBuilding: {summary.BuildingName}\nAccess: {summary.AccessRule} ({access.Reason})")
                .WithChangedEntity(summary.PlaceId)
                .WithHistoryEvent(historyEvent);
        }
    }
}
