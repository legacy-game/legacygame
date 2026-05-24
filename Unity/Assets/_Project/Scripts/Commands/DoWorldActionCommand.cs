using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class DoWorldActionCommand : IWorldCommand
    {
        private readonly WorldEntityId _actorId;
        private readonly WorldActionKind _action;
        private readonly WorldEntityId _targetPlaceId;
        private readonly WorldEntityId _targetEntityId;

        public DoWorldActionCommand(
            WorldEntityId actorId,
            WorldActionKind action,
            WorldEntityId targetPlaceId,
            WorldEntityId targetEntityId = default)
        {
            _actorId = actorId;
            _action = action;
            _targetPlaceId = targetPlaceId;
            _targetEntityId = targetEntityId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetCitizen(_actorId, out CitizenState actor)) {
                return WorldCommandResult.Failure($"Citizen not found: {_actorId}");
            }

            if (!context.State.TryGetPlace(_targetPlaceId, out PlaceState place)) {
                return WorldCommandResult.Failure($"Place not found: {_targetPlaceId}");
            }

            var roleSystem = new RoleSystem(context.State);
            RoleAuthorizationResult authorization = roleSystem.CanPerform(actor.Id, _action, place.Id);
            if (!authorization.IsAllowed) {
                return WorldCommandResult.Failure(authorization.Reason);
            }

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.WorldActionPerformed,
                $"{actor.DisplayName} performed {_action} at {place.DisplayName}.",
                new[] { actor.Id },
                new[] { place.Id });

            WorldCommandResult result = WorldCommandResult
                .Success($"{actor.DisplayName} performed {_action} at {place.DisplayName}.")
                .WithChangedEntity(actor.Id)
                .WithChangedEntity(place.Id)
                .WithHistoryEvent(historyEvent);

            if (!string.IsNullOrEmpty(_targetEntityId.Value)) {
                result.WithChangedEntity(_targetEntityId);
            }

            return result;
        }
    }
}
