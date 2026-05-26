namespace Legacy.World
{
    public static class JobCatalog
    {
        public const string CafeWorker = "job_cafe_worker";
        public const string PharmacyClerk = "job_pharmacy_clerk";
        public const string TransitDriver = "job_transit_driver";

        private static readonly JobDefinition[] Definitions = {
            new JobDefinition(CafeWorker, "Cafe worker", RoleCatalog.CafeWorker, PayModelKind.PerTask, 125, SkillKind.Barista),
            new JobDefinition(PharmacyClerk, "Pharmacy clerk", RoleCatalog.Shopkeeper, PayModelKind.PerTask, 300, SkillKind.Retail),
            new JobDefinition(TransitDriver, "Transit driver", RoleCatalog.TransitDriver, PayModelKind.Hourly, 1200, SkillKind.Driving)
        };

        public static bool TryGet(string id, out JobDefinition definition)
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
