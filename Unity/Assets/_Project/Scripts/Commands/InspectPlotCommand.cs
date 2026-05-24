using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class InspectPlotCommand : IWorldCommand
    {
        private readonly WorldEntityId _plotId;
        private readonly WorldEntityId _inspectorId;

        public InspectPlotCommand(WorldEntityId plotId, WorldEntityId inspectorId)
        {
            _plotId = plotId;
            _inspectorId = inspectorId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            var propertySystem = new PropertySystem(context.State);
            if (!propertySystem.TryGetPlotSummary(_plotId, out PlotPropertySummary summary)) {
                return WorldCommandResult.Failure($"Plot not found: {_plotId}");
            }

            PropertyAccessResult access = propertySystem.CheckPlotAccess(_plotId, _inspectorId);

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.PlotInspected,
                $"{summary.DisplayName} plot was inspected.",
                new[] { _inspectorId },
                new[] { summary.PlotId });

            return WorldCommandResult
                .Success($"{summary.DisplayName}\nOwned by {summary.OwnerName}\nBuildings: {summary.BuildingCount}\nAccess: {summary.AccessRule} ({access.Reason})")
                .WithChangedEntity(summary.PlotId)
                .WithHistoryEvent(historyEvent);
        }
    }

    public sealed class InspectTerritoryCommand : IWorldCommand
    {
        private readonly WorldEntityId _territoryId;
        private readonly WorldEntityId _inspectorId;

        public InspectTerritoryCommand(WorldEntityId territoryId, WorldEntityId inspectorId)
        {
            _territoryId = territoryId;
            _inspectorId = inspectorId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetTerritoryChunk(_territoryId, out TerritoryChunkState territory)) {
                return WorldCommandResult.Failure($"Territory not found: {_territoryId}");
            }

            string owner = string.IsNullOrEmpty(territory.ClaimOwnerId.Value)
                ? "None"
                : territory.ClaimOwnerId.ToString();
            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.TerritoryInspected,
                $"{territory.DisplayName} territory was inspected.",
                new[] { _inspectorId },
                new[] { territory.Id });

            return WorldCommandResult
                .Success($"{territory.DisplayName}\nBiome: {territory.Biome}\nClaim: {territory.ClaimStatus}\nOwner: {owner}\nBuildable: {territory.IsBuildable}")
                .WithChangedEntity(territory.Id)
                .WithHistoryEvent(historyEvent);
        }
    }
}
