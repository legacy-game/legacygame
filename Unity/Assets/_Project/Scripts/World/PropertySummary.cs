namespace Legacy.World
{
    public readonly struct BuildingPropertySummary
    {
        public WorldEntityId BuildingId { get; }
        public string DisplayName { get; }
        public WorldEntityId OwnerCitizenId { get; }
        public string OwnerName { get; }
        public WorldEntityId PlotId { get; }
        public string PlotName { get; }
        public WorldEntityId InteriorPlaceId { get; }
        public string InteriorPlaceName { get; }
        public AccessRule AccessRule { get; }
        public PlaceKind Kind { get; }

        public BuildingPropertySummary(
            WorldEntityId buildingId,
            string displayName,
            WorldEntityId ownerCitizenId,
            string ownerName,
            WorldEntityId plotId,
            string plotName,
            WorldEntityId interiorPlaceId,
            string interiorPlaceName,
            AccessRule accessRule,
            PlaceKind kind)
        {
            BuildingId = buildingId;
            DisplayName = displayName;
            OwnerCitizenId = ownerCitizenId;
            OwnerName = ownerName;
            PlotId = plotId;
            PlotName = plotName;
            InteriorPlaceId = interiorPlaceId;
            InteriorPlaceName = interiorPlaceName;
            AccessRule = accessRule;
            Kind = kind;
        }
    }

    public readonly struct PlotPropertySummary
    {
        public WorldEntityId PlotId { get; }
        public string DisplayName { get; }
        public WorldEntityId OwnerCitizenId { get; }
        public string OwnerName { get; }
        public AccessRule AccessRule { get; }
        public int BuildingCount { get; }

        public PlotPropertySummary(
            WorldEntityId plotId,
            string displayName,
            WorldEntityId ownerCitizenId,
            string ownerName,
            AccessRule accessRule,
            int buildingCount)
        {
            PlotId = plotId;
            DisplayName = displayName;
            OwnerCitizenId = ownerCitizenId;
            OwnerName = ownerName;
            AccessRule = accessRule;
            BuildingCount = buildingCount;
        }
    }

    public readonly struct PlacePropertySummary
    {
        public WorldEntityId PlaceId { get; }
        public string DisplayName { get; }
        public WorldEntityId SceneId { get; }
        public PlaceKind Kind { get; }
        public WorldEntityId OwnerCitizenId { get; }
        public string OwnerName { get; }
        public AccessRule AccessRule { get; }
        public bool HasBuilding { get; }
        public WorldEntityId BuildingId { get; }
        public string BuildingName { get; }

        public PlacePropertySummary(
            WorldEntityId placeId,
            string displayName,
            WorldEntityId sceneId,
            PlaceKind kind,
            WorldEntityId ownerCitizenId,
            string ownerName,
            AccessRule accessRule,
            bool hasBuilding,
            WorldEntityId buildingId,
            string buildingName)
        {
            PlaceId = placeId;
            DisplayName = displayName;
            SceneId = sceneId;
            Kind = kind;
            OwnerCitizenId = ownerCitizenId;
            OwnerName = ownerName;
            AccessRule = accessRule;
            HasBuilding = hasBuilding;
            BuildingId = buildingId;
            BuildingName = buildingName;
        }
    }
}
