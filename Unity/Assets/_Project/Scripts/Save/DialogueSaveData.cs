using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class DialogueSaveData
    {
        public string citizenId;
        public string lastLineId;
        public int conversationCount;
        public DateTimeSaveData lastTalkedAt;
    }
}
