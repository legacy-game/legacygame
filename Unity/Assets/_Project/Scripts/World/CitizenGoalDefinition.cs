using System;

namespace Legacy.World
{
    public readonly struct CitizenGoalDefinition
    {
        public CitizenGoalKind Kind { get; }
        public WorldEntityId TargetPlaceId { get; }
        public GridCoord TargetCoord { get; }
        public CitizenActivityState Activity { get; }
        public int Urgency { get; }
        public string Reason { get; }

        public CitizenGoalDefinition(
            CitizenGoalKind kind,
            WorldEntityId targetPlaceId,
            GridCoord targetCoord,
            CitizenActivityState activity,
            int urgency,
            string reason)
        {
            if (urgency < 0) {
                throw new ArgumentOutOfRangeException(nameof(urgency), "Urgency must not be negative.");
            }

            Kind = kind;
            TargetPlaceId = targetPlaceId;
            TargetCoord = targetCoord;
            Activity = activity;
            Urgency = urgency;
            Reason = string.IsNullOrWhiteSpace(reason) ? kind.ToString() : reason;
        }
    }
}
