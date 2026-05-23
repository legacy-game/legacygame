using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class BuildingSaveData
    {
        public string id;
        public string regionId;
        public string plotId;
        public string exteriorSceneId;
        public string interiorSceneId;
        public string interiorPlaceId;
        public string displayName;
        public string ownerCitizenId;
        public string kind;
        public GridSaveData entranceCoord;
    }
}
