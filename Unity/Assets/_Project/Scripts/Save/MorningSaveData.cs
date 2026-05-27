using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class MorningSaveData
    {
        public string status;
        public DateTimeSaveData startedAt;
        public DateTimeSaveData endedAt;
        public int tasksCompleted;
        public int moneyEarnedCents;
    }
}
