using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class ShiftSummarySaveData
    {
        public string shiftId;
        public string workerCitizenId;
        public string workplaceId;
        public DateTimeSaveData startedAt;
        public DateTimeSaveData endedAt;
        public int tasksCompleted;
        public int earnedCents;
        public int historyEventsAtEnd;
    }
}
