namespace Legacy.World
{
    public static class RoutineCatalog
    {
        public const string PellMorningRoutineId = "routine_pell_morning";
        public const string RowanCafeShiftRoutineId = "routine_rowan_cafe_shift";

        private static readonly WorldEntityId StreetPlaceId = new("place_linden_street");
        private static readonly WorldEntityId CafePlaceId = new("place_linden_cafe_interior");

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
