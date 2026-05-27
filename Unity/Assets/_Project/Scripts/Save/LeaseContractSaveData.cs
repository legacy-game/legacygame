using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class LeaseContractSaveData
    {
        public string id;
        public string buildingId;
        public string landlordCitizenId;
        public string tenantCitizenId;
        public int rentCents;
        public int dueDayOfMonth;
        public string status;
        public DateTimeSaveData startedAt;
        public DateTimeSaveData lastPaidAt;
        public int paymentsMade;
    }
}
