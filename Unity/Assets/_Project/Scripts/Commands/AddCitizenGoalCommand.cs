using Legacy.History;
using Legacy.Time;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class AddCitizenGoalCommand : IWorldCommand
    {
        private readonly WorldEntityId _goalId;
        private readonly WorldEntityId _citizenId;
        private readonly CitizenGoalDefinition _definition;
        private readonly GameDateTime _expiresAt;
        private readonly WorldEntityId _actorId;

        public AddCitizenGoalCommand(
            WorldEntityId goalId,
            WorldEntityId citizenId,
            CitizenGoalDefinition definition,
            GameDateTime expiresAt,
            WorldEntityId actorId)
        {
            _goalId = goalId;
            _citizenId = citizenId;
            _definition = definition;
            _expiresAt = expiresAt;
            _actorId = actorId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetCitizen(_citizenId, out CitizenState citizen)) {
                return WorldCommandResult.Failure($"Citizen not found: {_citizenId}");
            }

            if (!context.State.TryGetPlace(_definition.TargetPlaceId, out PlaceState targetPlace)) {
                return WorldCommandResult.Failure($"Place not found: {_definition.TargetPlaceId}");
            }

            var goal = new CitizenGoalState(_goalId, citizen.Id, _definition, context.State.CurrentTime, _expiresAt);
            context.State.AddCitizenGoal(goal);

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.CitizenGoalAdded,
                $"{citizen.DisplayName} got a goal: {_definition.Reason}.",
                new[] { _actorId, citizen.Id },
                new[] { targetPlace.Id });

            return WorldCommandResult
                .Success($"{citizen.DisplayName} goal added: {_definition.Reason}.")
                .WithChangedEntity(citizen.Id)
                .WithHistoryEvent(historyEvent);
        }
    }
}
