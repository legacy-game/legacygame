using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class CitizenRegistrationState
    {
        public WorldEntityId CitizenId { get; }
        public GameDateTime RegisteredAt { get; }
        public WorldEntityId StartingResidencePlaceId { get; }

        public CitizenRegistrationState(
            WorldEntityId citizenId,
            GameDateTime registeredAt,
            WorldEntityId startingResidencePlaceId)
        {
            CitizenId = citizenId;
            RegisteredAt = registeredAt;
            StartingResidencePlaceId = startingResidencePlaceId;
        }
    }
}
