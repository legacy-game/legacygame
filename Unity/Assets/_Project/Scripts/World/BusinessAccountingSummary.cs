namespace Legacy.World
{
    public readonly struct BusinessAccountingSummary
    {
        public WorldEntityId WorkplaceId { get; }
        public int IncomeCents { get; }
        public int ExpenseCents { get; }
        public int NetCents { get; }
        public int EntryCount { get; }

        public BusinessAccountingSummary(
            WorldEntityId workplaceId,
            int incomeCents,
            int expenseCents,
            int entryCount)
        {
            WorkplaceId = workplaceId;
            IncomeCents = incomeCents;
            ExpenseCents = expenseCents;
            NetCents = incomeCents - expenseCents;
            EntryCount = entryCount;
        }
    }
}
