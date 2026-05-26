using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class PerformanceRecordSaveData
    {
        public string id;
        public string citizenId;
        public string workplaceId;
        public string note;
        public int score;
        public DateTimeSaveData createdAt;
    }
}
