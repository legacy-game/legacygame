using System.Collections.Generic;
using Legacy.Time;

namespace Legacy.World
{
    public static class CitizenGoalEvaluator
    {
        public static CitizenGoalEvaluationResult Evaluate(WorldState state, CitizenState citizen, CitizenGoalState goal, GameDateTime now)
        {
            if (goal.Kind != CitizenGoalKind.TravelToPlace) {
                return CitizenGoalEvaluationResult.Fail($"Unsupported goal kind: {goal.Kind}.");
            }

            if (IsAfter(now, goal.ExpiresAt)) {
                return CitizenGoalEvaluationResult.Fail($"Goal expired: {goal.Reason}.");
            }

            if (!state.TryGetPlace(goal.TargetPlaceId, out PlaceState _)) {
                return CitizenGoalEvaluationResult.Fail($"Target place not found: {goal.TargetPlaceId}.");
            }

            if (!CanInterruptCurrentRoutine(citizen)) {
                return CitizenGoalEvaluationResult.Wait($"{citizen.DisplayName} is busy with a non-interruptible routine.");
            }

            var propertySystem = new PropertySystem(state);
            PropertyAccessResult access = propertySystem.CheckPlaceAccess(goal.TargetPlaceId, citizen.Id);
            if (!access.IsAllowed) {
                return CitizenGoalEvaluationResult.Wait(access.Reason);
            }

            List<TravelOption> options = CitizenTravelPlanner.GetOptions(state, citizen, goal.ToDefinition());
            if (options.Count == 0) {
                return CitizenGoalEvaluationResult.Wait("No valid travel approach is currently available.");
            }

            return CitizenGoalEvaluationResult.CanAttempt();
        }

        private static bool CanInterruptCurrentRoutine(CitizenState citizen)
        {
            if (string.IsNullOrWhiteSpace(citizen.Routine.RoutineId) || string.IsNullOrWhiteSpace(citizen.Routine.ActiveStepId)) {
                return true;
            }

            if (!RoutineCatalog.TryGet(citizen.Routine.RoutineId, out RoutineDefinition routine)) {
                return true;
            }

            for (int i = 0; i < routine.Steps.Count; i++) {
                RoutineStepDefinition step = routine.Steps[i];
                if (step.Id == citizen.Routine.ActiveStepId) {
                    return step.IsInterruptible;
                }
            }

            return true;
        }

        private static bool IsAfter(GameDateTime left, GameDateTime right)
        {
            return ToAbsoluteMinute(left) > ToAbsoluteMinute(right);
        }

        private static long ToAbsoluteMinute(GameDateTime dateTime)
        {
            return (long)dateTime.Date.AbsoluteDay * 1440L + dateTime.Time.TotalMinutes;
        }
    }
}
