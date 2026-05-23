using Legacy.History;
using Legacy.Time;

namespace Legacy.World
{
    public static class WorldFactory
    {
        public const int CurrentSchemaVersion = 1;

        public static WorldState CreateVeyneSeedWorld(long worldSeed = 1001)
        {
            var veyneId = new WorldEntityId("region_veyne");
            var exteriorSceneId = new WorldEntityId("scene_veyne_linden_street");
            var cafeInteriorSceneId = new WorldEntityId("scene_linden_cafe_interior");
            var streetPlaceId = new WorldEntityId("place_linden_street");
            var cafeInteriorPlaceId = new WorldEntityId("place_linden_cafe_interior");
            var pharmacyInteriorSceneId = new WorldEntityId("scene_pell_pharmacy_interior");
            var pharmacyInteriorPlaceId = new WorldEntityId("place_pell_pharmacy_interior");
            var noahId = new WorldEntityId("citizen_noaharan");
            var rowanId = new WorldEntityId("citizen_rowan");
            var pellId = new WorldEntityId("citizen_old_mr_pell");
            var lindenPlotId = new WorldEntityId("plot_linden_14");
            var pharmacyPlotId = new WorldEntityId("plot_linden_16");
            var cafeId = new WorldEntityId("building_linden_cafe");
            var pharmacyId = new WorldEntityId("building_pell_pharmacy");

            var state = new WorldState(
                CurrentSchemaVersion,
                worldSeed,
                new GameDateTime(new GameDate(2003, 5, 14), new TimeOfDay(6, 30)),
                exteriorSceneId);

            state.AddRegion(new RegionState(veyneId, "Veyne", new GridBounds(new GridCoord(0, 0), 64, 64)));
            state.AddScene(new WorldSceneState(exteriorSceneId, veyneId, "Linden Street"));
            state.AddScene(new WorldSceneState(cafeInteriorSceneId, veyneId, "Linden Cafe Interior"));
            state.AddScene(new WorldSceneState(pharmacyInteriorSceneId, veyneId, "Pell Pharmacy Interior"));
            state.AddPlace(new PlaceState(streetPlaceId, veyneId, exteriorSceneId, "Linden Street", PlaceKind.PublicSpace));
            state.AddPlace(new PlaceState(cafeInteriorPlaceId, veyneId, cafeInteriorSceneId, "Linden Cafe Interior", PlaceKind.Business));
            state.AddPlace(new PlaceState(pharmacyInteriorPlaceId, veyneId, pharmacyInteriorSceneId, "Pell Pharmacy Interior", PlaceKind.Business));
            state.AddCitizen(new CitizenState(noahId, "Noaharan", cafeId, cafeId, veyneId, exteriorSceneId, streetPlaceId, new GridCoord(12, 8), CitizenActivityState.Working));
            state.AddCitizen(new CitizenState(rowanId, "Rowan", cafeId, cafeId, veyneId, cafeInteriorSceneId, cafeInteriorPlaceId, new GridCoord(4, 3), CitizenActivityState.Working));
            state.AddCitizen(new CitizenState(pellId, "Old Mr. Pell", pharmacyId, pharmacyId, veyneId, exteriorSceneId, streetPlaceId, new GridCoord(18, 8), CitizenActivityState.Offscreen));
            state.AddPlot(new PlotState(lindenPlotId, veyneId, "14 Linden Street", new GridBounds(new GridCoord(10, 6), 8, 6), noahId, AccessRule.Public));
            state.AddPlot(new PlotState(pharmacyPlotId, veyneId, "16 Linden Street", new GridBounds(new GridCoord(18, 6), 8, 6), pellId, AccessRule.Public));
            state.AddBuilding(new BuildingState(cafeId, veyneId, lindenPlotId, exteriorSceneId, cafeInteriorSceneId, cafeInteriorPlaceId, "Linden Cafe", noahId, PlaceKind.Business, new GridCoord(12, 7)));
            state.AddBuilding(new BuildingState(pharmacyId, veyneId, pharmacyPlotId, exteriorSceneId, pharmacyInteriorSceneId, pharmacyInteriorPlaceId, "Pell Pharmacy", pellId, PlaceKind.Business, new GridCoord(18, 7)));

            var history = new HistoryLog();
            state.AddHistoryEvent(history.Create(
                state.CurrentTime,
                HistoryEventKind.WorldCreated,
                "A new shared world begins in Veyne.",
                new[] { noahId },
                new[] { cafeId, pharmacyId }));

            return state;
        }
    }
}
