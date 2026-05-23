using Legacy.World;
using NUnit.Framework;

namespace Legacy.Tests.EditMode
{
    public sealed class WorldScaleTests
    {
        [Test]
        public void IndexedLookups_HandleLargeEntityCounts()
        {
            var regionId = new WorldEntityId("region_scale");
            var sceneId = new WorldEntityId("scene_scale");
            var secondSceneId = new WorldEntityId("scene_scale_second");
            var placeId = new WorldEntityId("place_scale");
            var secondPlaceId = new WorldEntityId("place_scale_second");
            var state = new WorldState(
                WorldFactory.CurrentSchemaVersion,
                42,
                new Legacy.Time.GameDateTime(new Legacy.Time.GameDate(2003, 1, 1), new Legacy.Time.TimeOfDay(6, 0)),
                sceneId);

            state.AddRegion(new RegionState(regionId, "Scale Test", new GridBounds(new GridCoord(0, 0), 1000, 1000)));
            state.AddScene(new WorldSceneState(sceneId, regionId, "Scale Scene"));
            state.AddScene(new WorldSceneState(secondSceneId, regionId, "Scale Scene 2"));
            state.AddPlace(new PlaceState(placeId, regionId, sceneId, "Scale Place", PlaceKind.PublicSpace));
            state.AddPlace(new PlaceState(secondPlaceId, regionId, secondSceneId, "Scale Place 2", PlaceKind.PublicSpace));

            for (int i = 0; i < 10000; i++) {
                var citizenId = new WorldEntityId($"citizen_{i:00000}");
                state.AddCitizen(new CitizenState(
                    citizenId,
                    $"Citizen {i}",
                    new WorldEntityId("building_none"),
                    new WorldEntityId("building_none"),
                    regionId,
                    sceneId,
                    placeId,
                    new GridCoord(i % 1000, i / 1000)));
            }

            Assert.That(state.CitizensById.ContainsKey(new WorldEntityId("citizen_09999")), Is.True);
            Assert.That(state.CitizensById.Count, Is.EqualTo(10000));
            Assert.That(state.GetCitizenIdsInScene(sceneId).Count, Is.EqualTo(10000));
            Assert.That(state.GetCitizenIdsInSceneChunk(sceneId, new GridChunkCoord(0, 0)).Count, Is.EqualTo(160));
            Assert.That(state.GetCitizenIdsNear(sceneId, new GridCoord(8, 8), 0).Count, Is.EqualTo(160));

            state.MoveCitizen(
                new WorldEntityId("citizen_09999"),
                regionId,
                secondSceneId,
                secondPlaceId,
                new GridCoord(5, 5),
                CitizenActivityState.Visiting);

            Assert.That(state.GetCitizenIdsInScene(sceneId).Count, Is.EqualTo(9999));
            Assert.That(state.GetCitizenIdsInScene(secondSceneId).Count, Is.EqualTo(1));
            Assert.That(state.GetCitizenIdsInSceneChunk(secondSceneId, new GridChunkCoord(0, 0)).Count, Is.EqualTo(1));
            Assert.That(state.GetCitizenIdsInSceneChunk(sceneId, GridChunkCoord.FromGridCoord(new GridCoord(999, 9))).Count, Is.EqualTo(79));
        }

        [Test]
        public void BuildingSpatialIndex_ReturnsBuildingsBySceneChunk()
        {
            WorldState state = WorldFactory.CreateVeyneSeedWorld();
            var exteriorSceneId = new WorldEntityId("scene_veyne_linden_street");
            var cafeInteriorSceneId = new WorldEntityId("scene_linden_cafe_interior");

            Assert.That(state.GetBuildingIdsInSceneChunk(exteriorSceneId, GridChunkCoord.FromGridCoord(new GridCoord(12, 7))).Count, Is.EqualTo(1));
            Assert.That(state.GetBuildingIdsInSceneChunk(cafeInteriorSceneId, new GridChunkCoord(0, 0)).Count, Is.EqualTo(1));
        }
    }
}
