using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class TerritoryChunkState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId RegionId { get; }
        public GridChunkCoord Coord { get; }
        public string DisplayName { get; private set; }
        public TerritoryBiome Biome { get; private set; }
        public TerritoryClaimStatus ClaimStatus { get; private set; }
        public WorldEntityId ClaimOwnerId { get; private set; }
        public WorldEntityId SettlementId { get; private set; }
        public WorldEntityId JurisdictionId { get; private set; }
        public bool IsBuildable { get; private set; }
        public bool IsDiscovered { get; private set; }

        public TerritoryChunkState(
            WorldEntityId id,
            WorldEntityId regionId,
            GridChunkCoord coord,
            string displayName,
            TerritoryBiome biome,
            TerritoryClaimStatus claimStatus,
            WorldEntityId claimOwnerId,
            WorldEntityId settlementId,
            WorldEntityId jurisdictionId,
            bool isBuildable,
            bool isDiscovered)
        {
            if (string.IsNullOrWhiteSpace(displayName)) {
                throw new ArgumentException("Display name must not be empty.", nameof(displayName));
            }

            Id = id;
            RegionId = regionId;
            Coord = coord;
            DisplayName = displayName;
            Biome = biome;
            ClaimStatus = claimStatus;
            ClaimOwnerId = claimOwnerId;
            SettlementId = settlementId;
            JurisdictionId = jurisdictionId;
            IsBuildable = isBuildable;
            IsDiscovered = isDiscovered;
        }

        public void Claim(TerritoryClaimStatus claimStatus, WorldEntityId claimOwnerId)
        {
            ClaimStatus = claimStatus;
            ClaimOwnerId = claimOwnerId;
        }
    }
}
