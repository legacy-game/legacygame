using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class JobApplicationState
    {
        public WorldEntityId Id { get; }
        public WorldEntityId PostingId { get; }
        public WorldEntityId ApplicantCitizenId { get; }
        public JobApplicationStatus Status { get; private set; }
        public GameDateTime CreatedAt { get; }

        public JobApplicationState(WorldEntityId id, WorldEntityId postingId, WorldEntityId applicantCitizenId, JobApplicationStatus status, GameDateTime createdAt)
        {
            Id = id;
            PostingId = postingId;
            ApplicantCitizenId = applicantCitizenId;
            Status = status;
            CreatedAt = createdAt;
        }

        public void Offer()
        {
            Status = JobApplicationStatus.Offered;
        }

        public void Accept()
        {
            Status = JobApplicationStatus.Accepted;
        }

        public void Reject()
        {
            Status = JobApplicationStatus.Rejected;
        }

        public void Withdraw()
        {
            Status = JobApplicationStatus.Withdrawn;
        }
    }
}
