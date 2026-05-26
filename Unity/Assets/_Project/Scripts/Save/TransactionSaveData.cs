using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class TransactionSaveData
    {
        public string id;
        public DateTimeSaveData timestamp;
        public string payerAccountId;
        public string payeeAccountId;
        public int amountCents;
        public string reason;
        public string relatedPlaceId;
        public string actionKind;
    }
}
