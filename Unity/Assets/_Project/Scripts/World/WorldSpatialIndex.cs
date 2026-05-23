using System;
using System.Collections.Generic;

namespace Legacy.World
{
    public sealed class WorldSpatialIndex
    {
        private readonly Dictionary<WorldEntityId, Dictionary<GridChunkCoord, List<WorldEntityId>>> _buildingIdsBySceneChunk = new();
        private readonly Dictionary<WorldEntityId, Dictionary<GridChunkCoord, List<WorldEntityId>>> _citizenIdsBySceneChunk = new();

        public void AddBuilding(BuildingState building)
        {
            AddToSceneChunk(_buildingIdsBySceneChunk, building.ExteriorSceneId, building.EntranceCoord, building.Id);
            AddToSceneChunk(_buildingIdsBySceneChunk, building.InteriorSceneId, new GridCoord(0, 0), building.Id);
        }

        public void AddCitizen(CitizenState citizen)
        {
            AddToSceneChunk(_citizenIdsBySceneChunk, citizen.CurrentSceneId, citizen.CurrentCoord, citizen.Id);
        }

        public void MoveCitizen(CitizenState citizen, WorldEntityId newSceneId, GridCoord newCoord)
        {
            RemoveFromSceneChunk(_citizenIdsBySceneChunk, citizen.CurrentSceneId, citizen.CurrentCoord, citizen.Id);
            AddToSceneChunk(_citizenIdsBySceneChunk, newSceneId, newCoord, citizen.Id);
        }

        public IReadOnlyList<WorldEntityId> GetBuildingIdsInSceneChunk(WorldEntityId sceneId, GridChunkCoord chunk)
        {
            return TryGetSceneChunk(_buildingIdsBySceneChunk, sceneId, chunk, out List<WorldEntityId> ids)
                ? ids
                : Array.Empty<WorldEntityId>();
        }

        public IReadOnlyList<WorldEntityId> GetCitizenIdsInSceneChunk(WorldEntityId sceneId, GridChunkCoord chunk)
        {
            return TryGetSceneChunk(_citizenIdsBySceneChunk, sceneId, chunk, out List<WorldEntityId> ids)
                ? ids
                : Array.Empty<WorldEntityId>();
        }

        public List<WorldEntityId> GetCitizenIdsNear(WorldEntityId sceneId, GridCoord center, int chunkRadius)
        {
            GridChunkCoord centerChunk = GridChunkCoord.FromGridCoord(center);
            var result = new List<WorldEntityId>();

            for (int y = centerChunk.Y - chunkRadius; y <= centerChunk.Y + chunkRadius; y++) {
                for (int x = centerChunk.X - chunkRadius; x <= centerChunk.X + chunkRadius; x++) {
                    result.AddRange(GetCitizenIdsInSceneChunk(sceneId, new GridChunkCoord(x, y)));
                }
            }

            return result;
        }

        private static void AddToSceneChunk(
            Dictionary<WorldEntityId, Dictionary<GridChunkCoord, List<WorldEntityId>>> index,
            WorldEntityId sceneId,
            GridCoord coord,
            WorldEntityId entityId)
        {
            if (!index.TryGetValue(sceneId, out Dictionary<GridChunkCoord, List<WorldEntityId>> sceneIndex)) {
                sceneIndex = new Dictionary<GridChunkCoord, List<WorldEntityId>>();
                index.Add(sceneId, sceneIndex);
            }

            GridChunkCoord chunk = GridChunkCoord.FromGridCoord(coord);
            if (!sceneIndex.TryGetValue(chunk, out List<WorldEntityId> ids)) {
                ids = new List<WorldEntityId>();
                sceneIndex.Add(chunk, ids);
            }

            if (!ids.Contains(entityId)) {
                ids.Add(entityId);
            }
        }

        private static void RemoveFromSceneChunk(
            Dictionary<WorldEntityId, Dictionary<GridChunkCoord, List<WorldEntityId>>> index,
            WorldEntityId sceneId,
            GridCoord coord,
            WorldEntityId entityId)
        {
            if (TryGetSceneChunk(index, sceneId, GridChunkCoord.FromGridCoord(coord), out List<WorldEntityId> ids)) {
                ids.Remove(entityId);
            }
        }

        private static bool TryGetSceneChunk(
            Dictionary<WorldEntityId, Dictionary<GridChunkCoord, List<WorldEntityId>>> index,
            WorldEntityId sceneId,
            GridChunkCoord chunk,
            out List<WorldEntityId> ids)
        {
            ids = null;
            return index.TryGetValue(sceneId, out Dictionary<GridChunkCoord, List<WorldEntityId>> sceneIndex)
                && sceneIndex.TryGetValue(chunk, out ids);
        }
    }
}
