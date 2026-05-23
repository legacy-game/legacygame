using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Legacy.World;
using UnityEngine;

namespace Legacy.Save
{
    public sealed class SaveManager : ISaveManager
    {
        private const int BackupCount = 5;
        private readonly SavePaths _paths;

        public SaveManager(SavePaths paths)
        {
            _paths = paths;
        }

        public async Task<SaveResult> SaveAsync(string slot, WorldState state, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(slot)) {
                throw new ArgumentException("Slot must not be empty.", nameof(slot));
            }

            if (state == null) {
                throw new ArgumentNullException(nameof(state));
            }

            string slotDirectory = _paths.SlotDirectory(slot);
            Directory.CreateDirectory(slotDirectory);
            Directory.CreateDirectory(_paths.BackupDirectory(slot));

            string worldPath = _paths.WorldPath(slot);
            string tempPath = _paths.TempWorldPath(slot);
            string json = JsonUtility.ToJson(WorldSaveMapper.ToSaveData(state), true);

            await File.WriteAllTextAsync(tempPath, json, ct);
            RotateBackup(slot, worldPath);

            if (File.Exists(worldPath)) {
                File.Delete(worldPath);
            }

            File.Move(tempPath, worldPath);
            return SaveResult.Ok;
        }

        public async Task<WorldState> LoadAsync(string slot, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(slot)) {
                throw new ArgumentException("Slot must not be empty.", nameof(slot));
            }

            string worldPath = _paths.WorldPath(slot);
            string json = await File.ReadAllTextAsync(worldPath, ct);
            WorldSaveData data = JsonUtility.FromJson<WorldSaveData>(json);
            return WorldSaveMapper.ToRuntime(SaveMigrator.Migrate(data));
        }

        private void RotateBackup(string slot, string worldPath)
        {
            if (!File.Exists(worldPath)) {
                return;
            }

            string backupDirectory = _paths.BackupDirectory(slot);
            string backupPath = Path.Combine(backupDirectory, $"world.{DateTime.UtcNow:yyyyMMdd-HHmmss}.json");
            File.Copy(worldPath, backupPath, true);

            string[] backups = Directory.GetFiles(backupDirectory, "world.*.json");
            Array.Sort(backups, StringComparer.Ordinal);

            for (int i = 0; i < backups.Length - BackupCount; i++) {
                File.Delete(backups[i]);
            }
        }
    }
}
