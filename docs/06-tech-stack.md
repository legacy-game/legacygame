# Tech Stack

Locked stack for MVP. Designed to be: free, beginner-friendly, AI-assisted-friendly, and a clean foundation if/when multiplayer comes later.

---

## Engine

- **Unity 6.0.1f1** (already installed — see [`ProjectSettings/ProjectVersion.txt`](../ProjectSettings/ProjectVersion.txt))
- **Render pipeline:** Universal Render Pipeline (URP) — `com.unity.render-pipelines.universal` 17.1.0 (already installed)
- **2D Renderer** (URP feature) — locked for the project's lifetime
- **Scripting backend:** Mono in editor, IL2CPP for shipping builds (when builds happen)
- **API compatibility level:** .NET Standard 2.1 (default)
- **Color space:** Linear (URP default)

---

## Required Unity packages (to add)

These are not yet in [`Packages/manifest.json`](../Packages/manifest.json). Add them before Phase 1 of [12-roadmap.md](12-roadmap.md).

| Package | Purpose |
|---|---|
| `com.unity.2d.aseprite` | Native `.ase` import for pixel art |
| `com.unity.2d.pixel-perfect` | Pixel Perfect Camera component |
| `com.unity.cinemachine` | Camera follow + framing |
| `com.unity.nuget.newtonsoft-json` | JSON serialization for save files |
| `com.unity.localization` | Localization keys from day 1 (English-only initially) |

Optional but recommended:

| Package | Purpose |
|---|---|
| `com.unity.2d.spriteshape` | Curved sprite geometry (rivers, organic paths) |
| `com.unity.addressables` | Asset loading (defer until needed; YAGNI for slice) |

---

## Already-installed packages we use

From [`Packages/manifest.json`](../Packages/manifest.json):

- `com.unity.inputsystem` 1.14.0 — Input System (modern)
- `com.unity.render-pipelines.universal` 17.1.0 — URP
- `com.unity.ai.navigation` 2.0.7 — for future NPC pathfinding (overkill for MVP grid, available if we want)
- `com.unity.timeline` 1.8.7 — for sunset / cutscene sequences
- `com.unity.test-framework` 1.5.1 — unit + integration tests
- `com.unity.ugui` 2.0.0 — UGUI for UI
- All `com.unity.modules.*` standard modules

---

## Already-installed packages we don't need (and may remove)

- `com.unity.collab-proxy` 2.12.4 — Unity Cloud collaboration; not used (we use git)
- `com.unity.multiplayer.center` 1.0.0 — Multiplayer planning tool; not needed in MVP
- `com.unity.visualscripting` 1.9.6 — Visual scripting; not used (we write C#)

**Decision:** leave them installed for now; remove when we do a package audit at the end of Phase 0.

---

## UI framework choice

**Decision: UGUI** (not UI Toolkit).

### Why
- More documentation, tutorials, AI training data
- Easier for first-time Unity developers
- Sufficient performance for our use case
- Workflow well-understood

### When we'd reconsider
- v2.0+, if UI complexity grows substantially (rich in-world computer screens, deep modal hierarchies)

---

## Rendering specifics

### Pixel-perfect setup
- **Pixel Perfect Camera** component on the main camera
- **Reference Resolution:** 480 × 270
- **Pixels Per Unit:** 32
- **Upscale Render Texture:** ON
- **Crop Frame:** None
- **Stretch Fill:** OFF
- **Snap:** Pixel snapping on character movement

### URP 2D settings
- **Renderer:** 2D Renderer
- **Renderer Features:** none initially; add post-processing only if absolutely needed
- **Post-processing:** Off in MVP. The pixel-art look does not benefit from typical post effects.

### 2D Lighting
- **Sprite Light** for sun (1 global light, animated by time-of-day curve)
- **Point lights** for interior bulbs (warm color, low intensity)
- **Shadow:** off for sprites in MVP (cost vs. value)

### Sorting and depth
- Use a single shared **Custom Axis Sort Mode** along Y so sprites sort by Y position automatically
- Tilemap layers (Ground, Walls, Props_Lower, Props_Upper) for explicit ordering

---

## Audio stack

- **Unity built-in `AudioSource`** + `AudioMixer`
- One **AudioMixer** with groups: Master → { Music, SFX, Ambient, UI }
- 2D audio for music and UI; **3D-spatial audio set to 2D pan mode** for in-world SFX so they pan with screen position
- **No middleware** (Wwise/FMOD) — overkill for MVP

---

## Data & serialization

- **Newtonsoft.Json** for all save data (more flexible than Unity's `JsonUtility`)
- **ScriptableObjects** for all static game data:
  - Item definitions
  - Recipe definitions
  - NPC archetypes
  - Dialogue assets
  - Tile sets
  - Building kit parts
  - Schedules
- Save location: `Application.persistentDataPath/Legacy/saves/<slot>/`
- One save per "world" (later: multi-world support)
- Save schema versioned for forward migration

See [09-save-system.md](09-save-system.md) for full save spec.

---

## Networking & DB

- **None in MVP.** Single-player only.
- When the day comes:
  - **Prototype-scale multiplayer:** Mirror or FishNet (both Unity-native, free, mature)
  - **Production-scale persistent online:** Custom authoritative server in C# (.NET 8+) or Go, with PostgreSQL + Redis
  - **Database for prototype:** SQLite (local, file-based)
  - **Voice chat:** Vivox (Unity service) or LiveKit
- These are explicitly **out of scope** for MVP and not justified by anything in [05-vertical-slice-cafe-day.md](05-vertical-slice-cafe-day.md).

---

## Source control

- **Git** with **Git LFS** for binary assets
- Repository hosted on **GitHub** (private repo recommended) or **GitLab**
- `.gitignore` based on Unity's standard template — covers `Library/`, `Temp/`, `Logs/`, `obj/`, `Build/`, auto-generated `*.csproj` and `*.sln`
- **LFS-tracked extensions:** `*.png`, `*.psd`, `*.ase`, `*.wav`, `*.mp3`, `*.ogg`, `*.unity`, `*.prefab`, `*.fbx`, `*.tga`, `*.exr`, `*.hdr`

### Commit style
- One branch per feature/system: `feature/dialogue-system`, `fix/clock-pause-bug`
- Commit messages: `[Module] short summary` — e.g. `[NPCs] add scheduled arrival timer`
- Trunk-based with short-lived branches; merge to `main` frequently

### Unity-specific git settings
- `EditorSettings.serializationMode = ForceText` (text scenes/prefabs for diff-ability)
- `EditorSettings.assetPipelineMode = Version 2` (default in Unity 6)

---

## IDE

- **Recommended:** **JetBrains Rider** — free for non-commercial use, best Unity experience
- **Alternative:** **VS Code** with `C# Dev Kit` + `Unity` extensions (free)
- **Avoid:** Visual Studio 2022 Community for Unity — works but Rider/VS Code is faster

---

## External asset tools

| Tool | Purpose | Cost |
|---|---|---|
| **Aseprite** | Pixel art (industry standard) | $20 one-time on Steam |
| **Audacity** | Audio editing | Free |
| **REAPER** | DAW for music/SFX composition | Free indefinite eval, ~$60 personal license |
| **OBS Studio** | Recording playtests, devlogs | Free |
| **Tiled** | Optional alternative tilemap editor | Free |

---

## Testing

- **Unity Test Framework** (already installed)
- **Edit-mode tests** for pure logic:
  - Quality scoring math
  - Time arithmetic
  - Dialogue state machine
  - Inventory operations
  - Save-file roundtrip
- **Play-mode tests** for integration:
  - NPC schedule firing at correct in-game time
  - Save → load → state preserved
  - Customer visit state machine completes all transitions

---

## CI / build

- **GitHub Actions** with [game-ci/unity-actions](https://github.com/game-ci/unity-actions) for headless builds
- Free tier sufficient for a small team
- Build targets in MVP: Windows standalone first; macOS and Linux secondary

---

## Platforms (MVP)

| Platform | Status |
|---|---|
| Windows 10/11 (x64) | Primary target |
| macOS (Intel + Apple Silicon) | Secondary, after Windows ships |
| Linux (x64) | Tertiary, supported via Unity build but unverified |
| Steam Deck | Out of scope for MVP; eventually verified compatible |
| Web (WebGL) | Out of scope (pixel art works but save/audio are flaky) |
| Mobile / console | Out of scope, ever (genre and controls don't fit) |

---

## Display & input

- **Internal resolution:** 480 × 270
- **Window resolutions:** scaled integer multiples (1× = 480×270, 2× = 960×540, 3× = 1440×810, 4× = 1920×1080)
- **Fullscreen:** support borderless fullscreen at any monitor resolution; integer-scale where possible
- **Aspect ratio:** 16:9 (480×270). Ultrawide handled with letterbox bars in MVP.
- **Input:** keyboard + mouse primary; gamepad support targeted but not blocking for slice

---

## Performance targets

- **60 fps** on a 2020-era mid-range laptop (e.g. Intel i5-10th gen, 8GB RAM, integrated graphics)
- **<2 GB** RAM total
- **<500 MB** download size for MVP build
- **<5 sec** cold start to main menu
- **<2 sec** scene transitions

---

## Localization

- Use **Unity Localization** package from day 1, English-only
- All player-facing strings via key + table lookup
- Locale-aware date/time formatting via .NET's `CultureInfo` (default `en-US` for MVP)
- Localization to other languages is a v1.0+ concern

---

## Accessibility (MVP minimum)

- Keyboard-only playable (no mouse-required interactions)
- Rebindable keys via Input System
- Text size legible at all supported resolutions
- Color choices not solely conveying information (use icons + color together)
- No flashing/strobing effects
- Subtitle/dialogue text always on
- Mute toggle on every audio category (Master, Music, SFX, Ambient)

Full accessibility (screen reader, colorblind palettes, motor accessibility) is a v1.0+ goal.

---

## What we will not use (and why)

| Thing | Why not |
|---|---|
| **DOTS / ECS** | Overkill; our object counts are tiny; complexity not justified |
| **Visual Scripting** | We write C#; visual scripting trains nothing useful |
| **Unity Multiplayer / Netcode for GameObjects** | If/when we do multiplayer, Mirror or FishNet are better fits |
| **Asset Store characters/animations** | Style won't match our pixel art |
| **AI image generation in shipped assets** | Inconsistent style at 32×32; we use original or commissioned art |
| **Microtransactions / IAP packages** | Not part of this game, ever |
| **Analytics SDKs (Unity Analytics, GameAnalytics)** | Privacy stance; we'll measure manually if needed |
