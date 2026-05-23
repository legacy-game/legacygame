using Legacy.Commands;
using Legacy.History;
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
            Assert.That(runtime.State.RecentHistory[^1].Description, Does.Contain("arrived"));
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
            Assert.That(runtime.State.HistoryArchive.Count, Is.GreaterThanOrEqualTo(10));
            Assert.That(runtime.State.GetHistoryForPlace(cafeId).Count, Is.GreaterThanOrEqualTo(WorldState.MaxRecentHistoryEvents + 10));
        }
    }
}
