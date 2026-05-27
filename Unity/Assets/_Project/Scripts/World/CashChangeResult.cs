using System.Collections.Generic;

namespace Legacy.World
{
    public sealed class CashChangeResult
    {
        private readonly List<CashDenominationStackState> _change = new();

        public bool Succeeded { get; }
        public string Message { get; }
        public int TenderedCents { get; }
        public int PriceCents { get; }
        public int ChangeCents { get; }
        public IReadOnlyList<CashDenominationStackState> Change => _change;

        public CashChangeResult(bool succeeded, string message, int tenderedCents, int priceCents, int changeCents)
        {
            Succeeded = succeeded;
            Message = message ?? string.Empty;
            TenderedCents = tenderedCents;
            PriceCents = priceCents;
            ChangeCents = changeCents;
        }

        public CashChangeResult WithChange(CashDenomination denomination, int count)
        {
            if (count > 0) {
                _change.Add(new CashDenominationStackState(denomination, count));
            }

            return this;
        }
    }
}
