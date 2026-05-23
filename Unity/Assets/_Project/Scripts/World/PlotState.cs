using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class PlotState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId RegionId { get; }
        public string DisplayName { get; private set; }
        public GridBounds Bounds { get; private set; }
        public WorldEntityId OwnerCitizenId { get; private set; }
        public AccessRule AccessRule { get; private set; }

        public PlotState(
            WorldEntityId id,
            WorldEntityId regionId,
            string displayName,
            GridBounds bounds,
            WorldEntityId ownerCitizenId,
            AccessRule accessRule)
        {
            if (string.IsNullOrWhiteSpace(displayName)) {
                throw new ArgumentException("Display name must not be empty.", nameof(displayName));
            }

            Id = id;
            RegionId = regionId;
            DisplayName = displayName;
            Bounds = bounds;
            OwnerCitizenId = ownerCitizenId;
            AccessRule = accessRule;
        }

        public void TransferOwnership(WorldEntityId newOwnerCitizenId)
        {
            OwnerCitizenId = newOwnerCitizenId;
        }
    }
}
