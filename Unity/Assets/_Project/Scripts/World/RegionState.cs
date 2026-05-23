using System;
using System.Collections.Generic;

namespace Legacy.World
{
    [Serializable]
    public sealed class RegionState
    {
        private readonly HashSet<WorldEntityId> _citizenIds = new();
        private readonly HashSet<WorldEntityId> _plotIds = new();
        private readonly HashSet<WorldEntityId> _buildingIds = new();
        private readonly HashSet<WorldEntityId> _sceneIds = new();
        private readonly HashSet<WorldEntityId> _placeIds = new();

        public WorldEntityId Id { get; }
        public string DisplayName { get; private set; }
        public GridBounds Bounds { get; private set; }

        public IReadOnlyCollection<WorldEntityId> CitizenIds => _citizenIds;
        public IReadOnlyCollection<WorldEntityId> PlotIds => _plotIds;
        public IReadOnlyCollection<WorldEntityId> BuildingIds => _buildingIds;
        public IReadOnlyCollection<WorldEntityId> SceneIds => _sceneIds;
        public IReadOnlyCollection<WorldEntityId> PlaceIds => _placeIds;

        public RegionState(WorldEntityId id, string displayName, GridBounds bounds)
        {
            if (string.IsNullOrWhiteSpace(displayName)) {
                throw new ArgumentException("Display name must not be empty.", nameof(displayName));
            }

            Id = id;
            DisplayName = displayName;
            Bounds = bounds;
        }

        public void AddCitizen(WorldEntityId citizenId)
        {
            _citizenIds.Add(citizenId);
        }

        public void AddPlot(WorldEntityId plotId)
        {
            _plotIds.Add(plotId);
        }

        public void AddBuilding(WorldEntityId buildingId)
        {
            _buildingIds.Add(buildingId);
        }

        public void AddScene(WorldEntityId sceneId)
        {
            _sceneIds.Add(sceneId);
        }

        public void AddPlace(WorldEntityId placeId)
        {
            _placeIds.Add(placeId);
        }
    }
}
