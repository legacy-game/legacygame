using System;

namespace Legacy.World
{
    public readonly struct RoutineStepDefinition
    {
        public string Id { get; }
        public int StartMinute { get; }
        public int EndMinute { get; }
        public WorldEntityId TargetPlaceId { get; }
        public GridCoord TargetCoord { get; }
        public CitizenActivityState Activity { get; }
        public int Priority { get; }
        public bool IsInterruptible { get; }
        public string Intent { get; }
        public string ArrivalDescription { get; }
        public int Urgency { get; }

        public RoutineStepDefinition(
            string id,
            int startMinute,
            int endMinute,
            WorldEntityId targetPlaceId,
            GridCoord targetCoord,
            CitizenActivityState activity,
            int priority,
            bool isInterruptible,
            string intent,
            string arrivalDescription,
            int urgency = 25)
        {
            if (string.IsNullOrWhiteSpace(id)) {
                throw new ArgumentException("Routine step id must not be empty.", nameof(id));
            }

            if (startMinute < 0 || startMinute >= 1440) {
                throw new ArgumentOutOfRangeException(nameof(startMinute), "Start minute must be within the day.");
            }

            if (endMinute < 0 || endMinute > 1440) {
                throw new ArgumentOutOfRangeException(nameof(endMinute), "End minute must be within the day.");
            }

            Id = id;
            StartMinute = startMinute;
            EndMinute = endMinute;
            TargetPlaceId = targetPlaceId;
            TargetCoord = targetCoord;
            Activity = activity;
            Priority = priority;
            IsInterruptible = isInterruptible;
            Intent = string.IsNullOrWhiteSpace(intent) ? id : intent;
            ArrivalDescription = string.IsNullOrWhiteSpace(arrivalDescription) ? Intent : arrivalDescription;
            Urgency = urgency;
        }

        public CitizenGoalDefinition CreateGoal()
        {
            return new CitizenGoalDefinition(
                CitizenGoalKind.TravelToPlace,
                TargetPlaceId,
                TargetCoord,
                Activity,
                Urgency,
                Intent);
        }
    }
}
