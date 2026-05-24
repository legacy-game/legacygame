namespace Legacy.World
{
    public readonly struct TravelOption
    {
        public string ApproachId { get; }
        public string DisplayName { get; }
        public int Weight { get; }
        public int EstimatedMinutes { get; }
        public string Reason { get; }

        public TravelOption(string approachId, string displayName, int weight, int estimatedMinutes, string reason)
        {
            ApproachId = approachId ?? string.Empty;
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? ApproachId : displayName;
            Weight = weight;
            EstimatedMinutes = estimatedMinutes;
            Reason = string.IsNullOrWhiteSpace(reason) ? DisplayName : reason;
        }
    }
}
