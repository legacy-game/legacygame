using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class PublicRecordState
    {
        public WorldEntityId CitizenId { get; }
        public int ReputationScore { get; private set; }
        public int ReportsFiled { get; private set; }
        public int ReportsReceived { get; private set; }
        public GameDateTime LastUpdatedAt { get; private set; }

        public PublicRecordState(
            WorldEntityId citizenId,
            int reputationScore,
            int reportsFiled,
            int reportsReceived,
            GameDateTime lastUpdatedAt)
        {
            CitizenId = citizenId;
            ReputationScore = reputationScore;
            ReportsFiled = reportsFiled;
            ReportsReceived = reportsReceived;
            LastUpdatedAt = lastUpdatedAt;
        }

        public void RecordReportFiled(GameDateTime timestamp)
        {
            ReportsFiled++;
            LastUpdatedAt = timestamp;
        }

        public void RecordReportReceived(int reputationImpact, GameDateTime timestamp)
        {
            ReportsReceived++;
            ReputationScore += reputationImpact;
            LastUpdatedAt = timestamp;
        }
    }
}
