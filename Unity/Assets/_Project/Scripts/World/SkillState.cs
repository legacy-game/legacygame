using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class SkillState
    {
        public WorldEntityId CitizenId { get; }
        public SkillKind Skill { get; }
        public int Experience { get; private set; }

        public SkillState(WorldEntityId citizenId, SkillKind skill, int experience)
        {
            CitizenId = citizenId;
            Skill = skill;
            Experience = experience;
        }

        public void AddExperience(int amount)
        {
            if (amount > 0) {
                Experience += amount;
            }
        }
    }
}
