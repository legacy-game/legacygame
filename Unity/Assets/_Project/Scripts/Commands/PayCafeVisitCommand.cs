using System.Collections.Generic;
using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class PayCafeVisitCommand : IWorldCommand
    {
        private readonly WorldEntityId _visitId;
        private readonly WorldEntityId _workerCitizenId;
        private readonly IReadOnlyList<CashDenominationStackState> _tender;

        public PayCafeVisitCommand(WorldEntityId visitId, WorldEntityId workerCitizenId, IReadOnlyList<CashDenominationStackState> tender)
        {
            _visitId = visitId;
            _workerCitizenId = workerCitizenId;
            _tender = tender;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetVisit(_visitId, out VisitState visit)) {
                return WorldCommandResult.Failure($"Visit not found: {_visitId}");
            }

            if (!visit.IsCafeVisit || visit.CafeStage != CafeVisitStage.Receive) {
                return WorldCommandResult.Failure($"Cafe visit must be at Receive before payment; current stage is {visit.CafeStage}.");
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

            RoleAuthorizationResult authorization = new RoleSystem(context.State).CanPerform(worker.Id, WorldActionKind.ServeCustomer, workplace.PlaceId);
            if (!authorization.IsAllowed) {
                return WorldCommandResult.Failure(authorization.Reason);
            }

            if (!context.State.TryGetCashDrawerForOwner(workplace.Id, CashContainerKind.Till, out CashDrawerState till)) {
                return WorldCommandResult.Failure("Cafe cash drawer not found.");
            }

            if (!context.State.TryGetMoneyAccountForOwner(customer.Id, out MoneyAccountState customerAccount)) {
                return WorldCommandResult.Failure("Customer cash account not found.");
            }

            if (!context.State.TryGetMoneyAccount(workplace.BusinessAccountId, out MoneyAccountState businessAccount)) {
                return WorldCommandResult.Failure("Business account not found.");
            }

            if (customerAccount.BalanceCents < visit.PriceCents) {
                return WorldCommandResult.Failure($"{customerAccount.DisplayName} does not have enough money.");
            }

            if (!CashDrawerSystem.TryAcceptSale(till, _tender, visit.PriceCents, out CashChangeResult cash)) {
                return WorldCommandResult.Failure(cash.Message);
            }

            var economy = new EconomySystem(context.State);
            EconomyTransferResult transfer = economy.Transfer(
                customerAccount.Id,
                businessAccount.Id,
                visit.PriceCents,
                visit.RecipeId,
                workplace.PlaceId,
                WorldActionKind.ServeCustomer,
                context.State.CurrentTime);
            if (!transfer.Succeeded) {
                return WorldCommandResult.Failure(transfer.Message);
            }

            var ledger = new BusinessLedgerEntryState(
                context.State.CreateNextBusinessLedgerEntryId(),
                workplace.Id,
                businessAccount.Id,
                BusinessLedgerEntryKind.Income,
                visit.PriceCents,
                $"Cafe sale: {visit.RecipeId}",
                context.State.CurrentTime,
                visit.Id);
            context.State.AddBusinessLedgerEntry(ledger);
            visit.MarkCafePaid(cash.TenderedCents);

            HistoryEvent sale = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.CafeSaleCompleted,
                $"{worker.DisplayName} accepted payment from {customer.DisplayName} for {EconomySystem.FormatMoney(visit.PriceCents)}.",
                new[] { worker.Id, customer.Id },
                new[] { workplace.PlaceId });
            HistoryEvent change = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.CashChangeMade,
                $"{EconomySystem.FormatMoney(cash.ChangeCents)} change made from {EconomySystem.FormatMoney(cash.TenderedCents)} tendered.",
                new[] { worker.Id, customer.Id },
                new[] { workplace.PlaceId });

            return WorldCommandResult
                .Success($"Payment accepted. Change due: {EconomySystem.FormatMoney(cash.ChangeCents)}.")
                .WithChangedEntity(visit.Id)
                .WithChangedEntity(till.Id)
                .WithChangedEntity(customerAccount.Id)
                .WithChangedEntity(businessAccount.Id)
                .WithChangedEntity(ledger.Id)
                .WithHistoryEvent(sale)
                .WithHistoryEvent(change);
        }
    }
}
