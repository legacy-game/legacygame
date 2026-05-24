namespace Legacy.World
{
    public readonly struct RoleAuthorizationResult
    {
        public bool IsAllowed { get; }
        public string Reason { get; }
        public string RoleId { get; }

        public RoleAuthorizationResult(bool isAllowed, string reason, string roleId = "")
        {
            IsAllowed = isAllowed;
            Reason = string.IsNullOrWhiteSpace(reason) ? (isAllowed ? "Allowed." : "Denied.") : reason;
            RoleId = roleId ?? string.Empty;
        }
    }
}
