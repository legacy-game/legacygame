using System;
using System.Collections.Generic;

namespace Legacy.World
{
    public sealed class SceneEntityIndex
    {
        private readonly Dictionary<WorldEntityId, List<WorldEntityId>> _buildingIdsBySceneId = new();
        private readonly Dictionary<WorldEntityId, List<WorldEntityId>> _citizenIdsBySceneId = new();

        public void AddBuilding(BuildingState building)
        {
            AddToIndex(_buildingIdsBySceneId, building.ExteriorSceneId, building.Id);
            AddToIndex(_buildingIdsBySceneId, building.InteriorSceneId, building.Id);
        }

        public void AddCitizen(CitizenState citizen)
        {
            AddToIndex(_citizenIdsBySceneId, citizen.CurrentSceneId, citizen.Id);
        }

        public void MoveCitizen(CitizenState citizen, WorldEntityId newSceneId)
        {
            RemoveFromIndex(_citizenIdsBySceneId, citizen.CurrentSceneId, citizen.Id);
            AddToIndex(_citizenIdsBySceneId, newSceneId, citizen.Id);
        }

        public IReadOnlyList<WorldEntityId> GetBuildingIdsInScene(WorldEntityId sceneId)
        {
            return _buildingIdsBySceneId.TryGetValue(sceneId, out List<WorldEntityId> ids)
                ? ids
                : Array.Empty<WorldEntityId>();
        }

        public IReadOnlyList<WorldEntityId> GetCitizenIdsInScene(WorldEntityId sceneId)
        {
            return _citizenIdsBySceneId.TryGetValue(sceneId, out List<WorldEntityId> ids)
                ? ids
                : Array.Empty<WorldEntityId>();
        }

        private static void AddToIndex(Dictionary<WorldEntityId, List<WorldEntityId>> index, WorldEntityId sceneId, WorldEntityId entityId)
        {
            if (!index.TryGetValue(sceneId, out List<WorldEntityId> ids)) {
                ids = new List<WorldEntityId>();
                index.Add(sceneId, ids);
            }

            if (!ids.Contains(entityId)) {
                ids.Add(entityId);
            }
        }

        private static void RemoveFromIndex(Dictionary<WorldEntityId, List<WorldEntityId>> index, WorldEntityId sceneId, WorldEntityId entityId)
        {
            if (index.TryGetValue(sceneId, out List<WorldEntityId> ids)) {
                ids.Remove(entityId);
            }
        }
    }
}
