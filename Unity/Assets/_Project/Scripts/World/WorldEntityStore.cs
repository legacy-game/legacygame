using System.Collections.Generic;

namespace Legacy.World
{
    public sealed class WorldEntityStore
    {
        private readonly Dictionary<WorldEntityId, RegionState> _regionsById = new();
        private readonly Dictionary<WorldEntityId, WorldSceneState> _scenesById = new();
        private readonly Dictionary<WorldEntityId, PlaceState> _placesById = new();
        private readonly Dictionary<WorldEntityId, CitizenState> _citizensById = new();
        private readonly Dictionary<WorldEntityId, PlotState> _plotsById = new();
        private readonly Dictionary<WorldEntityId, BuildingState> _buildingsById = new();

        public IReadOnlyDictionary<WorldEntityId, RegionState> RegionsById => _regionsById;
        public IReadOnlyDictionary<WorldEntityId, WorldSceneState> ScenesById => _scenesById;
        public IReadOnlyDictionary<WorldEntityId, PlaceState> PlacesById => _placesById;
        public IReadOnlyDictionary<WorldEntityId, CitizenState> CitizensById => _citizensById;
        public IReadOnlyDictionary<WorldEntityId, PlotState> PlotsById => _plotsById;
        public IReadOnlyDictionary<WorldEntityId, BuildingState> BuildingsById => _buildingsById;

        public void AddRegion(RegionState region)
        {
            _regionsById.Add(region.Id, region);
        }

        public void AddScene(WorldSceneState scene)
        {
            _scenesById.Add(scene.Id, scene);

            if (_regionsById.TryGetValue(scene.RegionId, out RegionState region)) {
                region.AddScene(scene.Id);
            }
        }

        public void AddPlace(PlaceState place)
        {
            _placesById.Add(place.Id, place);

            if (_regionsById.TryGetValue(place.RegionId, out RegionState region)) {
                region.AddPlace(place.Id);
            }
        }

        public void AddCitizen(CitizenState citizen)
        {
            _citizensById.Add(citizen.Id, citizen);

            if (_regionsById.TryGetValue(citizen.CurrentRegionId, out RegionState region)) {
                region.AddCitizen(citizen.Id);
            }
        }

        public void AddPlot(PlotState plot)
        {
            _plotsById.Add(plot.Id, plot);

            if (_regionsById.TryGetValue(plot.RegionId, out RegionState region)) {
                region.AddPlot(plot.Id);
            }
        }

        public void AddBuilding(BuildingState building)
        {
            _buildingsById.Add(building.Id, building);

            if (_regionsById.TryGetValue(building.RegionId, out RegionState region)) {
                region.AddBuilding(building.Id);
            }
        }

        public bool TryGetCitizen(WorldEntityId id, out CitizenState citizen)
        {
            return _citizensById.TryGetValue(id, out citizen);
        }

        public bool TryGetBuilding(WorldEntityId id, out BuildingState building)
        {
            return _buildingsById.TryGetValue(id, out building);
        }

        public bool TryGetScene(WorldEntityId id, out WorldSceneState scene)
        {
            return _scenesById.TryGetValue(id, out scene);
        }

        public bool TryGetPlace(WorldEntityId id, out PlaceState place)
        {
            return _placesById.TryGetValue(id, out place);
        }

        public bool TryGetPlot(WorldEntityId id, out PlotState plot)
        {
            return _plotsById.TryGetValue(id, out plot);
        }
    }
}
