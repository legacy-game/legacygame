using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class CivicReportSaveData
    {
        public string id;
        public string reporterCitizenId;
        public string subjectCitizenId;
        public string relatedPlaceId;
        public string summary;
        public DateTimeSaveData createdAt;
        public string status;
    }
}
