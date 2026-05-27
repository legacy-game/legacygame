using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class RelationshipSaveData
    {
        public string ownerCitizenId;
        public string otherCitizenId;
        public int affinity;
        public int familiarity;
        public DateTimeSaveData lastInteractionAt;
    }
}
