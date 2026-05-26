using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class JobTaskSaveData
    {
        public string id;
        public string definitionId;
        public string workplaceId;
        public string assignedWorkerId;
        public string shiftId;
        public string targetEntityId;
        public string status;
        public int quality;
        public DateTimeSaveData createdAt;
        public DateTimeSaveData startedAt;
        public DateTimeSaveData completedAt;
        public bool hasMiniGameResult;
        public int miniGameScore;
        public int miniGameMaxScore;
        public int miniGameQuality;
        public int miniGameDurationSeconds;
        public int miniGameMistakes;
    }
}
