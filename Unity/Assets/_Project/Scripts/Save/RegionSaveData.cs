using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class RegionSaveData
    {
        public string id;
        public string displayName;
        public GridBoundsSaveData bounds;
    }
}
