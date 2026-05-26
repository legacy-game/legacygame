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
            if (_state.TryGetWorkplaceByPlace(placeId, out WorkplaceState workplace)) {
                foreach (EmploymentContractState contract in _state.EmploymentContractsById.Values) {
                    if (contract.Status != EmploymentContractStatus.Active ||
                        contract.WorkerCitizenId != citizenId ||
                        contract.WorkplaceId != workplace.Id) {
                        continue;
                    }

                    if (!RoleCatalog.TryGet(contract.RoleId, out RoleDefinition contractRole)) {
                        continue;
                    }

                    if (contractRole.Allows(action)) {
                        return new RoleAuthorizationResult(true, $"{contractRole.DisplayName} can perform {action}.", contract.RoleId);
                    }
                }
            }

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

        public bool HasActiveRole(WorldEntityId citizenId, string roleId, WorldEntityId placeId)
        {
            foreach (RoleAssignmentState assignment in _state.RoleAssignments) {
                if (assignment.IsActive &&
                    assignment.CitizenId == citizenId &&
                    assignment.RoleId == roleId &&
                    assignment.WorkplacePlaceId == placeId) {
                    return true;
                }
            }

            return false;
        }
    }
}
