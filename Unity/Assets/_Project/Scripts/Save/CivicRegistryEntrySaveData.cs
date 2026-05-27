using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class CivicRegistryEntrySaveData
    {
        public string id;
        public string citizenId;
        public string kind;
        public string summary;
        public DateTimeSaveData createdAt;
        public string sourceReportId;
        public string filedByCitizenId;
        public string relatedPlaceId;
    }
}
