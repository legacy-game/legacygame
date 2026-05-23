# System List

Reference catalog of every named system the full game contains. Each system is designed to be **expansive, not restrictive** per [04-design-principles.md](04-design-principles.md).

## Tags

- **[SPINE]** — required for the first playable shared-world prototype; blocks other work if absent
- **[MVP]** — required in MVP at minimal scope
- **[V1.0]** — required for first public multiplayer release
- **[V2.0+]** — long-horizon vision; not on near-term plan

---

## 1. Foundation (platform)

| ID | System | Tag | One-line scope |
|---|---|---|---|
| F1 | Account & Auth | V1.0 | Player accounts, login, ban state, multi-character ownership |
| F2 | Persistence Layer | V1.0 | Authoritative database of the entire world (characters, property, items, history) |
| F3 | Networking & Replication | V1.0 | Server-authoritative client/server sync of nearby world state |
| F4 | Region Sharding & Handoff | V1.0 | Spatial partitioning of the world across server nodes; clean transitions between them |
| F5 | World Clock & Calendar | SPINE | Authoritative game time, including day/night and date — local-only in MVP |
| F6 | History Log | V1.0 | Append-only record of named world events; "world that remembers" backbone |
| F7 | Telemetry & Server Health | V1.0 | Metrics, logs, alerting for the live world |
| F8 | Server Authority & Anti-Cheat | V1.0 | All gameplay decisions made server-side; client is a renderer/input device |

## 2. Character & Life

| ID | System | Tag | One-line scope |
|---|---|---|---|
| C1 | Character Creator | SPINE | Visual + biographical creation (face, body, name, starting region, background) |
| C2 | Stats & Skills | SPINE | Per-character attributes and learnable skills tied to jobs and combat (minimal MVP) |
| C3 | Health & Disease | V1.0 | Hunger, sleep, sickness, injury, chronic conditions, medical care |
| C4 | Event-Based Death | V1.0 | Probabilistic death from disease, violence, accident, rare old-age fade |
| C5 | Family Lines | V1.0 | Persistent lineage tree spanning multiple characters across players and NPCs |
| C6 | Marriage & Adoption | V1.0 | Pair-bonding and family-formation between any combination of players and NPCs |
| C7 | Inheritance Resolution | V1.0 | Property/title/debt transfer at death; old family becomes NPC-controlled |
| C8 | Reputation & Public Record | V1.0 | Persistent in-world standing that survives the character's death |
| C9 | Education & Schooling | V1.0 | Childhood schooling for NPC children; skill acquisition pathway for adults |

## 3. World

| ID | System | Tag | One-line scope |
|---|---|---|---|
| W1 | World Map & Geography | V1.0 | Global map structure, biomes, named regions |
| W2 | Region Definition / Streaming | SPINE | Per-region tile data, prop placement, on-demand streaming — single region in MVP |
| W3 | Tilemap (32×32) | SPINE | Tiled rendering and collision foundation |
| W4 | Modular Building Kit | SPINE | Snap-together walls/floors/roofs/doors/windows for procedural building assembly |
| W5 | Day/Night & Weather | SPINE | Lighting and weather simulation per region — day only in MVP |
| W6 | Calendar / Seasons | V1.0 | Year structure, holidays, seasonal effects |
| W7 | Frontier Expansion | V1.0 | Periodic opening of new regions for new-player settlement and growth |
| W8 | Public Infrastructure (abstracted) | V1.0 | Roads, power, water, sewer existence and damage states |

## 4. Property & Economy

| ID | System | Tag | One-line scope |
|---|---|---|---|
| P1 | Land Parcel Ownership | V1.0 | Owned, leased, public, and unclaimed parcels |
| P2 | Building Ownership & Tenancy | V1.0 | Ownership records and rental/lease contracts on buildings |
| P3 | Vehicles & Ownership | V1.0 | Cars, trucks, motorcycles; registration, parking, theft, accidents |
| P4 | Items & Inventory | SPINE | Item definitions, stacks, containers, persistence |
| P5 | Currency & Banking | SPINE | In-world cash + bank accounts; loans, savings, transfers — cash only in MVP |
| P6 | Storefronts & Retail | SPINE | Player-run shops with shelves, pricing, point-of-sale — café in MVP |
| P7 | Markets & Trade | V1.0 | Wholesale, supply chains, inter-region commerce |
| P8 | Supply / Demand / Pricing | V1.0 | Dynamic pricing driven by player consumption and production |
| P9 | Taxation | V1.0 | Government revenue from property, sales, and income taxes |
| P10 | Insurance | V2.0+ | Optional coverage for theft, fire, vehicle, life |
| P11 | Black Market | V1.0 | Gray-zone economy for illegal goods, services, and weapons |

## 5. Jobs (load-bearing)

| ID | System | Tag | One-line scope |
|---|---|---|---|
| J1 | Job Framework | SPINE | Definition layer: shifts, employers, wages, requirements, performance |
| J2 | Job Mini-Game Library | SPINE | Per-profession gameplay loops — café only in MVP |
| J3 | Employment Contracts | V1.0 | Hireable/fireable relationships between employers and employees |
| J4 | NPC Worker Backfill | V1.0 | NPCs autonomously fill posted jobs that no player takes |
| J5 | Apprenticeship / Mentorship | V1.0 | Player-to-player skill transfer |

## 6. Society & Politics

| ID | System | Tag | One-line scope |
|---|---|---|---|
| S1 | Government Structure | V1.0 | Offices (mayor, councilors, regional officials), terms, hierarchies |
| S2 | Elections & Voting | V1.0 | Candidate registration, campaigning, voting, results |
| S3 | Lawmaking & Statutes | V1.0 | Authored, debated, ratified laws that have real in-world effects |
| S4 | Petitions & Referenda | V1.0 | Citizen-initiated processes for legal change |
| S5 | Civic Records / Public Registry | V1.0 | Public-facing records of laws, owners, officials, court rulings |
| S6 | Revolution (tiered) | V1.0 | Civic upheaval; armed uprising unlocks when civic path is suppressed |

## 7. Crime & Justice

| ID | System | Tag | One-line scope |
|---|---|---|---|
| X1 | Crime Recognition | V1.0 | Detection of criminal acts via witnesses and evidence |
| X2 | Forensics | V1.0 | Period-appropriate fingerprints, blood typing, early DNA, CCTV in cities |
| X3 | Investigation Tools | V1.0 | Detective workflows for player and NPC cops |
| X4 | Arrest & Detention | V1.0 | Holding cells, due process, bail |
| X5 | Courts & Trials | V1.0 | Player or NPC judges, prosecution, defense, evidence presentation |
| X6 | Jury System | V1.0 | Citizen jury draft + verdict |
| X7 | Prison / Punishment | V1.0 | Sentences, prison interiors as playable spaces, parole |
| X8 | Social Stigma | V1.0 | Public record of convictions affecting employment, relationships, housing |

## 8. NPCs

| ID | System | Tag | One-line scope |
|---|---|---|---|
| N1 | NPC Population & Spawning | SPINE | Per-region NPC population maintenance — ~10 NPCs in MVP |
| N2 | Traits & Personality | SPINE | Generated personality system (no LLM) driving behavior |
| N3 | Schedule / Routine | SPINE | Daily life patterns (work, home, leisure) |
| N4 | Dialogue | SPINE | Templated/generated dialogue tied to traits, memory, and context |
| N5 | NPC Worker Role | V1.0 | NPCs as employees of player or NPC businesses |
| N6 | NPC Heir / Spouse Role | SPINE | NPCs as marriageable, adoptable, inheritable family members — spouse + daughter in MVP |
| N7 | Memory & Relationships | SPINE | NPCs remember player actions and form opinions over time |
| N8 | Pets & Animals | V1.0 | Owned pets and livestock with simple needs and roles |

## 9. Communication & Social

| ID | System | Tag | One-line scope |
|---|---|---|---|
| M1 | Proximity Voice Chat | V1.0 | Spatialized in-world voice |
| M2 | Text Chat | V1.0 | Local, party, private channels |
| M3 | In-Game Phone | V1.0 | Landlines and dumb cell phones (calls, SMS, contacts) |
| M4 | In-Game Email | V1.0 | Persistent inbox accessible from computers |
| M5 | In-Game Forums & Simple Web | V1.0 | Early-2000s-style player-run sites and message boards |
| M6 | Friends & Acquaintances | V1.0 | Player-curated relationship lists |
| M7 | Planned Events | V1.0 | Scheduled events (weddings, funerals, concerts, parties) with RSVPs |
| M8 | Shared Activities | V1.0 | Listen-along, watch-along, board games, shared spaces of attention |
| M9 | Postal / Package System | V1.0 | Physical mail and parcels moved between regions |

## 10. Culture (all player-created)

| ID | System | Tag | One-line scope |
|---|---|---|---|
| U1 | Music Creation | V1.0 | Hybrid: audio upload + in-game loop/sample composer |
| U2 | Music Fingerprinting & Moderation | V1.0 | Automated copyright detection on uploads |
| U3 | Music Distribution | V1.0 | Records, CDs, radio plays, jukeboxes, concerts, royalties |
| U4 | Writing & Publishing | V1.0 | In-game text editor for books, newspapers, zines, pamphlets |
| U5 | Visual Art | V1.0 | In-game pixel painter + image upload with moderation; galleries |
| U6 | Radio Broadcast | V1.0 | Live and scheduled player-run radio stations heard across regions |
| U7 | Film & TV | V2.0+ | Player-recorded in-game footage published as movies/shows |
| U8 | Charts & Cultural Rankings | V1.0 | Aggregated public popularity for songs, books, shows |
| U9 | Seeded Canon Library | V1.0 | Commissioned launch-day fictional canon (music, books, films) |

## 11. Combat & Conflict

| ID | System | Tag | One-line scope |
|---|---|---|---|
| K1 | Combat System | V1.0 | Stat-based, low-twitch, brief and consequential |
| K2 | Weapons & Rarity | V1.0 | Fists/improvised → knives/bats → handguns → rifles/SMGs, increasingly rare |
| K3 | Injuries & Recovery | V1.0 | Wounds, bleeding, hospital care, scars, lasting effects |
| K4 | Self-Defense & Bystanders | V1.0 | Defensive actions, NPC witnesses, mob response |

## 12. Moderation & Safety

| ID | System | Tag | One-line scope |
|---|---|---|---|
| T1 | Reporting System | V1.0 | In-game player-to-staff reporting flow |
| T2 | Live Trust & Safety Tools | V1.0 | Moderator dashboards, evidence review, action audit log |
| T3 | Word Filter & Content Detection | V1.0 | Text and image scanning against hard-floor policies |
| T4 | Appeal & Ban System | V1.0 | Banning workflow with appeal channel |
| T5 | Region/Property-Owner Controls | V1.0 | Private-property allow/block lists, age-gating, custom rules within global floor |

## 13. UI & UX

| ID | System | Tag | One-line scope |
|---|---|---|---|
| I1 | HUD | SPINE | Intentionally minimal heads-up display |
| I2 | Inventory & Character Sheet | SPINE | Item management and biographical/stat view |
| I3 | Map UI | V1.0 | Multi-scale world/region/city maps |
| I4 | Phone UI | V1.0 | In-world device with calls, SMS, contacts |
| I5 | Computer UI | V1.0 | In-world device with email, web, work apps, music player |
| I6 | Notification System | SPINE | Diegetic and non-diegetic notifications, tuned for cozy tone — minimal MVP |
| I7 | Onboarding & Tutorial Flow | SPINE | First-life tutorial that respects veteran players |

## 14. Engine & Pipeline

| ID | System | Tag | One-line scope |
|---|---|---|---|
| E1 | 2.5D Pixel Rendering | SPINE | Unity 6 URP 2D Renderer configured for pixel-perfect output at low internal resolution |
| E2 | Tilemap System | SPINE | 32×32 tile authoring/runtime with z-sorting |
| E3 | Asset Pipeline | SPINE | Sprite/tile/audio import, validation, atlas generation |
| E4 | Animation Framework | SPINE | 4-directional sprite animation, ordered z-sort, layered sprites for clothing/equipment |
| E5 | Spatial Audio | SPINE | Positional audio, occlusion (light), music ducking |
| E6 | LOD / Region Streaming | V1.0 | Stream regions in/out around active players |
| E7 | Build & Deploy Pipeline | V1.0 | CI, build artifacts, server deployment |

---

## MVP spine summary

The systems needed to ship the [05-vertical-slice-cafe-day.md](05-vertical-slice-cafe-day.md) milestone:

**Foundation:** F5
**Character:** C1, C2 (minimal)
**World:** W2, W3, W4, W5 (day only)
**Property:** P1, P2, P4, P5, P6 in minimal shared-world form
**Jobs:** J1, J2 with café as first job testbed, not the game identity
**NPCs:** N1 (~10 NPCs), N2, N3, N4, N6 (spouse + daughter), N7 (minimal)
**UI:** I1, I2, I6 (minimal), I7
**Engine:** E1, E2, E3, E4, E5

**~25 systems in minimum-viable form.** Everything else is V1.0+ or beyond.
