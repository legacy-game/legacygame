using System.Collections.Generic;
using System.Text;
using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class QueryPublicRecordCommand : IWorldCommand
    {
        private readonly WorldEntityId _subjectCitizenId;
        private readonly WorldEntityId _requesterCitizenId;
        private readonly int _maxRegistryEntries;
        private readonly int _maxHistoryEvents;

        public QueryPublicRecordCommand(
            WorldEntityId subjectCitizenId,
            WorldEntityId requesterCitizenId,
            int maxRegistryEntries = 5,
            int maxHistoryEvents = 5)
        {
            _subjectCitizenId = subjectCitizenId;
            _requesterCitizenId = requesterCitizenId;
            _maxRegistryEntries = maxRegistryEntries;
            _maxHistoryEvents = maxHistoryEvents;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetCitizen(_subjectCitizenId, out CitizenState subject)) {
                return WorldCommandResult.Failure($"Citizen not found: {_subjectCitizenId}");
            }

            if (!context.State.TryGetCitizen(_requesterCitizenId, out CitizenState requester)) {
                return WorldCommandResult.Failure($"Requester not found: {_requesterCitizenId}");
            }

            context.State.TryGetPublicRecord(subject.Id, out PublicRecordState publicRecord);
            List<CivicRegistryEntryState> registryEntries = context.State.GetCivicRegistryEntriesForCitizen(subject.Id);
            IReadOnlyList<HistoryEvent> actorHistory = context.State.GetHistoryForActor(subject.Id);

            string message = BuildMessage(subject, publicRecord, registryEntries, actorHistory);
            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.PublicRecordQueried,
                $"{requester.DisplayName} queried the public record for {subject.DisplayName}.",
                new[] { requester.Id, subject.Id });

            return WorldCommandResult
                .Success(message)
                .WithChangedEntity(subject.Id)
                .WithHistoryEvent(historyEvent);
        }

        private string BuildMessage(
            CitizenState subject,
            PublicRecordState publicRecord,
            IReadOnlyList<CivicRegistryEntryState> registryEntries,
            IReadOnlyList<HistoryEvent> actorHistory)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Public record: {subject.DisplayName}");
            builder.AppendLine($"Reputation: {(publicRecord == null ? 0 : publicRecord.ReputationScore)}");
            builder.AppendLine($"Reports filed: {(publicRecord == null ? 0 : publicRecord.ReportsFiled)}");
            builder.AppendLine($"Reports received: {(publicRecord == null ? 0 : publicRecord.ReportsReceived)}");
            builder.AppendLine("Registry:");
            AppendRegistryEntries(builder, registryEntries);
            builder.AppendLine("History:");
            AppendHistory(builder, actorHistory);
            return builder.ToString().TrimEnd();
        }

        private void AppendRegistryEntries(StringBuilder builder, IReadOnlyList<CivicRegistryEntryState> registryEntries)
        {
            if (registryEntries.Count == 0) {
                builder.AppendLine("- No public registry entries.");
                return;
            }

            int start = registryEntries.Count - ClampToNonNegative(_maxRegistryEntries);
            if (start < 0) {
                start = 0;
            }

            for (int i = start; i < registryEntries.Count; i++) {
                CivicRegistryEntryState entry = registryEntries[i];
                builder.AppendLine($"- {entry.Kind}: {entry.Summary}");
            }
        }

        private void AppendHistory(StringBuilder builder, IReadOnlyList<HistoryEvent> actorHistory)
        {
            if (actorHistory.Count == 0) {
                builder.AppendLine("- No public history events.");
                return;
            }

            int start = actorHistory.Count - ClampToNonNegative(_maxHistoryEvents);
            if (start < 0) {
                start = 0;
            }

            for (int i = start; i < actorHistory.Count; i++) {
                HistoryEvent historyEvent = actorHistory[i];
                builder.AppendLine($"- {historyEvent.Kind}: {historyEvent.Description}");
            }
        }

        private static int ClampToNonNegative(int value)
        {
            return value < 0 ? 0 : value;
        }
    }
}
