using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class CitizenMemorySaveData
    {
        public string id;
        public string citizenId;
        public string subjectCitizenId;
        public string kind;
        public string summary;
        public DateTimeSaveData createdAt;
        public string sourceHistoryEventId;
        public int salience;
    }
}
