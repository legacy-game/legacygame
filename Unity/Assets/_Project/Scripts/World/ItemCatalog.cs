namespace Legacy.World
{
    public static class ItemCatalog
    {
        public const string CoffeeBeans = "coffee_beans";
        public const string Milk = "milk";
        public const string PaperCup = "paper_cup";
        public const string PreparedCoffee = "prepared_coffee";
        public const string PharmacyStockBox = "pharmacy_stock_box";
        public const string StockedShelf = "stocked_shelf";
        public const string NewspaperBundle = "newspaper_bundle";

        private static readonly ItemDefinition[] Definitions = {
            new ItemDefinition(CoffeeBeans, "Coffee beans", ItemKind.Ingredient, 45, 99),
            new ItemDefinition(Milk, "Milk", ItemKind.Ingredient, 80, 24),
            new ItemDefinition(PaperCup, "Paper cup", ItemKind.Supply, 8, 200),
            new ItemDefinition(PreparedCoffee, "Prepared coffee", ItemKind.PreparedGood, 275, 12),
            new ItemDefinition(PharmacyStockBox, "Pharmacy stock box", ItemKind.Supply, 1200, 20),
            new ItemDefinition(StockedShelf, "Stocked shelf", ItemKind.PreparedGood, 0, 1),
            new ItemDefinition(NewspaperBundle, "Newspaper bundle", ItemKind.Supply, 350, 20)
        };

        public static bool TryGet(string id, out ItemDefinition definition)
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
