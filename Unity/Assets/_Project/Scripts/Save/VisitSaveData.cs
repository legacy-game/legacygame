using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class VisitSaveData
    {
        public string id;
        public string visitorCitizenId;
        public string workplaceId;
        public string placeId;
        public string intent;
        public string requestedTaskDefinitionId;
        public string linkedTaskId;
        public string status;
        public string cafeStage;
        public string recipeId;
        public string preparedItemId;
        public int priceCents;
        public int prepQuality;
        public int tenderedCents;
        public DateTimeSaveData arrivalTime;
        public DateTimeSaveData departureTime;
        public string arrivalLine;
        public string completionLine;
    }
}
