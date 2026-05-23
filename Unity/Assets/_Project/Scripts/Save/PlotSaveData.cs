using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class PlotSaveData
    {
        public string id;
        public string regionId;
        public string displayName;
        public GridBoundsSaveData bounds;
        public string ownerCitizenId;
        public string accessRule;
    }
}
