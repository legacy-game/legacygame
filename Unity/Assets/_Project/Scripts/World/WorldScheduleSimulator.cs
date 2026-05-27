using System.Collections.Generic;
using Legacy.History;
using Legacy.Time;

namespace Legacy.World
{
    public static class WorldScheduleSimulator
    {
        public static List<HistoryEvent> AdvanceSchedules(WorldState state, HistoryLog history, GameDateTime previousTime)
        {
            var events = new List<HistoryEvent>();
            long currentAbsoluteMinute = ToAbsoluteMinute(state.CurrentTime);

            foreach (CitizenState citizen in state.CitizensById.Values) {
                if (TryProcessActiveGoal(state, history, citizen, currentAbsoluteMinute, events)) {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(citizen.Routine.RoutineId)) {
                    continue;
                }

                if (!RoutineCatalog.TryGet(citizen.Routine.RoutineId, out RoutineDefinition routine)) {
                    continue;
                }

                while (TrySelectDueStep(routine, previousTime, state.CurrentTime, citizen, out RoutineStepDefinition step, out int stage)) {
                    if (!state.TryGetPlace(step.TargetPlaceId, out PlaceState targetPlace)) {
                        citizen.SetRoutineProgress(citizen.Routine.ActiveStepId, $"Waiting for {step.TargetPlaceId}", currentAbsoluteMinute, citizen.ScheduleStage);
                        break;
                    }

                    CitizenGoalDefinition goal = step.CreateGoal();
                    TravelPlan travelPlan = CitizenTravelPlanner.Plan(state, citizen, goal, state.CurrentTime);
                    string intent = $"{step.Intent} via {travelPlan.DisplayName}";

                    state.MoveCitizen(citizen.Id, targetPlace.RegionId, targetPlace.SceneId, targetPlace.Id, goal.TargetCoord, goal.Activity);
                    citizen.SetRoutineProgress(step.Id, intent, currentAbsoluteMinute, stage);
                    events.Add(history.Create(
                        state.CurrentTime,
                        HistoryEventKind.CitizenMoved,
                        $"{step.ArrivalDescription} ({travelPlan.Reason})",
                        new[] { citizen.Id },
                        new[] { targetPlace.Id }));
                    TryQueueVisitTask(state, history, citizen, targetPlace, step, events);
                }
            }

            return events;
        }

        private static void TryQueueVisitTask(
            WorldState state,
            HistoryLog history,
            CitizenState citizen,
            PlaceState targetPlace,
            RoutineStepDefinition step,
            List<HistoryEvent> events)
        {
            if (!TryGetVisitTemplate(citizen.Id, targetPlace.Id, out WorldEntityId workplaceId, out string taskDefinitionId, out string intent, out string arrivalLine, out string completionLine)) {
                return;
            }

            if (!state.TryGetWorkplace(workplaceId, out WorkplaceState workplace)) {
                return;
            }

            if (state.HasOpenVisitFor(citizen.Id, workplace.Id, out VisitState _)) {
                return;
            }

            var visitId = new WorldEntityId($"visit_{citizen.Id.Value}_{step.Id}_{state.CurrentTime.Date.AbsoluteDay}");
            var taskId = new WorldEntityId($"task_{visitId.Value}");
            if (state.TryGetVisit(visitId, out VisitState _) || state.TryGetJobTask(taskId, out JobTaskState _)) {
                return;
            }

            var visit = new VisitState(
                visitId,
                citizen.Id,
                workplace.Id,
                targetPlace.Id,
                intent,
                taskDefinitionId,
                default,
                VisitStatus.Arrived,
                state.CurrentTime,
                state.CurrentTime,
                arrivalLine,
                completionLine);
            var task = new JobTaskState(
                taskId,
                taskDefinitionId,
                workplace.Id,
                default,
                default,
                citizen.Id,
                JobTaskStatus.Queued,
                0,
                state.CurrentTime,
                state.CurrentTime,
                state.CurrentTime);

            visit.LinkTask(task.Id);
            state.AddVisit(visit);
            state.AddJobTask(task);

            events.Add(history.Create(
                state.CurrentTime,
                HistoryEventKind.VisitArrived,
                $"{citizen.DisplayName} arrived: {intent}. {arrivalLine}",
                new[] { citizen.Id },
                new[] { targetPlace.Id }));
            events.Add(history.Create(
                state.CurrentTime,
                HistoryEventKind.JobTaskCreated,
                $"{citizen.DisplayName} created task: {intent}.",
                new[] { citizen.Id },
                new[] { targetPlace.Id }));
        }

        private static bool TryGetVisitTemplate(
            WorldEntityId citizenId,
            WorldEntityId placeId,
            out WorldEntityId workplaceId,
            out string taskDefinitionId,
            out string intent,
            out string arrivalLine,
            out string completionLine)
        {
            if (citizenId == new WorldEntityId("citizen_mr_holland") && placeId == new WorldEntityId("place_linden_cafe_interior")) {
                workplaceId = new WorldEntityId("workplace_linden_cafe");
                taskDefinitionId = JobTaskCatalog.ServeCafeCustomer;
                intent = "Coffee order";
                arrivalLine = "Morning. Coffee, please.";
                completionLine = "Thanks. Needed that.";
                return true;
            }

            if (citizenId == new WorldEntityId("citizen_sasha") && placeId == new WorldEntityId("place_pell_pharmacy_interior")) {
                workplaceId = new WorldEntityId("workplace_pell_pharmacy");
                taskDefinitionId = JobTaskCatalog.StockPharmacyShelves;
                intent = "Restock pharmacy shelves";
                arrivalLine = "Delivery came in. Shelves need doing.";
                completionLine = "Good. That saves me a trip.";
                return true;
            }

            workplaceId = default;
            taskDefinitionId = string.Empty;
            intent = string.Empty;
            arrivalLine = string.Empty;
            completionLine = string.Empty;
            return false;
        }

        private static bool TryProcessActiveGoal(
            WorldState state,
            HistoryLog history,
            CitizenState citizen,
            long currentAbsoluteMinute,
            List<HistoryEvent> events)
        {
            if (!state.TryGetActiveCitizenGoal(citizen.Id, state.CurrentTime, out CitizenGoalState goal)) {
                return false;
            }

            CitizenGoalEvaluationResult evaluation = CitizenGoalEvaluator.Evaluate(state, citizen, goal, state.CurrentTime);
            if (evaluation.Status == CitizenGoalEvaluationStatus.Wait) {
                citizen.SetCurrentIntent($"Waiting: {evaluation.Reason}", currentAbsoluteMinute);
                return true;
            }

            if (evaluation.Status == CitizenGoalEvaluationStatus.Fail) {
                goal.Fail();
                events.Add(history.Create(
                    state.CurrentTime,
                    HistoryEventKind.CitizenGoalFailed,
                    $"{citizen.DisplayName} could not complete goal: {goal.Reason}. {evaluation.Reason}",
                    new[] { citizen.Id },
                    new[] { goal.TargetPlaceId }));
                return true;
            }

            if (!state.TryGetPlace(goal.TargetPlaceId, out PlaceState targetPlace)) {
                goal.Fail();
                events.Add(history.Create(
                    state.CurrentTime,
                    HistoryEventKind.CitizenGoalFailed,
                    $"{citizen.DisplayName} could not complete goal: {goal.Reason}.",
                    new[] { citizen.Id },
                    new[] { goal.TargetPlaceId }));
                return true;
            }

            CitizenGoalDefinition definition = goal.ToDefinition();
            TravelPlan travelPlan = CitizenTravelPlanner.Plan(state, citizen, definition, state.CurrentTime);
            state.MoveCitizen(citizen.Id, targetPlace.RegionId, targetPlace.SceneId, targetPlace.Id, definition.TargetCoord, definition.Activity);
            citizen.SetCurrentIntent($"{goal.Reason} via {travelPlan.DisplayName}", currentAbsoluteMinute);
            goal.Complete();

            events.Add(history.Create(
                state.CurrentTime,
                HistoryEventKind.CitizenMoved,
                $"{citizen.DisplayName} completed goal: {goal.Reason}. ({travelPlan.Reason})",
                new[] { citizen.Id },
                new[] { targetPlace.Id }));
            events.Add(history.Create(
                state.CurrentTime,
                HistoryEventKind.CitizenGoalCompleted,
                $"{citizen.DisplayName} completed goal: {goal.Reason}.",
                new[] { citizen.Id },
                new[] { targetPlace.Id }));
            return true;
        }

        private static bool TrySelectDueStep(
            RoutineDefinition routine,
            GameDateTime previousTime,
            GameDateTime currentTime,
            CitizenState citizen,
            out RoutineStepDefinition selectedStep,
            out int selectedStage)
        {
            selectedStep = default;
            selectedStage = 0;

            for (int i = 0; i < routine.Steps.Count; i++) {
                RoutineStepDefinition step = routine.Steps[i];
                int stage = i + 1;

                if (citizen.ScheduleStage >= stage || !Crossed(previousTime, currentTime, step.StartMinute)) {
                    continue;
                }

                if (!CanInterruptCurrentStep(citizen, routine)) {
                    continue;
                }

                selectedStep = step;
                selectedStage = stage;
                return true;
            }

            return false;
        }

        private static bool CanInterruptCurrentStep(CitizenState citizen, RoutineDefinition routine)
        {
            if (string.IsNullOrWhiteSpace(citizen.Routine.ActiveStepId)) {
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

        private static bool Crossed(GameDateTime previousTime, GameDateTime currentTime, int triggerMinute)
        {
            long previousAbsoluteMinute = ToAbsoluteMinute(previousTime);
            long currentAbsoluteMinute = ToAbsoluteMinute(currentTime);
            long firstTriggerAbsoluteMinute = (long)previousTime.Date.AbsoluteDay * 1440L + triggerMinute;

            for (long trigger = firstTriggerAbsoluteMinute; trigger <= currentAbsoluteMinute; trigger += 1440L) {
                if (previousAbsoluteMinute < trigger && currentAbsoluteMinute >= trigger) {
                    return true;
                }
            }

            return false;
        }

        private static long ToAbsoluteMinute(GameDateTime dateTime)
        {
            return (long)dateTime.Date.AbsoluteDay * 1440L + dateTime.Time.TotalMinutes;
        }
    }
}
