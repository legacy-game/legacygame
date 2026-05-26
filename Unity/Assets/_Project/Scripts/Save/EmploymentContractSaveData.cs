using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class EmploymentContractSaveData
    {
        public string id;
        public string postingId;
        public string employerCitizenId;
        public string workerCitizenId;
        public string workplaceId;
        public string jobDefinitionId;
        public string roleId;
        public string payModel;
        public int payCents;
        public string status;
        public DateTimeSaveData startedAt;
    }
}
