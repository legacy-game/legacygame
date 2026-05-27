using Legacy.Time;

namespace Legacy.World
{
    public sealed class EconomySystem
    {
        private readonly WorldState _state;

        public EconomySystem(WorldState state)
        {
            _state = state;
        }

        public EconomyTransferResult Transfer(
            WorldEntityId payerAccountId,
            WorldEntityId payeeAccountId,
            int amountCents,
            string reason,
            WorldEntityId relatedPlaceId,
            WorldActionKind actionKind,
            GameDateTime timestamp)
        {
            if (amountCents <= 0) {
                return new EconomyTransferResult(false, "Payment amount must be positive.");
            }

            if (!_state.TryGetMoneyAccount(payerAccountId, out MoneyAccountState payer)) {
                return new EconomyTransferResult(false, $"Payer account not found: {payerAccountId}");
            }

            if (!_state.TryGetMoneyAccount(payeeAccountId, out MoneyAccountState payee)) {
                return new EconomyTransferResult(false, $"Payee account not found: {payeeAccountId}");
            }

            if (payer.BalanceCents < amountCents) {
                return new EconomyTransferResult(false, $"{payer.DisplayName} does not have enough money.");
            }

            payer.Debit(amountCents);
            payee.Credit(amountCents);

            var transaction = new TransactionState(
                _state.CreateNextTransactionId(),
                timestamp,
                payerAccountId,
                payeeAccountId,
                amountCents,
                reason,
                relatedPlaceId,
                actionKind);
            _state.AddTransaction(transaction);

            return new EconomyTransferResult(true, $"{payer.DisplayName} paid {FormatMoney(amountCents)} to {payee.DisplayName}.", transaction);
        }

        public EconomyTransferResult RecordIncome(
            WorldEntityId accountId,
            int amountCents,
            string reason,
            GameDateTime timestamp)
        {
            if (amountCents <= 0) {
                return new EconomyTransferResult(false, "Income amount must be positive.");
            }

            if (!_state.TryGetMoneyAccount(accountId, out MoneyAccountState account)) {
                return new EconomyTransferResult(false, $"Account not found: {accountId}");
            }

            account.Credit(amountCents);
            return new EconomyTransferResult(true, $"{FormatMoney(amountCents)} income recorded for {account.DisplayName}.");
        }

        public bool TryGetBusinessSummary(WorldEntityId workplaceId, out BusinessAccountingSummary summary)
        {
            summary = default;
            if (!_state.TryGetWorkplace(workplaceId, out WorkplaceState _)) {
                return false;
            }

            int incomeCents = 0;
            int expenseCents = 0;
            int entryCount = 0;
            foreach (BusinessLedgerEntryState entry in _state.BusinessLedgerEntries) {
                if (entry.WorkplaceId != workplaceId) {
                    continue;
                }

                entryCount++;
                if (entry.Kind == BusinessLedgerEntryKind.Expense || entry.Kind == BusinessLedgerEntryKind.RentPayment) {
                    expenseCents += entry.AmountCents;
                } else {
                    incomeCents += entry.AmountCents;
                }
            }

            summary = new BusinessAccountingSummary(workplaceId, incomeCents, expenseCents, entryCount);
            return true;
        }

        public static string FormatMoney(int cents)
        {
            return $"${cents / 100}.{cents % 100:00}";
        }
    }
}
