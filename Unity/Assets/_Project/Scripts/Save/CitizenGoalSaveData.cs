using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class CitizenGoalSaveData
    {
        public string id;
        public string citizenId;
        public string kind;
        public string targetPlaceId;
        public GridSaveData targetCoord;
        public string activity;
        public int urgency;
        public string reason;
        public DateTimeSaveData createdAt;
        public DateTimeSaveData expiresAt;
        public string status;
    }
}
