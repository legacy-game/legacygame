namespace Legacy.World
{
    public readonly struct TravelPlan
    {
        public CitizenGoalDefinition Goal { get; }
        public string ApproachId { get; }
        public string DisplayName { get; }
        public int EstimatedMinutes { get; }
        public string Reason { get; }

        public TravelPlan(CitizenGoalDefinition goal, string approachId, string displayName, int estimatedMinutes, string reason)
        {
            Goal = goal;
            ApproachId = approachId ?? string.Empty;
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? ApproachId : displayName;
            EstimatedMinutes = estimatedMinutes;
            Reason = string.IsNullOrWhiteSpace(reason) ? DisplayName : reason;
        }
    }
}
