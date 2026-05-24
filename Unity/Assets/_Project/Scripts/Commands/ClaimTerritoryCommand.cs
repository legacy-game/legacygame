using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class ClaimTerritoryCommand : IWorldCommand
    {
        private readonly WorldEntityId _territoryId;
        private readonly WorldEntityId _claimOwnerId;
        private readonly WorldEntityId _actorId;
        private readonly TerritoryClaimStatus _claimStatus;

        public ClaimTerritoryCommand(
            WorldEntityId territoryId,
            WorldEntityId claimOwnerId,
            WorldEntityId actorId,
            TerritoryClaimStatus claimStatus = TerritoryClaimStatus.Private)
        {
            _territoryId = territoryId;
            _claimOwnerId = claimOwnerId;
            _actorId = actorId;
            _claimStatus = claimStatus;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetTerritoryChunk(_territoryId, out TerritoryChunkState territory)) {
                return WorldCommandResult.Failure($"Territory not found: {_territoryId}");
            }

            if (territory.ClaimStatus != TerritoryClaimStatus.Unclaimed) {
                return WorldCommandResult.Failure($"{territory.DisplayName} is already claimed as {territory.ClaimStatus}.");
            }

            territory.Claim(_claimStatus, _claimOwnerId);

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.TerritoryClaimed,
                $"{territory.DisplayName} was claimed as {_claimStatus} by {_claimOwnerId}.",
                new[] { _actorId, _claimOwnerId },
                new[] { territory.Id });

            return WorldCommandResult
                .Success($"{territory.DisplayName} claimed as {_claimStatus}.")
                .WithChangedEntity(territory.Id)
                .WithHistoryEvent(historyEvent);
        }
    }
}
