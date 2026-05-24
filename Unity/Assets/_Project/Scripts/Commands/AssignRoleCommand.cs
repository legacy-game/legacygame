using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class AssignRoleCommand : IWorldCommand
    {
        private readonly WorldEntityId _assignmentId;
        private readonly WorldEntityId _citizenId;
        private readonly string _roleId;
        private readonly WorldEntityId _workplacePlaceId;
        private readonly WorldEntityId _actorId;

        public AssignRoleCommand(
            WorldEntityId assignmentId,
            WorldEntityId citizenId,
            string roleId,
            WorldEntityId workplacePlaceId,
            WorldEntityId actorId)
        {
            _assignmentId = assignmentId;
            _citizenId = citizenId;
            _roleId = roleId;
            _workplacePlaceId = workplacePlaceId;
            _actorId = actorId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetCitizen(_citizenId, out CitizenState citizen)) {
                return WorldCommandResult.Failure($"Citizen not found: {_citizenId}");
            }

            if (!context.State.TryGetPlace(_workplacePlaceId, out PlaceState place)) {
                return WorldCommandResult.Failure($"Place not found: {_workplacePlaceId}");
            }

            if (!RoleCatalog.TryGet(_roleId, out RoleDefinition role)) {
                return WorldCommandResult.Failure($"Role not found: {_roleId}");
            }

            var assignment = new RoleAssignmentState(_assignmentId, citizen.Id, role.Id, place.Id);
            context.State.AddRoleAssignment(assignment);

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.RoleAssigned,
                $"{citizen.DisplayName} was assigned role {role.DisplayName} at {place.DisplayName}.",
                new[] { _actorId, citizen.Id },
                new[] { place.Id });

            return WorldCommandResult
                .Success($"{citizen.DisplayName} assigned role {role.DisplayName} at {place.DisplayName}.")
                .WithChangedEntity(citizen.Id)
                .WithHistoryEvent(historyEvent);
        }
    }
}
