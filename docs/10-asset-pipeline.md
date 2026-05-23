# Asset Pipeline

How assets flow from source files to Unity to runtime.

For art *direction* (palette, mood, style), see [10a-art-direction.md](10a-art-direction.md).
For audio *direction* (mood, instrumentation), see [10b-audio-direction.md](10b-audio-direction.md).

---

## Pixel art

### Source files
- Authored in **Aseprite**
- Stored in `art/_aseprite/` *outside* `Assets/` — these are the editable masters
- Mirrored copies of the exported `.ase` go into `Assets/_Project/Art/...` for direct Unity import via the Aseprite Importer package

### Import settings (in Unity)
| Setting | Value |
|---|---|
| **Import Mode** | Aseprite Importer |
| **Pixels Per Unit** | **32** |
| **Filter Mode** | **Point (no filter)** |
| **Compression** | **None** |
| **Generate Mip Maps** | OFF |
| **Wrap Mode** | Clamp |
| **Sprite Mode** | Multiple (auto-sliced from Aseprite tags) |
| **Mesh Type** | Tight |
| **Generate Physics Shape** | OFF (default; ON only where collider is needed) |

### Aseprite file structure
- **One Aseprite file per character archetype**
- Layers used for clothing/equipment overlay (so we can layer hat / shirt / pants without re-drawing the whole sprite)
- **Tags** define animation clips:
  - `idle_down`, `idle_up`, `idle_left`, `idle_right`
  - `walk_down`, `walk_up`, `walk_left`, `walk_right`
  - `interact_down`, etc. (when needed)
- Tags are imported as Unity AnimationClips automatically

### Sprite atlases
- One sprite atlas per category to batch draw calls:
  - `Atlas_Characters_Player`
  - `Atlas_Characters_NPCs`
  - `Atlas_Tiles_Interior`
  - `Atlas_Tiles_Exterior`
  - `Atlas_Props_Cafe`
  - `Atlas_Props_Apartment`
  - `Atlas_UI`
- Auto-included rules per category
- Padding: 4px (avoid bleeding at low zoom)

---

## Tiles (32×32)

### Authoring
- Tiles authored in Aseprite as tile templates (32×32 grid)
- For terrain that benefits from blending: use Aseprite's tile mode + Unity's RuleTile

### Import workflow
1. Aseprite file containing a tilesheet (a grid of 32×32 tiles)
2. Imported as Sprite Sheet, sliced 32×32 automatically by Aseprite Importer
3. Tiles authored as Tile assets via Unity's Tile Palette workflow
4. Tilemap placed in scene using Tile Palette window

### Tilemap layers per scene
- `Ground` — floor/sidewalk/grass; bottom of z-order
- `Walls` — vertical surfaces; mid z-order; provides collision
- `Roofs` — overhead pieces; top z-order on interiors
- `Props_Lower` — sprites sorted below player
- `Props_Upper` — sprites sorted above player (z-sorted by Y)

### Tile naming
- `tile_<set>_<descriptor>` — `tile_interior_wood_floor_01`, `tile_exterior_sidewalk_corner_ne`
- Variants suffixed: `_01`, `_02` for random visual variation

---

## Animation

### 4-directional movement (per character)
- 4 idle frames + 8 walk frames per direction = 48 frames per character archetype
- Stored as Aseprite tags inside the character's `.ase` file
- Animator Controller per character archetype with state transitions driven by `velocity` and `facing` parameters

### Animation curves
- All character animation is frame-stepped (no interpolation)
- Animator update mode: **Animate Physics** off; **Unscaled Time** off (we control time elsewhere)

### Layered sprites (clothing/equipment)
- Player and NPCs use 3 sprite layers stacked at same position:
  - `Body` (skin + base)
  - `Clothing` (shirt/pants/dress)
  - `Hair` (over Clothing, under Hat)
- Each layer has its own SpriteRenderer; same Animator Controller drives all three via shared parameters
- Allows mix-and-match without exploding the asset count

---

## Audio

### Source files
- Authored in **REAPER** or **Audacity**
- Stored in `audio/` outside `Assets/`
- Exported masters to `Assets/_Project/Audio/...`

### Formats
| Type | Format | Sample rate | Channels |
|---|---|---|---|
| Music | OGG Vorbis | 44.1 kHz | Stereo |
| SFX | WAV (16-bit PCM) | 44.1 kHz | Mono |
| Ambient beds | OGG Vorbis | 44.1 kHz | Stereo |
| UI clicks/blips | WAV | 44.1 kHz | Mono |

### Loudness targets
- Music: **-14 LUFS** integrated
- SFX: **-6 dB peak** normalized
- Ambient: **-18 LUFS** integrated
- All mixed through the AudioMixer at runtime — no need to pre-attenuate

### Import settings
| Setting | Music | SFX | Ambient |
|---|---|---|---|
| **Load Type** | Streaming | Decompress on Load | Streaming |
| **Compression Format** | Vorbis | PCM | Vorbis |
| **Quality** | 80 | n/a | 60 |
| **Force To Mono** | No | Yes | No |
| **Preload Audio Data** | No | Yes | No |

### Naming
- `mus_<scene>_<mood>` — `mus_cafe_morning`, `mus_apartment_evening`
- `sfx_<category>_<name>` — `sfx_cafe_grinder_loop`, `sfx_footstep_wood_01`
- `amb_<location>` — `amb_cafe_interior_murmur`, `amb_street_linden_day`
- Variants suffixed `_01`, `_02` for random selection

---

## File naming (general)

| Asset type | Pattern | Example |
|---|---|---|
| Character spritesheet | `char_<name>` | `char_mara`, `char_holland` |
| Tile sheet | `tile_<set>` | `tile_interior_wood`, `tile_exterior_brick` |
| Prop | `prop_<category>_<name>` | `prop_cafe_table_01`, `prop_apartment_couch` |
| Building piece | `kit_<category>_<name>` | `kit_wall_brick_straight`, `kit_door_interior` |
| UI sprite | `ui_<category>_<name>` | `ui_hud_clock_frame`, `ui_dialogue_bg` |
| Music | `mus_<scene>_<mood>` | `mus_cafe_morning` |
| SFX | `sfx_<category>_<name>` | `sfx_cafe_grinder` |
| Ambient | `amb_<location>` | `amb_street_day` |

**No spaces, no capital letters in asset names.** Underscores only.

---

## Versioning art

- Aseprite source files are LFS-tracked
- Old versions live in git history (don't keep `_v2.ase` files; use git)
- If branching art experiments, use a git branch, not file copies

---

## Asset import validation

- A small editor script (`Editor/AssetImportValidator.cs`) checks on import:
  - Pixel art: PPU = 32, filter = point, compression = none → warn if wrong
  - Audio: sample rate = 44.1 kHz → warn if wrong
  - Naming: matches the convention → warn if wrong
- Warnings only; never blocks import. Author fixes when convenient.

---

## Asset bundles / Addressables

- **Not used in MVP.** Direct asset references via Inspector or ScriptableObjects.
- **Reconsider for v1.0** if download size or memory becomes a real problem (likely with multi-region streaming).

---

## Asset hygiene

- Every asset has a meaningful name (no `Sprite_001.png`)
- Unused assets are deleted (don't accumulate)
- Every imported asset is used in at least one scene or prefab (validated by `Find References In Scene`)
- Quarterly asset audit: scan for orphan assets and remove

---

## Pipeline for new contributors

1. Pull repo, ensure Git LFS installed (`git lfs install`)
2. Open in Unity 6.0.1f1
3. Wait for initial asset import (can take 5–15 minutes first time)
4. Verify nothing is missing or pink in the test scene
5. Author your asset in the appropriate external tool
6. Save / export per the rules above
7. Drop into the matching `Assets/_Project/...` folder
8. Verify import settings (the import validator will warn if wrong)
9. Reference it in a scene, prefab, or ScriptableObject
10. Commit with `[Art]` / `[Audio]` prefix
