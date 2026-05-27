using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class DialogueState
    {
        public WorldEntityId CitizenId { get; }
        public string LastLineId { get; private set; }
        public int ConversationCount { get; private set; }
        public GameDateTime LastTalkedAt { get; private set; }

        public DialogueState(
            WorldEntityId citizenId,
            string lastLineId = "",
            int conversationCount = 0,
            GameDateTime lastTalkedAt = default)
        {
            CitizenId = citizenId;
            LastLineId = lastLineId ?? string.Empty;
            ConversationCount = conversationCount;
            LastTalkedAt = lastTalkedAt;
        }

        public void RecordTalk(string lineId, GameDateTime talkedAt)
        {
            LastLineId = lineId ?? string.Empty;
            ConversationCount++;
            LastTalkedAt = talkedAt;
        }
    }
}
