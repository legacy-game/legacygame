using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class RoleAssignmentState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId CitizenId { get; }
        public string RoleId { get; }
        public WorldEntityId WorkplacePlaceId { get; }
        public bool IsActive { get; private set; }

        public RoleAssignmentState(
            WorldEntityId id,
            WorldEntityId citizenId,
            string roleId,
            WorldEntityId workplacePlaceId,
            bool isActive = true)
        {
            if (string.IsNullOrWhiteSpace(roleId)) {
                throw new ArgumentException("Role id must not be empty.", nameof(roleId));
            }

            Id = id;
            CitizenId = citizenId;
            RoleId = roleId;
            WorkplacePlaceId = workplacePlaceId;
            IsActive = isActive;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
