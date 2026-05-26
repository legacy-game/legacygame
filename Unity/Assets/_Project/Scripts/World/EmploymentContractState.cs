using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class EmploymentContractState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId PostingId { get; }
        public WorldEntityId EmployerCitizenId { get; }
        public WorldEntityId WorkerCitizenId { get; }
        public WorldEntityId WorkplaceId { get; }
        public string JobDefinitionId { get; }
        public string RoleId { get; }
        public PayModelKind PayModel { get; private set; }
        public int PayCents { get; private set; }
        public EmploymentContractStatus Status { get; private set; }
        public GameDateTime StartedAt { get; }

        public EmploymentContractState(
            WorldEntityId id,
            WorldEntityId postingId,
            WorldEntityId employerCitizenId,
            WorldEntityId workerCitizenId,
            WorldEntityId workplaceId,
            string jobDefinitionId,
            string roleId,
            PayModelKind payModel,
            int payCents,
            EmploymentContractStatus status,
            GameDateTime startedAt)
        {
            Id = id;
            PostingId = postingId;
            EmployerCitizenId = employerCitizenId;
            WorkerCitizenId = workerCitizenId;
            WorkplaceId = workplaceId;
            JobDefinitionId = jobDefinitionId;
            RoleId = roleId;
            PayModel = payModel;
            PayCents = payCents;
            Status = status;
            StartedAt = startedAt;
        }

        public void Suspend()
        {
            Status = EmploymentContractStatus.Suspended;
        }

        public void Quit()
        {
            Status = EmploymentContractStatus.Quit;
        }

        public void Fire()
        {
            Status = EmploymentContractStatus.Fired;
        }
    }
}
