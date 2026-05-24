using Legacy.History;
using Legacy.Time;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class RegisterCitizenCommand : IWorldCommand
    {
        private readonly WorldEntityId _citizenId;
        private readonly WorldEntityId _registryPlaceId;
        private readonly WorldEntityId _startingResidencePlaceId;
        private readonly WorldEntityId _firstGoalTargetPlaceId;
        private readonly GridCoord _firstGoalTargetCoord;

        public RegisterCitizenCommand(
            WorldEntityId citizenId,
            WorldEntityId registryPlaceId,
            WorldEntityId startingResidencePlaceId,
            WorldEntityId firstGoalTargetPlaceId,
            GridCoord firstGoalTargetCoord)
        {
            _citizenId = citizenId;
            _registryPlaceId = registryPlaceId;
            _startingResidencePlaceId = startingResidencePlaceId;
            _firstGoalTargetPlaceId = firstGoalTargetPlaceId;
            _firstGoalTargetCoord = firstGoalTargetCoord;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetCitizen(_citizenId, out CitizenState citizen)) {
                return WorldCommandResult.Failure($"Citizen not found: {_citizenId}");
            }

            if (context.State.TryGetCitizenRegistration(citizen.Id, out CitizenRegistrationState _)) {
                return WorldCommandResult.Failure($"{citizen.DisplayName} is already registered.");
            }

            if (!context.State.TryGetPlace(_registryPlaceId, out PlaceState registryPlace)) {
                return WorldCommandResult.Failure($"Registry place not found: {_registryPlaceId}");
            }

            if (!context.State.TryGetPlace(_startingResidencePlaceId, out PlaceState _)) {
                return WorldCommandResult.Failure($"Starting residence place not found: {_startingResidencePlaceId}");
            }

            if (!context.State.TryGetPlace(_firstGoalTargetPlaceId, out PlaceState firstGoalPlace)) {
                return WorldCommandResult.Failure($"First goal place not found: {_firstGoalTargetPlaceId}");
            }

            var registration = new CitizenRegistrationState(citizen.Id, context.State.CurrentTime, _startingResidencePlaceId);
            context.State.AddCitizenRegistration(registration);

            var firstGoal = new CitizenGoalState(
                new WorldEntityId($"goal_opening_{citizen.Id.Value}"),
                citizen.Id,
                new CitizenGoalDefinition(
                    CitizenGoalKind.TravelToPlace,
                    firstGoalPlace.Id,
                    _firstGoalTargetCoord,
                    CitizenActivityState.Visiting,
                    80,
                    "Visit Linden Cafe"),
                context.State.CurrentTime,
                context.State.CurrentTime.AddMinutes(240));
            context.State.AddCitizenGoal(firstGoal);

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.CitizenRegistered,
                $"{citizen.DisplayName} registered as a resident at {registryPlace.DisplayName}.",
                new[] { citizen.Id },
                new[] { registryPlace.Id });

            return WorldCommandResult
                .Success($"{citizen.DisplayName} registered.\nFirst goal: Visit Linden Cafe.")
                .WithChangedEntity(citizen.Id)
                .WithChangedEntity(registryPlace.Id)
                .WithHistoryEvent(historyEvent);
        }
    }
}
