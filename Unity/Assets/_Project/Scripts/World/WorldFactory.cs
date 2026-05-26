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
            var bookshopInteriorSceneId = new WorldEntityId("scene_marlows_books_interior");
            var bookshopInteriorPlaceId = new WorldEntityId("place_marlows_books_interior");
            var noahId = new WorldEntityId("citizen_noaharan");
            var rowanId = new WorldEntityId("citizen_rowan");
            var pellId = new WorldEntityId("citizen_old_mr_pell");
            var hollandId = new WorldEntityId("citizen_mr_holland");
            var sashaId = new WorldEntityId("citizen_sasha");
            var edieId = new WorldEntityId("citizen_edie");
            var marlowId = new WorldEntityId("citizen_marlow");
            var bookshopPlotId = new WorldEntityId("plot_linden_12");
            var lindenPlotId = new WorldEntityId("plot_linden_14");
            var pharmacyPlotId = new WorldEntityId("plot_linden_16");
            var bookshopId = new WorldEntityId("building_marlows_books");
            var cafeId = new WorldEntityId("building_linden_cafe");
            var pharmacyId = new WorldEntityId("building_pell_pharmacy");
            var cafeWorkplaceId = new WorldEntityId("workplace_linden_cafe");
            var pharmacyWorkplaceId = new WorldEntityId("workplace_pell_pharmacy");
            var bookshopWorkplaceId = new WorldEntityId("workplace_marlows_books");

            var state = new WorldState(
                CurrentSchemaVersion,
                worldSeed,
                new GameDateTime(new GameDate(2003, 5, 14), new TimeOfDay(6, 30)),
                exteriorSceneId);

            state.AddRegion(new RegionState(veyneId, "Veyne", new GridBounds(new GridCoord(0, 0), 64, 64)));
            state.AddScene(new WorldSceneState(exteriorSceneId, veyneId, "Linden Street"));
            state.AddScene(new WorldSceneState(cafeInteriorSceneId, veyneId, "Linden Cafe Interior"));
            state.AddScene(new WorldSceneState(pharmacyInteriorSceneId, veyneId, "Pell Pharmacy Interior"));
            state.AddScene(new WorldSceneState(bookshopInteriorSceneId, veyneId, "Marlow's Books Interior"));
            state.AddPlace(new PlaceState(streetPlaceId, veyneId, exteriorSceneId, "Linden Street", PlaceKind.PublicSpace));
            state.AddPlace(new PlaceState(cafeInteriorPlaceId, veyneId, cafeInteriorSceneId, "Linden Cafe Interior", PlaceKind.Business));
            state.AddPlace(new PlaceState(pharmacyInteriorPlaceId, veyneId, pharmacyInteriorSceneId, "Pell Pharmacy Interior", PlaceKind.Business));
            state.AddPlace(new PlaceState(bookshopInteriorPlaceId, veyneId, bookshopInteriorSceneId, "Marlow's Books Interior", PlaceKind.Business));
            state.AddTerritoryChunk(new TerritoryChunkState(new WorldEntityId("territory_veyne_linden_row"), veyneId, new GridChunkCoord(0, 0), "Linden Row", TerritoryBiome.Urban, TerritoryClaimStatus.Municipal, veyneId, default, veyneId, true, true));
            state.AddTerritoryChunk(new TerritoryChunkState(new WorldEntityId("territory_aldwich_west_forest"), veyneId, new GridChunkCoord(-1, 0), "West Aldwich Forest", TerritoryBiome.Forest, TerritoryClaimStatus.Unclaimed, default, default, default, false, true));
            state.AddTerritoryChunk(new TerritoryChunkState(new WorldEntityId("territory_aldwich_south_grassland"), veyneId, new GridChunkCoord(0, -1), "South Aldwich Grassland", TerritoryBiome.Grassland, TerritoryClaimStatus.Unclaimed, default, default, default, true, true));
            state.AddTerritoryChunk(new TerritoryChunkState(new WorldEntityId("territory_aldwich_lakeside"), veyneId, new GridChunkCoord(1, 0), "Aldwich Lakeside", TerritoryBiome.Lakeside, TerritoryClaimStatus.Unclaimed, default, default, default, true, false));
            state.AddCitizen(new CitizenState(noahId, "Noaharan", cafeId, cafeId, veyneId, exteriorSceneId, streetPlaceId, new GridCoord(12, 8), CitizenActivityState.Present));
            state.AddCitizen(new CitizenState(rowanId, "Rowan", cafeId, cafeId, veyneId, cafeInteriorSceneId, cafeInteriorPlaceId, new GridCoord(4, 3), CitizenActivityState.Working, 0, new CitizenRoutineState(RoutineCatalog.RowanCafeShiftRoutineId)));
            state.AddCitizen(new CitizenState(pellId, "Old Mr. Pell", pharmacyId, pharmacyId, veyneId, exteriorSceneId, streetPlaceId, new GridCoord(18, 8), CitizenActivityState.Offscreen, 0, new CitizenRoutineState(RoutineCatalog.PellMorningRoutineId)));
            state.AddCitizen(new CitizenState(hollandId, "Mr. Holland", cafeId, cafeId, veyneId, exteriorSceneId, streetPlaceId, new GridCoord(10, 8), CitizenActivityState.Offscreen, 0, new CitizenRoutineState(RoutineCatalog.HollandMorningRoutineId)));
            state.AddCitizen(new CitizenState(sashaId, "Sasha", pharmacyId, pharmacyId, veyneId, exteriorSceneId, streetPlaceId, new GridCoord(20, 8), CitizenActivityState.Offscreen, 0, new CitizenRoutineState(RoutineCatalog.SashaErrandRoutineId)));
            state.AddCitizen(new CitizenState(edieId, "Edie", bookshopId, bookshopId, veyneId, exteriorSceneId, streetPlaceId, new GridCoord(8, 8), CitizenActivityState.Offscreen, 0, new CitizenRoutineState(RoutineCatalog.EdieMorningRoutineId)));
            state.AddCitizen(new CitizenState(marlowId, "Marlow", bookshopId, bookshopId, veyneId, exteriorSceneId, streetPlaceId, new GridCoord(8, 7), CitizenActivityState.Working));
            state.AddPlot(new PlotState(bookshopPlotId, veyneId, "12 Linden Street", new GridBounds(new GridCoord(6, 6), 4, 6), marlowId, AccessRule.Public));
            state.AddPlot(new PlotState(lindenPlotId, veyneId, "14 Linden Street", new GridBounds(new GridCoord(10, 6), 8, 6), noahId, AccessRule.Public));
            state.AddPlot(new PlotState(pharmacyPlotId, veyneId, "16 Linden Street", new GridBounds(new GridCoord(18, 6), 8, 6), pellId, AccessRule.Public));
            state.AddBuilding(new BuildingState(bookshopId, veyneId, bookshopPlotId, exteriorSceneId, bookshopInteriorSceneId, bookshopInteriorPlaceId, "Marlow's Books", marlowId, PlaceKind.Business, new GridCoord(8, 7)));
            state.AddBuilding(new BuildingState(cafeId, veyneId, lindenPlotId, exteriorSceneId, cafeInteriorSceneId, cafeInteriorPlaceId, "Linden Cafe", noahId, PlaceKind.Business, new GridCoord(12, 7)));
            state.AddBuilding(new BuildingState(pharmacyId, veyneId, pharmacyPlotId, exteriorSceneId, pharmacyInteriorSceneId, pharmacyInteriorPlaceId, "Pell Pharmacy", pellId, PlaceKind.Business, new GridCoord(18, 7)));
            state.AddRoleAssignment(new RoleAssignmentState(new WorldEntityId("roleassign_rowan_cafe_worker"), rowanId, RoleCatalog.CafeWorker, cafeInteriorPlaceId));
            state.AddRoleAssignment(new RoleAssignmentState(new WorldEntityId("roleassign_pell_shopkeeper"), pellId, RoleCatalog.Shopkeeper, pharmacyInteriorPlaceId));
            state.AddRoleAssignment(new RoleAssignmentState(new WorldEntityId("roleassign_marlow_bookseller"), marlowId, RoleCatalog.Shopkeeper, bookshopInteriorPlaceId));
            state.AddMoneyAccount(new MoneyAccountState(new WorldEntityId("account_noaharan_cash"), noahId, "Noaharan cash", 4500));
            state.AddMoneyAccount(new MoneyAccountState(new WorldEntityId("account_rowan_cash"), rowanId, "Rowan cash", 3200));
            state.AddMoneyAccount(new MoneyAccountState(new WorldEntityId("account_pell_cash"), pellId, "Old Mr. Pell cash", 2800));
            state.AddMoneyAccount(new MoneyAccountState(new WorldEntityId("account_holland_cash"), hollandId, "Mr. Holland cash", 1800));
            state.AddMoneyAccount(new MoneyAccountState(new WorldEntityId("account_sasha_cash"), sashaId, "Sasha cash", 2200));
            state.AddMoneyAccount(new MoneyAccountState(new WorldEntityId("account_edie_cash"), edieId, "Edie cash", 2600));
            state.AddMoneyAccount(new MoneyAccountState(new WorldEntityId("account_marlow_cash"), marlowId, "Marlow cash", 3800));
            state.AddMoneyAccount(new MoneyAccountState(new WorldEntityId("account_marlows_books"), bookshopId, "Marlow's Books till", 7600));
            state.AddMoneyAccount(new MoneyAccountState(new WorldEntityId("account_linden_cafe"), cafeId, "Linden Cafe till", 12500));
            state.AddMoneyAccount(new MoneyAccountState(new WorldEntityId("account_pell_pharmacy"), pharmacyId, "Pell Pharmacy till", 9800));
            state.AddWorkplace(new WorkplaceState(cafeWorkplaceId, cafeId, cafeInteriorPlaceId, noahId, new WorldEntityId("account_linden_cafe"), WorkplaceStatus.Open, 7 * 60, 17 * 60));
            state.AddWorkplace(new WorkplaceState(pharmacyWorkplaceId, pharmacyId, pharmacyInteriorPlaceId, pellId, new WorldEntityId("account_pell_pharmacy"), WorkplaceStatus.Open, 8 * 60, 18 * 60));
            state.AddWorkplace(new WorkplaceState(bookshopWorkplaceId, bookshopId, bookshopInteriorPlaceId, marlowId, new WorldEntityId("account_marlows_books"), WorkplaceStatus.Open, 9 * 60, 17 * 60));
            var cafeInventory = new WorkplaceInventoryState(cafeWorkplaceId);
            cafeInventory.Add("coffee_beans", 25);
            cafeInventory.Add("milk", 12);
            state.AddWorkplaceInventory(cafeInventory);
            var pharmacyInventory = new WorkplaceInventoryState(pharmacyWorkplaceId);
            pharmacyInventory.Add("pharmacy_stock_box", 8);
            state.AddWorkplaceInventory(pharmacyInventory);
            var bookshopInventory = new WorkplaceInventoryState(bookshopWorkplaceId);
            bookshopInventory.Add("newspaper_bundle", 10);
            state.AddWorkplaceInventory(bookshopInventory);
            state.AddJobPosting(new JobPostingState(new WorldEntityId("posting_cafe_worker_001"), JobCatalog.CafeWorker, noahId, cafeWorkplaceId, RoleCatalog.CafeWorker, PayModelKind.PerTask, 125, 2, JobPostingStatus.Open, new GameDateTime(new GameDate(2003, 5, 14), new TimeOfDay(6, 30))));
            state.AddJobPosting(new JobPostingState(new WorldEntityId("posting_pharmacy_clerk_001"), JobCatalog.PharmacyClerk, pellId, pharmacyWorkplaceId, RoleCatalog.Shopkeeper, PayModelKind.PerTask, 300, 1, JobPostingStatus.Open, new GameDateTime(new GameDate(2003, 5, 14), new TimeOfDay(6, 30))));

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
