using System.Collections.Generic;
using Legacy.History;
using Legacy.Time;

namespace Legacy.World
{
    public sealed class WorldState
    {
        public const int MaxRecentHistoryEvents = HistoryStore.MaxRecentHistoryEvents;

        private readonly WorldEntityStore _entities = new();
        private readonly SceneEntityIndex _sceneIndex = new();
        private readonly WorldSpatialIndex _spatialIndex = new();
        private readonly HistoryStore _history;

        public int SchemaVersion { get; }
        public long WorldSeed { get; }
        public GameDateTime CurrentTime { get; private set; }
        public WorldEntityId CurrentSceneId { get; private set; }

        public IReadOnlyDictionary<WorldEntityId, RegionState> RegionsById => _entities.RegionsById;
        public IReadOnlyDictionary<WorldEntityId, WorldSceneState> ScenesById => _entities.ScenesById;
        public IReadOnlyDictionary<WorldEntityId, PlaceState> PlacesById => _entities.PlacesById;
        public IReadOnlyDictionary<WorldEntityId, CitizenState> CitizensById => _entities.CitizensById;
        public IReadOnlyDictionary<WorldEntityId, PlotState> PlotsById => _entities.PlotsById;
        public IReadOnlyDictionary<WorldEntityId, BuildingState> BuildingsById => _entities.BuildingsById;
        public IReadOnlyList<HistoryEvent> RecentHistory => _history.RecentHistory;
        public IHistoryArchive HistoryArchive => _history.Archive;

        public WorldState(int schemaVersion, long worldSeed, GameDateTime currentTime, WorldEntityId currentSceneId, IHistoryArchive historyArchive = null)
        {
            SchemaVersion = schemaVersion;
            WorldSeed = worldSeed;
            CurrentTime = currentTime;
            CurrentSceneId = currentSceneId;
            _history = new HistoryStore(historyArchive);
        }

        public void SetTime(GameDateTime time)
        {
            CurrentTime = time;
        }

        public void SetCurrentScene(WorldEntityId sceneId)
        {
            CurrentSceneId = sceneId;
        }

        public void AddRegion(RegionState region)
        {
            _entities.AddRegion(region);
        }

        public void AddScene(WorldSceneState scene)
        {
            _entities.AddScene(scene);
        }

        public void AddPlace(PlaceState place)
        {
            _entities.AddPlace(place);
        }

        public void AddCitizen(CitizenState citizen)
        {
            _entities.AddCitizen(citizen);
            _sceneIndex.AddCitizen(citizen);
            _spatialIndex.AddCitizen(citizen);
        }

        public void AddPlot(PlotState plot)
        {
            _entities.AddPlot(plot);
        }

        public void AddBuilding(BuildingState building)
        {
            _entities.AddBuilding(building);
            _sceneIndex.AddBuilding(building);
            _spatialIndex.AddBuilding(building);
        }

        public void MoveCitizen(WorldEntityId citizenId, WorldEntityId regionId, WorldEntityId sceneId, WorldEntityId placeId, GridCoord coord, CitizenActivityState activity)
        {
            if (!_entities.TryGetCitizen(citizenId, out CitizenState citizen)) {
                return;
            }

            _sceneIndex.MoveCitizen(citizen, sceneId);
            _spatialIndex.MoveCitizen(citizen, sceneId, coord);
            citizen.MoveTo(regionId, sceneId, placeId, coord, activity);
        }

        public IReadOnlyList<WorldEntityId> GetBuildingIdsInScene(WorldEntityId sceneId)
        {
            return _sceneIndex.GetBuildingIdsInScene(sceneId);
        }

        public IReadOnlyList<WorldEntityId> GetCitizenIdsInScene(WorldEntityId sceneId)
        {
            return _sceneIndex.GetCitizenIdsInScene(sceneId);
        }

        public IReadOnlyList<WorldEntityId> GetBuildingIdsInSceneChunk(WorldEntityId sceneId, GridChunkCoord chunk)
        {
            return _spatialIndex.GetBuildingIdsInSceneChunk(sceneId, chunk);
        }

        public IReadOnlyList<WorldEntityId> GetCitizenIdsInSceneChunk(WorldEntityId sceneId, GridChunkCoord chunk)
        {
            return _spatialIndex.GetCitizenIdsInSceneChunk(sceneId, chunk);
        }

        public List<WorldEntityId> GetCitizenIdsNear(WorldEntityId sceneId, GridCoord center, int chunkRadius = 1)
        {
            return _spatialIndex.GetCitizenIdsNear(sceneId, center, chunkRadius);
        }

        public void AddHistoryEvent(HistoryEvent historyEvent)
        {
            _history.Add(historyEvent);
        }

        public IReadOnlyList<HistoryEvent> GetHistoryForActor(WorldEntityId actorId)
        {
            return _history.GetForActor(actorId);
        }

        public IReadOnlyList<HistoryEvent> GetHistoryForPlace(WorldEntityId placeId)
        {
            return _history.GetForPlace(placeId);
        }

        public IReadOnlyList<HistoryEvent> GetHistoryByKind(HistoryEventKind kind)
        {
            return _history.GetByKind(kind);
        }

        public List<HistoryEvent> GetHistoryBetween(GameDateTime startInclusive, GameDateTime endInclusive)
        {
            return _history.GetBetween(startInclusive, endInclusive);
        }

        public bool TryGetCitizen(WorldEntityId id, out CitizenState citizen)
        {
            return _entities.TryGetCitizen(id, out citizen);
        }

        public bool TryGetBuilding(WorldEntityId id, out BuildingState building)
        {
            return _entities.TryGetBuilding(id, out building);
        }

        public bool TryGetScene(WorldEntityId id, out WorldSceneState scene)
        {
            return _entities.TryGetScene(id, out scene);
        }

        public bool TryGetPlace(WorldEntityId id, out PlaceState place)
        {
            return _entities.TryGetPlace(id, out place);
        }

        public bool TryGetPlot(WorldEntityId id, out PlotState plot)
        {
            return _entities.TryGetPlot(id, out plot);
        }
    }
}
