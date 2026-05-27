using System;
using System.Collections.Generic;

namespace Legacy.Save
{
    [Serializable]
    public sealed class InventoryContainerSaveData
    {
        public string id;
        public string ownerEntityId;
        public string kind;
        public string displayName;
        public List<InventoryStackSaveData> stacks = new();
    }
}
