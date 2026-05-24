namespace Legacy.World
{
    public sealed class PropertySystem
    {
        private readonly WorldState _state;

        public PropertySystem(WorldState state)
        {
            _state = state;
        }

        public bool TryGetBuildingSummary(WorldEntityId buildingId, out BuildingPropertySummary summary)
        {
            summary = default;
            if (!_state.TryGetBuilding(buildingId, out BuildingState building)) {
                return false;
            }

            string ownerName = GetCitizenName(building.OwnerCitizenId);
            string plotName = _state.TryGetPlot(building.PlotId, out PlotState plot)
                ? plot.DisplayName
                : building.PlotId.ToString();
            string placeName = _state.TryGetPlace(building.InteriorPlaceId, out PlaceState place)
                ? place.DisplayName
                : building.InteriorPlaceId.ToString();
            AccessRule accessRule = plot != null ? plot.AccessRule : AccessRule.Private;

            summary = new BuildingPropertySummary(
                building.Id,
                building.DisplayName,
                building.OwnerCitizenId,
                ownerName,
                building.PlotId,
                plotName,
                building.InteriorPlaceId,
                placeName,
                accessRule,
                building.Kind);
            return true;
        }

        public bool TryGetPlotSummary(WorldEntityId plotId, out PlotPropertySummary summary)
        {
            summary = default;
            if (!_state.TryGetPlot(plotId, out PlotState plot)) {
                return false;
            }

            summary = new PlotPropertySummary(
                plot.Id,
                plot.DisplayName,
                plot.OwnerCitizenId,
                GetCitizenName(plot.OwnerCitizenId),
                plot.AccessRule,
                CountBuildingsOnPlot(plot.Id));
            return true;
        }

        public bool TryGetPlaceSummary(WorldEntityId placeId, out PlacePropertySummary summary)
        {
            summary = default;
            if (!_state.TryGetPlace(placeId, out PlaceState place)) {
                return false;
            }

            BuildingState building = FindBuildingForInteriorPlace(place.Id);
            WorldEntityId ownerId = building != null ? building.OwnerCitizenId : default;
            AccessRule accessRule = GetPlaceAccessRule(place, building);

            summary = new PlacePropertySummary(
                place.Id,
                place.DisplayName,
                place.SceneId,
                place.Kind,
                ownerId,
                building != null ? GetCitizenName(ownerId) : "Public",
                accessRule,
                building != null,
                building != null ? building.Id : default,
                building != null ? building.DisplayName : "No building");
            return true;
        }

        public bool TryFindPlotAt(WorldEntityId regionId, GridCoord coord, out PlotState plot)
        {
            foreach (PlotState candidate in _state.PlotsById.Values) {
                if (candidate.RegionId == regionId && candidate.Bounds.Contains(coord)) {
                    plot = candidate;
                    return true;
                }
            }

            plot = null;
            return false;
        }

        public PropertyAccessResult CheckBuildingAccess(WorldEntityId buildingId, WorldEntityId citizenId)
        {
            if (!_state.TryGetBuilding(buildingId, out BuildingState building)) {
                return new PropertyAccessResult(false, AccessRule.Private, $"Building not found: {buildingId}");
            }

            AccessRule accessRule = _state.TryGetPlot(building.PlotId, out PlotState plot)
                ? plot.AccessRule
                : AccessRule.Private;
            return CheckAccess(accessRule, citizenId, building.OwnerCitizenId, building.Id);
        }

        public PropertyAccessResult CheckPlotAccess(WorldEntityId plotId, WorldEntityId citizenId)
        {
            if (!_state.TryGetPlot(plotId, out PlotState plot)) {
                return new PropertyAccessResult(false, AccessRule.Private, $"Plot not found: {plotId}");
            }

            WorldEntityId workplaceBuildingId = FindFirstBuildingIdOnPlot(plot.Id);
            return CheckAccess(plot.AccessRule, citizenId, plot.OwnerCitizenId, workplaceBuildingId);
        }

        public PropertyAccessResult CheckPlaceAccess(WorldEntityId placeId, WorldEntityId citizenId)
        {
            if (!_state.TryGetPlace(placeId, out PlaceState place)) {
                return new PropertyAccessResult(false, AccessRule.Private, $"Place not found: {placeId}");
            }

            BuildingState building = FindBuildingForInteriorPlace(place.Id);
            if (building == null) {
                return new PropertyAccessResult(true, AccessRule.Public, "Public space.");
            }

            AccessRule accessRule = GetPlaceAccessRule(place, building);
            return CheckAccess(accessRule, citizenId, building.OwnerCitizenId, building.Id);
        }

        private PropertyAccessResult CheckAccess(AccessRule rule, WorldEntityId citizenId, WorldEntityId ownerId, WorldEntityId workplaceBuildingId)
        {
            if (rule == AccessRule.Public) {
                return new PropertyAccessResult(true, rule, "Public access.");
            }

            if (!_state.TryGetCitizen(citizenId, out CitizenState citizen)) {
                return new PropertyAccessResult(false, rule, $"Citizen not found: {citizenId}");
            }

            if (citizen.Id == ownerId) {
                return new PropertyAccessResult(true, rule, "Owner access.");
            }

            if (rule == AccessRule.EmployeesOnly && citizen.WorkplaceBuildingId == workplaceBuildingId) {
                return new PropertyAccessResult(true, rule, "Employee access.");
            }

            if (rule == AccessRule.Government && citizen.WorkplaceBuildingId == workplaceBuildingId) {
                return new PropertyAccessResult(true, rule, "Government workplace access.");
            }

            return new PropertyAccessResult(false, rule, $"{rule} access denied.");
        }

        private AccessRule GetPlaceAccessRule(PlaceState place, BuildingState building)
        {
            if (building == null) {
                return AccessRule.Public;
            }

            return _state.TryGetPlot(building.PlotId, out PlotState plot)
                ? plot.AccessRule
                : AccessRule.Private;
        }

        private BuildingState FindBuildingForInteriorPlace(WorldEntityId placeId)
        {
            foreach (BuildingState building in _state.BuildingsById.Values) {
                if (building.InteriorPlaceId == placeId) {
                    return building;
                }
            }

            return null;
        }

        private WorldEntityId FindFirstBuildingIdOnPlot(WorldEntityId plotId)
        {
            foreach (BuildingState building in _state.BuildingsById.Values) {
                if (building.PlotId == plotId) {
                    return building.Id;
                }
            }

            return default;
        }

        private int CountBuildingsOnPlot(WorldEntityId plotId)
        {
            int count = 0;
            foreach (BuildingState building in _state.BuildingsById.Values) {
                if (building.PlotId == plotId) {
                    count++;
                }
            }

            return count;
        }

        private string GetCitizenName(WorldEntityId citizenId)
        {
            return _state.TryGetCitizen(citizenId, out CitizenState citizen)
                ? citizen.DisplayName
                : citizenId.ToString();
        }
    }
}
