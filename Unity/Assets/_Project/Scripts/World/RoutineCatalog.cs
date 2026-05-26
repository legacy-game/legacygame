namespace Legacy.World
{
    public static class RoutineCatalog
    {
        public const string PellMorningRoutineId = "routine_pell_morning";
        public const string RowanCafeShiftRoutineId = "routine_rowan_cafe_shift";
        public const string HollandMorningRoutineId = "routine_holland_morning";
        public const string SashaErrandRoutineId = "routine_sasha_errand";
        public const string EdieMorningRoutineId = "routine_edie_morning";

        private static readonly WorldEntityId StreetPlaceId = new("place_linden_street");
        private static readonly WorldEntityId CafePlaceId = new("place_linden_cafe_interior");
        private static readonly WorldEntityId PharmacyPlaceId = new("place_pell_pharmacy_interior");
        private static readonly WorldEntityId BookshopPlaceId = new("place_marlows_books_interior");

        private static readonly RoutineDefinition[] Definitions = {
            new RoutineDefinition(
                PellMorningRoutineId,
                "Old Mr. Pell morning errands",
                new[] {
                    new RoutineStepDefinition(
                        "morning_cafe_visit",
                        7 * 60 + 35,
                        7 * 60 + 50,
                        CafePlaceId,
                        new GridCoord(4, 3),
                        CitizenActivityState.Visiting,
                        20,
                        true,
                        "Visit Linden Cafe",
                        "Old Mr. Pell arrived at Linden Cafe.",
                        35),
                    new RoutineStepDefinition(
                        "return_to_street",
                        7 * 60 + 50,
                        8 * 60 + 30,
                        StreetPlaceId,
                        new GridCoord(18, 8),
                        CitizenActivityState.Offscreen,
                        10,
                        true,
                        "Leave Linden Cafe",
                        "Old Mr. Pell left Linden Cafe.",
                        20)
                }),
            new RoutineDefinition(
                RowanCafeShiftRoutineId,
                "Rowan cafe morning shift",
                new[] {
                    new RoutineStepDefinition(
                        "open_cafe_shift",
                        7 * 60,
                        11 * 60,
                        CafePlaceId,
                        new GridCoord(4, 3),
                        CitizenActivityState.Working,
                        30,
                        false,
                        "Work cafe shift",
                        "Rowan started a shift at Linden Cafe.",
                        60)
                }),
            new RoutineDefinition(
                HollandMorningRoutineId,
                "Mr. Holland morning coffee",
                new[] {
                    new RoutineStepDefinition(
                        "morning_coffee",
                        8 * 60 + 5,
                        8 * 60 + 25,
                        CafePlaceId,
                        new GridCoord(3, 3),
                        CitizenActivityState.Visiting,
                        25,
                        true,
                        "Visit Linden Cafe",
                        "Mr. Holland arrived for morning coffee.",
                        25),
                    new RoutineStepDefinition(
                        "leave_cafe",
                        8 * 60 + 25,
                        9 * 60,
                        StreetPlaceId,
                        new GridCoord(10, 8),
                        CitizenActivityState.Offscreen,
                        10,
                        true,
                        "Leave Linden Cafe",
                        "Mr. Holland left Linden Cafe.",
                        15)
                }),
            new RoutineDefinition(
                SashaErrandRoutineId,
                "Sasha quick pharmacy errand",
                new[] {
                    new RoutineStepDefinition(
                        "pharmacy_pickup",
                        8 * 60 + 45,
                        9 * 60 + 5,
                        PharmacyPlaceId,
                        new GridCoord(3, 3),
                        CitizenActivityState.Visiting,
                        35,
                        true,
                        "Visit Pell Pharmacy",
                        "Sasha arrived at Pell Pharmacy.",
                        15),
                    new RoutineStepDefinition(
                        "return_to_route",
                        9 * 60 + 5,
                        9 * 60 + 30,
                        StreetPlaceId,
                        new GridCoord(20, 8),
                        CitizenActivityState.Offscreen,
                        20,
                        true,
                        "Leave Pell Pharmacy",
                        "Sasha left Pell Pharmacy.",
                        10)
                }),
            new RoutineDefinition(
                EdieMorningRoutineId,
                "Edie morning reporting route",
                new[] {
                    new RoutineStepDefinition(
                        "bookshop_notes",
                        9 * 60 + 15,
                        9 * 60 + 35,
                        BookshopPlaceId,
                        new GridCoord(2, 3),
                        CitizenActivityState.Visiting,
                        30,
                        true,
                        "Visit Marlow's Books",
                        "Edie arrived at Marlow's Books to check notices.",
                        20),
                    new RoutineStepDefinition(
                        "cross_to_cafe",
                        9 * 60 + 35,
                        10 * 60,
                        CafePlaceId,
                        new GridCoord(5, 3),
                        CitizenActivityState.Visiting,
                        35,
                        true,
                        "Visit Linden Cafe",
                        "Edie crossed the street to Linden Cafe.",
                        20)
                })
        };

        public static bool TryGet(string routineId, out RoutineDefinition definition)
        {
            for (int i = 0; i < Definitions.Length; i++) {
                if (Definitions[i].Id == routineId) {
                    definition = Definitions[i];
                    return true;
                }
            }

            definition = null;
            return false;
        }
    }
}
