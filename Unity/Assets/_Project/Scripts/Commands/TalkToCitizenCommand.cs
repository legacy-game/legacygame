using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class TalkToCitizenCommand : IWorldCommand
    {
        private readonly WorldEntityId _actorId;
        private readonly WorldEntityId _targetCitizenId;
        private readonly string _topic;
        private readonly string _lineId;

        public TalkToCitizenCommand(WorldEntityId actorId, WorldEntityId targetCitizenId, string topic = "", string lineId = "")
        {
            _actorId = actorId;
            _targetCitizenId = targetCitizenId;
            _topic = topic ?? string.Empty;
            _lineId = lineId ?? string.Empty;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetCitizen(_actorId, out CitizenState actor)) {
                return WorldCommandResult.Failure($"Citizen not found: {_actorId}");
            }

            if (!context.State.TryGetCitizen(_targetCitizenId, out CitizenState target)) {
                return WorldCommandResult.Failure($"Citizen not found: {_targetCitizenId}");
            }

            DialogueLine line = SelectLine(target.Id);
            DialogueState dialogue = context.State.GetOrCreateDialogueState(target.Id);
            RelationshipState relationship = context.State.GetOrCreateRelationship(target.Id, actor.Id);

            dialogue.RecordTalk(line.Id, context.State.CurrentTime);
            relationship.ApplyInteraction(line.AffinityDelta, line.FamiliarityDelta, context.State.CurrentTime);

            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.CitizenTalked,
                $"{actor.DisplayName} talked with {target.DisplayName}: \"{line.Text}\"",
                new[] { actor.Id, target.Id },
                new[] { target.CurrentPlaceId });

            var memory = new CitizenMemoryState(
                context.State.CreateNextCitizenMemoryId(),
                target.Id,
                actor.Id,
                "conversation",
                BuildMemorySummary(actor, line),
                context.State.CurrentTime,
                history.Id,
                1 + line.FamiliarityDelta);
            context.State.AddCitizenMemory(memory);

            return WorldCommandResult
                .Success($"{target.DisplayName}: {line.Text}")
                .WithChangedEntity(actor.Id)
                .WithChangedEntity(target.Id)
                .WithChangedEntity(memory.Id)
                .WithHistoryEvent(history);
        }

        private DialogueLine SelectLine(WorldEntityId targetCitizenId)
        {
            if (!string.IsNullOrWhiteSpace(_lineId)) {
                if (!DialogueCatalog.TryGet(_lineId, out DialogueLine line)) {
                    return DialogueCatalog.SelectLine(targetCitizenId, _topic);
                }

                return line.SpeakerCitizenId == targetCitizenId
                    ? line
                    : DialogueCatalog.SelectLine(targetCitizenId, _topic);
            }

            return DialogueCatalog.SelectLine(targetCitizenId, _topic);
        }

        private static string BuildMemorySummary(CitizenState actor, DialogueLine line)
        {
            return string.IsNullOrWhiteSpace(line.MemorySummary)
                ? $"{actor.DisplayName} talked with me."
                : $"{actor.DisplayName}: {line.MemorySummary}";
        }
    }
}
