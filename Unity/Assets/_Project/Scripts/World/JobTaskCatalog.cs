namespace Legacy.World
{
    public static class JobTaskCatalog
    {
        public const string ServeCafeCustomer = "task_serve_cafe_customer";
        public const string StockPharmacyShelves = "task_stock_pharmacy_shelves";

        private static readonly JobTaskDefinition[] Definitions = {
            new JobTaskDefinition(
                ServeCafeCustomer,
                "Serve cafe customer",
                RoleCatalog.CafeWorker,
                WorldActionKind.ServeCustomer,
                MiniGameKind.CafePrep,
                8,
                125,
                ItemCatalog.CoffeeBeans,
                1,
                ItemCatalog.PreparedCoffee,
                1,
                CafeRecipeCatalog.HouseCoffee),
            new JobTaskDefinition(
                StockPharmacyShelves,
                "Stock pharmacy shelves",
                RoleCatalog.Shopkeeper,
                WorldActionKind.StockShelves,
                MiniGameKind.StockShelves,
                10,
                300,
                ItemCatalog.PharmacyStockBox,
                1,
                ItemCatalog.StockedShelf,
                1)
        };

        public static bool TryGet(string id, out JobTaskDefinition definition)
        {
            for (int i = 0; i < Definitions.Length; i++) {
                if (Definitions[i].Id == id) {
                    definition = Definitions[i];
                    return true;
                }
            }

            definition = null;
            return false;
        }
    }
}
