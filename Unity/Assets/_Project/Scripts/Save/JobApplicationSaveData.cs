using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class JobApplicationSaveData
    {
        public string id;
        public string postingId;
        public string applicantCitizenId;
        public string status;
        public DateTimeSaveData createdAt;
    }
}
