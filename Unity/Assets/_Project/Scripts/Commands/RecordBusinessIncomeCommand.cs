using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class RecordBusinessIncomeCommand : IWorldCommand
    {
        private readonly WorldEntityId _workplaceId;
        private readonly int _amountCents;
        private readonly string _reason;
        private readonly WorldEntityId _actorId;

        public RecordBusinessIncomeCommand(
            WorldEntityId workplaceId,
            int amountCents,
            string reason,
            WorldEntityId actorId)
        {
            _workplaceId = workplaceId;
            _amountCents = amountCents;
            _reason = reason;
            _actorId = actorId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (_amountCents <= 0) {
                return WorldCommandResult.Failure("Income amount must be positive.");
            }

            if (string.IsNullOrWhiteSpace(_reason)) {
                return WorldCommandResult.Failure("Income reason must not be empty.");
            }

            if (!context.State.TryGetWorkplace(_workplaceId, out WorkplaceState workplace)) {
                return WorldCommandResult.Failure($"Workplace not found: {_workplaceId}");
            }

            if (workplace.OwnerCitizenId != _actorId) {
                return WorldCommandResult.Failure("Only the owner can record business income.");
            }

            var economy = new EconomySystem(context.State);
            EconomyTransferResult income = economy.RecordIncome(
                workplace.BusinessAccountId,
                _amountCents,
                _reason,
                context.State.CurrentTime);
            if (!income.Succeeded) {
                return WorldCommandResult.Failure(income.Message);
            }

            var ledgerEntry = new BusinessLedgerEntryState(
                context.State.CreateNextBusinessLedgerEntryId(),
                workplace.Id,
                workplace.BusinessAccountId,
                BusinessLedgerEntryKind.Income,
                _amountCents,
                _reason,
                context.State.CurrentTime,
                workplace.PlaceId);
            context.State.AddBusinessLedgerEntry(ledgerEntry);

            HistoryEvent history = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.BusinessIncomeRecorded,
                $"{EconomySystem.FormatMoney(_amountCents)} income recorded for {_reason}.",
                new[] { _actorId },
                new[] { workplace.PlaceId });

            return WorldCommandResult
                .Success($"{EconomySystem.FormatMoney(_amountCents)} income recorded.")
                .WithChangedEntity(workplace.Id)
                .WithChangedEntity(workplace.BusinessAccountId)
                .WithChangedEntity(ledgerEntry.Id)
                .WithHistoryEvent(history);
        }
    }
}
