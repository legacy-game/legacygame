using System;
using System.Collections.Generic;

namespace Legacy.Save
{
    [Serializable]
    public sealed class CashDrawerSaveData
    {
        public string id;
        public string ownerEntityId;
        public string kind;
        public string displayName;
        public List<CashDenominationStackSaveData> stacks = new();
    }
}
