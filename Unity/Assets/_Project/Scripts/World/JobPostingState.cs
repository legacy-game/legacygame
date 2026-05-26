using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class JobPostingState
    {
        public WorldEntityId Id { get; }
        public string JobDefinitionId { get; }
        public WorldEntityId EmployerCitizenId { get; }
        public WorldEntityId WorkplaceId { get; }
        public string RoleId { get; }
        public PayModelKind PayModel { get; private set; }
        public int PayCents { get; private set; }
        public int OpenSlots { get; private set; }
        public JobPostingStatus Status { get; private set; }
        public GameDateTime CreatedAt { get; }

        public JobPostingState(
            WorldEntityId id,
            string jobDefinitionId,
            WorldEntityId employerCitizenId,
            WorldEntityId workplaceId,
            string roleId,
            PayModelKind payModel,
            int payCents,
            int openSlots,
            JobPostingStatus status,
            GameDateTime createdAt)
        {
            if (string.IsNullOrWhiteSpace(jobDefinitionId)) {
                throw new ArgumentException("Job definition id must not be empty.", nameof(jobDefinitionId));
            }

            Id = id;
            JobDefinitionId = jobDefinitionId;
            EmployerCitizenId = employerCitizenId;
            WorkplaceId = workplaceId;
            RoleId = roleId;
            PayModel = payModel;
            PayCents = payCents;
            OpenSlots = openSlots;
            Status = status;
            CreatedAt = createdAt;
        }

        public void FillSlot()
        {
            if (OpenSlots > 0) {
                OpenSlots--;
            }

            if (OpenSlots == 0) {
                Status = JobPostingStatus.Filled;
            }
        }

        public void Close()
        {
            Status = JobPostingStatus.Closed;
        }
    }
}
