using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class CitizenSaveData
    {
        public string id;
        public string displayName;
        public string homeBuildingId;
        public string workplaceBuildingId;
        public string currentRegionId;
        public string currentSceneId;
        public string currentPlaceId;
        public GridSaveData currentCoord;
        public string activity;
        public int scheduleStage;
        public string routineId;
        public string activeRoutineStepId;
        public string currentIntent;
        public long lastRoutineAbsoluteMinute;
    }
}
