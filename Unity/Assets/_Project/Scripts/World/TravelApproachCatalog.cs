namespace Legacy.World
{
    public static class TravelApproachCatalog
    {
        private static readonly TravelApproachDefinition[] Defaults = {
            new TravelApproachDefinition(
                "travel_independent",
                "Independent travel",
                55,
                -1,
                -20,
                5,
                2,
                false,
                0,
                false,
                "Travel independently."),
            new TravelApproachDefinition(
                "travel_public_route",
                "Public route",
                30,
                0,
                1,
                10,
                0,
                true,
                8,
                false,
                "Use a public route."),
            new TravelApproachDefinition(
                "travel_social_help",
                "Ask for help",
                20,
                0,
                1,
                8,
                0,
                false,
                0,
                true,
                "Ask someone nearby for help.")
        };

        public static TravelApproachDefinition[] GetDefaults()
        {
            return Defaults;
        }
    }
}
