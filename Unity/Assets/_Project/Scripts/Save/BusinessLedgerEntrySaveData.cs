using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class BusinessLedgerEntrySaveData
    {
        public string id;
        public string workplaceId;
        public string accountId;
        public string kind;
        public int amountCents;
        public string reason;
        public DateTimeSaveData timestamp;
        public string relatedEntityId;
    }
}
