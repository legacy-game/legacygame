using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class CitizenRegistrationSaveData
    {
        public string citizenId;
        public DateTimeSaveData registeredAt;
        public string startingResidencePlaceId;
    }
}
