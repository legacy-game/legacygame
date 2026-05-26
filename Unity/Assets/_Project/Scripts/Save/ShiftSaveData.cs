using System;
using System.Collections.Generic;

namespace Legacy.Save
{
    [Serializable]
    public sealed class ShiftSaveData
    {
        public string id;
        public string contractId;
        public string workerCitizenId;
        public string workplaceId;
        public DateTimeSaveData startedAt;
        public DateTimeSaveData expectedEndAt;
        public DateTimeSaveData endedAt;
        public string status;
        public int earnedCents;
        public List<string> completedTaskIds = new();
    }
}
