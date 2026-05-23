using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class GridBoundsSaveData
    {
        public GridSaveData min;
        public int width;
        public int height;
    }
}
