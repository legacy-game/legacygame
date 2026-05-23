using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class MoveCitizenCommand : IWorldCommand
    {
        private readonly WorldEntityId _citizenId;
        private readonly WorldEntityId _regionId;
        private readonly WorldEntityId _sceneId;
        private readonly WorldEntityId _placeId;
        private readonly GridCoord _targetCoord;
        private readonly CitizenActivityState _activity;

        public MoveCitizenCommand(
            WorldEntityId citizenId,
            WorldEntityId regionId,
            WorldEntityId sceneId,
            WorldEntityId placeId,
            GridCoord targetCoord,
            CitizenActivityState activity)
        {
            _citizenId = citizenId;
            _regionId = regionId;
            _sceneId = sceneId;
            _placeId = placeId;
            _targetCoord = targetCoord;
            _activity = activity;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetCitizen(_citizenId, out CitizenState citizen)) {
                return WorldCommandResult.Failure($"Citizen not found: {_citizenId}");
            }

            context.State.MoveCitizen(citizen.Id, _regionId, _sceneId, _placeId, _targetCoord, _activity);

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.CitizenMoved,
                $"{citizen.DisplayName} moved to {_targetCoord}.",
                new[] { citizen.Id });

            return WorldCommandResult
                .Success($"{citizen.DisplayName} moved.")
                .WithChangedEntity(citizen.Id)
                .WithHistoryEvent(historyEvent);
        }
    }
}
