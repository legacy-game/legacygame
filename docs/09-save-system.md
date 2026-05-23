# Save System

The MVP save system. Single-player local. JSON. Versioned. Human-readable for debugging.

---

## Goals (MVP)

- Save a complete world snapshot at end-of-day and on manual save
- Survive crashes via autosave (every 5 in-game minutes)
- Versioned schema so old saves can load with migrations
- Human-readable on disk for debugging
- Round-trip integrity: load → save → load produces identical state

---

## Format

- **JSON via Newtonsoft.Json** (more flexible than Unity's `JsonUtility`)
- Pretty-printed with 2-space indent in development; compact in shipping builds
- UTF-8 encoded

---

## File layout

```
%AppData%\Legacy\saves\<slot>\
  ├── world.json          # The full world snapshot
  ├── profile.json        # Player profile metadata (not world state)
  ├── thumbnail.png       # 480x270 screenshot of last gameplay frame
  └── backups/
      ├── world.20260522-200100.json   # Autosave backups (last 5 kept)
      └── ...
```

Resolved via `Application.persistentDataPath`:
- Windows: `C:\Users\<user>\AppData\LocalLow\<Company>\Legacy\saves\<slot>\`
- macOS: `~/Library/Application Support/<Company>/Legacy/saves/<slot>/`
- Linux: `~/.config/unity3d/<Company>/Legacy/saves/<slot>/`

---

## Schema (v1)

```jsonc
{
  "schemaVersion": 1,
  "saveTime": "2026-05-22T20:00:00Z",
  "playTimeRealSeconds": 1842,
  "gameVersion": "0.1.0",

  "world": {
    "currentDayInGame": 1,
    "currentDateInGame": "2003-05-14",
    "currentTimeInGame": "06:30",
    "currentScene": "Veyne_ApartmentInterior"
  },

  "player": {
    "name": "Mara",
    "pronouns": "they/them",
    "appearance": {
      "skinIndex": 2,
      "hairColorIndex": 4,
      "shirtColorIndex": 1
    },
    "position": { "scene": "Veyne_ApartmentInterior", "x": 12.5, "y": 4.0 },
    "facing": "down",
    "wallet": {
      "bills":  { "1": 5, "5": 3, "10": 1, "20": 2, "50": 0, "100": 0 },
      "coins":  { "0.05": 4, "0.10": 2, "0.25": 7, "1.00": 3 }
    },
    "stats": {
      "baristaSkill": 0.0
    },
    "inventory": []
  },

  "family": {
    "spouse": {
      "id": "rowan",
      "name": "Rowan",
      "relationship": 10,
      "lastInteractionDay": 0
    },
    "children": [
      {
        "id": "june",
        "name": "June",
        "age": 14,
        "relationship": 8,
        "lastInteractionDay": 0
      }
    ]
  },

  "npcs": [
    {
      "id": "holland",
      "displayName": "Mr. Holland",
      "relationship": 0,
      "lastSeenDay": 0,
      "lastTopic": null,
      "mood": "warm",
      "metPlayer": false
    }
  ],

  "cafe": {
    "tillToday": 0.00,
    "tillLifetime": 0.00,
    "shiftsCompleted": 0,
    "inventory": {
      "coffeeBeans": 100,
      "milk": 20,
      "pastries": 8,
      "tea": 30
    },
    "openSign": false
  },

  "history": [
    {
      "id": "evt_0001",
      "timestamp": "2003-05-14T06:30:00",
      "kind": "DayStarted",
      "description": "A new day begins in Veyne.",
      "actors": []
    }
  ]
}
```

---

## `profile.json` (separate, lighter)

```jsonc
{
  "schemaVersion": 1,
  "slotName": "Mara",
  "createdAt": "2026-05-21T20:00:00Z",
  "lastPlayedAt": "2026-05-22T20:00:00Z",
  "totalPlayTimeRealSeconds": 1842,
  "completedDays": 1,
  "thumbnailPath": "thumbnail.png"
}
```

The MainMenu reads `profile.json` for each slot to render the slot list — it never has to load the heavy `world.json` for menu display.

---

## When we save

| Event | What writes | Backup? |
|---|---|---|
| End of in-game day | Full save | Yes (rotates) |
| Manual save (menu) | Full save | Yes |
| Autosave (every 5 in-game minutes) | Full save | Yes |
| Application quit | Full save | No |
| Application loses focus (configurable) | Full save | No |
| Crash | (handled via autosave) | n/a |

---

## Backup rotation

- Keep the **last 5 autosaves** in `backups/`
- Named with timestamp suffix
- On load, if `world.json` is corrupt, automatically offer the most recent backup

---

## Migration

Each schema version is paired with a one-way migration function.

```csharp
public static class Migrations
{
    public static JObject Migrate(JObject save)
    {
        int version = save.Value<int?>("schemaVersion") ?? 0;

        while (version < CurrentVersion) {
            save = version switch
            {
                1 => Migrate_v1_to_v2(save),
                2 => Migrate_v2_to_v3(save),
                _ => throw new InvalidOperationException($"No migration from v{version}")
            };
            version++;
        }

        return save;
    }

    private static JObject Migrate_v1_to_v2(JObject save) { /* ... */ }
}
```

Migrations live in `Assets/_Project/Scripts/Save/Migrations/`, one file per bump.

**Rule:** when changing the schema in a non-trivial way, bump the version *and* write the migration *in the same PR*. No exceptions.

---

## What is NOT in MVP saves

- Multiplayer state
- Multi-character / family-line continuity beyond current household
- World map / multiple regions
- Player-created cultural artifacts (audio files, text documents, paintings) — those live in a separate `userdata/` folder
- Replay history (we record events, not replays)

---

## API

```csharp
namespace Legacy.Save
{
    public interface ISaveManager
    {
        UniTask<SaveResult> SaveAsync(string slot, CancellationToken ct = default);
        UniTask<SaveResult> LoadAsync(string slot, CancellationToken ct = default);
        UniTask<SlotInfo[]> ListSlotsAsync();
        UniTask<bool> DeleteSlotAsync(string slot);
        UniTask<bool> ExportSlotAsync(string slot, string targetPath);
    }
}
```

*(Substitute `Task` for `UniTask` since we are not using UniTask per [08-code-conventions.md](08-code-conventions.md). API surface is illustrative.)*

```csharp
public sealed class SaveManager : ISaveManager
{
    public async Task<SaveResult> SaveAsync(string slot, CancellationToken ct = default)
    {
        var snapshot = WorldSnapshot.Capture();
        var json = JsonConvert.SerializeObject(snapshot, _jsonSettings);

        var path = _paths.WorldPath(slot);
        await File.WriteAllTextAsync(path, json, ct);

        await ProfileWriter.UpdateAsync(slot, snapshot, ct);
        await BackupRotator.RotateAsync(slot, ct);

        EventBus.Publish(new WorldSaved(slot));
        return SaveResult.Ok;
    }
}
```

---

## Performance

- Slice save target: **< 200 ms** end-to-end
- JSON serialization on a background thread when possible
- File write atomic via write-to-temp + rename

---

## Save corruption handling

1. On load, try `world.json` first
2. If parse fails: log error, attempt latest backup
3. If all backups fail: surface a clear error to player; offer to start a new game; preserve the corrupt files in a `corrupt/` folder so support/dev can inspect

Never silently lose progress. Never silently overwrite a save that failed to load.

---

## Testing

- **Unit:** schema serialization round-trip for each top-level type
- **Unit:** migration v1 → v2 → ... → current
- **Integration:** change shared world state (time, ownership, citizen/NPC state, money, history), save, kill process, reload, verify state
- **Stress:** save → load → save → load 100 times, verify no drift in JSON
