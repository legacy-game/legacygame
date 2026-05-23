using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class CitizenState
    {
        public WorldEntityId Id { get; }
        public string DisplayName { get; private set; }
        public WorldEntityId HomeBuildingId { get; private set; }
        public WorldEntityId WorkplaceBuildingId { get; private set; }
        public WorldEntityId CurrentRegionId { get; private set; }
        public WorldEntityId CurrentSceneId { get; private set; }
        public WorldEntityId CurrentPlaceId { get; private set; }
        public GridCoord CurrentCoord { get; private set; }
        public CitizenActivityState Activity { get; private set; }
        public int ScheduleStage { get; private set; }

        public CitizenState(
            WorldEntityId id,
            string displayName,
            WorldEntityId homeBuildingId,
            WorldEntityId workplaceBuildingId,
            WorldEntityId currentRegionId,
            WorldEntityId currentSceneId,
            WorldEntityId currentPlaceId,
            GridCoord currentCoord,
            CitizenActivityState activity = CitizenActivityState.Offscreen,
            int scheduleStage = 0)
        {
            if (string.IsNullOrWhiteSpace(displayName)) {
                throw new ArgumentException("Display name must not be empty.", nameof(displayName));
            }

            Id = id;
            DisplayName = displayName;
            HomeBuildingId = homeBuildingId;
            WorkplaceBuildingId = workplaceBuildingId;
            CurrentRegionId = currentRegionId;
            CurrentSceneId = currentSceneId;
            CurrentPlaceId = currentPlaceId;
            CurrentCoord = currentCoord;
            Activity = activity;
            ScheduleStage = scheduleStage;
        }

        public void MoveTo(WorldEntityId regionId, WorldEntityId sceneId, WorldEntityId placeId, GridCoord coord, CitizenActivityState activity)
        {
            CurrentRegionId = regionId;
            CurrentSceneId = sceneId;
            CurrentPlaceId = placeId;
            CurrentCoord = coord;
            Activity = activity;
        }

        public void SetScheduleStage(int scheduleStage)
        {
            ScheduleStage = scheduleStage;
        }
    }
}
