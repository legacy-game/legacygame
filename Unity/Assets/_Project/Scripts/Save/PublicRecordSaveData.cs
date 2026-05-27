using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class PublicRecordSaveData
    {
        public string citizenId;
        public int reputationScore;
        public int reportsFiled;
        public int reportsReceived;
        public DateTimeSaveData lastUpdatedAt;
    }
}
