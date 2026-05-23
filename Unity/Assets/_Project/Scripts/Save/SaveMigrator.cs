namespace Legacy.Save
{
    public static class SaveMigrator
    {
        public const int CurrentVersion = 1;

        public static WorldSaveData Migrate(WorldSaveData saveData)
        {
            // Schema v1 is the first world-memory format. Future migrations should be one-way and deterministic.
            return saveData;
        }
    }
}
