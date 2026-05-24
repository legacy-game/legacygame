using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class CitizenRoutineState
    {
        public string RoutineId { get; private set; }
        public string ActiveStepId { get; private set; }
        public string CurrentIntent { get; private set; }
        public long LastProcessedAbsoluteMinute { get; private set; }

        public CitizenRoutineState(
            string routineId = "",
            string activeStepId = "",
            string currentIntent = "",
            long lastProcessedAbsoluteMinute = -1)
        {
            RoutineId = routineId ?? string.Empty;
            ActiveStepId = activeStepId ?? string.Empty;
            CurrentIntent = currentIntent ?? string.Empty;
            LastProcessedAbsoluteMinute = lastProcessedAbsoluteMinute;
        }

        public void SetRoutine(string routineId)
        {
            RoutineId = routineId ?? string.Empty;
            ActiveStepId = string.Empty;
            CurrentIntent = string.Empty;
            LastProcessedAbsoluteMinute = -1;
        }

        public void SetProgress(string activeStepId, string currentIntent, long lastProcessedAbsoluteMinute)
        {
            ActiveStepId = activeStepId ?? string.Empty;
            CurrentIntent = currentIntent ?? string.Empty;
            LastProcessedAbsoluteMinute = lastProcessedAbsoluteMinute;
        }

        public void SetIntent(string currentIntent, long lastProcessedAbsoluteMinute)
        {
            CurrentIntent = currentIntent ?? string.Empty;
            LastProcessedAbsoluteMinute = lastProcessedAbsoluteMinute;
        }
    }
}
