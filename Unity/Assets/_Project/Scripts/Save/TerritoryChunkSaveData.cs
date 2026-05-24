using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class TerritoryChunkSaveData
    {
        public string id;
        public string regionId;
        public int chunkX;
        public int chunkY;
        public string displayName;
        public string biome;
        public string claimStatus;
        public string claimOwnerId;
        public string settlementId;
        public string jurisdictionId;
        public bool isBuildable;
        public bool isDiscovered;
    }
}
