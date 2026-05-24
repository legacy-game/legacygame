namespace Legacy.World
{
    public readonly struct PropertyAccessResult
    {
        public bool IsAllowed { get; }
        public AccessRule Rule { get; }
        public string Reason { get; }

        public PropertyAccessResult(bool isAllowed, AccessRule rule, string reason)
        {
            IsAllowed = isAllowed;
            Rule = rule;
            Reason = reason;
        }
    }
}
