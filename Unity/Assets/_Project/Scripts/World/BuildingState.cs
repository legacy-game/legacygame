using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class BuildingState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId RegionId { get; }
        public WorldEntityId PlotId { get; }
        public WorldEntityId ExteriorSceneId { get; }
        public WorldEntityId InteriorSceneId { get; }
        public WorldEntityId InteriorPlaceId { get; }
        public string DisplayName { get; private set; }
        public WorldEntityId OwnerCitizenId { get; private set; }
        public PlaceKind Kind { get; private set; }
        public GridCoord EntranceCoord { get; private set; }

        public BuildingState(
            WorldEntityId id,
            WorldEntityId regionId,
            WorldEntityId plotId,
            WorldEntityId exteriorSceneId,
            WorldEntityId interiorSceneId,
            WorldEntityId interiorPlaceId,
            string displayName,
            WorldEntityId ownerCitizenId,
            PlaceKind kind,
            GridCoord entranceCoord)
        {
            if (string.IsNullOrWhiteSpace(displayName)) {
                throw new ArgumentException("Display name must not be empty.", nameof(displayName));
            }

            Id = id;
            RegionId = regionId;
            PlotId = plotId;
            ExteriorSceneId = exteriorSceneId;
            InteriorSceneId = interiorSceneId;
            InteriorPlaceId = interiorPlaceId;
            DisplayName = displayName;
            OwnerCitizenId = ownerCitizenId;
            Kind = kind;
            EntranceCoord = entranceCoord;
        }

        public void TransferOwnership(WorldEntityId newOwnerCitizenId)
        {
            OwnerCitizenId = newOwnerCitizenId;
        }
    }
}
