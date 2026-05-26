using Legacy.Commands;
using Legacy.Save;
using Legacy.World;
using NUnit.Framework;

namespace Legacy.Tests.EditMode
{
    public sealed class WorldSaveMapperTests
    {
        [Test]
        public void SaveRoundTrip_PreservesOwnershipTimeAndHistory()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var buildingId = new WorldEntityId("building_linden_cafe");
            var rowanId = new WorldEntityId("citizen_rowan");

            runtime.Execute(new AdvanceTimeCommand(10));
            runtime.Execute(new AdvanceTimeCommand(60));
            runtime.Execute(new TransferBuildingOwnershipCommand(
                buildingId,
                rowanId,
                new WorldEntityId("citizen_noaharan")));

            WorldSaveData saveData = WorldSaveMapper.ToSaveData(runtime.State);
            WorldState loaded = WorldSaveMapper.ToRuntime(saveData);
            var hollandId = new WorldEntityId("citizen_old_mr_pell");

            Assert.That(loaded.CurrentTime, Is.EqualTo(runtime.State.CurrentTime));
            Assert.That(loaded.BuildingsById[buildingId].OwnerCitizenId, Is.EqualTo(rowanId));
            Assert.That(loaded.CitizensById[hollandId].Activity, Is.EqualTo(CitizenActivityState.Visiting));
            Assert.That(loaded.CitizensById[hollandId].ScheduleStage, Is.EqualTo(1));
            Assert.That(loaded.CitizensById[hollandId].Routine.RoutineId, Is.EqualTo(RoutineCatalog.PellMorningRoutineId));
            Assert.That(loaded.CitizensById[hollandId].Routine.ActiveStepId, Is.EqualTo("morning_cafe_visit"));
            Assert.That(loaded.CitizensById[hollandId].Routine.CurrentIntent, Does.StartWith("Visit Linden Cafe via "));
            Assert.That(loaded.RecentHistory.Count, Is.EqualTo(runtime.State.RecentHistory.Count));
        }

        [Test]
        public void SaveRoundTrip_PreservesArchivedHistory()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var cafeId = new WorldEntityId("building_linden_cafe");
            var noahId = new WorldEntityId("citizen_noaharan");

            for (int i = 0; i < WorldState.MaxRecentHistoryEvents + 5; i++) {
                runtime.Execute(new InspectBuildingCommand(cafeId, noahId));
            }

            WorldSaveData saveData = WorldSaveMapper.ToSaveData(runtime.State);
            WorldState loaded = WorldSaveMapper.ToRuntime(saveData);

            Assert.That(loaded.RecentHistory.Count, Is.EqualTo(WorldState.MaxRecentHistoryEvents));
            Assert.That(loaded.HistoryArchive.Count, Is.EqualTo(runtime.State.HistoryArchive.Count));
            Assert.That(loaded.GetHistoryForPlace(cafeId).Count, Is.EqualTo(runtime.State.GetHistoryForPlace(cafeId).Count));
        }

        [Test]
        public void RuntimeAfterLoad_ContinuesHistoryEventIdsAfterArchivedHistory()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var cafeId = new WorldEntityId("building_linden_cafe");
            var noahId = new WorldEntityId("citizen_noaharan");

            for (int i = 0; i < WorldState.MaxRecentHistoryEvents + 5; i++) {
                runtime.Execute(new InspectBuildingCommand(cafeId, noahId));
            }

            WorldSaveData saveData = WorldSaveMapper.ToSaveData(runtime.State);
            WorldState loaded = WorldSaveMapper.ToRuntime(saveData);
            var loadedRuntime = new WorldRuntime(loaded);
            long expectedEventNumber = loaded.HistoryArchive.Count + loaded.RecentHistory.Count + 1;

            WorldCommandResult result = loadedRuntime.Execute(new InspectBuildingCommand(cafeId, noahId));

            Assert.That(result.HistoryEvents[0].Id.Value, Is.EqualTo($"evt_{expectedEventNumber:000000}"));
        }

        [Test]
        public void SaveRoundTrip_PreservesCitizenGoals()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var pellId = new WorldEntityId("citizen_old_mr_pell");
            var goal = new CitizenGoalDefinition(
                CitizenGoalKind.TravelToPlace,
                new WorldEntityId("place_pell_pharmacy_interior"),
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

            WorldSaveData saveData = WorldSaveMapper.ToSaveData(runtime.State);
            WorldState loaded = WorldSaveMapper.ToRuntime(saveData);

            Assert.That(loaded.CitizenGoals.Count, Is.EqualTo(1));
            Assert.That(loaded.CitizenGoals[0].CitizenId, Is.EqualTo(pellId));
            Assert.That(loaded.CitizenGoals[0].Reason, Is.EqualTo("Go to the pharmacy"));
            Assert.That(loaded.CitizenGoals[0].Status, Is.EqualTo(CitizenGoalStatus.Active));
        }

        [Test]
        public void SaveRoundTrip_PreservesTerritoryClaims()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var territoryId = new WorldEntityId("territory_aldwich_south_grassland");
            var ownerId = new WorldEntityId("citizen_noaharan");

            runtime.Execute(new ClaimTerritoryCommand(territoryId, ownerId, ownerId));

            WorldSaveData saveData = WorldSaveMapper.ToSaveData(runtime.State);
            WorldState loaded = WorldSaveMapper.ToRuntime(saveData);

            Assert.That(loaded.TerritoryChunksById.Count, Is.EqualTo(runtime.State.TerritoryChunksById.Count));
            Assert.That(loaded.TerritoryChunksById[territoryId].ClaimStatus, Is.EqualTo(TerritoryClaimStatus.Private));
            Assert.That(loaded.TerritoryChunksById[territoryId].ClaimOwnerId, Is.EqualTo(ownerId));
            Assert.That(loaded.TerritoryChunksById[territoryId].Biome, Is.EqualTo(TerritoryBiome.Grassland));
        }

        [Test]
        public void SaveRoundTrip_PreservesRoleAssignments()
        {
            WorldState state = WorldFactory.CreateVeyneSeedWorld();
            WorldSaveData saveData = WorldSaveMapper.ToSaveData(state);
            WorldState loaded = WorldSaveMapper.ToRuntime(saveData);
            var roleSystem = new RoleSystem(loaded);

            RoleAuthorizationResult result = roleSystem.CanPerform(
                new WorldEntityId("citizen_rowan"),
                WorldActionKind.ServeCustomer,
                new WorldEntityId("place_linden_cafe_interior"));

            Assert.That(loaded.RoleAssignments.Count, Is.EqualTo(state.RoleAssignments.Count));
            Assert.That(result.IsAllowed, Is.True);
            Assert.That(result.RoleId, Is.EqualTo(RoleCatalog.CafeWorker));
        }

        [Test]
        public void SaveRoundTrip_PreservesMoneyTransactionsAndPaidWorkHistory()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());

            runtime.Execute(new DoWorldActionCommand(
                new WorldEntityId("citizen_rowan"),
                WorldActionKind.ServeCustomer,
                new WorldEntityId("place_linden_cafe_interior")));
            runtime.Execute(new AcceptJobCommand(
                new WorldEntityId("roleassign_noah_pharmacy_clerk"),
                new WorldEntityId("citizen_noaharan"),
                RoleCatalog.Shopkeeper,
                new WorldEntityId("place_pell_pharmacy_interior")));
            runtime.Execute(new DoWorldActionCommand(
                new WorldEntityId("citizen_noaharan"),
                WorldActionKind.StockShelves,
                new WorldEntityId("place_pell_pharmacy_interior")));

            WorldSaveData saveData = WorldSaveMapper.ToSaveData(runtime.State);
            WorldState loaded = WorldSaveMapper.ToRuntime(saveData);

            Assert.That(loaded.MoneyAccountsById[new WorldEntityId("account_linden_cafe")].BalanceCents, Is.EqualTo(runtime.State.MoneyAccountsById[new WorldEntityId("account_linden_cafe")].BalanceCents));
            Assert.That(loaded.MoneyAccountsById[new WorldEntityId("account_pell_pharmacy")].BalanceCents, Is.EqualTo(runtime.State.MoneyAccountsById[new WorldEntityId("account_pell_pharmacy")].BalanceCents));
            Assert.That(loaded.MoneyAccountsById[new WorldEntityId("account_noaharan_cash")].BalanceCents, Is.EqualTo(runtime.State.MoneyAccountsById[new WorldEntityId("account_noaharan_cash")].BalanceCents));
            Assert.That(loaded.Transactions.Count, Is.EqualTo(3));
            Assert.That(loaded.GetHistoryByKind(Legacy.History.HistoryEventKind.PaymentRecorded).Count, Is.EqualTo(3));
            Assert.That(loaded.GetHistoryByKind(Legacy.History.HistoryEventKind.JobActionPerformed).Count, Is.EqualTo(2));
            Assert.That(loaded.GetHistoryByKind(Legacy.History.HistoryEventKind.JobAccepted).Count, Is.EqualTo(1));
        }

        [Test]
        public void SaveRoundTrip_PreservesCitizenRegistration()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var citizenId = new WorldEntityId("citizen_noaharan");

            runtime.Execute(new RegisterCitizenCommand(
                citizenId,
                new WorldEntityId("place_linden_street"),
                new WorldEntityId("place_linden_street"),
                new WorldEntityId("place_linden_cafe_interior"),
                new GridCoord(4, 3)));

            WorldSaveData saveData = WorldSaveMapper.ToSaveData(runtime.State);
            WorldState loaded = WorldSaveMapper.ToRuntime(saveData);

            Assert.That(loaded.TryGetCitizenRegistration(citizenId, out CitizenRegistrationState registration), Is.True);
            Assert.That(registration.StartingResidencePlaceId, Is.EqualTo(new WorldEntityId("place_linden_street")));
            Assert.That(loaded.CitizenGoals.Count, Is.EqualTo(1));
            Assert.That(loaded.CitizenGoals[0].Reason, Is.EqualTo("Visit Linden Cafe"));
        }
    }
}
