using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class CashDenominationStackState
    {
        public CashDenomination Denomination { get; }
        public int Count { get; private set; }
        public int TotalCents => (int)Denomination * Count;

        public CashDenominationStackState(CashDenomination denomination, int count)
        {
            Denomination = denomination;
            Count = count;
        }

        public void Add(int count)
        {
            Count += count;
        }

        public bool TryRemove(int count)
        {
            if (Count < count) {
                return false;
            }

            Count -= count;
            return true;
        }
    }
}
