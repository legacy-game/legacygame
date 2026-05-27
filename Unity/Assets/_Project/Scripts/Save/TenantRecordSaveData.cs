using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class TenantRecordSaveData
    {
        public string id;
        public string leaseContractId;
        public string buildingId;
        public string tenantCitizenId;
        public string status;
        public DateTimeSaveData startedAt;
        public DateTimeSaveData endedAt;
    }
}
