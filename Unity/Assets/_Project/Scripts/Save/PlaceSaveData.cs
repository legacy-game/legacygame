using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class PlaceSaveData
    {
        public string id;
        public string regionId;
        public string sceneId;
        public string displayName;
        public string kind;
    }
}
