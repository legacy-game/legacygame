using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class WorldSceneState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId RegionId { get; }
        public string DisplayName { get; }

        public WorldSceneState(WorldEntityId id, WorldEntityId regionId, string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName)) {
                throw new ArgumentException("Display name must not be empty.", nameof(displayName));
            }

            Id = id;
            RegionId = regionId;
            DisplayName = displayName;
        }
    }
}
