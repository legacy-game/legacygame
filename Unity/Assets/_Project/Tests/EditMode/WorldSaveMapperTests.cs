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
    }
}
