using System;
using System.Collections.Generic;

namespace Legacy.Save
{
    [Serializable]
    public sealed class HistoryEventSaveData
    {
        public string id;
        public DateTimeSaveData timestamp;
        public string kind;
        public string description;
        public List<string> actorIds = new();
        public List<string> placeIds = new();
    }
}
