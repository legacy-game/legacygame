# Project Structure

Folder layout for the Unity project. The leading underscore in `_Project` keeps our folders sorted to the top above third-party packages.

---

## Top-level repository layout

```
legacy/
├── Assets/
│   ├── _Project/        # Everything we author
│   ├── Plugins/         # Third-party DLLs
│   ├── ThirdParty/      # Imported asset-store / external packages
│   └── ...              # Unity-generated metadata
├── Packages/            # Unity package manifest
├── ProjectSettings/     # Unity project settings
├── docs/                # This GDD
├── art/                 # Non-imported art source files (NOT under Assets/)
│   └── _aseprite/       # Aseprite source (mirrored to Assets/_Project/Art/)
├── audio/               # Non-imported audio source (NOT under Assets/)
│   ├── music/
│   └── sfx/
├── .gitignore
├── .gitattributes       # Git LFS rules
└── README.md
```

---

## `Assets/_Project/` (our authoring root)

```
Assets/_Project/
├── Art/
│   ├── Characters/
│   │   ├── Player/
│   │   └── NPCs/
│   │       ├── Family/
│   │       ├── Customers/
│   │       └── Background/
│   ├── Buildings/
│   │   ├── Cafe/
│   │   ├── Apartment/
│   │   └── Street/
│   ├── Props/
│   ├── Tiles/
│   │   ├── Interior/
│   │   └── Exterior/
│   ├── UI/
│   ├── VFX/
│   └── Atlases/         # Generated sprite atlases per category
├── Audio/
│   ├── Music/
│   ├── SFX/
│   │   ├── Cafe/
│   │   ├── Footsteps/
│   │   └── UI/
│   └── Ambient/
├── Data/
│   ├── Items/
│   ├── Recipes/
│   ├── NPCs/
│   │   ├── Archetypes/
│   │   └── Instances/
│   ├── Dialogue/
│   ├── Schedules/
│   ├── BuildingKits/
│   └── Tiles/
├── Prefabs/
│   ├── Characters/
│   ├── Buildings/
│   ├── Items/
│   ├── UI/
│   ├── VFX/
│   └── Systems/         # Singletons & manager prefabs
├── Scenes/
│   ├── Bootstrap.unity         # First scene; spawns systems; transitions to MainMenu
│   ├── MainMenu.unity
│   └── Town/
│       ├── Veyne_Exterior.unity
│       ├── Veyne_CafeInterior.unity
│       └── Veyne_ApartmentInterior.unity
├── Scripts/             # See "Scripts" section below
├── Settings/            # URP assets, Input actions, Pixel Perfect Camera assets
└── Shaders/             # If/when needed
```

The default `Assets/Scenes/SampleScene.unity` should be **deleted** during Phase 0 setup. New scenes live under `Assets/_Project/Scenes/`.

---

## `Assets/_Project/Scripts/` (code root)

```
Scripts/
├── Core/                # Bootstrap, GameLoop, ServiceLocator, EventBus
│   ├── Bootstrap.cs
│   ├── GameLoop.cs
│   ├── ServiceLocator.cs
│   ├── EventBus.cs
│   └── Logger.cs
├── World/               # Tilemap, building kit, region streaming
│   ├── Region.cs
│   ├── TilemapLoader.cs
│   └── BuildingKit/
│       ├── BuildingPart.cs
│       ├── BuildingComposer.cs
│       └── Editor/
│           └── BuildingComposerWindow.cs
├── Time/                # World clock, schedule events
│   ├── WorldClock.cs
│   ├── ScheduledEvent.cs
│   └── Scheduler.cs
├── Characters/          # Player + character data
│   ├── PlayerController.cs
│   ├── PlayerData.cs
│   └── CharacterMover.cs
├── NPCs/                # AI, schedule, personality, memory
│   ├── NPCController.cs
│   ├── NPCArchetype.cs
│   ├── NPCInstance.cs
│   ├── PersonalityTraits.cs
│   ├── ScheduleStep.cs
│   ├── Memory.cs
│   └── CustomerVisitState.cs
├── Dialogue/
│   ├── DialogueAsset.cs
│   ├── DialogueLine.cs
│   ├── DialogueSystem.cs
│   └── UI/
│       ├── DialogueBoxUI.cs
│       └── DialogueBubbleUI.cs
├── Interaction/
│   ├── IInteractable.cs
│   ├── PlayerInteractor.cs
│   └── InteractionPromptUI.cs
├── Inventory/
│   ├── ItemDefinition.cs
│   ├── ItemStack.cs
│   ├── Inventory.cs
│   └── UI/
│       └── InventoryUI.cs
├── Currency/
│   ├── Wallet.cs
│   ├── Denomination.cs
│   └── UI/
│       └── CashDrawerUI.cs
├── Jobs/
│   ├── Job.cs
│   ├── JobShift.cs
│   └── Cafe/
│       ├── CafeShift.cs
│       ├── Recipe.cs
│       ├── PrepStation.cs
│       └── MiniGames/
│           ├── GrindMiniGame.cs
│           ├── TampMiniGame.cs
│           ├── PullShotMiniGame.cs
│           ├── PourMilkMiniGame.cs
│           ├── OvenMiniGame.cs
│           └── SteepMiniGame.cs
├── UI/
│   ├── HUD/
│   │   ├── ClockHUD.cs
│   │   └── WalletHUD.cs
│   ├── Menus/
│   │   ├── MainMenu.cs
│   │   └── PauseMenu.cs
│   └── Notifications/
│       └── NotificationManager.cs
├── Save/
│   ├── SaveData.cs
│   ├── SaveManager.cs
│   └── Migrations/
│       └── (one file per schema bump)
├── Audio/
│   ├── AudioManager.cs
│   ├── MusicDirector.cs
│   └── AmbientBed.cs
└── Utils/
    ├── Extensions/
    ├── DebugTools/
    └── Math/
```

---

## Namespaces

Mirror the folder structure under the `Legacy.` root namespace:

```csharp
namespace Legacy.Core { /* ... */ }
namespace Legacy.World { /* ... */ }
namespace Legacy.World.BuildingKit { /* ... */ }
namespace Legacy.Time { /* ... */ }
namespace Legacy.Characters { /* ... */ }
namespace Legacy.NPCs { /* ... */ }
namespace Legacy.Dialogue { /* ... */ }
namespace Legacy.Dialogue.UI { /* ... */ }
namespace Legacy.Interaction { /* ... */ }
namespace Legacy.Inventory { /* ... */ }
namespace Legacy.Currency { /* ... */ }
namespace Legacy.Jobs { /* ... */ }
namespace Legacy.Jobs.Cafe { /* ... */ }
namespace Legacy.Jobs.Cafe.MiniGames { /* ... */ }
namespace Legacy.UI.HUD { /* ... */ }
namespace Legacy.UI.Menus { /* ... */ }
namespace Legacy.UI.Notifications { /* ... */ }
namespace Legacy.Save { /* ... */ }
namespace Legacy.Audio { /* ... */ }
namespace Legacy.Utils { /* ... */ }
```

Editor scripts go under `Editor/` subfolders and use the `.Editor` namespace suffix:
```csharp
namespace Legacy.World.BuildingKit.Editor { /* ... */ }
```

---

## Assembly definitions (optional, for compile speed)

When the project grows past ~200 scripts, split into assembly definitions:

```
Legacy.Core.asmdef
Legacy.World.asmdef
Legacy.NPCs.asmdef
Legacy.Jobs.asmdef
Legacy.UI.asmdef
Legacy.Editor.asmdef   (Editor-only)
Legacy.Tests.asmdef    (Test-only)
```

**Defer adding these until needed.** Premature .asmdefs cause more pain than they solve.

---

## Scenes

| Scene | Purpose |
|---|---|
| `Bootstrap.unity` | First scene loaded. Instantiates singletons (ServiceLocator, EventBus, SaveManager, AudioManager). Loads MainMenu additively, then unloads itself. |
| `MainMenu.unity` | Title screen, new game / continue / quit. |
| `Town/Veyne_Exterior.unity` | The Linden Street block. |
| `Town/Veyne_CafeInterior.unity` | Inside the café. |
| `Town/Veyne_ApartmentInterior.unity` | The apartment upstairs. |

### Scene loading
- Use **additive scene loading**: keep Bootstrap's systems alive, additively load gameplay scenes
- One scene loaded at a time for gameplay; transition via fade-to-black

---

## Where things go (quick reference)

| What | Where |
|---|---|
| A new C# class | Appropriate subfolder under `Assets/_Project/Scripts/` |
| A new pixel-art sprite | Aseprite source in `art/_aseprite/`, imported to `Assets/_Project/Art/...` |
| A new audio clip | Source in `audio/`, imported to `Assets/_Project/Audio/...` |
| A new ScriptableObject asset (item, recipe, NPC) | `Assets/_Project/Data/...` in the matching subfolder |
| A new prefab | `Assets/_Project/Prefabs/...` in the matching subfolder |
| A new scene | `Assets/_Project/Scenes/...` |
| A new shader | `Assets/_Project/Shaders/` |
| A test | Mirror under `Assets/_Project/Tests/` with `.asmdef` referencing target assembly |

---

## `.gitignore` essentials

```
# Unity-generated
/Library/
/Temp/
/Obj/
/Build/
/Builds/
/Logs/
/UserSettings/
/MemoryCaptures/

# IDE
/.vs/
/.idea/
/.vscode/*
!/.vscode/settings.json
!/.vscode/extensions.json
*.csproj
*.unityproj
*.sln
*.suo
*.tmp
*.user
*.userprefs
*.pidb
*.booproj
*.svd
*.pdb
*.opendb
*.VC.db

# OS
.DS_Store
Thumbs.db

# Build artifacts
*.apk
*.aab
*.unitypackage

# Crash reports
sysinfo.txt

# Asset Store
/Assets/AssetStoreTools*
```

---

## `.gitattributes` (LFS)

```
# Unity tracked assets
*.unity filter=lfs diff=lfs merge=lfs -text
*.prefab filter=lfs diff=lfs merge=lfs -text
*.asset filter=lfs diff=lfs merge=lfs -text

# Images
*.png filter=lfs diff=lfs merge=lfs -text
*.psd filter=lfs diff=lfs merge=lfs -text
*.ase filter=lfs diff=lfs merge=lfs -text
*.aseprite filter=lfs diff=lfs merge=lfs -text
*.tga filter=lfs diff=lfs merge=lfs -text
*.exr filter=lfs diff=lfs merge=lfs -text
*.hdr filter=lfs diff=lfs merge=lfs -text

# Audio
*.wav filter=lfs diff=lfs merge=lfs -text
*.mp3 filter=lfs diff=lfs merge=lfs -text
*.ogg filter=lfs diff=lfs merge=lfs -text

# 3D (just in case)
*.fbx filter=lfs diff=lfs merge=lfs -text

# Text files (force text)
*.cs text
*.md text
*.json text
*.txt text
*.yaml text
```

---

## Documentation co-location

- Cross-cutting design lives in `docs/` (this folder)
- System-specific deep notes can live next to code as `README.md` in the folder
  - Example: `Assets/_Project/Scripts/Jobs/Cafe/README.md` for café-loop implementation notes
- Keep `docs/` as the source of truth; in-code READMEs are auxiliary
