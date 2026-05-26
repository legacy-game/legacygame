using System;
using System.Collections.Generic;

namespace Legacy.Save
{
    [Serializable]
    public sealed class WorkplaceInventorySaveData
    {
        public string workplaceId;
        public List<InventoryStackSaveData> stacks = new();
    }
}
