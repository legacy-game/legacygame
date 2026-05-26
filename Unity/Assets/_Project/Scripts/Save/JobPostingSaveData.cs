using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class JobPostingSaveData
    {
        public string id;
        public string jobDefinitionId;
        public string employerCitizenId;
        public string workplaceId;
        public string roleId;
        public string payModel;
        public int payCents;
        public int openSlots;
        public string status;
        public DateTimeSaveData createdAt;
    }
}
