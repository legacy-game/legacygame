using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class PlaceState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId RegionId { get; }
        public WorldEntityId SceneId { get; }
        public string DisplayName { get; }
        public PlaceKind Kind { get; }

        public PlaceState(WorldEntityId id, WorldEntityId regionId, WorldEntityId sceneId, string displayName, PlaceKind kind)
        {
            if (string.IsNullOrWhiteSpace(displayName)) {
                throw new ArgumentException("Display name must not be empty.", nameof(displayName));
            }

            Id = id;
            RegionId = regionId;
            SceneId = sceneId;
            DisplayName = displayName;
            Kind = kind;
        }
    }
}
