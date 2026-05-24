# Glossary

Vocabulary used across the GDD. When in doubt, here.

---

## Project terms

| Term | Definition |
|---|---|
| **legacy** | Working title of this project |
| **GDD** | Game Design Document — this folder |
| **MVP** | Minimum Viable Product — the first shippable artifact |
| **First playable shared-world prototype** | The current milestone: a small playable block of Veyne where citizens, property ownership, jobs, money, time, and history all persist in one shared `WorldState`. |
| **Café testbed** | The early café prototype used to test jobs, NPCs, dialogue, money, and persistence. It is not the game’s current milestone. |
| **Spine** | The minimum set of systems without which nothing else can be built. Tagged `[SPINE]` in [02-system-list.md](02-system-list.md) |
| **Pillar** | A locked, foundational design commitment. See [01-vision-and-pillars.md](01-vision-and-pillars.md) |
| **Principle** | A hard design rule we don't break. See [04-design-principles.md](04-design-principles.md) |

## Design concepts

| Term | Definition |
|---|---|
| **Expansive system** | A system designed to describe *what becomes possible*. Opposite of restrictive. |
| **Restrictive system** | A system designed to describe *what is permitted*. To be avoided in our design. |
| **Cozy moment, civic meta** | The two-layer structure: peaceful day-to-day with a civic/political/dramatic layer on top |
| **Frontier** | Undeveloped wilderness/rural territory outside the seeded capital and older towns. Players can eventually claim, build, settle, and govern it. |
| **Territory chunk** | A large land unit above plots/parcels, used to track biome, claim status, buildability, settlement, and jurisdiction. |
| **Settlement** | A named inhabited place that can grow from claimed territory into a village, town, city, or capital. |
| **Jurisdiction** | A governed area with public records, rules, borders, and authority. Country-scale jurisdictions emerge late from civic systems. |
| **History log** | Append-only world record of named events. The "world that remembers" backbone. |
| **Generated personality** | NPC traits (warmth, talkativeness, generosity, mood) rolled at spawn from archetype ranges. Not LLM-driven. |
| **Seeded canon** | Commissioned cultural content (music, books, films) shipped with the game as the world's pre-existing "classical era." |
| **Player-driven** | A system whose primary inputs come from players, not developers or scripts |
| **Authored content** | Developer-written narrative — quests, scripted stories, fixed plot. We do not ship this. |
| **Diegetic** | Existing within the fiction of the game. The wall clock telling time is diegetic; a "TIME: 06:30" debug overlay is not. |

## Game-world entities

| Term | Definition |
|---|---|
| **Marenne** [PLACEHOLDER] | The seeded starting country; not the permanent limit of future player-founded civilizations |
| **Hesperin** [PLACEHOLDER] | The seeded capital city and long-term dense civilization anchor |
| **Aldwich Province** [PLACEHOLDER] | The province where Veyne sits |
| **Veyne** [PLACEHOLDER] | The first playable town/region and the testbed for shared-world systems |
| **Linden Café** | Player's café at 14 Linden Street |
| **Linden Street** | The cross-street the café is on |
| **The Crow** | A dive bar on Main Street; recurring social space |
| **Veyne Sentinel** | Local newspaper |
| **FC Westmere** | Aldwich Province's football team |
| **Atlas Hands** | Seeded canon indie-folk musician |
| **Lina Forge** | Seeded canon 1990s pop star |

## Slice characters (all [PLACEHOLDER] names)

| Name | Role |
|---|---|
| **Mara / Eli** | Player character — café owner |
| **Rowan** | Player's spouse |
| **June** | Player's 14-year-old daughter |
| **Mr. Holland** | Retired teacher, regular customer |
| **Sasha** | 28-year-old courier, terse regular |
| **Edie** | 41-year-old journalist |
| **Frances** | 35-year-old single mother |
| **Old Mr. Pell** | 78-year-old widower |
| **The Stranger** | Procedurally-generated walk-in (different every save) |

## System tags

| Tag | Meaning |
|---|---|
| **[LOCKED]** | Decision is final unless the entire project pivots |
| **[OPEN]** | Decision pending; alternatives listed |
| **[PLACEHOLDER]** | Name or value is provisional, will be revised |
| **[MVP]** | Required for the first playable shared-world prototype |
| **[V1.0]** | Required for first public multiplayer release |
| **[V2.0+]** | Long-horizon vision; not on near-term plan |
| **[SPINE]** | Load-bearing, blocks other work if absent |

## Tech terms

| Term | Definition |
|---|---|
| **URP** | Universal Render Pipeline (Unity) |
| **URP 2D Renderer** | The 2D-specific renderer feature of URP |
| **PPU** | Pixels Per Unit (Unity sprite import setting) |
| **LFS** | Large File Storage (Git extension for binary assets) |
| **Tilemap** | Unity's 2D tile-based map system |
| **ScriptableObject** | Unity asset that holds serialized data; used here for all static game content |
| **MonoBehaviour** | Unity component class; the view layer in our architecture |
| **PrepStation** | A café equipment object (grinder, oven, etc.) that hosts a mini-game |
| **Visit state machine** | Per-customer state tracker for the café work loop |
| **Z-sort** | Sprite sorting by Y position for depth in 2.5D |
| **TimeBand** | A named portion of the day (Dawn, Morning, Midday, Afternoon, Dusk, Evening, Night) |

## Project structure terms

| Term | Definition |
|---|---|
| **`_Project/`** | Our authored asset root (leading underscore sorts it first) |
| **`ThirdParty/`** | Imported external packages, kept separate from our work |
| **`docs/`** | This folder — the GDD |
| **`art/`** | Non-imported pixel art source files (Aseprite masters) |
| **`audio/`** | Non-imported audio source files |

## Audio terms

| Term | Definition |
|---|---|
| **Bed** | Ambient sound layer (room tone, street noise) that plays continuously per location |
| **Crossfade** | Smooth audio transition between two clips |
| **Snapshot** | Saved state of mixer settings, used for context-sensitive volumes (e.g. ducking on dialogue) |
| **Blip** | Tiny per-character sound played per displayed letter in typewriter dialogue |
| **LUFS** | Loudness Units Full Scale — perceptual loudness measure |
| **Ducking** | Temporarily reducing one audio source's volume so another can be heard clearly |

## Roles

| Role | What they do |
|---|---|
| **Dev lead** | Project owner; technical direction; final design call |
| **Gameplay programmer** | Implements game logic in C# |
| **Pixel artist** | Authors sprites, tiles, props, UI in Aseprite |
| **Composer** | Writes music, ambient beds |
| **Sound designer** | Records / synthesizes / composes SFX |
| **Writer** | Authors NPC dialogue, newspaper articles, world flavor |
| **World-builder** | Authors world-bible material, town layout, NPC schedules |
| **Playtester** | Plays builds; gives structured feedback |
| **Community / moderator** | (Post-launch only) Manages community channels and moderation |

---

## When you don't know a term

1. Check this glossary
2. Check the relevant system doc in [11-systems/](11-systems/)
3. Check [04-design-principles.md](04-design-principles.md)
4. Ask in the team Discord
5. If nobody knows, the term may not exist yet — flag it and we'll add it
