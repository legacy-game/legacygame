using Legacy.Commands;
using Legacy.History;
using Legacy.Time;
using Legacy.World;
using NUnit.Framework;

namespace Legacy.Tests.EditMode
{
    public sealed class WorldCommandTests
    {
        [Test]
        public void InspectBuilding_WritesHistoryEvent()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());

            WorldCommandResult result = runtime.Execute(new InspectBuildingCommand(
                new WorldEntityId("building_linden_cafe"),
                new WorldEntityId("citizen_noaharan")));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.RecentHistory[^1].Kind, Is.EqualTo(HistoryEventKind.BuildingInspected));
        }

        [Test]
        public void InspectTerritory_WritesHistoryEvent()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());

            WorldCommandResult result = runtime.Execute(new Legacy.Commands.InspectTerritoryCommand(
                new WorldEntityId("territory_aldwich_south_grassland"),
                new WorldEntityId("citizen_noaharan")));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Message, Does.Contain("South Aldwich Grassland"));
            Assert.That(runtime.State.RecentHistory[^1].Kind, Is.EqualTo(HistoryEventKind.TerritoryInspected));
        }

        [Test]
        public void ClaimTerritory_ChangesClaimAndWritesHistory()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var territoryId = new WorldEntityId("territory_aldwich_south_grassland");
            var ownerId = new WorldEntityId("citizen_noaharan");

            WorldCommandResult result = runtime.Execute(new ClaimTerritoryCommand(
                territoryId,
                ownerId,
                ownerId));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.TerritoryChunksById[territoryId].ClaimStatus, Is.EqualTo(TerritoryClaimStatus.Private));
            Assert.That(runtime.State.TerritoryChunksById[territoryId].ClaimOwnerId, Is.EqualTo(ownerId));
            Assert.That(runtime.State.RecentHistory[^1].Kind, Is.EqualTo(HistoryEventKind.TerritoryClaimed));
        }

        [Test]
        public void RoleSystem_AuthorizesActionsByAssignedRoleAndPlace()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var noahId = new WorldEntityId("citizen_noaharan");
            var cafePlaceId = new WorldEntityId("place_linden_cafe_interior");

            runtime.Execute(new AcceptJobCommand(
                new WorldEntityId("roleassign_noah_cafe_worker"),
                noahId,
                RoleCatalog.CafeWorker,
                cafePlaceId));

            var roleSystem = new RoleSystem(runtime.State);
            RoleAuthorizationResult serve = roleSystem.CanPerform(noahId, WorldActionKind.ServeCustomer, cafePlaceId);
            RoleAuthorizationResult patrol = roleSystem.CanPerform(noahId, WorldActionKind.PatrolDistrict, cafePlaceId);

            Assert.That(serve.IsAllowed, Is.True);
            Assert.That(serve.RoleId, Is.EqualTo(RoleCatalog.CafeWorker));
            Assert.That(patrol.IsAllowed, Is.False);
        }

        [Test]
        public void AcceptJobCommand_AddsRoleAndWritesHistory()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var noahId = new WorldEntityId("citizen_noaharan");
            var pharmacyPlaceId = new WorldEntityId("place_pell_pharmacy_interior");

            WorldCommandResult result = runtime.Execute(new AcceptJobCommand(
                new WorldEntityId("roleassign_noah_pharmacy_clerk"),
                noahId,
                RoleCatalog.Shopkeeper,
                pharmacyPlaceId));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(new RoleSystem(runtime.State).CanPerform(noahId, WorldActionKind.StockShelves, pharmacyPlaceId).IsAllowed, Is.True);
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.JobAccepted).Count, Is.EqualTo(1));
        }

        [Test]
        public void AssignRoleCommand_AddsAssignmentAndWritesHistory()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var rowanId = new WorldEntityId("citizen_rowan");
            var cafePlaceId = new WorldEntityId("place_linden_cafe_interior");

            WorldCommandResult result = runtime.Execute(new AssignRoleCommand(
                new WorldEntityId("roleassign_rowan_transit"),
                rowanId,
                RoleCatalog.TransitDriver,
                cafePlaceId,
                new WorldEntityId("citizen_noaharan")));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.RoleAssignments.Count, Is.GreaterThanOrEqualTo(4));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.RoleAssigned).Count, Is.EqualTo(1));
        }

        [Test]
        public void DoWorldAction_JobActionRequiresQueuedTaskFlow()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            int startingCafeBalance = runtime.State.MoneyAccountsById[new WorldEntityId("account_linden_cafe")].BalanceCents;
            int startingActorBalance = runtime.State.MoneyAccountsById[new WorldEntityId("account_rowan_cash")].BalanceCents;

            WorldCommandResult result = runtime.Execute(new DoWorldActionCommand(
                new WorldEntityId("citizen_rowan"),
                WorldActionKind.ServeCustomer,
                new WorldEntityId("place_linden_cafe_interior")));

            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Message, Does.Contain("No queued work"));
            Assert.That(runtime.State.MoneyAccountsById[new WorldEntityId("account_linden_cafe")].BalanceCents, Is.EqualTo(startingCafeBalance));
            Assert.That(runtime.State.MoneyAccountsById[new WorldEntityId("account_rowan_cash")].BalanceCents, Is.EqualTo(startingActorBalance));
            Assert.That(runtime.State.Transactions.Count, Is.EqualTo(0));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.PaymentRecorded).Count, Is.EqualTo(0));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.WorldActionPerformed).Count, Is.EqualTo(0));
        }

        [Test]
        public void DoWorldAction_UnauthorizedRoleFailsWithoutHistory()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            int startingCafeBalance = runtime.State.MoneyAccountsById[new WorldEntityId("account_linden_cafe")].BalanceCents;

            WorldCommandResult result = runtime.Execute(new DoWorldActionCommand(
                new WorldEntityId("citizen_noaharan"),
                WorldActionKind.PatrolDistrict,
                new WorldEntityId("place_linden_cafe_interior")));

            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Message, Does.Contain("not authorized"));
            Assert.That(runtime.State.MoneyAccountsById[new WorldEntityId("account_linden_cafe")].BalanceCents, Is.EqualTo(startingCafeBalance));
            Assert.That(runtime.State.Transactions.Count, Is.EqualTo(0));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.PaymentRecorded).Count, Is.EqualTo(0));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.WorldActionPerformed).Count, Is.EqualTo(0));
        }

        [Test]
        public void EconomyTransfer_RejectsInsufficientFunds()
        {
            WorldState state = WorldFactory.CreateVeyneSeedWorld();
            var economy = new EconomySystem(state);
            int startingHollandBalance = state.MoneyAccountsById[new WorldEntityId("account_holland_cash")].BalanceCents;
            int startingCafeBalance = state.MoneyAccountsById[new WorldEntityId("account_linden_cafe")].BalanceCents;

            EconomyTransferResult result = economy.Transfer(
                new WorldEntityId("account_holland_cash"),
                new WorldEntityId("account_linden_cafe"),
                startingHollandBalance + 1,
                "Impossible purchase",
                new WorldEntityId("place_linden_cafe_interior"),
                WorldActionKind.ServeCustomer,
                state.CurrentTime);

            Assert.That(result.Succeeded, Is.False);
            Assert.That(state.MoneyAccountsById[new WorldEntityId("account_holland_cash")].BalanceCents, Is.EqualTo(startingHollandBalance));
            Assert.That(state.MoneyAccountsById[new WorldEntityId("account_linden_cafe")].BalanceCents, Is.EqualTo(startingCafeBalance));
            Assert.That(state.Transactions.Count, Is.EqualTo(0));
        }

        [Test]
        public void RegisterCitizen_AddsRegistrationFirstGoalAndHistory()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var citizenId = new WorldEntityId("citizen_noaharan");

            WorldCommandResult result = runtime.Execute(new RegisterCitizenCommand(
                citizenId,
                new WorldEntityId("place_linden_street"),
                new WorldEntityId("place_linden_street"),
                new WorldEntityId("place_linden_cafe_interior"),
                new GridCoord(4, 3)));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.TryGetCitizenRegistration(citizenId, out CitizenRegistrationState registration), Is.True);
            Assert.That(registration.StartingResidencePlaceId, Is.EqualTo(new WorldEntityId("place_linden_street")));
            Assert.That(runtime.State.CitizenGoals.Count, Is.EqualTo(1));
            Assert.That(runtime.State.CitizenGoals[0].Reason, Is.EqualTo("Visit Linden Cafe"));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.CitizenRegistered).Count, Is.EqualTo(1));
        }

        [Test]
        public void RegisterCitizen_FailsIfAlreadyRegistered()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var citizenId = new WorldEntityId("citizen_noaharan");
            var command = new RegisterCitizenCommand(
                citizenId,
                new WorldEntityId("place_linden_street"),
                new WorldEntityId("place_linden_street"),
                new WorldEntityId("place_linden_cafe_interior"),
                new GridCoord(4, 3));

            runtime.Execute(command);
            WorldCommandResult secondResult = runtime.Execute(command);

            Assert.That(secondResult.Succeeded, Is.False);
            Assert.That(secondResult.Message, Does.Contain("already registered"));
            Assert.That(runtime.State.CitizenRegistrations.Count, Is.EqualTo(1));
        }

        [Test]
        public void FileCivicReport_AddsReportRegistryReputationAndHistory()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var reporterId = new WorldEntityId("citizen_noaharan");
            var subjectId = new WorldEntityId("citizen_rowan");
            var placeId = new WorldEntityId("place_linden_cafe_interior");

            WorldCommandResult result = runtime.Execute(new FileCivicReportCommand(
                new WorldEntityId("civic_report_rowan_noise"),
                reporterId,
                subjectId,
                placeId,
                "Kept the cafe open after quiet hours.",
                -2));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.CivicReportsById.Count, Is.EqualTo(1));
            Assert.That(runtime.State.CivicRegistryEntries.Count, Is.EqualTo(1));
            Assert.That(runtime.State.PublicRecordsByCitizenId[subjectId].ReputationScore, Is.EqualTo(-2));
            Assert.That(runtime.State.PublicRecordsByCitizenId[subjectId].ReportsReceived, Is.EqualTo(1));
            Assert.That(runtime.State.PublicRecordsByCitizenId[reporterId].ReportsFiled, Is.EqualTo(1));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.CivicReportFiled).Count, Is.EqualTo(1));
            Assert.That(runtime.State.GetHistoryForActor(subjectId)[^1].Kind, Is.EqualTo(HistoryEventKind.CivicReportFiled));
        }

        [Test]
        public void QueryPublicRecord_ReturnsRegistryAndHistorySummary()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var reporterId = new WorldEntityId("citizen_noaharan");
            var subjectId = new WorldEntityId("citizen_rowan");

            runtime.Execute(new FileCivicReportCommand(
                new WorldEntityId("civic_report_rowan_noise"),
                reporterId,
                subjectId,
                new WorldEntityId("place_linden_cafe_interior"),
                "Kept the cafe open after quiet hours.",
                -2));

            WorldCommandResult result = runtime.Execute(new QueryPublicRecordCommand(subjectId, reporterId));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Message, Does.Contain("Public record: Rowan"));
            Assert.That(result.Message, Does.Contain("Reputation: -2"));
            Assert.That(result.Message, Does.Contain("CivicReport: Kept the cafe open after quiet hours."));
            Assert.That(result.Message, Does.Contain("CivicReportFiled"));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.PublicRecordQueried).Count, Is.EqualTo(1));
        }

        [Test]
        public void TransferBuildingOwnership_ChangesOwnerAndWritesHistory()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var buildingId = new WorldEntityId("building_linden_cafe");
            var rowanId = new WorldEntityId("citizen_rowan");

            WorldCommandResult result = runtime.Execute(new TransferBuildingOwnershipCommand(
                buildingId,
                rowanId,
                new WorldEntityId("citizen_noaharan")));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.BuildingsById[buildingId].OwnerCitizenId, Is.EqualTo(rowanId));
            Assert.That(runtime.State.RecentHistory[^1].Kind, Is.EqualTo(HistoryEventKind.BuildingOwnershipTransferred));
        }

        [Test]
        public void AdvanceTime_CrossingHollandVisit_MovesCitizenAsData()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var hollandId = new WorldEntityId("citizen_old_mr_pell");

            WorldCommandResult result = runtime.Execute(new AdvanceTimeCommand(65));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.CitizensById[hollandId].Activity, Is.EqualTo(CitizenActivityState.Visiting));
            Assert.That(runtime.State.CitizensById[hollandId].CurrentPlaceId, Is.EqualTo(new WorldEntityId("place_linden_cafe_interior")));
            Assert.That(runtime.State.CitizensById[hollandId].ScheduleStage, Is.EqualTo(1));
            Assert.That(runtime.State.CitizensById[hollandId].Routine.ActiveStepId, Is.EqualTo("morning_cafe_visit"));
            Assert.That(runtime.State.CitizensById[hollandId].Routine.CurrentIntent, Does.StartWith("Visit Linden Cafe via "));
            Assert.That(runtime.State.RecentHistory[^1].Description, Does.Contain("arrived"));
        }

        [Test]
        public void AdvanceTime_CrossingHollandLeave_MovesCitizenBackToStreet()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var hollandId = new WorldEntityId("citizen_old_mr_pell");

            WorldCommandResult result = runtime.Execute(new AdvanceTimeCommand(85));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.CitizensById[hollandId].Activity, Is.EqualTo(CitizenActivityState.Offscreen));
            Assert.That(runtime.State.CitizensById[hollandId].CurrentPlaceId, Is.EqualTo(new WorldEntityId("place_linden_street")));
            Assert.That(runtime.State.CitizensById[hollandId].ScheduleStage, Is.EqualTo(2));
            Assert.That(runtime.State.CitizensById[hollandId].Routine.ActiveStepId, Is.EqualTo("return_to_street"));
            Assert.That(runtime.State.CitizensById[hollandId].Routine.CurrentIntent, Does.StartWith("Leave Linden Cafe via "));
            Assert.That(runtime.State.RecentHistory[^1].Description, Does.Contain("left"));
        }

        [Test]
        public void AdvanceTime_EvaluatesMultipleCitizenRoutines()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var rowanId = new WorldEntityId("citizen_rowan");
            var pellId = new WorldEntityId("citizen_old_mr_pell");

            WorldCommandResult result = runtime.Execute(new AdvanceTimeCommand(65));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.CitizensById[rowanId].Routine.ActiveStepId, Is.EqualTo("open_cafe_shift"));
            Assert.That(runtime.State.CitizensById[rowanId].Routine.CurrentIntent, Does.StartWith("Work cafe shift via "));
            Assert.That(runtime.State.CitizensById[pellId].Routine.ActiveStepId, Is.EqualTo("morning_cafe_visit"));
            Assert.That(runtime.State.GetHistoryForActor(rowanId).Count, Is.GreaterThanOrEqualTo(1));
            Assert.That(runtime.State.GetHistoryForActor(pellId).Count, Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void TravelPlanner_EvaluatesDataDefinedApproaches()
        {
            WorldState state = WorldFactory.CreateVeyneSeedWorld();
            CitizenState pell = state.CitizensById[new WorldEntityId("citizen_old_mr_pell")];
            var goal = new CitizenGoalDefinition(
                CitizenGoalKind.TravelToPlace,
                new WorldEntityId("place_linden_cafe_interior"),
                new GridCoord(4, 3),
                CitizenActivityState.Visiting,
                75,
                "Go to Linden Cafe");
            var approaches = new[] {
                new TravelApproachDefinition("approach_slow", "Slow approach", 20, 0, 0, 30, 1, false, 0, false, "Take the slow route."),
                new TravelApproachDefinition("approach_fast", "Fast approach", 80, 0, 0, 5, 0, false, 0, false, "Take the fast route."),
                new TravelApproachDefinition("approach_social", "Social approach", 40, 0, 0, 10, 0, false, 0, true, "Ask someone for help.")
            };

            var options = CitizenTravelPlanner.GetOptions(state, pell, goal, approaches);
            TravelPlan plan = CitizenTravelPlanner.Plan(state, pell, goal, state.CurrentTime);

            Assert.That(HasApproach(options, "approach_slow"), Is.True);
            Assert.That(HasApproach(options, "approach_fast"), Is.True);
            Assert.That(HasApproach(options, "approach_social"), Is.True);
            Assert.That(string.IsNullOrWhiteSpace(plan.ApproachId), Is.False);
        }

        [Test]
        public void AddCitizenGoalCommand_AddsGoalAndWritesHistory()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var pellId = new WorldEntityId("citizen_old_mr_pell");
            var goal = new CitizenGoalDefinition(
                CitizenGoalKind.TravelToPlace,
                new WorldEntityId("place_linden_cafe_interior"),
                new GridCoord(4, 3),
                CitizenActivityState.Visiting,
                90,
                "Make a special visit");

            WorldCommandResult result = runtime.Execute(new AddCitizenGoalCommand(
                new WorldEntityId("goal_pell_special_visit"),
                pellId,
                goal,
                runtime.State.CurrentTime.AddMinutes(120),
                new WorldEntityId("citizen_noaharan")));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.CitizenGoals.Count, Is.EqualTo(1));
            Assert.That(runtime.State.CitizenGoals[0].CitizenId, Is.EqualTo(pellId));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.CitizenGoalAdded).Count, Is.EqualTo(1));
        }

        [Test]
        public void SmokeGoalCommand_AddsVisiblePellPharmacyGoal()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var pellId = new WorldEntityId("citizen_old_mr_pell");
            var pharmacyPlaceId = new WorldEntityId("place_pell_pharmacy_interior");
            var goal = new CitizenGoalDefinition(
                CitizenGoalKind.TravelToPlace,
                pharmacyPlaceId,
                new GridCoord(18, 7),
                CitizenActivityState.Visiting,
                100,
                "Go to Pell Pharmacy");

            WorldCommandResult result = runtime.Execute(new AddCitizenGoalCommand(
                new WorldEntityId("goal_pell_pharmacy_smoke"),
                pellId,
                goal,
                runtime.State.CurrentTime.AddMinutes(180),
                new WorldEntityId("citizen_noaharan")));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.CitizenGoals.Count, Is.EqualTo(1));
            Assert.That(runtime.State.CitizenGoals[0].Status, Is.EqualTo(CitizenGoalStatus.Active));
            Assert.That(runtime.State.CitizenGoals[0].TargetPlaceId, Is.EqualTo(pharmacyPlaceId));
            Assert.That(runtime.State.CitizenGoals[0].Reason, Is.EqualTo("Go to Pell Pharmacy"));
        }

        [Test]
        public void AdvanceTime_ActiveGoalOverridesRoutineStep()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var pellId = new WorldEntityId("citizen_old_mr_pell");
            var pharmacyPlaceId = new WorldEntityId("place_pell_pharmacy_interior");
            var goal = new CitizenGoalDefinition(
                CitizenGoalKind.TravelToPlace,
                pharmacyPlaceId,
                new GridCoord(2, 2),
                CitizenActivityState.Visiting,
                100,
                "Go to the pharmacy");

            runtime.Execute(new AddCitizenGoalCommand(
                new WorldEntityId("goal_pell_pharmacy"),
                pellId,
                goal,
                runtime.State.CurrentTime.AddMinutes(120),
                new WorldEntityId("citizen_noaharan")));
            WorldCommandResult result = runtime.Execute(new AdvanceTimeCommand(65));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.CitizensById[pellId].CurrentPlaceId, Is.EqualTo(pharmacyPlaceId));
            Assert.That(runtime.State.CitizensById[pellId].Routine.ActiveStepId, Is.EqualTo(string.Empty));
            Assert.That(runtime.State.CitizensById[pellId].ScheduleStage, Is.EqualTo(0));
            Assert.That(runtime.State.CitizenGoals[0].Status, Is.EqualTo(CitizenGoalStatus.Completed));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.CitizenGoalCompleted).Count, Is.EqualTo(1));
        }

        [Test]
        public void AdvanceTime_GoalWaitsWhenTargetAccessIsDenied()
        {
            var state = CreateAccessRuleWorld(AccessRule.EmployeesOnly);
            var runtime = new WorldRuntime(state);
            var visitorId = new WorldEntityId("citizen_visitor");
            var goal = new CitizenGoalDefinition(
                CitizenGoalKind.TravelToPlace,
                new WorldEntityId("place_test_shop"),
                new GridCoord(3, 3),
                CitizenActivityState.Visiting,
                80,
                "Visit employee-only shop");

            runtime.Execute(new AddCitizenGoalCommand(
                new WorldEntityId("goal_visit_employee_shop"),
                visitorId,
                goal,
                runtime.State.CurrentTime.AddMinutes(120),
                visitorId));
            runtime.Execute(new AdvanceTimeCommand(10));

            Assert.That(runtime.State.CitizenGoals[0].Status, Is.EqualTo(CitizenGoalStatus.Active));
            Assert.That(runtime.State.CitizensById[visitorId].CurrentPlaceId, Is.EqualTo(new WorldEntityId("place_test_street")));
            Assert.That(runtime.State.CitizensById[visitorId].Routine.CurrentIntent, Does.StartWith("Waiting:"));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.CitizenGoalCompleted).Count, Is.EqualTo(0));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.CitizenGoalFailed).Count, Is.EqualTo(0));
        }

        [Test]
        public void AdvanceTime_GoalFailsWhenTargetPlaceIsMissing()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var pellId = new WorldEntityId("citizen_old_mr_pell");
            var goal = new CitizenGoalDefinition(
                CitizenGoalKind.TravelToPlace,
                new WorldEntityId("place_missing"),
                new GridCoord(1, 1),
                CitizenActivityState.Visiting,
                80,
                "Visit missing place");

            runtime.State.AddCitizenGoal(new CitizenGoalState(
                new WorldEntityId("goal_missing_place"),
                pellId,
                goal,
                runtime.State.CurrentTime,
                runtime.State.CurrentTime.AddMinutes(120)));
            runtime.Execute(new AdvanceTimeCommand(10));

            Assert.That(runtime.State.CitizenGoals[0].Status, Is.EqualTo(CitizenGoalStatus.Failed));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.CitizenGoalFailed).Count, Is.EqualTo(1));
            Assert.That(runtime.State.RecentHistory[^1].Description, Does.Contain("Target place not found"));
        }

        [Test]
        public void AdvanceTime_UsesRuntimeClock()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());

            WorldCommandResult result = runtime.Execute(new AdvanceTimeCommand(10));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.Clock.Now, Is.EqualTo(runtime.State.CurrentTime));
            Assert.That(runtime.State.CurrentTime.Time, Is.EqualTo(new TimeOfDay(6, 40)));
        }

        [Test]
        public void PropertySystem_SummarizesBuildingPlotAndPlace()
        {
            WorldState state = WorldFactory.CreateVeyneSeedWorld();
            var propertySystem = new PropertySystem(state);

            bool hasBuilding = propertySystem.TryGetBuildingSummary(new WorldEntityId("building_pell_pharmacy"), out BuildingPropertySummary building);
            bool hasPlot = propertySystem.TryGetPlotSummary(new WorldEntityId("plot_linden_16"), out PlotPropertySummary plot);
            bool hasPlace = propertySystem.TryGetPlaceSummary(new WorldEntityId("place_pell_pharmacy_interior"), out PlacePropertySummary place);

            Assert.That(hasBuilding, Is.True);
            Assert.That(building.OwnerName, Is.EqualTo("Old Mr. Pell"));
            Assert.That(building.PlotName, Is.EqualTo("16 Linden Street"));
            Assert.That(hasPlot, Is.True);
            Assert.That(plot.BuildingCount, Is.EqualTo(1));
            Assert.That(hasPlace, Is.True);
            Assert.That(place.BuildingName, Is.EqualTo("Pell Pharmacy"));
            Assert.That(place.AccessRule, Is.EqualTo(AccessRule.Public));
        }

        [Test]
        public void PropertySystem_ChecksEmployeeOnlyAccess()
        {
            WorldState state = CreateAccessRuleWorld(AccessRule.EmployeesOnly);
            var propertySystem = new PropertySystem(state);

            PropertyAccessResult ownerAccess = propertySystem.CheckBuildingAccess(new WorldEntityId("building_test_shop"), new WorldEntityId("citizen_owner"));
            PropertyAccessResult employeeAccess = propertySystem.CheckBuildingAccess(new WorldEntityId("building_test_shop"), new WorldEntityId("citizen_employee"));
            PropertyAccessResult visitorAccess = propertySystem.CheckBuildingAccess(new WorldEntityId("building_test_shop"), new WorldEntityId("citizen_visitor"));

            Assert.That(ownerAccess.IsAllowed, Is.True);
            Assert.That(employeeAccess.IsAllowed, Is.True);
            Assert.That(visitorAccess.IsAllowed, Is.False);
        }

        [Test]
        public void InspectPlotAndPlace_WriteHistoryEvents()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var actorId = new WorldEntityId("citizen_noaharan");

            WorldCommandResult plotResult = runtime.Execute(new InspectPlotCommand(new WorldEntityId("plot_linden_16"), actorId));
            WorldCommandResult placeResult = runtime.Execute(new InspectPlaceCommand(new WorldEntityId("place_pell_pharmacy_interior"), actorId));

            Assert.That(plotResult.Succeeded, Is.True);
            Assert.That(placeResult.Succeeded, Is.True);
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.PlotInspected).Count, Is.EqualTo(1));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.PlaceInspected).Count, Is.EqualTo(1));
        }

        [Test]
        public void TalkToCitizen_WritesDialogueRelationshipMemoryAndHistory()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var actorId = new WorldEntityId("citizen_noaharan");
            var rowanId = new WorldEntityId("citizen_rowan");

            WorldCommandResult result = runtime.Execute(new TalkToCitizenCommand(actorId, rowanId, "morning"));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Message, Does.Contain("Rowan:"));
            Assert.That(runtime.State.DialogueStatesByCitizenId[rowanId].LastLineId, Is.EqualTo(DialogueCatalog.RowanMorningGreeting));
            Assert.That(runtime.State.DialogueStatesByCitizenId[rowanId].ConversationCount, Is.EqualTo(1));

            RelationshipState relationship = runtime.State.GetOrCreateRelationship(rowanId, actorId);
            Assert.That(relationship.Familiarity, Is.EqualTo(1));
            Assert.That(relationship.Affinity, Is.EqualTo(1));

            Assert.That(runtime.State.GetMemoriesForCitizen(rowanId).Count, Is.EqualTo(1));
            Assert.That(runtime.State.GetMemoriesForCitizen(rowanId)[0].SubjectCitizenId, Is.EqualTo(actorId));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.CitizenTalked).Count, Is.EqualTo(1));
        }

        [Test]
        public void HistoryIndexes_QueryByActorPlaceKindAndTimeRange()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var cafeId = new WorldEntityId("building_linden_cafe");
            var noahId = new WorldEntityId("citizen_noaharan");

            runtime.Execute(new InspectBuildingCommand(cafeId, noahId));
            runtime.Execute(new TransferBuildingOwnershipCommand(cafeId, new WorldEntityId("citizen_rowan"), noahId));

            Assert.That(runtime.State.GetHistoryForActor(noahId).Count, Is.GreaterThanOrEqualTo(2));
            Assert.That(runtime.State.GetHistoryForPlace(cafeId).Count, Is.GreaterThanOrEqualTo(2));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.BuildingOwnershipTransferred).Count, Is.EqualTo(1));

            var eventsInMorning = runtime.State.GetHistoryBetween(
                new Legacy.Time.GameDateTime(new Legacy.Time.GameDate(2003, 5, 14), new Legacy.Time.TimeOfDay(6, 0)),
                new Legacy.Time.GameDateTime(new Legacy.Time.GameDate(2003, 5, 14), new Legacy.Time.TimeOfDay(7, 0)));

            Assert.That(eventsInMorning.Count, Is.GreaterThanOrEqualTo(3));
        }

        [Test]
        public void HistoryArchive_KeepsOldEventsQueryableAfterRecentWindowOverflows()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var cafeId = new WorldEntityId("building_linden_cafe");
            var noahId = new WorldEntityId("citizen_noaharan");

            for (int i = 0; i < WorldState.MaxRecentHistoryEvents + 10; i++) {
                runtime.Execute(new InspectBuildingCommand(cafeId, noahId));
            }

            Assert.That(runtime.State.RecentHistory.Count, Is.EqualTo(WorldState.MaxRecentHistoryEvents));
            Assert.That(runtime.State.HistoryArchive.Count, Is.EqualTo(11));
            Assert.That(runtime.State.GetHistoryForPlace(cafeId).Count, Is.EqualTo(WorldState.MaxRecentHistoryEvents + 11));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.BuildingInspected).Count, Is.EqualTo(WorldState.MaxRecentHistoryEvents + 10));

            var eventsInMorning = runtime.State.GetHistoryBetween(
                new Legacy.Time.GameDateTime(new Legacy.Time.GameDate(2003, 5, 14), new Legacy.Time.TimeOfDay(6, 0)),
                new Legacy.Time.GameDateTime(new Legacy.Time.GameDate(2003, 5, 14), new Legacy.Time.TimeOfDay(7, 0)));

            Assert.That(eventsInMorning.Count, Is.EqualTo(WorldState.MaxRecentHistoryEvents + 11));
        }

        private static WorldState CreateAccessRuleWorld(AccessRule accessRule)
        {
            var regionId = new WorldEntityId("region_test");
            var exteriorSceneId = new WorldEntityId("scene_test_street");
            var interiorSceneId = new WorldEntityId("scene_test_shop");
            var streetPlaceId = new WorldEntityId("place_test_street");
            var shopPlaceId = new WorldEntityId("place_test_shop");
            var plotId = new WorldEntityId("plot_test_shop");
            var buildingId = new WorldEntityId("building_test_shop");
            var ownerId = new WorldEntityId("citizen_owner");
            var employeeId = new WorldEntityId("citizen_employee");
            var visitorId = new WorldEntityId("citizen_visitor");

            var state = new WorldState(
                WorldFactory.CurrentSchemaVersion,
                1,
                new GameDateTime(new GameDate(2003, 5, 14), new TimeOfDay(6, 30)),
                exteriorSceneId);

            state.AddRegion(new RegionState(regionId, "Test", new GridBounds(new GridCoord(0, 0), 16, 16)));
            state.AddScene(new WorldSceneState(exteriorSceneId, regionId, "Street"));
            state.AddScene(new WorldSceneState(interiorSceneId, regionId, "Shop"));
            state.AddPlace(new PlaceState(streetPlaceId, regionId, exteriorSceneId, "Street", PlaceKind.PublicSpace));
            state.AddPlace(new PlaceState(shopPlaceId, regionId, interiorSceneId, "Shop", PlaceKind.Business));
            state.AddPlot(new PlotState(plotId, regionId, "Shop Plot", new GridBounds(new GridCoord(2, 2), 4, 4), ownerId, accessRule));
            state.AddBuilding(new BuildingState(buildingId, regionId, plotId, exteriorSceneId, interiorSceneId, shopPlaceId, "Test Shop", ownerId, PlaceKind.Business, new GridCoord(3, 3)));
            state.AddCitizen(new CitizenState(ownerId, "Owner", buildingId, buildingId, regionId, exteriorSceneId, streetPlaceId, new GridCoord(1, 1)));
            state.AddCitizen(new CitizenState(employeeId, "Employee", buildingId, buildingId, regionId, exteriorSceneId, streetPlaceId, new GridCoord(1, 2)));
            state.AddCitizen(new CitizenState(visitorId, "Visitor", buildingId, new WorldEntityId("building_elsewhere"), regionId, exteriorSceneId, streetPlaceId, new GridCoord(1, 3)));
            return state;
        }

        private static bool HasApproach(System.Collections.Generic.IReadOnlyList<TravelOption> options, string approachId)
        {
            for (int i = 0; i < options.Count; i++) {
                if (options[i].ApproachId == approachId) {
                    return true;
                }
            }

            return false;
        }
    }
}
