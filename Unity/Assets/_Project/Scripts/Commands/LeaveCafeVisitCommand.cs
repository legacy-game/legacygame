using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class LeaveCafeVisitCommand : IWorldCommand
    {
        private readonly WorldEntityId _visitId;
        private readonly WorldEntityId _workerCitizenId;

        public LeaveCafeVisitCommand(WorldEntityId visitId, WorldEntityId workerCitizenId)
        {
            _visitId = visitId;
            _workerCitizenId = workerCitizenId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetVisit(_visitId, out VisitState visit)) {
                return WorldCommandResult.Failure($"Visit not found: {_visitId}");
            }

            if (!visit.IsCafeVisit || visit.CafeStage != CafeVisitStage.Chat) {
                return WorldCommandResult.Failure($"Cafe visit must be at Chat before leaving; current stage is {visit.CafeStage}.");
            }

            if (!context.State.TryGetCitizen(_workerCitizenId, out CitizenState worker)) {
                return WorldCommandResult.Failure($"Worker not found: {_workerCitizenId}");
            }

            if (!context.State.TryGetCitizen(visit.VisitorCitizenId, out CitizenState customer)) {
                return WorldCommandResult.Failure($"Customer not found: {visit.VisitorCitizenId}");
            }

            if (!context.State.TryGetWorkplace(visit.WorkplaceId, out WorkplaceState workplace)) {
                return WorldCommandResult.Failure($"Workplace not found: {visit.WorkplaceId}");
            }

            visit.MarkCafeLeft(context.State.CurrentTime);
            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.VisitCompleted,
                $"{customer.DisplayName} left {workplace.PlaceId}: {visit.CompletionLine}",
                new[] { customer.Id, worker.Id },
                new[] { workplace.PlaceId });

            return WorldCommandResult
                .Success(string.IsNullOrWhiteSpace(visit.CompletionLine) ? $"{customer.DisplayName} left." : visit.CompletionLine)
                .WithChangedEntity(visit.Id)
                .WithHistoryEvent(history);
        }
    }
}
