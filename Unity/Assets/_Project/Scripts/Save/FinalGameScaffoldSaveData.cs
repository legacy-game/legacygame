using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class FinalGameScaffoldSaveData
    {
        public string id;
        public string kind;
        public string displayName;
        public string summary;
        public string implementationNotes;
        public DateTimeSaveData createdAt;
        public bool isGameplayEnabled;
    }
}
