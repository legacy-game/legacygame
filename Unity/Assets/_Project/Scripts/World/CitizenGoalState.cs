using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class CitizenGoalState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId CitizenId { get; }
        public CitizenGoalKind Kind { get; }
        public WorldEntityId TargetPlaceId { get; }
        public GridCoord TargetCoord { get; }
        public CitizenActivityState Activity { get; }
        public int Urgency { get; }
        public string Reason { get; }
        public GameDateTime CreatedAt { get; }
        public GameDateTime ExpiresAt { get; }
        public CitizenGoalStatus Status { get; private set; }

        public CitizenGoalState(
            WorldEntityId id,
            WorldEntityId citizenId,
            CitizenGoalDefinition definition,
            GameDateTime createdAt,
            GameDateTime expiresAt,
            CitizenGoalStatus status = CitizenGoalStatus.Active)
        {
            Id = id;
            CitizenId = citizenId;
            Kind = definition.Kind;
            TargetPlaceId = definition.TargetPlaceId;
            TargetCoord = definition.TargetCoord;
            Activity = definition.Activity;
            Urgency = definition.Urgency;
            Reason = definition.Reason;
            CreatedAt = createdAt;
            ExpiresAt = expiresAt;
            Status = status;
        }

        public CitizenGoalDefinition ToDefinition()
        {
            return new CitizenGoalDefinition(Kind, TargetPlaceId, TargetCoord, Activity, Urgency, Reason);
        }

        public void Complete()
        {
            Status = CitizenGoalStatus.Completed;
        }

        public void Fail()
        {
            Status = CitizenGoalStatus.Failed;
        }

        public void Expire()
        {
            Status = CitizenGoalStatus.Expired;
        }
    }
}
