using System.Collections.Generic;

namespace Legacy.World
{
    public static class CashDrawerSystem
    {
        private static readonly CashDenomination[] DescendingDenominations = {
            CashDenomination.TwentyDollar,
            CashDenomination.TenDollar,
            CashDenomination.FiveDollar,
            CashDenomination.OneDollar,
            CashDenomination.Quarter,
            CashDenomination.Dime,
            CashDenomination.Nickel,
            CashDenomination.Penny
        };

        public static bool TryAcceptSale(CashDrawerState drawer, IReadOnlyList<CashDenominationStackState> tender, int priceCents, out CashChangeResult result)
        {
            if (drawer == null) {
                result = new CashChangeResult(false, "Cash drawer not found.", 0, priceCents, 0);
                return false;
            }

            if (priceCents <= 0) {
                result = new CashChangeResult(false, "Sale price must be positive.", 0, priceCents, 0);
                return false;
            }

            int tenderedCents = Total(tender);
            if (tenderedCents < priceCents) {
                result = new CashChangeResult(false, "Tendered cash does not cover the sale.", tenderedCents, priceCents, 0);
                return false;
            }

            int changeCents = tenderedCents - priceCents;
            result = MakeChange(drawer, tender, tenderedCents, priceCents, changeCents);
            if (!result.Succeeded) {
                return false;
            }

            AddTender(drawer, tender);
            for (int i = 0; i < result.Change.Count; i++) {
                drawer.TryRemove(result.Change[i].Denomination, result.Change[i].Count);
            }

            return true;
        }

        private static CashChangeResult MakeChange(
            CashDrawerState drawer,
            IReadOnlyList<CashDenominationStackState> tender,
            int tenderedCents,
            int priceCents,
            int changeCents)
        {
            var result = new CashChangeResult(true, "Cash accepted.", tenderedCents, priceCents, changeCents);
            int remaining = changeCents;
            for (int i = 0; i < DescendingDenominations.Length; i++) {
                CashDenomination denomination = DescendingDenominations[i];
                int value = (int)denomination;
                int available = drawer.CountOf(denomination) + CountOf(tender, denomination);
                int count = System.Math.Min(remaining / value, available);
                if (count <= 0) {
                    continue;
                }

                remaining -= value * count;
                result.WithChange(denomination, count);
            }

            return remaining == 0
                ? result
                : new CashChangeResult(false, "Cash drawer cannot make exact change.", tenderedCents, priceCents, changeCents);
        }

        private static void AddTender(CashDrawerState drawer, IReadOnlyList<CashDenominationStackState> tender)
        {
            if (tender == null) {
                return;
            }

            for (int i = 0; i < tender.Count; i++) {
                drawer.Add(tender[i].Denomination, tender[i].Count);
            }
        }

        private static int Total(IReadOnlyList<CashDenominationStackState> tender)
        {
            int total = 0;
            if (tender == null) {
                return total;
            }

            for (int i = 0; i < tender.Count; i++) {
                total += tender[i].TotalCents;
            }

            return total;
        }

        private static int CountOf(IReadOnlyList<CashDenominationStackState> stacks, CashDenomination denomination)
        {
            if (stacks == null) {
                return 0;
            }

            int count = 0;
            for (int i = 0; i < stacks.Count; i++) {
                if (stacks[i].Denomination == denomination) {
                    count += stacks[i].Count;
                }
            }

            return count;
        }
    }
}
