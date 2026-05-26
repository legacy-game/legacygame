using System;
using System.Collections.Generic;

namespace Legacy.Save
{
    [Serializable]
    public sealed class WorkplaceSaveData
    {
        public string id;
        public string buildingId;
        public string placeId;
        public string ownerCitizenId;
        public string businessAccountId;
        public string status;
        public int opensAtMinute;
        public int closesAtMinute;
        public List<string> activeShiftIds = new();
        public List<string> queuedTaskIds = new();
    }
}
