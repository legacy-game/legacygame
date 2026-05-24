using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class RoleAssignmentSaveData
    {
        public string id;
        public string citizenId;
        public string roleId;
        public string workplacePlaceId;
        public bool isActive;
    }
}
