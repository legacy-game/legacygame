using System;

namespace Legacy.World
{
    public readonly struct TravelApproachDefinition
    {
        public string Id { get; }
        public string DisplayName { get; }
        public int BaseWeight { get; }
        public int DistanceWeightModifier { get; }
        public int UrgencyWeightModifier { get; }
        public int BaseMinutes { get; }
        public int MinutesPerTile { get; }
        public bool RequiresDifferentSceneOrLongDistance { get; }
        public int MinimumDistance { get; }
        public bool RequiresNearbyCitizen { get; }
        public string Reason { get; }

        public TravelApproachDefinition(
            string id,
            string displayName,
            int baseWeight,
            int distanceWeightModifier,
            int urgencyWeightModifier,
            int baseMinutes,
            int minutesPerTile,
            bool requiresDifferentSceneOrLongDistance,
            int minimumDistance,
            bool requiresNearbyCitizen,
            string reason)
        {
            if (string.IsNullOrWhiteSpace(id)) {
                throw new ArgumentException("Travel approach id must not be empty.", nameof(id));
            }

            Id = id;
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? id : displayName;
            BaseWeight = baseWeight;
            DistanceWeightModifier = distanceWeightModifier;
            UrgencyWeightModifier = urgencyWeightModifier;
            BaseMinutes = baseMinutes;
            MinutesPerTile = minutesPerTile;
            RequiresDifferentSceneOrLongDistance = requiresDifferentSceneOrLongDistance;
            MinimumDistance = minimumDistance;
            RequiresNearbyCitizen = requiresNearbyCitizen;
            Reason = string.IsNullOrWhiteSpace(reason) ? DisplayName : reason;
        }
    }
}
