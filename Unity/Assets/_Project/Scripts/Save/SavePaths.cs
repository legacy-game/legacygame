using System.IO;

namespace Legacy.Save
{
    public sealed class SavePaths
    {
        private readonly string _root;

        public SavePaths(string root)
        {
            _root = root;
        }

        public string SlotDirectory(string slot)
        {
            return Path.Combine(_root, "Legacy", "saves", slot);
        }

        public string WorldPath(string slot)
        {
            return Path.Combine(SlotDirectory(slot), "world.json");
        }

        public string TempWorldPath(string slot)
        {
            return Path.Combine(SlotDirectory(slot), "world.tmp.json");
        }

        public string BackupDirectory(string slot)
        {
            return Path.Combine(SlotDirectory(slot), "backups");
        }
    }
}
