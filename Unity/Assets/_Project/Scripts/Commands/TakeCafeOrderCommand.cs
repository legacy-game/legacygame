using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class TakeCafeOrderCommand : IWorldCommand
    {
        private readonly WorldEntityId _visitId;
        private readonly WorldEntityId _workerCitizenId;
        private readonly string _recipeId;

        public TakeCafeOrderCommand(WorldEntityId visitId, WorldEntityId workerCitizenId, string recipeId)
        {
            _visitId = visitId;
            _workerCitizenId = workerCitizenId;
            _recipeId = recipeId ?? string.Empty;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetVisit(_visitId, out VisitState visit)) {
                return WorldCommandResult.Failure($"Visit not found: {_visitId}");
            }

            if (!visit.IsCafeVisit) {
                return WorldCommandResult.Failure("Visit is not a cafe customer visit.");
            }

            if (visit.CafeStage != CafeVisitStage.Enter) {
                return WorldCommandResult.Failure($"Cafe visit is {visit.CafeStage}; only Enter can take an order.");
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

            if (string.IsNullOrEmpty(visit.LinkedTaskId.Value) ||
                !context.State.TryGetJobTask(visit.LinkedTaskId, out JobTaskState task) ||
                task.Status != JobTaskStatus.Queued) {
                return WorldCommandResult.Failure("Cafe visit does not have a queued prep task.");
            }

            RoleAuthorizationResult authorization = new RoleSystem(context.State).CanPerform(worker.Id, WorldActionKind.ServeCustomer, workplace.PlaceId);
            if (!authorization.IsAllowed) {
                return WorldCommandResult.Failure(authorization.Reason);
            }

            if (!CafeRecipeCatalog.TryGet(_recipeId, out CafeRecipeDefinition recipe)) {
                return WorldCommandResult.Failure($"Cafe recipe not found: {_recipeId}");
            }

            if (!context.State.TryGetInventoryContainerForOwner(workplace.Id, InventoryContainerKind.WorkplaceStorage, out InventoryContainerState inventory)) {
                return WorldCommandResult.Failure("Cafe inventory container not found.");
            }

            if (!CafeRecipeSystem.CanPrepare(inventory, recipe, out string recipeReason)) {
                return WorldCommandResult.Failure(recipeReason);
            }

            visit.PlaceCafeOrder(recipe);
            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.CafeOrderTaken,
                $"{worker.DisplayName} took {customer.DisplayName}'s order for {recipe.DisplayName}.",
                new[] { worker.Id, customer.Id },
                new[] { workplace.PlaceId });

            return WorldCommandResult
                .Success($"{customer.DisplayName} ordered {recipe.DisplayName}.")
                .WithChangedEntity(visit.Id)
                .WithHistoryEvent(history);
        }
    }
}
