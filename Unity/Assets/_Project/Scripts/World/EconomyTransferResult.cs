namespace Legacy.World
{
    public readonly struct EconomyTransferResult
    {
        public bool Succeeded { get; }
        public string Message { get; }
        public TransactionState Transaction { get; }

        public EconomyTransferResult(bool succeeded, string message, TransactionState transaction = null)
        {
            Succeeded = succeeded;
            Message = message;
            Transaction = transaction;
        }
    }
}
