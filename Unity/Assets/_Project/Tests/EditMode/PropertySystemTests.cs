using Legacy.Commands;
using Legacy.History;
using Legacy.Time;
using Legacy.World;
using NUnit.Framework;

namespace Legacy.Tests.EditMode
{
    public sealed class PropertySystemTests
    {
        [Test]
        public void BuildingSummary_ConnectsBuildingPlotPlaceOwnerAndAccess()
        {
            WorldState state = WorldFactory.CreateVeyneSeedWorld();
            var propertySystem = new PropertySystem(state);

            bool found = propertySystem.TryGetBuildingSummary(
                new WorldEntityId("building_linden_cafe"),
                out BuildingPropertySummary summary);

            Assert.That(found, Is.True);
            Assert.That(summary.DisplayName, Is.EqualTo("Linden Cafe"));
            Assert.That(summary.OwnerName, Is.EqualTo("Noaharan"));
            Assert.That(summary.PlotName, Is.EqualTo("14 Linden Street"));
            Assert.That(summary.InteriorPlaceName, Is.EqualTo("Linden Cafe Interior"));
            Assert.That(summary.AccessRule, Is.EqualTo(AccessRule.Public));
        }

        [Test]
        public void AccessRules_AllowOwnersAndEmployeesButDenyUnrelatedCitizens()
        {
            WorldState state = CreateAccessRuleWorld();
            var propertySystem = new PropertySystem(state);
            var buildingId = new WorldEntityId("building_test_shop");

            PropertyAccessResult ownerAccess = propertySystem.CheckBuildingAccess(buildingId, new WorldEntityId("citizen_owner"));
            PropertyAccessResult employeeAccess = propertySystem.CheckBuildingAccess(buildingId, new WorldEntityId("citizen_employee"));
            PropertyAccessResult strangerAccess = propertySystem.CheckBuildingAccess(buildingId, new WorldEntityId("citizen_stranger"));

            Assert.That(ownerAccess.IsAllowed, Is.True);
            Assert.That(employeeAccess.IsAllowed, Is.True);
            Assert.That(strangerAccess.IsAllowed, Is.False);
            Assert.That(strangerAccess.Rule, Is.EqualTo(AccessRule.EmployeesOnly));
        }

        [Test]
        public void TryFindPlotAt_ReturnsPlotContainingGridCoord()
        {
            WorldState state = WorldFactory.CreateVeyneSeedWorld();
            var propertySystem = new PropertySystem(state);

            bool found = propertySystem.TryFindPlotAt(
                new WorldEntityId("region_veyne"),
                new GridCoord(12, 7),
                out PlotState plot);

            Assert.That(found, Is.True);
            Assert.That(plot.Id, Is.EqualTo(new WorldEntityId("plot_linden_14")));
        }

        [Test]
        public void InspectPlotAndPlace_WriteHistoryEvents()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var inspectorId = new WorldEntityId("citizen_noaharan");

            WorldCommandResult plotResult = runtime.Execute(new InspectPlotCommand(
                new WorldEntityId("plot_linden_14"),
                inspectorId));
            WorldCommandResult placeResult = runtime.Execute(new InspectPlaceCommand(
                new WorldEntityId("place_linden_cafe_interior"),
                inspectorId));

            Assert.That(plotResult.Succeeded, Is.True);
            Assert.That(placeResult.Succeeded, Is.True);
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.PlotInspected).Count, Is.EqualTo(1));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.PlaceInspected).Count, Is.EqualTo(1));
        }

        [Test]
        public void TransferPlotOwnership_ChangesPlotOwnerAndWritesHistory()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());
            var plotId = new WorldEntityId("plot_linden_14");
            var rowanId = new WorldEntityId("citizen_rowan");

            WorldCommandResult result = runtime.Execute(new TransferPlotOwnershipCommand(
                plotId,
                rowanId,
                new WorldEntityId("citizen_noaharan")));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.PlotsById[plotId].OwnerCitizenId, Is.EqualTo(rowanId));
            Assert.That(runtime.State.GetHistoryByKind(HistoryEventKind.PlotOwnershipTransferred).Count, Is.EqualTo(1));
        }

        private static WorldState CreateAccessRuleWorld()
        {
            var regionId = new WorldEntityId("region_test");
            var sceneId = new WorldEntityId("scene_test");
            var placeId = new WorldEntityId("place_test_shop");
            var plotId = new WorldEntityId("plot_test_shop");
            var buildingId = new WorldEntityId("building_test_shop");

            var state = new WorldState(
                WorldFactory.CurrentSchemaVersion,
                2002,
                new GameDateTime(new GameDate(2003, 1, 1), new TimeOfDay(8, 0)),
                sceneId);

            state.AddRegion(new RegionState(regionId, "Test Region", new GridBounds(new GridCoord(0, 0), 20, 20)));
            state.AddScene(new WorldSceneState(sceneId, regionId, "Test Scene"));
            state.AddPlace(new PlaceState(placeId, regionId, sceneId, "Test Shop", PlaceKind.Business));
            state.AddCitizen(new CitizenState(new WorldEntityId("citizen_owner"), "Owner", buildingId, buildingId, regionId, sceneId, placeId, new GridCoord(1, 1)));
            state.AddCitizen(new CitizenState(new WorldEntityId("citizen_employee"), "Employee", buildingId, buildingId, regionId, sceneId, placeId, new GridCoord(2, 1)));
            state.AddCitizen(new CitizenState(new WorldEntityId("citizen_stranger"), "Stranger", buildingId, new WorldEntityId("building_elsewhere"), regionId, sceneId, placeId, new GridCoord(3, 1)));
            state.AddPlot(new PlotState(plotId, regionId, "Test Plot", new GridBounds(new GridCoord(0, 0), 5, 5), new WorldEntityId("citizen_owner"), AccessRule.EmployeesOnly));
            state.AddBuilding(new BuildingState(buildingId, regionId, plotId, sceneId, sceneId, placeId, "Test Shop", new WorldEntityId("citizen_owner"), PlaceKind.Business, new GridCoord(1, 1)));

            return state;
        }
    }
}
