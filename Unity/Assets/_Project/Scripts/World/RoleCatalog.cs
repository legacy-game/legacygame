namespace Legacy.World
{
    public static class RoleCatalog
    {
        public const string CafeOwner = "role_cafe_owner";
        public const string CafeWorker = "role_cafe_worker";
        public const string Shopkeeper = "role_shopkeeper";
        public const string TransitDriver = "role_transit_driver";
        public const string PoliceOfficer = "role_police_officer";
        public const string MedicalWorker = "role_medical_worker";

        private static readonly RoleDefinition[] Definitions = {
            new RoleDefinition(
                CafeOwner,
                "Cafe owner",
                false,
                new[] {
                    WorldActionKind.InspectProperty,
                    WorldActionKind.ServeCustomer,
                    WorldActionKind.ManageBusiness,
                    WorldActionKind.StockShelves
                }),
            new RoleDefinition(
                CafeWorker,
                "Cafe worker",
                false,
                new[] {
                    WorldActionKind.ServeCustomer,
                    WorldActionKind.StockShelves
                }),
            new RoleDefinition(
                Shopkeeper,
                "Shopkeeper",
                false,
                new[] {
                    WorldActionKind.InspectProperty,
                    WorldActionKind.ManageBusiness,
                    WorldActionKind.StockShelves
                }),
            new RoleDefinition(
                TransitDriver,
                "Transit driver",
                true,
                new[] {
                    WorldActionKind.DriveTransitRoute,
                    WorldActionKind.FileCivicReport
                }),
            new RoleDefinition(
                PoliceOfficer,
                "Police officer",
                true,
                new[] {
                    WorldActionKind.PatrolDistrict,
                    WorldActionKind.FileCivicReport
                }),
            new RoleDefinition(
                MedicalWorker,
                "Medical worker",
                true,
                new[] {
                    WorldActionKind.ProvideCare,
                    WorldActionKind.FileCivicReport
                })
        };

        public static bool TryGet(string roleId, out RoleDefinition definition)
        {
            for (int i = 0; i < Definitions.Length; i++) {
                if (Definitions[i].Id == roleId) {
                    definition = Definitions[i];
                    return true;
                }
            }

            definition = null;
            return false;
        }
    }
}
