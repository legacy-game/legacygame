namespace Legacy.World
{
    public sealed class RoleSystem
    {
        private readonly WorldState _state;

        public RoleSystem(WorldState state)
        {
            _state = state;
        }

        public RoleAuthorizationResult CanPerform(WorldEntityId citizenId, WorldActionKind action, WorldEntityId placeId)
        {
            foreach (RoleAssignmentState assignment in _state.RoleAssignments) {
                if (!assignment.IsActive || assignment.CitizenId != citizenId) {
                    continue;
                }

                if (assignment.WorkplacePlaceId != placeId) {
                    continue;
                }

                if (!RoleCatalog.TryGet(assignment.RoleId, out RoleDefinition definition)) {
                    continue;
                }

                if (definition.Allows(action)) {
                    return new RoleAuthorizationResult(true, $"{definition.DisplayName} can perform {action}.", assignment.RoleId);
                }
            }

            return new RoleAuthorizationResult(false, $"Citizen {citizenId} is not authorized to perform {action} at {placeId}.");
        }
    }
}
