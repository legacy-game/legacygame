using System;
using System.Collections.Generic;
using Legacy.Time;

namespace Legacy.World
{
    public static class CitizenTravelPlanner
    {
        public static List<TravelOption> GetOptions(WorldState state, CitizenState citizen, CitizenGoalDefinition goal)
        {
            return GetOptions(state, citizen, goal, TravelApproachCatalog.GetDefaults());
        }

        public static List<TravelOption> GetOptions(
            WorldState state,
            CitizenState citizen,
            CitizenGoalDefinition goal,
            IReadOnlyList<TravelApproachDefinition> approaches)
        {
            var options = new List<TravelOption>();
            int distance = citizen.CurrentCoord.ManhattanDistanceTo(goal.TargetCoord);
            bool targetIsDifferentScene = state.TryGetPlace(goal.TargetPlaceId, out PlaceState targetPlace)
                && targetPlace.SceneId != citizen.CurrentSceneId;
            bool hasNearbyCitizen = HasRideCandidateNearby(state, citizen);

            for (int i = 0; i < approaches.Count; i++) {
                TravelApproachDefinition approach = approaches[i];
                if (approach.RequiresDifferentSceneOrLongDistance && !targetIsDifferentScene && distance < approach.MinimumDistance) {
                    continue;
                }

                if (approach.RequiresNearbyCitizen && !hasNearbyCitizen) {
                    continue;
                }

                int weight = approach.BaseWeight
                    + distance * approach.DistanceWeightModifier
                    + goal.Urgency * approach.UrgencyWeightModifier;
                if (weight <= 0) {
                    continue;
                }

                int estimatedMinutes = Math.Max(1, approach.BaseMinutes + distance * approach.MinutesPerTile);
                options.Add(new TravelOption(
                    approach.Id,
                    approach.DisplayName,
                    weight,
                    estimatedMinutes,
                    approach.Reason));
            }

            return options;
        }

        public static TravelPlan Plan(WorldState state, CitizenState citizen, CitizenGoalDefinition goal, GameDateTime currentTime)
        {
            List<TravelOption> options = GetOptions(state, citizen, goal);
            if (options.Count == 0) {
                return new TravelPlan(goal, string.Empty, "No approach", 0, "No travel option available.");
            }

            int totalWeight = 0;
            for (int i = 0; i < options.Count; i++) {
                totalWeight += Math.Max(0, options[i].Weight);
            }

            if (totalWeight <= 0) {
                TravelOption fallback = options[0];
                return new TravelPlan(goal, fallback.ApproachId, fallback.DisplayName, fallback.EstimatedMinutes, fallback.Reason);
            }

            int roll = (StableHash(state.WorldSeed, citizen.Id.Value, goal.TargetPlaceId.Value, goal.Reason, currentTime.Date.AbsoluteDay) & int.MaxValue) % totalWeight;
            int cursor = 0;
            for (int i = 0; i < options.Count; i++) {
                cursor += Math.Max(0, options[i].Weight);
                if (roll < cursor) {
                    TravelOption selected = options[i];
                    return new TravelPlan(goal, selected.ApproachId, selected.DisplayName, selected.EstimatedMinutes, selected.Reason);
                }
            }

            TravelOption last = options[^1];
            return new TravelPlan(goal, last.ApproachId, last.DisplayName, last.EstimatedMinutes, last.Reason);
        }

        private static bool HasRideCandidateNearby(WorldState state, CitizenState citizen)
        {
            List<WorldEntityId> nearbyCitizenIds = state.GetCitizenIdsNear(citizen.CurrentSceneId, citizen.CurrentCoord);
            for (int i = 0; i < nearbyCitizenIds.Count; i++) {
                if (nearbyCitizenIds[i] != citizen.Id) {
                    return true;
                }
            }

            return false;
        }

        private static int StableHash(long seed, string citizenId, string targetPlaceId, string reason, int absoluteDay)
        {
            unchecked {
                int hash = (int)(seed ^ (seed >> 32));
                hash = AddString(hash, citizenId);
                hash = AddString(hash, targetPlaceId);
                hash = AddString(hash, reason);
                hash = (hash * 397) ^ absoluteDay;
                return hash;
            }
        }

        private static int AddString(int hash, string value)
        {
            unchecked {
                for (int i = 0; i < value.Length; i++) {
                    hash = (hash * 397) ^ value[i];
                }

                return hash;
            }
        }
    }
}
