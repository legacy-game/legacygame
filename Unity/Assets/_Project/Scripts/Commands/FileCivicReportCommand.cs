using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class FileCivicReportCommand : IWorldCommand
    {
        private readonly WorldEntityId _reportId;
        private readonly WorldEntityId _reporterCitizenId;
        private readonly WorldEntityId _subjectCitizenId;
        private readonly WorldEntityId _relatedPlaceId;
        private readonly string _summary;
        private readonly int _reputationImpact;

        public FileCivicReportCommand(
            WorldEntityId reportId,
            WorldEntityId reporterCitizenId,
            WorldEntityId subjectCitizenId,
            WorldEntityId relatedPlaceId,
            string summary,
            int reputationImpact = -1)
        {
            _reportId = reportId;
            _reporterCitizenId = reporterCitizenId;
            _subjectCitizenId = subjectCitizenId;
            _relatedPlaceId = relatedPlaceId;
            _summary = summary ?? string.Empty;
            _reputationImpact = reputationImpact;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (context.State.TryGetCivicReport(_reportId, out CivicReportState _)) {
                return WorldCommandResult.Failure($"Civic report already exists: {_reportId}");
            }

            if (!context.State.TryGetCitizen(_reporterCitizenId, out CitizenState reporter)) {
                return WorldCommandResult.Failure($"Reporter not found: {_reporterCitizenId}");
            }

            if (!context.State.TryGetCitizen(_subjectCitizenId, out CitizenState subject)) {
                return WorldCommandResult.Failure($"Report subject not found: {_subjectCitizenId}");
            }

            if (string.IsNullOrWhiteSpace(_summary)) {
                return WorldCommandResult.Failure("Civic report summary must not be empty.");
            }

            if (HasId(_relatedPlaceId) && !context.State.TryGetPlace(_relatedPlaceId, out PlaceState _)) {
                return WorldCommandResult.Failure($"Related place not found: {_relatedPlaceId}");
            }

            int reputationImpact = Clamp(_reputationImpact, -5, 5);
            var report = new CivicReportState(
                _reportId,
                reporter.Id,
                subject.Id,
                _relatedPlaceId,
                _summary.Trim(),
                context.State.CurrentTime);
            context.State.AddCivicReport(report);

            PublicRecordState subjectRecord = context.State.GetOrCreatePublicRecord(subject.Id, context.State.CurrentTime);
            subjectRecord.RecordReportReceived(reputationImpact, context.State.CurrentTime);

            PublicRecordState reporterRecord = context.State.GetOrCreatePublicRecord(reporter.Id, context.State.CurrentTime);
            reporterRecord.RecordReportFiled(context.State.CurrentTime);

            var entry = new CivicRegistryEntryState(
                new WorldEntityId($"civic_entry_{_reportId.Value}"),
                subject.Id,
                CivicRegistryEntryKind.CivicReport,
                report.Summary,
                context.State.CurrentTime,
                report.Id,
                reporter.Id,
                _relatedPlaceId);
            context.State.AddCivicRegistryEntry(entry);

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.CivicReportFiled,
                $"{reporter.DisplayName} filed a civic report about {subject.DisplayName}: {report.Summary}",
                new[] { reporter.Id, subject.Id },
                HasId(_relatedPlaceId) ? new[] { _relatedPlaceId } : null);

            WorldCommandResult result = WorldCommandResult
                .Success($"Civic report filed for {subject.DisplayName}. Public reputation: {subjectRecord.ReputationScore}.")
                .WithChangedEntity(subject.Id)
                .WithChangedEntity(reporter.Id)
                .WithHistoryEvent(historyEvent);

            if (HasId(_relatedPlaceId)) {
                result.WithChangedEntity(_relatedPlaceId);
            }

            return result;
        }

        private static int Clamp(int value, int min, int max)
        {
            if (value < min) {
                return min;
            }

            return value > max ? max : value;
        }

        private static bool HasId(WorldEntityId id)
        {
            return !string.IsNullOrEmpty(id.Value);
        }
    }
}
