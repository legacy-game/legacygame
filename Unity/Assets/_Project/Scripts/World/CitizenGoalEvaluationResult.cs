namespace Legacy.World
{
    public readonly struct CitizenGoalEvaluationResult
    {
        public CitizenGoalEvaluationStatus Status { get; }
        public string Reason { get; }

        public CitizenGoalEvaluationResult(CitizenGoalEvaluationStatus status, string reason)
        {
            Status = status;
            Reason = string.IsNullOrWhiteSpace(reason) ? status.ToString() : reason;
        }

        public static CitizenGoalEvaluationResult CanAttempt(string reason = "Goal can be attempted.")
        {
            return new CitizenGoalEvaluationResult(CitizenGoalEvaluationStatus.CanAttempt, reason);
        }

        public static CitizenGoalEvaluationResult Wait(string reason)
        {
            return new CitizenGoalEvaluationResult(CitizenGoalEvaluationStatus.Wait, reason);
        }

        public static CitizenGoalEvaluationResult Fail(string reason)
        {
            return new CitizenGoalEvaluationResult(CitizenGoalEvaluationStatus.Fail, reason);
        }
    }
}
