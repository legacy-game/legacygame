using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class JobDefinition
    {
        public string Id { get; }
        public string DisplayName { get; }
        public string RoleId { get; }
        public PayModelKind DefaultPayModel { get; }
        public int DefaultPayCents { get; }
        public SkillKind PrimarySkill { get; }

        public JobDefinition(string id, string displayName, string roleId, PayModelKind defaultPayModel, int defaultPayCents, SkillKind primarySkill)
        {
            if (string.IsNullOrWhiteSpace(id)) {
                throw new ArgumentException("Job id must not be empty.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(displayName)) {
                throw new ArgumentException("Display name must not be empty.", nameof(displayName));
            }

            Id = id;
            DisplayName = displayName;
            RoleId = roleId;
            DefaultPayModel = defaultPayModel;
            DefaultPayCents = defaultPayCents;
            PrimarySkill = primarySkill;
        }
    }
}
