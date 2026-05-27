using System.Collections.Generic;
using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class ProcessCafeSaleCommand : IWorldCommand
    {
        private readonly WorldEntityId _workerCitizenId;
        private readonly WorldEntityId _customerCitizenId;
        private readonly WorldEntityId _workplaceId;
        private readonly string _recipeId;
        private readonly IReadOnlyList<CashDenominationStackState> _tender;

        public ProcessCafeSaleCommand(
            WorldEntityId workerCitizenId,
            WorldEntityId customerCitizenId,
            WorldEntityId workplaceId,
            string recipeId,
            IReadOnlyList<CashDenominationStackState> tender)
        {
            _workerCitizenId = workerCitizenId;
            _customerCitizenId = customerCitizenId;
            _workplaceId = workplaceId;
            _recipeId = recipeId ?? string.Empty;
            _tender = tender;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetCitizen(_workerCitizenId, out CitizenState worker)) {
                return WorldCommandResult.Failure($"Worker not found: {_workerCitizenId}");
            }

            if (!context.State.TryGetCitizen(_customerCitizenId, out CitizenState customer)) {
                return WorldCommandResult.Failure($"Customer not found: {_customerCitizenId}");
            }

            if (!context.State.TryGetWorkplace(_workplaceId, out WorkplaceState workplace)) {
                return WorldCommandResult.Failure($"Workplace not found: {_workplaceId}");
            }

            var roleSystem = new RoleSystem(context.State);
            RoleAuthorizationResult authorization = roleSystem.CanPerform(worker.Id, WorldActionKind.ServeCustomer, workplace.PlaceId);
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

            if (!context.State.TryGetCashDrawerForOwner(workplace.Id, CashContainerKind.Till, out CashDrawerState till)) {
                return WorldCommandResult.Failure("Cafe cash drawer not found.");
            }

            if (!context.State.TryGetMoneyAccountForOwner(customer.Id, out MoneyAccountState customerAccount)) {
                return WorldCommandResult.Failure("Customer cash account not found.");
            }

            if (customerAccount.BalanceCents < recipe.PriceCents) {
                return WorldCommandResult.Failure($"{customerAccount.DisplayName} does not have enough money.");
            }

            CashChangeResult cash = null;
            if (!CashDrawerSystem.TryAcceptSale(till, _tender, recipe.PriceCents, out cash)) {
                return WorldCommandResult.Failure(cash.Message);
            }

            if (!CafeRecipeSystem.TryPrepare(inventory, recipe, out recipeReason)) {
                return WorldCommandResult.Failure(recipeReason);
            }

            var economy = new EconomySystem(context.State);
            EconomyTransferResult transfer = economy.Transfer(
                customerAccount.Id,
                workplace.BusinessAccountId,
                recipe.PriceCents,
                recipe.DisplayName,
                workplace.PlaceId,
                WorldActionKind.ServeCustomer,
                context.State.CurrentTime);
            if (!transfer.Succeeded) {
                return WorldCommandResult.Failure(transfer.Message);
            }

            HistoryEvent sale = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.CafeSaleCompleted,
                $"{worker.DisplayName} sold {recipe.DisplayName} to {customer.DisplayName} for {EconomySystem.FormatMoney(recipe.PriceCents)}.",
                new[] { worker.Id, customer.Id },
                new[] { workplace.PlaceId });
            HistoryEvent change = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.CashChangeMade,
                $"{EconomySystem.FormatMoney(cash.ChangeCents)} change made from {EconomySystem.FormatMoney(cash.TenderedCents)} tendered.",
                new[] { worker.Id, customer.Id },
                new[] { workplace.PlaceId });

            return WorldCommandResult
                .Success($"{recipe.DisplayName} sold. Change due: {EconomySystem.FormatMoney(cash.ChangeCents)}.")
                .WithChangedEntity(workplace.Id)
                .WithChangedEntity(inventory.Id)
                .WithChangedEntity(till.Id)
                .WithChangedEntity(customerAccount.Id)
                .WithChangedEntity(workplace.BusinessAccountId)
                .WithHistoryEvent(sale)
                .WithHistoryEvent(change);
        }
    }
}
