using System;
using System.Collections.Generic;

namespace Legacy.Save
{
    [Serializable]
    public sealed class WorldSaveData
    {
        public int schemaVersion;
        public long worldSeed;
        public DateTimeSaveData currentTime;
        public string currentSceneId;
        public List<RegionSaveData> regions = new();
        public List<WorldSceneSaveData> scenes = new();
        public List<PlaceSaveData> places = new();
        public List<CitizenSaveData> citizens = new();
        public List<PlotSaveData> plots = new();
        public List<BuildingSaveData> buildings = new();
        public List<HistoryEventSaveData> recentHistory = new();
        public List<HistoryEventSaveData> archivedHistory = new();
    }
}
