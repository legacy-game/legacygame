using System.Collections.Generic;
using Legacy.History;
using Legacy.Time;

namespace Legacy.World
{
    public static class WorldScheduleSimulator
    {
        private static readonly WorldEntityId HollandId = new("citizen_old_mr_pell");
        private static readonly WorldEntityId VeyneId = new("region_veyne");
        private static readonly WorldEntityId StreetSceneId = new("scene_veyne_linden_street");
        private static readonly WorldEntityId StreetPlaceId = new("place_linden_street");
        private static readonly WorldEntityId CafeSceneId = new("scene_linden_cafe_interior");
        private static readonly WorldEntityId CafePlaceId = new("place_linden_cafe_interior");
        private static readonly WorldEntityId CafeBuildingId = new("building_linden_cafe");

        public static List<HistoryEvent> AdvanceSchedules(WorldState state, HistoryLog history, GameDateTime previousTime)
        {
            var events = new List<HistoryEvent>();

            if (!state.TryGetCitizen(HollandId, out CitizenState holland)) {
                return events;
            }

            int previousMinute = previousTime.Time.TotalMinutes;
            int currentMinute = state.CurrentTime.Time.TotalMinutes;

            if (Crossed(previousMinute, currentMinute, 7 * 60 + 35) && holland.ScheduleStage < 1) {
                state.MoveCitizen(holland.Id, VeyneId, CafeSceneId, CafePlaceId, new GridCoord(4, 3), CitizenActivityState.Visiting);
                holland.SetScheduleStage(1);
                events.Add(history.Create(
                    state.CurrentTime,
                    HistoryEventKind.CitizenMoved,
                    "Old Mr. Pell arrived at Linden Cafe.",
                    new[] { holland.Id },
                    new[] { CafeBuildingId }));
            }

            if (Crossed(previousMinute, currentMinute, 7 * 60 + 50) && holland.ScheduleStage < 2) {
                state.MoveCitizen(holland.Id, VeyneId, StreetSceneId, StreetPlaceId, new GridCoord(18, 8), CitizenActivityState.Offscreen);
                holland.SetScheduleStage(2);
                events.Add(history.Create(
                    state.CurrentTime,
                    HistoryEventKind.CitizenMoved,
                    "Old Mr. Pell left Linden Cafe.",
                    new[] { holland.Id },
                    new[] { CafeBuildingId }));
            }

            return events;
        }

        private static bool Crossed(int previousMinute, int currentMinute, int triggerMinute)
        {
            return previousMinute < triggerMinute && currentMinute >= triggerMinute;
        }
    }
}
