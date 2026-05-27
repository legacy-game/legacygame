using Legacy.Commands;
using Legacy.History;
using Legacy.Save;
using Legacy.World;
using NUnit.Framework;

namespace Legacy.Tests.EditMode
{
    public sealed class FinalGameScaffoldTests
    {
        [Test]
        public void SeedWorld_IncludesDisabledFinalGameScaffolds()
        {
            WorldState state = WorldFactory.CreateVeyneSeedWorld();

            Assert.That(state.FinalGameScaffolds.Count, Is.EqualTo(7));
            Assert.That(HasScaffold(state, FinalGameScaffoldKind.FamilyLegacy), Is.True);
            Assert.That(HasScaffold(state, FinalGameScaffoldKind.HealthDeath), Is.True);
            Assert.That(HasScaffold(state, FinalGameScaffoldKind.GovernmentLaws), Is.True);
            Assert.That(HasScaffold(state, FinalGameScaffoldKind.CrimeJustice), Is.True);
            Assert.That(HasScaffold(state, FinalGameScaffoldKind.FrontierSettlement), Is.True);
            Assert.That(HasScaffold(state, FinalGameScaffoldKind.CultureArtifacts), Is.True);
            Assert.That(HasScaffold(state, FinalGameScaffoldKind.Moderation), Is.True);

            for (int i = 0; i < state.FinalGameScaffolds.Count; i++) {
                Assert.That(state.FinalGameScaffolds[i].IsGameplayEnabled, Is.False);
            }
        }

        [Test]
        public void SaveRoundTrip_PreservesFinalGameScaffolds()
        {
            WorldState state = WorldFactory.CreateVeyneSeedWorld();
            WorldSaveData saveData = WorldSaveMapper.ToSaveData(state);
            WorldState loaded = WorldSaveMapper.ToRuntime(saveData);

            Assert.That(saveData.finalGameScaffolds.Count, Is.EqualTo(7));
            Assert.That(loaded.FinalGameScaffolds.Count, Is.EqualTo(7));
            Assert.That(loaded.FinalGameScaffolds[0].Id, Is.EqualTo(state.FinalGameScaffolds[0].Id));
            Assert.That(loaded.FinalGameScaffolds[0].Kind, Is.EqualTo(state.FinalGameScaffolds[0].Kind));
            Assert.That(loaded.FinalGameScaffolds[0].Summary, Is.EqualTo(state.FinalGameScaffolds[0].Summary));
            Assert.That(loaded.FinalGameScaffolds[0].IsGameplayEnabled, Is.False);
        }

        [Test]
        public void FinalGameScaffolds_DoNotDisruptPlayableCafeSlice()
        {
            var runtime = new WorldRuntime(WorldFactory.CreateVeyneSeedWorld());

            WorldCommandResult result = runtime.Execute(new InspectBuildingCommand(
                new WorldEntityId("building_linden_cafe"),
                new WorldEntityId("citizen_noaharan")));

            Assert.That(result.Succeeded, Is.True);
            Assert.That(runtime.State.CitizensById.Count, Is.EqualTo(7));
            Assert.That(runtime.State.BuildingsById.Count, Is.EqualTo(3));
            Assert.That(runtime.State.WorkplacesById.Count, Is.EqualTo(3));
            Assert.That(runtime.State.RecentHistory[^1].Kind, Is.EqualTo(HistoryEventKind.BuildingInspected));
        }

        private static bool HasScaffold(WorldState state, FinalGameScaffoldKind kind)
        {
            for (int i = 0; i < state.FinalGameScaffolds.Count; i++) {
                if (state.FinalGameScaffolds[i].Kind == kind) {
                    return true;
                }
            }

            return false;
        }
    }
}
