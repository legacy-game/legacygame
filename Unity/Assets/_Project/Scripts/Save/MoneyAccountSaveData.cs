using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class MoneyAccountSaveData
    {
        public string id;
        public string ownerEntityId;
        public string displayName;
        public int balanceCents;
    }
}
