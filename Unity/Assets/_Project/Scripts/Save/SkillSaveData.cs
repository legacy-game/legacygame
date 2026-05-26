using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class SkillSaveData
    {
        public string citizenId;
        public string skill;
        public int experience;
    }
}
