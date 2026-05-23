# legacy - Complete Game Design Document

**Working title:** legacy
**Document version:** v1.0
**Date compiled:** 2026-05-22
**Engine:** Unity 6.0.1 (URP 2D)
**Aesthetic:** 2.5D pixel art at 32x32, Stardew style
**Current milestone:** First playable shared-world prototype

> This is the combined single-file export of the GDD that lives across many markdown files in /docs.
> For the canonical split-file version, see /docs/00-INDEX.md.

---

# legacy — Game Design Document

> **Working title:** *legacy*
> **Pitch:** A persistent online sandbox where you live an ordinary life in a small country that remembers everything you did.
> **Engine:** Unity 6.0.1 (URP 2D)
> **Visual style:** 2.5D pixel art at 32×32, Stardew-style 3/4 top-down
> **Status:** Pre-production. Current milestone is a first playable shared-world prototype.
> **Document version:** v1.0 (2026-05-22)

---

## How to read this GDD

This is a multi-file document. Start with the executive summary, then read the pillars and the design principles. Everything else is reference you return to as you build.

### Recommended reading order for a new team member
1. [01a-executive-summary.md](01a-executive-summary.md) — one-page TLDR
2. [01-vision-and-pillars.md](01-vision-and-pillars.md) — the locked vision
3. [04-design-principles.md](04-design-principles.md) — the rules we don't break
4. [03-player-freedom.md](03-player-freedom.md) — what the player can be, do, create
5. [12-roadmap.md](12-roadmap.md) — the first playable shared-world milestone
6. [04b-world-bible.md](04b-world-bible.md) — the fictional country, geography, history
7. [04a-tone-and-style-guide.md](04a-tone-and-style-guide.md) — voice, mood, dialogue style
8. [02-system-list.md](02-system-list.md) — every system the full game contains
9. [05-vertical-slice-cafe-day.md](05-vertical-slice-cafe-day.md) — old café testbed reference, not the current project milestone
10. [13-risk-register.md](13-risk-register.md) — what kills the project, and how we don't let it

### Role-specific reading paths

**Programmer**
01a → 01 → 04 → 05 → 06 → 07 → 08 → 09 → 10 → 11-systems/* → 12

**Pixel artist**
01a → 01 → 04a → 04b → 10 → 10a → 05

**Audio / composer**
01a → 01 → 04a → 10 → 10b → 11-systems/audio-architecture.md

**Writer / world-builder**
01a → 01 → 04a → 04b → 11-systems/npcs.md → 11-systems/dialogue.md → 05

**Playtester / community**
01a → 01 → 03 → 05 → 13

---

## Full document index

### Section 0 — Index
- [00-INDEX.md](00-INDEX.md) — this file

### Section 1 — Vision
- [01a-executive-summary.md](01a-executive-summary.md)
- [01-vision-and-pillars.md](01-vision-and-pillars.md)

### Section 2 — Systems catalog
- [02-system-list.md](02-system-list.md)

### Section 3 — Player perspective
- [03-player-freedom.md](03-player-freedom.md)

### Section 4 — Design principles & fiction
- [04-design-principles.md](04-design-principles.md)
- [04a-tone-and-style-guide.md](04a-tone-and-style-guide.md)
- [04b-world-bible.md](04b-world-bible.md)

### Section 5 — First milestone
- [12-roadmap.md](12-roadmap.md) — current first playable shared-world milestone
- [05-vertical-slice-cafe-day.md](05-vertical-slice-cafe-day.md) — café testbed reference retained for systems examples

### Section 6 — Technology
- [06-tech-stack.md](06-tech-stack.md)

### Section 7 — Project conventions
- [07-project-structure.md](07-project-structure.md)
- [08-code-conventions.md](08-code-conventions.md)
- [09-save-system.md](09-save-system.md)

### Section 10 — Asset pipelines
- [10-asset-pipeline.md](10-asset-pipeline.md)
- [10a-art-direction.md](10a-art-direction.md)
- [10b-audio-direction.md](10b-audio-direction.md)

### Section 11 — Subsystems (MVP)
- [11-systems/time-and-day.md](11-systems/time-and-day.md)
- [11-systems/npcs.md](11-systems/npcs.md)
- [11-systems/dialogue.md](11-systems/dialogue.md)
- [11-systems/interaction.md](11-systems/interaction.md)
- [11-systems/inventory.md](11-systems/inventory.md)
- [11-systems/currency.md](11-systems/currency.md)
- [11-systems/cafe-work-loop.md](11-systems/cafe-work-loop.md)
- [11-systems/building-kit.md](11-systems/building-kit.md)
- [11-systems/ui-architecture.md](11-systems/ui-architecture.md)
- [11-systems/audio-architecture.md](11-systems/audio-architecture.md)

### Section 12 — Roadmap
- [12-roadmap.md](12-roadmap.md)

### Section 13 — Risks
- [13-risk-register.md](13-risk-register.md)

### Section 14 — Tooling
- [14-tools-to-install.md](14-tools-to-install.md)

### Section 15 — Glossary
- [15-glossary.md](15-glossary.md)

---

## Document conventions

- **[LOCKED]** — decision is final unless the entire project pivots
- **[OPEN]** — decision pending; alternatives listed
- **[PLACEHOLDER]** — name or value is provisional, will be revised
- **[MVP]** — required for the first playable shared-world prototype
- **[V1.0]** — required for first public multiplayer release
- **[V2.0+]** — long-horizon vision; not on near-term plan
- **[SPINE]** — load-bearing, blocks other work if absent

When in doubt about anything in these docs, the order of authority is:
1. [04-design-principles.md](04-design-principles.md) — hard rules
2. [01-vision-and-pillars.md](01-vision-and-pillars.md) — locked vision
3. [12-roadmap.md](12-roadmap.md) — current milestone spec
4. The system doc in [11-systems/](11-systems/)
5. Everything else


---

# Executive Summary

## One-line pitch
*A persistent online sandbox where you live an ordinary life in a small country that remembers everything you did.*

## Genre
Persistent civilization sandbox / cozy life sim with a civic meta-layer.

## Reference cocktail
*Stardew Valley* (foreground feel) × *The Sims* (life simulation depth) × *Crusader Kings* (family legacy) × *EVE Online* (player politics & economy) × *Project Zomboid* (permadeath consequence) × *Disco Elysium* (atmosphere).

## Core fantasy
> "Live a meaningful life in a world that remembers."

## What the player does
Players are citizens, not heroes. They run businesses, work jobs, raise families, vote in elections, run for office, commit and investigate crimes, write music and books that other players listen to and read, attend each other's weddings and funerals, and slowly accumulate a legacy in a world whose history is permanent. The dominant moment-to-moment loops are **working** and **socializing**; combat and crime are rare punctuation, not the rhythm.

## What makes it different
1. **All culture is player-created.** Every song, book, painting, newspaper, radio show, and film in the world is made by players. No real licensed music. No scripted quests. No developer-authored narrative.
2. **The world persists and remembers.** Append-only history log records named events forever. Cities rise and fall. Dynasties form and collapse. Reputations outlive characters.
3. **Mortality is real but rare.** Characters die from disease, violence, accident, or rare old-age fade. Death is permanent for that character; family persists as NPCs; reputation lives in the world's history; the player starts a new life.
4. **Civilization is overthrowable.** Oligarchies will form (a feature, not a bug). Players can overthrow them — first through civic processes (petitions, referenda), then through armed uprising if the civic path is suppressed.
5. **Ordinary lives are first-class.** A café owner's life is as mechanically rewarding and as socially recognized as a mayor's.

## Setting
- **Era:** Static blended "circa 2003" early-2000s
- **World:** Fictional alt-Earth country, fictional cities, fictional pop culture
- **Tech:** Cars, dumb cell phones, internet exists as a place you go (not an overlay), no smartphones, no social media
- **Aesthetic:** 2.5D pixel art at 32×32 in Stardew Valley style
- **Combat:** Stat-based, weapons rare, mostly black market, every fight is a story

## Target audience
- Primary: Adults 20–40 who play *Stardew Valley*, *The Sims*, *Crusader Kings*, *Project Zomboid*, *EVE Online*, *Wurm Online*, *Haven & Hearth*, *Disco Elysium*. Players who want depth and meaning over twitch and dopamine.
- Secondary: Roleplayers and storytellers from MMO and tabletop communities.
- Audience size assumption: niche but loyal. Think 50k–500k lifetime players over years, not millions.

## Scope reality (this is in the GDD because it matters)
- The **full vision** is a 5–10 year project, in the same league as *Wurm Online*, *Project Zomboid*, *Dwarf Fortress*.
- The **current milestone** is a **first playable shared-world prototype**: one small block of Veyne where citizens, property ownership, jobs, money, time, NPC schedules, and history all exist in one persistent world state.
- The café is only the first test building inside that world. It is not the identity of the game.
- Multiplayer-readiness starts now at the architecture level: shared `WorldState`, command-based player actions, and server-authoritative assumptions, even before real online networking ships.
- Everything is downstream of the shared-world foundation working.

## Current status
- Unity 6.0.1 project initialized.
- Design documentation and early prototype code exist.
- The prototype is being refactored away from café-specific state and toward shared `WorldState`, property ownership, commands, persistence, and multiplayer-ready boundaries.
- Zero budget. Volunteer/love-and-credit basis until the first playable shared-world prototype proves the project has legs.
- Working toward the first playable shared-world milestone per [12-roadmap.md](12-roadmap.md).

## Definition of success
- **First playable success:** A player can enter Veyne, inspect shared property ownership, see citizens move on schedules, work a simple job, earn money, and see those actions recorded in persistent world history.
- **Year-1 success:** A local or limited online prototype proves the shared-world foundation: multiple players can see the same plots, owners, NPCs, money changes, and history events.
- **Long-horizon success:** A persistent online world with at least one regional shard running 24/7, with a player community that produces in-game culture (music, journalism, art) and writes its own history.

## What this GDD is not
- Not a marketing document.
- Not a pitch deck for investors. (Pitch decks live elsewhere when needed.)
- Not a contract. Anything can change with team consensus and design-principle alignment.
- Not exhaustive. Detailed system specs evolve in [11-systems/](11-systems/) as we build.


---

# Vision and Pillars

This document is the locked design vision for *legacy*. Anything in this file is **[LOCKED]** unless an explicit pivot decision is made and recorded in this file.

---

## Title
*legacy* (working title)

## Genre
Persistent civilization sandbox / cozy life sim with civic meta-layer.

## Tone
Peaceful but alive. Melancholic. Hopeful. Ordinary. Consequential.

---

## Core fantasy

> **Live a meaningful life in a world that remembers.**

Players are citizens of an evolving civilization, not heroes of a hand-authored story.

---

## Reference cocktail

| Game | What we take from it |
|---|---|
| *Stardew Valley* | Visual style, cozy moment-to-moment feel, simple skill loops |
| *The Sims* | Life simulation depth, NPC relationships, daily-routine rhythm |
| *Crusader Kings* | Family lines, dynasties, legacy across generations |
| *EVE Online* | Player politics, player economy, oligarchy mechanics |
| *Project Zomboid* | Permadeath consequence, persistent skill loss, mortality weight |
| *Disco Elysium* | Atmospheric melancholy, ordinary protagonists, dignity of the small life |

---

## Setting

- **Era:** Static blended "circa 2003" early-2000s [LOCKED]
- **World:** Pure fictional Earth-like world; fictional country (placeholder name **Marenne**); fictional cities; fictional geography [LOCKED — names PLACEHOLDER]
- **Internet:** Exists as a *functional layer* — email, instant messaging, simple web, forums. Internet is a place you go (sit at the computer), not an overlay carried in your pocket. [LOCKED]
- **Pop culture:** **No real-world pop culture.** All in-world music, books, films, news, art is player-created. Seeded fictional canon (commissioned originals) ships at launch as the world's "classical era." [LOCKED]
- **Combat era:** Modern stat-based combat. Guns rare and mostly black-market. Most violence is fists, knives, bats. Every fight is dramatic and infrequent. [LOCKED]

See [04b-world-bible.md](04b-world-bible.md) for full setting detail.

---

## World architecture (long-term vision)

- **One persistent online world** (eventual goal; single-player first)
- **Regionally simulated:** only nearby regions are actively rendered and simulated for each player
- **Region transitions:** handled via localized server nodes; world feels connected but is sharded behind the scenes
- **Frontier expansion:** new regions periodically open for new-player settlement and growth — the primary onboarding answer
- **World clock + persistent history log:** the "world that remembers" backbone

---

## Player life

- Each character is **mortal**.
- **Event-based death:** disease, violence, accident, rare old-age fade. Death is *rare* by design — tuned so the average active player loses a character maybe 1–3 times per year of play. [LOCKED — death rate]
- **On death:** character is permanently gone. Possessions remain in the world (inherited, claimed, decayed, or lost). Reputation lives forever in the world's history log.
- **Family lines:** persistent lineage tree spanning multiple characters across players and NPCs.
- **Marriage and adoption:** real systems. Spouses and heirs can be players or NPCs.
- **After death:** the player **restarts as a new character** in the world. Their previous family persists as NPCs. The new character has no inherited assets. The world remembers the previous character by name in its history log. The new character may, in time, learn of (or even meet) the NPCs that were their previous family. [LOCKED — restart model]

---

## Society

- **Property ownership:** land, homes, businesses, vehicles
- **Player economy:** jobs, wages, prices, supply/demand, taxes — all real, all player-driven
- **Player politics:** elected offices, written laws, referenda — players hold real civic power
- **Oligarchies will form.** This is *a feature.* Player societies, like real ones, accumulate inequality. [LOCKED]
- **Revolution is the release valve.** Players can overthrow concentrated power. [LOCKED]
- **Revolution is tiered:** civic upheaval (petition → referendum) is the default legal path; **armed uprising** unlocks when the civic path is suppressed. [LOCKED]
- **Justice is hybrid:** elected sheriff with NPC deputies, player judges with NPC bailiffs, player juries with NPC fallback. [LOCKED]

---

## Crime

- **All crime types** are possible — theft, fraud, assault/murder, political/corruption, cybercrime. [LOCKED]
- **Crime is infrequent and consequential.** Not the loop. The whole tone of the game collapses if crime becomes ambient.
- **Combat is stat-based, weapons rare.** Every fight is a story.

---

## Culture

- **100% player-created.** No real licensed music, film, text, or pop culture in the world. [LOCKED]
- **Music creation:** hybrid — audio upload (with fingerprinting moderation) + in-game loop/sample tool
- **Other cultural mediums** follow the same pattern: writing, visual art, journalism, radio, film, painting are all first-class creative careers
- **Seeded canon at launch:** the world ships with commissioned fictional songs, books, films, etc. as its "classical era." Players add to it.
- **Cultural careers are full first-class careers** with industries around them (record labels, publishers, radio stations, art galleries, film studios — player-owned).

---

## Moment-to-moment loop

- **Dominant:** **WORKING** (jobs as real gameplay mini-loops, not click-for-money)
- **Co-dominant:** **SOCIALIZING** (presence, conversation, shared activities)
- **Secondary** (player choice): building, producing, politicking, traveling, exploring
- **Combat and crime are punctuation, not the rhythm.**

This locks the design challenge: every job in the game must be a satisfying gameplay mini-loop. The breadth and depth of jobs is the single biggest content production challenge of the project.

---

## NPCs

- **Background NPCs** populate the world and fill jobs no player wants. Streets feel alive even when player count is low. [LOCKED]
- **NPC heirs/spouses** are available when no player takes the role. Marriage and adoption can occur with players *or* NPCs. [LOCKED]
- **Generated personalities** (not LLM-driven): each NPC has a trait set (warmth, talkativeness, generosity, mood, etc.), persistent traits, and persistent relationships. [LOCKED]
- **Quality bar:** an NPC-run café must feel alive enough that a player customer doesn't feel like they're in a ghost town. This is a non-negotiable target.

---

## Moderation

- **Tight floors.** Hard no-tolerance lines. Strict enforcement. [LOCKED]
- **All in-game "crime" is fiction.** Player-on-player abuse is not.
- **Hard bans on:** real-world hate, harassment, sexual content directed at players, doxxing, IRL incitement, CSAM (mandatory reporting).
- **In-character crime** (theft, fraud, fictional violence) is part of the game and not a moderation issue.
- See [13-risk-register.md](13-risk-register.md) for moderation as a project risk.

---

## Aesthetic

- **Full Stardew Valley 2.5D.** [LOCKED]
- 3/4 top-down oblique perspective, no camera rotation
- Pixel art at **480×270 internal resolution**, nearest-neighbor scaled to window
- **32×32 tile-based world** [LOCKED]
- 4-directional sprite characters
- Z-sorted sprite layering for depth
- **Modular kit-of-parts building system** (mandatory for content scale — see [11-systems/building-kit.md](11-systems/building-kit.md))
- Visual identity: "Stardew, but circa-2003 urban civilization"

See [10a-art-direction.md](10a-art-direction.md) for full art direction.

---

## What this game is NOT

Explicit non-goals. These exist to keep the team aligned and reject scope creep.

- **Not a survival game.** No hunger-death loop, no thirst meters as the primary tension, no exposure to elements as core mechanic.
- **Not a combat-focused MMO.** Combat is rare, consequential, and not the progression loop.
- **Not a life sim with scripted NPC routines.** All depth is emergent from systems, not authored.
- **Not a dopamine machine.** No power fantasy. No chosen-one mechanics. No "you are special."
- **Not a sandbox without consequence.** The world remembers; crime has weight; reputation persists; mortality is real.
- **Not free-for-all anarchy.** Tight moderation. Cozy tone protected at the system level.
- **Not Skyrim/quest-driven.** No quest log, no waypoints, no "go kill 10 rats." Stories emerge.
- **Not a roguelike.** Permadeath without procedural levels; the world is the persistence.
- **Not a clone of any single reference.** The cocktail is the differentiator.

---

## Decision log

Decisions made during vision lock-in. Recorded here so future contributors can understand *why*.

| Decision | Chosen | Alternatives considered | Why |
|---|---|---|---|
| Setting era | Static blended 2003 | Mid-20th century / 21st century / progressing era | Balanced modernity + atmosphere; no smartphones/social-media; pre-algorithm politics |
| Geography | Pure fictional Earth-like | Real city / alt-Earth fictional country | All culture is player-created — real-Earth references would crowd out player-created culture |
| Death model | Event-based, rare | Activity-based aging / compressed time / real time | Best fit for retention + meaningful mortality + cozy tone |
| Restart model | Restart as new character with reputation memory | Heir takeover / character-swap menu | Cleanest for retention; family persists as NPCs; world remembers |
| Pop culture | 100% player-created | Real licensed / hybrid | Players cannot be the world's biggest musician if Eminem is on the radio |
| Combat | Stat-based, rare, mostly fists/melee | Skill-based shooter | Permadeath + cozy tone requires combat that is dramatic and infrequent |
| Aesthetic | Full Stardew 2.5D pixel at 32×32 | Pixel at 16×16 / iso sprites / orthographic 3D | Most achievable visual + best fits modern urban content per tile |
| Justice | Hybrid (player + NPC) | Pure player / pure NPC | Scales with population; works at low and high CCU |
| Revolution | Tiered (civic → armed) | Pure civic / pure armed | Maps real revolution dynamics; gives nonviolent players a path |
| Moderation | Tight | Permissive / VRChat-federated | All-crime sandbox + voice chat + uploads requires hard floors to ship at all |
| Dominant loop | Working + Socializing | Survival / combat / exploration / building | Matches cozy tone; differentiates from every adjacent genre |


---

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


---

# Player Freedom

This is the companion to [02-system-list.md](02-system-list.md), written from inside the player's chair. Same content, viewed as freedom rather than as engineering.

Every system on the system list exists to *enable* one or more freedoms below. If a system in the list doesn't appear here as something a player can be, do, or create, the system is probably restrictive (a rulebook) rather than expansive (a capability) and should be redesigned per [04-design-principles.md](04-design-principles.md).

---

## What you can be

A non-exhaustive list of life paths a player might pursue. These are not classes you pick at character creation — they are emergent identities you grow into through what you do.

### Working lives
- Café owner, bartender, chef, baker, butcher, grocer
- Mechanic, taxi driver, bus driver, postman, courier
- Doctor, nurse, paramedic, dentist
- Teacher, school principal, university lecturer, librarian
- Farmer, fisherman, lumberjack, miner
- Factory worker, construction worker, electrician, plumber, welder
- Banker, accountant, real estate agent, insurance broker, lawyer
- Photographer, tailor, hairdresser, mortician
- Clerk, secretary, receptionist

### Civic & state lives
- Mayor, town councilor, regional senator
- Sheriff, deputy, beat cop, detective
- Judge, prosecutor, defense attorney, public defender
- Election official, civil servant, tax assessor
- Notary, registrar, archivist

### Cultural & creative lives
- Musician, songwriter, band member, record label owner
- Novelist, poet, essayist, zine publisher
- Painter, sculptor, photographer, gallery owner
- Journalist, editor, columnist, newspaper publisher
- Radio DJ, radio station owner
- Filmmaker, theater playwright, theater director

### Owner lives
- Landlord, real estate developer, business franchisor
- Industrialist, factory owner, mine owner

### Underworld lives
- Smuggler, fence, bookie, loan shark
- Gang leader, soldier, lookout, getaway driver
- Hitman, enforcer, private investigator
- Drug dealer, counterfeiter, hacker, identity forger
- Corrupt official (any of the above civic roles played dirty)

### Family & community lives
- Spouse, parent, grandparent, guardian
- Caregiver to elderly NPCs, foster parent
- Religious leader, community organizer, charity founder

### Other lives
- Frontier settler, exile, retiree, drifter, immigrant
- Just an ordinary citizen who shows up and is seen

*Or any combination across one life. Or different combinations across the lives of one player.*

---

## What you can do

A non-exhaustive list of verbs that exist in the world. Every verb is enabled by a system in [02-system-list.md](02-system-list.md).

### Property and money
- Buy land, sell land, lease land, abandon land
- Build a house, expand a house, renovate, demolish
- Open a shop, close a shop, sell a shop
- Hire a stranger, fire an employee, promote an employee
- Open a bank account, take a loan, default on a loan
- Insure a building, file an insurance claim
- Pay taxes, evade taxes, get audited

### Work
- Sign an employment contract
- Negotiate wages
- Take a job, quit a job, get fired
- Open a side business, expand into franchises
- Apprentice under another player
- Teach a skill to another player

### Social
- Meet someone
- Befriend someone
- Fall in love
- Marry
- Adopt a child
- Raise children
- Divorce
- Make an enemy
- Forgive an enemy
- Mourn someone
- Attend a funeral
- Host a party
- Throw a wedding
- Be a witness at someone else's wedding
- Show up at someone's hospital bed

### Civic
- Register to vote
- Vote
- Run for office
- Lose an election
- Win an election
- Author a law
- Sponsor a referendum
- Sign a petition
- Refuse to sign a petition
- Speak at a town meeting
- Disobey a law
- Get arrested
- Hire a lawyer
- Defend yourself in court
- Sue someone in civil court
- Serve on a jury
- Be the foreperson on a jury
- Bribe an official
- Expose a bribery
- Lead a revolution
- Surrender a revolution
- Be exiled
- Be pardoned

### Crime and justice
- Steal something small
- Plan a robbery
- Pull off a heist
- Get away with it
- Get caught
- Confess
- Hire an alibi
- Investigate a crime as a cop
- Investigate as a journalist
- Take revenge — or refuse to

### Creating culture
- Write a song
- Record an album
- Sell records
- Hear strangers humming your song years later
- Write a novel
- Publish a book
- Have your book banned
- Paint a picture
- Sell a painting
- Have a painting hung in a public gallery
- Photograph a famous event
- Start a newspaper
- Write a column
- Print a lie
- Get sued for it
- Found a radio station
- Host a show
- Take over the airwaves during a crisis
- Film a movie
- Win a player-voted award
- Score someone else's film with your music

### Travel
- Walk across town
- Take the bus
- Buy a car
- Drive across regions
- Move house
- Move to the frontier
- Found a new town
- Visit your hometown decades later

### Family & legacy
- Have children
- Watch them grow into NPCs
- Watch them be played by other players
- Pass the family business to a child
- Disinherit a child
- Write a will
- Die
- Be remembered
- Return decades later, as someone else, and walk past your old café

---

## What you can create

Artifacts you can produce that persist in the world after you're gone.

| Thing you create | How it persists |
|---|---|
| A song | As a record/CD/radio play; heard by others; recorded on charts; sampled by other players |
| A novel | As a printed book; in libraries; on bedside tables; banned by governments; quoted by characters |
| A painting | In galleries, homes, public buildings; sold and resold |
| A film / TV show | In cinemas, on the in-world TV networks |
| A newspaper | Distributed to doormats; archived in the public registry |
| A radio show | Heard live and rebroadcast |
| A business | Continues operating; can be inherited, sold, franchised |
| A neighborhood | Buildings remain; reputations of places persist |
| A political party | Outlasts founder; members carry on the platform |
| A union | Continues to bargain; affects wages and working conditions |
| A religion | Adherents continue practices; buildings remain |
| A cultural movement | Influences how players talk, dress, vote |
| A scandal | Recorded in the history log; quoted decades later |
| A family | Survives as NPC dynasty; new players can be born into it |
| A dynasty | Property, name, reputation accumulate across generations |
| A reputation | Logged events under your name persist forever |
| A legend | Famous players become history figures cited by NPCs |
| A monument | Physical building/statue erected in your name |
| A grave | Marker in a cemetery; visited; tended; ignored |

---

## What can emerge

Stories that no developer wrote, that came from the systems composing.

- A famous musician's record label secretly funds a revolutionary cell. When the revolution succeeds, the new mayor is the label's lawyer. Years later, the music industry of the country is dominated by that one label. Decades later, a song from that era is still sung at protests.

- A mayor marries a journalist's daughter. The marriage is celebrated as a unifying union. Three years later, the daughter writes an exposé about her father-in-law. She is jailed for sedition. The marriage ends. The mayor wins reelection in a landslide. The daughter dies in prison. Her writing becomes contraband.

- A dynasty of three generations of bakers. The grandfather opened the bakery in year 1. The father expanded it to a regional chain. The grandson, born into the family business, hates baking and runs for office. The bakery is sold to a stranger. The stranger lets it fail. Forty real years later, the building is a coffee shop. Old NPCs still call it "the bakery."

- A neighborhood gang of teenagers grows into a political party. Their original turf becomes their first electoral district. Their slogans started as graffiti.

- A famous lawyer's son becomes the criminal his father defends. The father wins. The son walks free. The father retires the next day.

- An exiled revolutionary returns to her home country decades later as a tourist. She is not recognized. She walks past her old apartment. The current tenant is her great-niece, born after the exile, played by a stranger. She does not speak.

- An NPC daughter of a former player marries a current player. The current player learns the family name. Looks it up in the history log. Realizes their spouse's mother was once a famous mayor played by someone they don't know. The world is older than them.

**None of these are in the system list. All of them are what the system list enables.**


---

# Design Principles

Hard rules every design decision must obey. These are not preferences. They are the constitution of the project. If a proposed feature violates one of these, the feature is wrong, not the rule.

---

## The ten rules

### 1. Expansive systems only
Every system describes **what becomes possible**, never **what is permitted**. If a system's spec reads like a rulebook, redesign it.

> **Wrong:** "Players choose from 30 pre-defined careers, each with a fixed wage table."
>
> **Right:** "Any player can offer to hire any other player or NPC to do anything for any wage. The game tracks contracts and reputations."

The two specs describe the same system. The first narrows freedom. The second widens it.

### 2. No authored narrative content
No scripted quests. No fixed career ladders. No developer-written stories. No NPC quest-givers with marked exclamation points. No "save the village" plotlines. The world ships with **seeded places, NPCs, and history**. It never ships with seeded **plots**. All narrative is player-emergent.

The seeded canon (commissioned music, books, films at launch) is the only exception, and it is artifact, not story.

### 3. The world must remember
Every meaningful action writes to the persistent history log. Default to recording; redact only on privacy/moderation grounds. If you can't tell whether something is "meaningful enough" to record, record it.

### 4. Mortality is rare and meaningful
Death frequency is tuned so the average active player loses a character maybe 1–3 times per year of play. Never less (mortality stops mattering), never more (retention dies). When death happens it should be a *story*, not a *grind*.

### 5. Cozy moment, civic meta
Minute-to-minute play is peaceful and ordinary. The dramatic layer (revolution, crime, scandal) sits *on top of*, never *in place of*, the ordinary. A player's typical 45-minute session is working and socializing; the big events are punctuation, weeks or months apart.

### 6. Geography matters
Travel takes time. Distance is a real cost. Locality drives culture. A neighborhood feels different from another neighborhood. A region feels different from another region. Players should know what part of town they're in by the look, the sound, and the people.

### 7. Ordinary lives are first-class
A café owner's life is as mechanically rewarding and as socially recognized as a mayor's. The systems that make a quiet life feel meaningful are not "side content." They are the main content.

### 8. Players are not heroes
No chosen-one mechanics. No destiny. No "special" players. No NPC dialogue treating the player as central. Status is earned in-world, by what the player does, and is local — not global — by default.

### 9. Server is authoritative
(For when multiplayer arrives.) The client is a renderer and an input device. The server holds the world. Never trust the client. Design every system as if the player will try to cheat (because some will).

### 10. Moderation is not an afterthought
Tight floors, designed before launch, staffed before launch. Player safety mechanisms (block, mute, report, region-owner controls) exist on day one of multiplayer. The decision to be a tightly moderated game is a feature-quality decision, not a legal one.

---

## Operating principles (softer; preferences, not rules)

These are heuristics for the team. Break them with care and reason.

- **Ship small, ship often.** Vertical slices over feature dumps. A working café beats a half-built town.
- **Build the boring foundation first.** Save format, time clock, scene management, audio mixer. These are unsexy and they save you months later.
- **Prefer composition over inheritance.** Component-based MonoBehaviours and pure C# classes; avoid deep hierarchies.
- **Data over code.** ScriptableObjects for all static game data so designers and writers can change content without recompiling.
- **Placeholder art is fine until it isn't.** Use the same placeholder style consistently; the brain forgives placeholder art when it's *coherent*.
- **One screen of code is better than three.** When in doubt, simpler.
- **Player time is sacred.** Don't grind their attention. Cozy doesn't mean slow-for-the-sake-of-slow.
- **Silence is content.** Quiet moments are part of the design, not bugs to fill.
- **The history log is the soul.** Anytime you implement a new system, ask: *what does this write to history?* If the answer is nothing, the system is probably anti-soul.

---

## Anti-patterns (don't do these)

- **Quest log.** No.
- **Waypoints / quest markers.** No.
- **Compass with named POIs.** No.
- **"You leveled up" notifications.** No. Skill gain is silent and ambient.
- **Loot rarity colors.** No.
- **"Press F to pay respects" dialog buttons.** No. Interaction is verbed and diegetic.
- **Big numbers (DPS, damage, XP).** No. Numbers exist where they matter (money, time, distance) and not where they don't.
- **"You are the chosen one" dialogue.** No.
- **Skinner-box reward schedules.** No.
- **Microtransactions.** No, ever. Cosmetics may eventually be earnable in-world; nothing is buyable for real money in-game.

---

## Tone discipline

For every design choice, ask:

1. Does this serve the **cozy moment-to-moment**?
2. Does this serve the **civic meta**?
3. Does this serve **legacy and memory**?
4. Does this **respect ordinary lives**?
5. Does this **avoid heroification**?

If a proposed system doesn't serve at least one of these, push back. If it actively works against one, refuse it.

---

## Decision-making

When designers disagree:

1. Re-read the **pillars** ([01-vision-and-pillars.md](01-vision-and-pillars.md)).
2. Re-read the **principles** (this doc).
3. If still unresolved, check the **decision log** in [01-vision-and-pillars.md](01-vision-and-pillars.md) — has this been decided before?
4. If not, prototype it small. Don't argue in the abstract.
5. Playtest with at least two people outside the design conversation.

When in doubt, prefer the answer that respects ordinary lives and the answer that lets the world remember.


---

# Tone and Style Guide

This document defines the *voice* of *legacy* — how it speaks, sounds, looks, and feels in writing, in dialogue, in UI text, in audio, in art. Everyone making content for the game should read this and internalize it.

If the tone of an asset doesn't match this guide, the asset is wrong for the game even if it is technically good.

---

## The North Star

> Quiet. Adult. Human. Specific. Hopeful but melancholic. Like a Sunday afternoon you'll only realize was beautiful a decade later.

If you remember nothing else, remember that sentence.

---

## Tonal touchstones

When in doubt, ask: *would this feel at home in...?*

### Yes
- Edward Hopper paintings (*Nighthawks*, *Morning Sun*, *Automat*)
- Wong Kar-wai films (*In the Mood for Love*, *Chungking Express*)
- *Disco Elysium* dialogue (when it's not being political)
- The opening hour of *Life is Strange* before anything supernatural happens
- *Stardew Valley* villager dialogue at its quietest
- *The Wire* season 2 (docks) — workplace dignity
- *Stranger Things* in its small-town texture (less the monsters, more the kitchens)
- *Twin Peaks* the diner scenes
- Indie folk and quiet jazz
- A long bus ride at dusk

### No
- *Skyrim* / *Witcher* high-fantasy register
- *GTA* satire and edgelord humor
- *Borderlands* quippy quirk
- Marvel banter
- Quirky-funny indie YA voice
- Twitch streamer energy
- "Wholesome cottagecore" treacle (we are warmer than cynical but never sweet)
- Edgy nihilism

---

## Writing voice

### Dialogue
- **Real speech, not theater speech.** People interrupt themselves. They trail off. They repeat things. They say "yeah" and "anyway" and "I dunno."
- **Period appropriate.** Mid-decade early-2000s. No 2026 internet slang. No "vibes" as a noun. No "literally" as intensifier (mostly).
- **Specific over abstract.** Not "I'm having a bad day," but "I forgot to buy bread again."
- **Short over long.** Most NPC lines are under 15 words. Three-sentence lines are rare and earned.
- **Subtext does the work.** What people don't say is louder than what they do.

### Character voice
- **Class and region matter.** A factory worker speaks differently from a magazine editor. A grandmother speaks differently from a teenager.
- **No accent caricature.** Variation comes from word choice, sentence rhythm, references — not phonetic spelling.
- **Vulgar speech allowed but earned.** "Shit" and "damn" exist. "Fuck" exists for moments that warrant it. The game is rated for adults; we are not afraid of language, but we are also not desperate for it.

### Examples

**Good NPC line:**
> "Cold one out there. ...You sleep okay?"

**Bad NPC line:**
> "Greetings, traveler! The weather is most chilly today, isn't it? I trust your slumber was restful!"

**Good signature/memory line:**
> "I used to sit at this table with my wife. Forty-one years. ...You make the tea like she did."

**Bad signature/memory line:**
> "Oh! You remind me of someone I used to know. They were very dear to me. *sighs nostalgically*"

---

## UI text voice

- **Verbs, not adjectives.** "Open shop" not "Begin opening process."
- **Short.** "Saved." beats "Your game has been saved successfully."
- **Diegetic where possible.** The clock on the wall tells time. The till totals the money. The newspaper on the table holds the news.
- **No exclamation marks** except for actual exclamations in dialogue. UI never exclaims at the player.
- **No second-person scolding.** Not "You can't do that!" but "The door is locked."
- **No emojis in UI**, ever. Pixel-art icons only.

### Notification examples

**Good:**
> Saved.
> June left her sandwich.
> Rowan called.
> The till is $42 short.

**Bad:**
> Game Saved Successfully!
> Quest Updated: Drop off sandwich!
> Notification: Rowan has sent you a phone call.

---

## Pacing

- **Cozy pace, not slow pace.** A café shift is busy. A conversation can be quick. We are not artificially slowing the player to manufacture immersion.
- **Loading is rare and used as breath.** Scene transitions are short. The porch scene at sunset is allowed to take a real minute because that minute is the point.
- **No forced waits.** "You must stand here for 30 seconds" is a design failure.

---

## Humor

- **Yes**, but quiet. Wry, dry, observed. The humor of small things — a kid who can't pronounce "espresso," a regular who orders the same thing every day for ten years and pretends to "try something new" every time.
- **No** quippy banter. No fourth-wall winks. No Marvel-style undercutting of emotional moments.
- **No mockery of the player.** The game doesn't laugh at people for being earnest.

---

## Emotional permission

The game is allowed to make players feel:
- Tired, in a good way
- Lonely, in an honest way
- Loved, in a small way
- Sad, in a clean way (grief, not despair)
- Hopeful, without sentimentality
- Nostalgic for things they're currently experiencing

The game should not make players feel:
- Stressed by artificial timers
- Anxious by fail-states
- Mocked
- Manipulated
- Targeted by FOMO
- Pumped on dopamine

---

## Visual tone (see [10a-art-direction.md](10a-art-direction.md) for full art direction)

In one paragraph: warm, muted palette. Earthy and analog. Real colors of real fabrics, real wood, real ceramic. Light is the protagonist — afternoon sun through a café window, the blue of a streetlight at 11pm, the orange of a kitchen lamp at 7am. Avoid neon. Avoid saturated primary colors. Pixel art with intentional restraint, not maximalist density.

---

## Audio tone (see [10b-audio-direction.md](10b-audio-direction.md) for full audio direction)

In one paragraph: music is sparse, instrumental, predominantly acoustic — piano, guitar, brushed drums, occasional strings, occasional analog synth pad. Avoid big drums. Avoid orchestral swells. Avoid "epic." Ambient sound (street murmur, café clatter, fan hum, rain on window) is often more important than the score. Silence is composed.

---

## Worldbuilding tone

- The country of **Marenne** [PLACEHOLDER name] is not a country we should ever fully explain. It is a country we should *feel*. References to its history, its rivalries, its festivals, its old wars come up sideways in NPC dialogue, in newspaper articles, in graffiti.
- **History is texture, not exposition.** Players learn the world by inhabiting it, not by reading a lore book.
- See [04b-world-bible.md](04b-world-bible.md) for the world bible we draw from when we need to write something concrete.

---

## Brand voice (for marketing, devlogs, store pages)

Same tone applies outside the game.

- **First person plural** when speaking as the team. "We are making a game where..."
- **Honest about scope.** "We don't know if we can ship the full vision. We know we can ship this slice."
- **No hype words.** No "epic," "next-gen," "revolutionary," "AAA," "groundbreaking."
- **Quiet confidence.** "This is what we're making." Period.

---

## The single test

If you're not sure whether a piece of writing, art, or audio belongs in *legacy*, ask:

> *Would this be at home in a 2003 small-town Sunday afternoon?*

If yes, probably right.
If no, probably wrong.


---

# World Bible

The setting reference document for *legacy*. This is what we draw from when we need to write a newspaper article, name a street, design a building, or have an NPC mention something from the wider world.

All names in this document are **[PLACEHOLDER]** and may be revised. They are recorded here so the team uses *consistent* placeholders, not so they are locked.

---

## The world in one paragraph

*legacy* is set in **Marenne**, a fictional mid-sized country in an alt-Earth world circa 2003. Marenne is broadly Western-modern: democratic republic, mixed-market economy, urbanized south, rural north, recovering from a turbulent late 20th century but currently stable and quiet. Its culture is a fusion that doesn't directly map to any real-world nation — read it as a parallel-universe country that could plausibly exist somewhere in Europe or the Americas, but isn't quite either. Players will never need to know the full history of Marenne; they will encounter it the way real people encounter their own country's past, in fragments and references.

---

## The country: Marenne [PLACEHOLDER]

- **Government:** Federal republic with three layers — federal, provincial, municipal
- **Population:** Roughly 38 million in fiction (player population is whatever the game has)
- **Capital:** **Hesperin** [PLACEHOLDER] — major city, not where the slice is set
- **Language:** Marennese — for game purposes, this is just *English with occasional invented place names and proper nouns*. We do not invent a conlang. Loanwords from neighboring fictional countries appear in food, music, slang.
- **Currency:** **Marenne dollar** ($) — uses cents. Bills $1, $5, $10, $20, $50, $100. Coins $0.05, $0.10, $0.25, $1.
- **Time/date format:** Day-month-year. 24-hour clock used in official contexts; 12-hour in everyday life.
- **Driving side:** Right. (Decision: matches more pixel-art reference material than left-hand drive.)
- **Major industries:** Manufacturing (north), agriculture (central plains), services and finance (south/coast)
- **National sport:** Football (soccer in US terms); also a regional cricket-like game called **stoop** [PLACEHOLDER]

### Recent history (drip-feed only)
- **1948–1958:** Post-war reconstruction. Big factories built. Population boom.
- **1962–1969:** Cultural ferment, civil-rights expansions, anti-establishment music scene
- **1973–1981:** Economic stagnation, a long unpopular war abroad, political distrust
- **1984–1992:** Reform era. New constitution amendments. Public confidence partially restored.
- **1995–1999:** Tech boom, optimism, urban renewal
- **2000–2003 (game present):** Quiet middle decade. Economy stable but slowing. Politics moving toward populism in some regions. Internet just starting to feel widespread.

NPCs and newspapers may *allude* to events from this timeline. We do not document them in detail. Writers may invent specifics as needed and add them to this doc.

---

## The province: Aldwich Province [PLACEHOLDER]

The province in which the slice town **Veyne** sits.

- **Capital:** **Westmere** [PLACEHOLDER] — a larger city, mentioned in dialogue, not visited in MVP
- **Geography:** Rolling hills, mixed forest, a major river (**the Aldwich** — town and province both named for it), a few small lakes
- **Climate:** Temperate. Four real seasons. Mild summers, snowy winters, beautiful springs and autumns
- **Industry in Aldwich:** Manufacturing in the river towns, agriculture in the hill country, fishing on the lakes
- **Vibe:** Working-class to middle-class. Practical. Not glamorous. Quietly proud.

---

## The town: Veyne [PLACEHOLDER]

Where the player lives in MVP.

### Basics
- **Population:** ~12,000 in fiction (will be heavily abstracted in MVP)
- **Founded:** 1837 [placeholder year], as a milling town on the Aldwich River
- **Economy:** Two small factories (textiles, agricultural equipment), one small university, family-owned retail, agriculture
- **Atmosphere:** Quiet small town. Everyone knows everyone. Not rural, not urban. Closer to a working-class American town from a 2003 indie film than to a New York neighborhood.

### Geography of Veyne (for art direction reference)
- **Main Street** runs east-west through downtown. Old brick buildings, mostly two stories. The café (14 Linden Street) is one block off Main on a quiet cross street.
- **The river** runs along the north edge of downtown. Old industrial buildings on the riverbank, mostly converted or empty. A pedestrian bridge connects to the residential north side.
- **The university** is on a hill at the east end of town. About 4,000 students. Most don't come downtown except for cheap food and used books.
- **The west end** is industrial — the factories, the rail spur, the warehouse district.
- **The south side** is residential. Modest single-family houses, some apartments, a few duplexes.
- **The north side** (across the river) is older, mostly working-class, walkable, dense.
- **The cemetery** is south of downtown, on a hill, with a view of the river.
- **The high school** is on the south side, where June goes.
- **The grocery store** (chain: **Hallow's Market** [PLACEHOLDER]) is on Main near the south end.

### Neighborhoods (named for atmosphere even in MVP dialogue)
- **Linden Row** — the quiet cross-street where the café is
- **Riverwalk** — the converted industrial north strip
- **The Mills** — west-end factory district
- **Old North** — across-the-river old neighborhood
- **University Hill** — east end
- **South Acres** — south-side residential

### Notable landmarks (Veyne — for dialogue references and signs)
- **The Crow** — a small dive bar on Main where Mara goes for drinks with Edie
- **Hallow's Market** — the grocery chain
- **Veyne Sentinel** — the local newspaper (player-written content in v1.0+, NPC-canon at launch)
- **The Old Mill** — converted to apartments and a brewpub
- **St. Margaret's** — small Catholic church near the cemetery [PLACEHOLDER name]
- **Aldwich Bridge** — the pedestrian bridge
- **The Hall** — Veyne's modest brick town hall

---

## NPCs of the slice — Veyne residents

These are written in detail in [05-vertical-slice-cafe-day.md](05-vertical-slice-cafe-day.md). Recap:

- **Mara / Eli** — player character, café owner [name chosen at creation]
- **Rowan** — player's spouse, late 30s, does the café books
- **June** — player's daughter, 14, high school student
- **Mr. Holland** — 62, retired teacher, warm regular
- **Sasha** — 28, courier, cold regular, generous tipper
- **Edie** — 41, journalist for the *Veyne Sentinel*, warm
- **Frances** — 35, single mother, weekly visitor, quiet
- **Old Mr. Pell** — 78, lonely widower, daily customer
- **The Theos / NPC employees** — to be added later

---

## Wider culture (drip-feed material)

Things NPCs and newspapers may reference; not detailed unless writers need them.

### Music
- A *Marennese folk revival* in the late 1970s; some living musicians from that era still tour
- A *garage rock scene* in Hesperin in the late 1990s; players' bands in-game may name-drop influences from this
- A late-1990s pop star, **Lina Forge** [PLACEHOLDER], whose songs are nostalgic for current adults
- **Atlas Hands** [PLACEHOLDER] — a contemporary indie-folk artist (player-created in v1.0+, NPC-canon at MVP) whose new album is reviewed in the in-game paper

### Sports
- **FC Westmere** — Aldwich Province's football team; mid-table, beloved
- **The 1998 cup final** — a famous match referenced occasionally
- Stoop — the regional sport; played in school yards, mentioned in passing

### Politics
- The current ruling federal party is the **Liberal-Democratic Alliance**, in coalition
- The main opposition is the **People's Reform Front** (centrist populist) and the smaller **Common Cause** (left-labor)
- In Aldwich Province, the **Aldwich Regionalists** hold the provincial government
- The current Veyne mayor is an NPC named **Owen Pell** (no relation to Mr. Pell), 56, second term, generally respected, slowly losing ground to a younger challenger from the Reform Front

These names provide texture; writers can reference them in newspapers, dialogue, and election flavor.

### Recent news (for the slice's morning paper)
- An editorial about an upcoming **neighborhood-association election** (the Reform Front is organizing locally)
- A music review of the new **Atlas Hands** album
- A small police-blotter item about a car theft two neighborhoods over
- A weather forecast (clear, mild)
- A sports recap (FC Westmere lost 2–1 to a rival)

---

## What we don't do

- **No fantasy.** No magic. No gods. No supernatural creatures. The world is mundane.
- **No real-world political analogs.** Marennese politics is its own thing; do not write a thinly-veiled Republican vs. Democrat caricature.
- **No real-world ethnic conflicts mapped onto Marenne.** The country is fictional and its history is its own. Real-world racism, antisemitism, etc. do not exist in Marenne in mappable form (though *inequality and class tension* do — they're necessary for the civic mechanics to matter).
- **No "lore dumps" in NPC dialogue.** No NPC ever recites the country's history at the player.
- **No globally famous players (yet).** Players are local until they earn otherwise.


---

# Roadmap

The current milestone is **the first playable shared-world prototype**.

This is not a café slice. The café is only the first test building inside Veyne. The real milestone is a small playable block where citizens, places, plots, ownership, jobs, money, time, and history all exist in one shared world state.

---

## Operating Assumptions

- Solo dev plus small volunteer team
- AI-assisted development
- Local-first implementation, multiplayer-ready architecture
- Unity handles presentation, input, and rendering
- Pure C# simulation owns world state and rules
- No per-player world copies: every player eventually sees the same plots, owners, citizens, and history

---

## Phase 0 — Project Foundation

Get the Unity project buildable and navigable.

### Tasks

- Set up git, Git LFS, `.gitignore`, `.gitattributes`
- Create `Assets/_Project/` structure
- Create `Bootstrap`, `MainMenu`, `Veyne_Exterior`, and `Veyne_CafeInterior`
- Add basic player movement, camera follow, scene transitions, and collision
- Add 2.5D sorting scaffold

### Definition of Done

The player can launch the project, enter Veyne, walk between exterior/interior scenes, and interact with placeholder objects.

---

## Phase 1 — Shared World State

Build the world model that every future player reads from.

### Tasks

- `WorldState`
- `RegionState`
- `CitizenState`
- `PlotState`
- `BuildingState`
- `NpcState`
- `EconomyState`
- `HistoryEvent`
- `GridCoord`

### Definition of Done

Veyne exists as data, not just as a Unity scene. It contains citizens, plots, buildings, owners, and history records.

---

## Phase 2 — Property and Ownership

Make shared property ownership visible and persistent.

### Tasks

- Add plot ownership records
- Add building ownership records
- Add access rules: `Public`, `Private`, `EmployeesOnly`, `Government`
- Add `PropertySystem`
- Add ownership commands
- Show building owner in the Unity scene from `WorldState`

### Definition of Done

Walk up to a building and see ownership pulled from shared world state:

```text
Linden Café
Owned by Noah
```

If ownership changes, the label changes for everyone who reads the world state.

---

## Phase 3 — Citizens and Places

Stop thinking in customers. Start thinking in citizens.

### Tasks

- Citizens have ids, names, role/kind, home plot, workplace, reputation placeholder
- Places have ids, names, ownership, access rules
- NPCs are citizens in the world, not scene-only objects
- Holland/Sasha become citizens who visit a place, not just café customers

### Definition of Done

The world can answer:

```text
who is this person?
where do they live?
where do they work?
what place are they visiting?
who owns that place?
```

---

## Phase 4 — Time, Schedules, and Movement

Citizens move through the world on schedules.

### Tasks

- Deterministic world clock state
- Schedule system
- Citizen visit events
- Simple movement between places
- Navigation prototype integration later
- Offline/scene-catchup logic for citizens

### Definition of Done

A citizen can be scheduled to visit a place at a time, and the world state can track whether they are traveling, inside, working, waiting, or leaving.

---

## Phase 5 — Jobs and Work

Create a general job framework. Café work is only one job.

### Tasks

- `JobDefinition`
- `WorkplaceState`
- `ShiftState`
- Job action commands
- At least two job examples, such as café owner and courier/clerk/mechanic
- Job action updates money and history

### Definition of Done

Two different jobs use the same system and can pay money, update world state, and write history events.

---

## Phase 6 — Economy Foundation

Money exists as a shared-world system.

### Tasks

- Citizen/business money records
- Business income
- Simple sale/payment command
- Price table placeholder
- Wage placeholder
- Transaction history

### Definition of Done

Money changes are recorded in world state and can be saved/loaded. Business income is not just a scene-local number.

---

## Phase 7 — History Log

The world remembers.

### Tasks

- Append-only history events
- Event ids
- Timestamps
- Actor ids
- Place ids
- Event kinds: ownership change, citizen visit, job action, payment, dialogue, death later
- Simple in-game/dev UI to inspect recent history

### Definition of Done

Meaningful actions write history events, and those events survive save/load.

---

## Phase 8 — Command Boundary

Make player actions network-ready.

### Tasks

- `IWorldCommand`
- Command context
- Command result
- Inspect property command
- Transfer plot command
- Talk to citizen command
- Do job action command
- Prepare/serve order commands remain as job-specific examples

### Definition of Done

Unity input sends commands. Commands mutate `WorldState`. Unity renders the result. This is the path future multiplayer uses.

---

## Phase 9 — First Playable Shared-World Prototype

This is the actual current milestone.

### Required Playable Features

- One small block of Veyne
- Several inspectable plots/buildings
- At least one player-owned building
- Several citizens
- At least two jobs/actions
- Money changes
- Ownership changes
- Citizen schedules
- Visible history log
- Save/load of one coherent `WorldState`

### Definition of Done

A tester can play and understand the real promise:

```text
this is one shared world where citizens, property, work, money, and history persist.
```

---

## Phase 10 — Multiplayer Proof

Only after the local shared-world foundation works.

### Tasks

- Local authoritative host owns `WorldState`
- Client sends commands
- Two clients see same region state
- Both see same plot owner
- Both see same citizens
- Only server-validated commands mutate state
- Server broadcasts snapshots/events

### Definition of Done

Two clients can connect to one local world and see the same shared ownership/citizen/job/history state.

---

## Hard No List Until Phase 9 Works

Do not build yet:

- Elections/government
- Crime/justice
- Combat
- Music creation
- Full region sharding
- Real account/auth
- Real database server
- Full economy simulation
- Frontier expansion

Those systems require the shared-world foundation first.

---

## Long-Horizon Direction

### Year 1

- First playable shared-world prototype
- Local multiplayer proof
- Veyne block with owned buildings
- First real property and job systems
- Basic history log
- Basic citizen schedules

### Year 2

- Persistent online single-region prototype
- More jobs
- More properties
- Basic government
- Basic family/legacy scaffolding

### Year 3+

- Multi-region world
- Crime/justice
- Elections
- Player-created culture
- Frontier expansion
- Full civilization history

---

## Risk-Aware Planning

The biggest risk is building content before the shared world exists.

If a feature does not help prove shared world state, ownership, citizens, jobs, money, history, or multiplayer-readiness, it waits.

---

## Visibility Cadence

- Weekly internal update
- Small playable build every 1–2 weeks
- Architecture review whenever a new system touches `WorldState`
- No system is considered real until it saves, loads, and can theoretically sync to another client


---

# Café Testbed — Café Day

> This document is retained as a **testbed reference**, not the current milestone.
> The current milestone is the **first playable shared-world prototype** in [12-roadmap.md](12-roadmap.md): a small block of Veyne with shared property ownership, citizens, jobs, money, schedules, persistence, and history.
> Café systems are useful only insofar as they prove general world systems.

The first concrete milestone. Single-player, ~30 minutes of play, one apartment-above-café in one town on one Tuesday in spring 2003. Built right, it is the emotional argument for the entire game.

This document is the authoritative spec for the slice. All MVP work serves this document. Anything not required by this document is **out of scope** until the slice ships.

---

## TL;DR

- One day in the life of a small-town café owner
- 30 minutes real time
- Single-player
- Six named customers, one spouse NPC, one teenage daughter NPC
- Full work loop with mini-games
- Save / quit / replay another day
- No combat, no multiplayer, no crime, no government, no death

---

## Setting

| Field | Value |
|---|---|
| Country | Marenne [PLACEHOLDER] |
| Province | Aldwich Province [PLACEHOLDER] |
| Town | Veyne [PLACEHOLDER] |
| Address | 14 Linden Street |
| Year | 2003 (in-fiction) |
| Day | Tuesday |
| Season | Late spring |
| Weather | Clear, mild, 17°C |
| Time span | 06:30 to 18:30 in-game |

---

## Locations (3 scenes)

### Veyne_Exterior
A short stretch of Linden Street, the cross-street where the café sits.
- Café facade (14 Linden Street) — door + large shop window
- Apartment door (separate, leads to stairs up to the apartment)
- A few other shop fronts (non-interactive in MVP — placeholder names: *Marlow's Books*, *Greene's Tailoring*, *Pell's Pharmacy*)
- Sidewalk, street, a couple parked cars (static), three trees, two streetlights, one mailbox, one bench
- A small park bench at the end of the visible street where the porch scene happens at sunset

### Veyne_CafeInterior
Inside Linden Café.
- Counter (player works behind it)
- Espresso machine, coffee grinder, oven, register
- Display case (pastries)
- 5 small tables (4 seats each), 1 bar counter at the window (2 seats)
- Door (chimes when opened)
- "OPEN/CLOSED" sign
- Wall art (placeholder)
- Back door to a tiny storeroom (non-explorable in MVP but visible)

### Veyne_ApartmentInterior
Modest two-bedroom apartment above the café.
- Bedroom (the player's bed)
- Kitchen/living area (one open space)
- Small dining table (2 chairs)
- Couch, coffee table, small TV (period-appropriate cathode TV, not played in MVP)
- Bathroom (visible, not used as room)
- Front door (leads to stairs down to street)
- Porch (small balcony with one chair) — accessed from the kitchen

---

## Player character

- Adult, name chosen at character creation
- Default name options: **Mara** or **Eli** (gender-neutral)
- Simple visual creator: 3 sliders (skin tone, hair color, shirt color)
- No backstory presented. You run the café. That is the entire premise.

---

## Family NPCs

### Rowan (spouse)
- Age late 30s, gender-neutral default; player picks pronouns at start
- Warm, dry sense of humor
- Stays in apartment in the morning; does the café books in the evening
- Lines: morning kitchen greeting (3–5 lines), evening "how was your day" (3–5 lines)
- Relationship starts at 10/10; ambient affection

### June (daughter)
- Age 14
- Curious, a bit moody, into books
- High school student
- Passes through the café before school at 06:55, and at lunch at 12:00
- Lines: morning ("don't forget your lunch" / "homework was awful"), lunch ("the play tryouts went weird")
- Relationship starts at 8/10

---

## Customer NPCs (6 total across the day)

Each has a name, age, generated trait set, preferred order, dialogue pool of ~10 lines, and one signature memory line that fires at relationship threshold.

### Mr. Holland — 62, retired teacher
- **Trait set:** warm, talkative, generous
- **Order:** black coffee
- **Arrives:** 07:35
- **Stays:** ~10 in-game minutes
- **Sample line (greeting):** "Morning, you. Black coffee, the usual."
- **Sample line (mid-chat):** "Forty years in front of a classroom. Forty. ...I still wake up before the alarm."
- **Signature line (relationship ≥ 3):** "You know, I taught for forty years in this town. Never had a worse student than this one kid in '78. Wonder where he ended up. Probably running for mayor."

### Sasha — 28, courier
- **Trait set:** cold, terse, generous
- **Order:** espresso, single shot
- **Arrives:** 08:05
- **Stays:** ~3 in-game minutes (in and out)
- **Sample line (greeting):** "Espresso. Quick."
- **Sample line (payment):** "...Keep it." [tips well]
- **Signature line (relationship ≥ 3):** "...Thanks. You don't talk much. I like that."

### Edie — 41, journalist
- **Trait set:** warm, talkative, average tipper
- **Order:** cappuccino + a pastry
- **Arrives:** 09:30
- **Stays:** ~12 in-game minutes (sits, reads notes)
- **Sample line (greeting):** "Cappuccino. Wet, not dry. And whatever pastry isn't dry."
- **Sample line (mid-chat):** "Story I'm working on. Mayor's chief of staff. Boring on paper, less boring if you read between the lines."
- **Signature line (relationship ≥ 3):** "Off the record — there's a meeting at the town hall tonight. Should be interesting." *(Narrative scaffolding for post-MVP content; in MVP, just texture.)*

### Frances — 35, single mother
- **Trait set:** quiet, warm, tired, stingy by necessity
- **Order:** plain coffee + a small pastry "to take to my kid"
- **Arrives:** 13:30 (afternoon shift)
- **Stays:** ~6 in-game minutes
- **Sample line (greeting):** "Hey. Just a coffee."
- **Sample line (mid-chat, low energy):** "Long week. Long month, honestly."
- **Signature line (relationship ≥ 2):** "[after a pause] My landlord raised the rent again. I shouldn't be telling you that. I don't know why I am."

### Old Mr. Pell — 78, widower
- **Trait set:** quiet, warm if you let him, fixed-income (small tip)
- **Order:** tea
- **Arrives:** 14:45 (afternoon shift)
- **Stays:** ~15 in-game minutes (sits at his usual table, takes his time)
- **Sample line (greeting):** "...just a tea, dear."
- **Sample line (mid-chat):** "The garden's coming in nicely. Tomatoes mostly. ...She loved tomatoes."
- **Signature line (relationship ≥ 2):** "I used to sit at this table with my wife. Forty-one years. ...You make the tea like she did."

### Stranger (procedural) — variable
- **Generation:** trait set rolled at slice start (warm/cold, talkative/quiet, generous/stingy, mood)
- **Order:** rolled from menu
- **Arrives:** 15:30
- **Stays:** ~8 in-game minutes
- **Greeting:** assembled from pool ("Hi." / "Afternoon." / "How's it going?")
- **Memorable moment:** one tagged line from the "stranger pool" — examples:
  > "Just passing through on my way north. Strange — it feels like I've been here before."
  > "I don't usually do this, but — could I sit a moment? Long day."
  > "You remind me of someone. Don't worry about it."
  > "Beautiful afternoon. ...I forgot what beautiful afternoons feel like."

---

## Time-of-day arc

Times are in-game. Real-time scale: 1 real second = 1 in-game minute. (12 in-game hours = 12 real minutes of pure clock; padded with transitions/cutscenes to ~30 minutes total play.)

| In-game | Beat | Notes |
|---|---|---|
| 06:30 | Wake | Bedroom. Alarm clock SFX. Player sits up. |
| 06:35 | Kitchen | Rowan already up. 3–5 line dialogue. Optional coffee at home. |
| 06:50 | Walk to café | Down the stairs. Scene transition to café interior. |
| 06:50–06:55 | Open up | Unlock door, flip OPEN sign, light switches on. |
| 06:55 | June passes through | Brief dialogue. She leaves. |
| 07:00 | Café opens | Clock visible on wall. |
| 07:35 | Mr. Holland arrives | Full customer loop. |
| 08:05 | Sasha arrives | Quick visit. |
| 09:30 | Edie arrives | Stays longer. |
| 10:30 | Lull | Player choice: clean tables, restock, read the paper. |
| 12:00 | June lunch visit | Sits in café briefly. Optional gift food. |
| 13:00 | Afternoon shift | Lull continues. |
| 13:30 | Frances arrives | |
| 14:45 | Mr. Pell arrives | Stays a while. |
| 15:30 | Stranger arrives | The memorable-moment slot. |
| 17:30 | Close shop | Lock door, flip CLOSED sign. |
| 17:35 | Cash up | Visible till total. Day's earnings displayed. |
| 17:45 | Walk home | Upstairs. |
| 17:50 | Rowan dinner | 3–5 line dialogue. |
| 17:55 | Porch | Sit. Watch sun set. Music swells. Time accelerates. |
| 18:30 | Fade | Title card: "Tuesday, May 14, 2003. The town of Veyne goes to sleep." |
| - | Save + menu | Play another day / Quit. |

---

## Customer visit loop

The core gameplay of the slice. State machine:

```
Enter → Approach → Order → AwaitPrep → Receive → Pay → Chat? → Leave
```

### 1. Enter
- NPC pathfinds in from off-screen edge of café (door area)
- Door chime SFX
- Idle animation while they look around briefly

### 2. Approach counter
- NPC walks to counter and stops at idle
- Interaction prompt for player appears

### 3. Order
- Player presses `[E]` to greet
- NPC delivers their order line via dialogue bubble
- Recipe card UI appears for player (small floating card showing the steps)

### 4. Preparation mini-game (15–30 seconds)
- Detailed per recipe below
- Player works at the relevant station (grinder, espresso machine, oven, kettle)
- Quality score 0–100 calculated from accuracy

### 5. Receive
- Player picks up finished item from prep station
- Carries item to counter
- Places on counter (animation)

### 6. Payment
- NPC drops a bill on the counter (procedurally chosen bill size)
- Player makes change by selecting denominations from cash drawer UI
- Speed and accuracy contribute small relationship bump

### 7. Chat? (optional)
- "Chat?" prompt appears
- If player engages: 1–2 dialogue exchanges from NPC's pool
- Relationship increments by 1
- At relationship threshold, NPC delivers signature line

### 8. Leave
- NPC says farewell line
- Drops tip into jar (visible coin/bill icon animation)
- Pathfinds out
- Door chime
- Relationship score persisted to save

---

## Mini-game specs (concrete numbers)

### Coffee — 4 steps

#### a. Grind beans
- Hold-and-release on a moving meter
- Meter fills left-to-right over 2.0 real seconds
- **Sweet spot:** 0.4s wide centered at 1.4s
- Release timing maps to score: 0–40 quality points
- Visual: grinder hopper, beans dropping animation
- Audio: grinder hum, then beans grinding

#### b. Tamp portafilter
- Single click on a sweet spot bar
- Bar sweeps left-right over 1.0s
- **Sweet spot:** 0.3s window
- Score: 0–20 quality points
- Visual: tamper pressing down
- Audio: light press SFX

#### c. Pull shot
- 25-second simulated timer (compressed to ~5 real seconds visually)
- **Green band:** 18s–22s
- Click "stop" when shot looks right
- Score: 0–25 quality points
- Visual: espresso pulling into shot glass, color darkening to ideal
- Audio: machine hum, hiss

#### d. Pour milk (if drink requires it)
- Drag cursor along a curved path under gravity-like deflection
- 4-second window
- Score: 0–15 quality points
- Visual: milk pitcher, latte art appearing
- Audio: steam wand, then pour
- Skipped for black coffee/espresso/Americano

**Total coffee quality:** 0–100

### Pastry — 2 steps

#### a. Place in oven
- Click & drag pastry onto oven tray, close oven
- No score (just interaction)
- Sets timer

#### b. Pull at correct moment
- 60-second simulated timer (compressed to ~10 real seconds visually)
- **Window:** ±4 seconds of bell
- Inside window: 0–100 quality based on closeness
- Late: burned, -50 to wallet (have to give it free) + customer impatience
- Audio: oven hum, bell ding at target

### Tea — 1 step
- Click steep, wait, click pull
- 5-second window
- Penalty for early/late
- Score: 0–100 quality

---

## Quality → tip math

```
quality      ∈ [0, 100]      // from mini-game
baseTip      ∈ [0.50, 3.00]  // per NPC trait (generosity)
multiplier   = 0.5 + (quality / 100) * 1.5
              // 0.5x at quality 0, 2.0x at quality 100
relMod       = 1.0 + (relationship / 10) * 0.5
              // 1.0x at rel 0, 1.5x at rel 10
finalTip     = baseTip * multiplier * relMod
```

Rounded to nearest $0.05.

---

## Systems exercised by the slice

From [02-system-list.md](02-system-list.md), in MVP form:

- **F5** World Clock
- **C1** Character Creator
- **C2** Stats (minimal — barista skill rises slowly with shifts)
- **N1** NPC population (~10 NPCs total)
- **N2** Traits & Personality
- **N3** Schedule / Routine
- **N4** Dialogue
- **N6** NPC Heir/Spouse (Rowan + June)
- **N7** Memory (minimal)
- **P4** Items & Inventory
- **P5** Currency (cash only)
- **P6** Storefront (the café)
- **J1** Job Framework
- **J2** Job Mini-Game Library (café only)
- **W2** Region Definition (1 region, hand-authored)
- **W3** Tilemap (32×32)
- **W4** Modular Building Kit (used to assemble café and apartment)
- **W5** Day cycle (exterior only)
- **I1** HUD
- **I2** Inventory UI
- **I6** Notifications (minimal)
- **I7** Onboarding (gentle first-life intro)
- **E1–E5** Engine spine

**~25 systems in their minimum-viable forms.**

---

## Assets needed (estimated counts)

### Characters
9 humans × 4 directions × (4 idle + 8 walk) frames = **432 character sprite frames**

### Tiles
- Interior: floors (wood, tile), walls (plaster, brick), windows, doors, ceiling: ~60 tiles
- Exterior: sidewalk, asphalt, curb, grass, brick facade, brick storefront, windows, awnings: ~50 tiles
- Counter/café-specific: ~25 tiles
- **Total: ~135 distinct 32×32 tiles**

### Props (placed sprites)
- Café interior: ~40 (chairs, tables, espresso machine, grinder, oven, register, display case, paintings, plants, hanging lamps, menu board)
- Apartment interior: ~25 (bed, couch, kitchen table, kitchen counter, stove, fridge, TV, bookshelf, lamps, plants, framed photos)
- Exterior: ~20 (parked cars, streetlights, trees, mailbox, bench, signs, awnings)
- **Total: ~85 props**

### UI
- HUD frames (clock, money, day): 5 sprites
- Dialogue box + portrait frame: 5 sprites
- Recipe card UI: 5 sprites
- Cash drawer UI (bills + coins denominated): 10 sprites
- Mini-game UI (grinder meter, tamper bar, shot timer, milk pour path): 10 sprites
- Buttons, icons, prompts: 15 sprites
- **Total: ~50 UI sprites**

### Audio
- **Music:** 3 short pieces (morning bed, midday bed, evening bed) — 90s loops each
- **SFX:** ~30 (footsteps wood/concrete, door chime, alarm clock, grinder, espresso pull, milk steamer, oven beep, register, coin clink, paper rustle, light switch, ambient café murmur loop, ambient street loop, walk SFX variants, dialogue blip variants)
- **Ambient:** 2 beds (interior café murmur, exterior Linden Street loop)

---

## Save model for the slice

Per [09-save-system.md](09-save-system.md).

Single-player local save. JSON. Stored at `Application.persistentDataPath/Legacy/saves/<slot>/world.json`. Schema versioned from v1.

Minimum fields saved:
- Current in-game date and time
- Player position and scene
- Player wallet (cash by denomination)
- Player stats (barista skill)
- Family relationships (Rowan, June)
- Each customer NPC's relationship score and last-seen day
- Café till lifetime and today
- Shifts completed
- History log entries

---

## Definition of done

The slice is shippable when **all** of the following are true.

### Functional
- A new player can launch the game, complete character creation, and play through Tuesday from 06:30 to 18:30 in approximately 30 real minutes without softlock, crash, or visible placeholder mid-play.
- All 6 customers can be served their preferred order via the full mini-game flow.
- Dialogue with Rowan and June triggers correctly at scheduled times.
- Save & quit at the end of the day → reload → start a new day → at least one NPC remembers the player (greeting line varies based on relationship).
- All audio plays at correct times and crossfades cleanly between time bands.

### Quality
- Visual style is cohesive (placeholder art is acceptable as long as it is consistent and readable).
- No frame drops below 60 fps on a 2020-era mid-range laptop.
- All text is readable at the target display resolutions (1080p, 1440p, 4K).
- All audio is balanced; no clipping; no jarring volume spikes.

### Emotional
- One full playthrough produces at least one moment that a friend would describe as "kind of beautiful," "unexpectedly moving," "I didn't think a game would do that," or similar.
- This is the most important criterion. If we ship something technically perfect but emotionally inert, we have failed.

---

## Out of scope for the slice

Explicitly out. These are noted because some are tempting to add and we must not.

- Multiplayer (any form)
- Combat, weapons, injury
- Crime, police, courts, prison
- Government, voting, elections, lawmaking
- Frontier, second region, world map
- Marriage system (Rowan exists; the *system* of marrying does not)
- Death system, inheritance, family lines beyond Rowan/June
- Music creation, publishing, radio
- Vehicles (parked cars are static props; no driving)
- Internet, phone, email (Rowan calls via off-screen "the phone rang" — no phone UI)
- Multiple in-game days (every day is Tuesday-shape; date increments only)
- Procedural map generation (Linden Street is hand-authored)
- Background NPC population beyond the 9 named characters (street is mostly empty)
- Weather variation (always clear and mild)
- Seasons (always late spring)

---

## Out-of-scope items that *will* be added in the next slice (Slice 2)

Just so the team knows what's queued and isn't lost.

- Background NPC population (extras walking the street, sitting at tables)
- A second day shape (Wednesday — different rhythm, different customers, optional events)
- A second business (the bookshop next door, walkable in but not workable)
- Phone calls (Rowan can call you on a landline)
- Newspaper system (the morning paper is a real interactable object with multiple in-fiction articles)
- Weather variation (one rainy day)


---

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


---

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


---

# Code Conventions

How we write C# in this project. Read once, follow forever.

---

## Language

- **C# 12** (Unity 6 supports it via the Roslyn compiler)
- **.NET Standard 2.1** API compatibility (Unity default)
- **Nullable reference types: OFF** project-wide (Unity ecosystem fights nullable; not worth the noise in MVP)

---

## Naming

| Element | Style | Example |
|---|---|---|
| Types (class, struct, enum, interface) | `PascalCase` | `NPCController`, `RecipeDefinition` |
| Interfaces | `IPascalCase` | `IInteractable`, `ISaveable` |
| Methods | `PascalCase` | `OpenShop()`, `TryServeCustomer()` |
| Public properties | `PascalCase` | `CurrentTime`, `IsOpen` |
| Public fields | `PascalCase` *(rare; prefer properties)* | |
| Private fields | `_camelCase` | `_clock`, `_currentCustomer` |
| Constants | `PascalCase` | `MaxInventorySlots = 12` |
| Static readonly | `PascalCase` | |
| Parameters & locals | `camelCase` | `customer`, `qualityScore` |
| Type parameters | `TPascalCase` | `TItem`, `TComponent` |
| Async methods | suffix `Async` | `SaveAsync()`, `LoadWorldAsync()` |
| Event names | `PascalCase`, past or present participle | `Saved`, `Saving`, `CustomerArrived` |
| Booleans | `Is/Has/Can/Should` prefix | `IsOpen`, `HasInventoryRoom`, `CanInteract` |

---

## Formatting

- **Braces on new line** for types, methods, properties
- **Braces on same line** for control flow (if/for/while/switch)
- **Always use braces** even for single-statement bodies
- **4-space indentation**, no tabs
- **120-column soft wrap**

```csharp
public sealed class NPCController : MonoBehaviour
{
    private readonly Memory _memory = new();

    public bool TryServeCustomer(NPC customer)
    {
        if (customer == null) {
            return false;
        }

        for (int i = 0; i < _stations.Length; i++) {
            // ...
        }

        return true;
    }
}
```

---

## Architecture rules

### Composition over inheritance
- No more than **one level** of inheritance for game-logic classes (component or interface preferred)
- Deep MonoBehaviour hierarchies are forbidden
- Use `interface` for polymorphic capability (`IInteractable`, `ISaveable`, `IDamageable`)

### MonoBehaviours vs. plain C# classes
- **MonoBehaviours** are the *view layer* — they listen for input, render state, and bridge Unity engine features (transforms, colliders, animators)
- **Plain C# classes** hold all *game logic* — they are testable, headless, and have no Unity dependencies
- Example: `NPCController : MonoBehaviour` (the view) wraps `NPCInstance : class` (the logic)

### Singletons
- **Forbidden** except for the `ServiceLocator` and `EventBus`
- Every other "global" is a service registered with the locator at boot
- Bootstrap scene registers all services in a known order

### Dependency direction
- `Core` depends on nothing
- `Time`, `World`, `Audio`, `Save` depend on `Core`
- `Characters`, `NPCs`, `Inventory`, `Currency`, `Dialogue`, `Interaction` depend on `Core` (+ each other narrowly)
- `Jobs` depends on the above
- `UI` depends on everything (it observes)
- **No cycles.** If you need a cycle, you're missing an abstraction.

### Communication between systems
- **Direct method calls** when one system owns another (Player owns Inventory)
- **EventBus** when systems must be decoupled (Jobs raises `CustomerServed`; UI listens)
- **No `FindObjectOfType` chains.** That pattern is forbidden in non-Editor code.

### Async vs. coroutines
- **`async/await`** for I/O and one-shot async work (file save, file load, future network)
- **Coroutines** still allowed for animation/timing where they read naturally (camera shake, fade, scheduled in-game events)
- **`UniTask`** is *not* used — built-in async/await is sufficient for our scale

### ScriptableObjects for data
- All *static* game content lives in ScriptableObjects:
  - Items, Recipes, NPC archetypes, Dialogue trees, Tile sets, Building parts, Schedules, etc.
- No hardcoded magic strings or numbers for game content
- Editor-authored, serialized, version-controlled as text

### Public surface area
- Default to `private` and `[SerializeField]` for everything inspector-bound
- Use `internal` for assembly-private types
- `public` only when an external caller actually needs it

```csharp
public sealed class CafeShift : MonoBehaviour
{
    [SerializeField] private PrepStation _coffeeStation;
    [SerializeField] private Recipe _defaultRecipe;

    private CafeShiftState _state;

    public bool IsOpen => _state == CafeShiftState.Open;

    public void Open()
    {
        // ...
    }
}
```

---

## Type modifiers

- **`sealed`** on all classes unless explicitly designed for inheritance
- **`readonly`** on all fields that don't change after construction
- **`record`** for plain data carriers where appropriate (C# 12 supports record class & record struct)
- **`struct`** only for small, immutable value types (rare in gameplay code)

---

## Performance defaults

- Atlas sprites per category; never one atlas per sprite
- **Object pooling** for: dialogue bubbles, particles, customer NPCs entering and leaving
- No `string` concatenation in hot paths — use `StringBuilder` or pre-formatted strings
- Avoid LINQ in tight loops (per-frame); LINQ is fine in setup, save/load, UI updates
- Profile before optimizing — but profile, don't guess

---

## Error handling

- **Throw** for *programmer errors* (a null where one is never valid; a switch reaching an impossible case)
- **Return result types or bools** for *expected failures* (a customer who couldn't be served because the queue is full)
- **Log warnings** for *recoverable anomalies* (missing dialogue line — substitute filler)
- **Log errors** for *unexpected bugs that don't crash* (save file missing field — apply default and warn)
- **Crashes are allowed** in development; never in shipping builds

```csharp
public bool TrySaveAsync(string slot, out SaveError error)
{
    if (string.IsNullOrEmpty(slot)) {
        throw new ArgumentException("Slot must not be empty.", nameof(slot));
    }

    if (!_saveDir.Exists) {
        error = SaveError.SaveDirectoryMissing;
        return false;
    }

    // ...
}
```

---

## Logging

- Use a single `Logger` wrapper (in `Scripts/Core/Logger.cs`), never raw `Debug.Log` in gameplay code
- Levels: `Trace`, `Info`, `Warn`, `Error`
- `Trace` and `Info` stripped from shipping builds
- Tag every log with module: `Logger.Info(Tag.NPCs, "Customer arrived: " + name)`

---

## Comments

- **Comments explain *why*, not *what*.** The code shows what. The comment shows intent, trade-off, constraint.
- **Avoid narrating comments.** `// Increment counter` is forbidden.
- **Public APIs get XML doc comments** (`///`) when the name isn't self-explanatory
- **TODO/FIXME/HACK** tagged comments include a name or ticket: `// TODO(@noah): handle dst-aware dates`
- **Don't comment out code.** Delete it. Git remembers.

---

## File layout

- One **public type** per file
- File name matches the type name exactly
- Private nested types allowed in the same file
- Order within a file:
  1. Usings (sorted, `System` first, then `Unity`, then `Legacy.*`, then `static using` last)
  2. Namespace
  3. Class declaration
  4. Constants
  5. Static fields
  6. Instance fields (`[SerializeField]` first, then private)
  7. Properties
  8. Events
  9. Unity lifecycle methods (Awake, OnEnable, Start, Update, OnDisable, OnDestroy)
  10. Public methods
  11. Private methods
  12. Inner types

---

## Testing

- **Unit tests** (Edit-mode) for all pure logic:
  - Quality scoring math
  - Time arithmetic
  - Dialogue state machine
  - Inventory operations
  - Save data round-trip
  - Wallet change-making
- **Integration tests** (Play-mode) for orchestration:
  - NPC schedule fires at expected in-game time
  - Save → load → state preserved
  - Customer visit state machine completes
- **Test file** mirrors source: `Scripts/Jobs/Cafe/CafeShift.cs` → `Tests/Jobs/Cafe/CafeShiftTests.cs`
- **Test name** describes behavior: `TrySaveAsync_WithEmptySlot_Throws()`

---

## Source-control hygiene

- One PR per system or per slice subtask
- Commit messages: `[Module] short summary` (`[NPCs] add scheduled arrival timer`)
- No commits of `Library/`, `Temp/`, `Logs/`, `Build/`, autogenerated `*.csproj`/`*.sln`
- Scenes and prefabs ForceText serialization (set in Editor → Project Settings)
- LFS for binary assets per [07-project-structure.md](07-project-structure.md)

---

## What we don't do

- **No Update spam.** Every `MonoBehaviour.Update` is a small global tax. Prefer central scheduling.
- **No magic strings for content.** Use enums or ScriptableObject references.
- **No premature optimization.** Profile before refactoring for perf.
- **No premature abstraction.** Build the second instance before generalizing the first.
- **No silent failures.** Either log, or surface, or crash. Never absorb.
- **No global mutable state** outside ServiceLocator-managed services.
- **No god classes.** If a class is over 300 lines, ask why. Over 500, refactor.

---

## A worked example

```csharp
// File: Assets/_Project/Scripts/Jobs/Cafe/MiniGames/GrindMiniGame.cs

using System;
using Legacy.Core;

namespace Legacy.Jobs.Cafe.MiniGames
{
    /// <summary>
    /// Grind step of the espresso prep loop. Pure logic; UI sits on top.
    /// </summary>
    public sealed class GrindMiniGame
    {
        private const float DurationSeconds = 2.0f;
        private const float SweetSpotCenter = 1.4f;
        private const float SweetSpotWidth = 0.4f;

        private float _elapsed;
        private bool _resolved;

        public bool IsResolved => _resolved;
        public float NormalizedTime => _elapsed / DurationSeconds;

        public void Tick(float deltaSeconds)
        {
            if (_resolved) {
                return;
            }

            _elapsed += deltaSeconds;
            if (_elapsed >= DurationSeconds) {
                _elapsed = DurationSeconds;
                Resolve();
            }
        }

        public int Resolve()
        {
            if (_resolved) {
                throw new InvalidOperationException("Already resolved.");
            }

            _resolved = true;

            float distance = MathF.Abs(_elapsed - SweetSpotCenter);
            float halfWidth = SweetSpotWidth * 0.5f;

            if (distance > halfWidth) {
                return 0;
            }

            float accuracy = 1f - (distance / halfWidth);
            return (int)MathF.Round(accuracy * 40f); // max 40 quality points
        }
    }
}
```


---

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


---

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


---

# Art Direction

The visual identity of *legacy*. Read this if you make any pixel for the game.

For tone in general (writing, music, voice), see [04a-tone-and-style-guide.md](04a-tone-and-style-guide.md).
For the pipeline (file formats, import settings), see [10-asset-pipeline.md](10-asset-pipeline.md).

---

## North star (visual)

> Stardew Valley pixel discipline. Edward Hopper light. Early-2000s small-town American/European urban texture. Warm, muted, analog, intentionally restrained.

If you remember nothing else, remember that.

---

## Reference cocktail

### Pixel-art reference (technique)
- *Stardew Valley* — pixel density, character readability, lighting at low res
- *Eastward* — higher pixel density urban environments, atmospheric lighting
- *Hyper Light Drifter* — chromatic discipline, painterly pixel sensibility
- *Sea of Stars* — character + environment integration at modern pixel scales
- *Coromon* — urban-area handling in pixel
- *Owlboy* — light/atmosphere with intentional pixel restraint

### Compositional / lighting reference (mood)
- Edward Hopper — *Nighthawks*, *Morning Sun*, *Automat*, *Early Sunday Morning*
- Wong Kar-wai stills — interior light, neon-but-not-neon, melancholic warmth
- Roger Deakins cinematography — natural light, soft golden hours
- Late 1990s–early 2000s American indie film color grading (Sofia Coppola's *Lost in Translation*, Wes Anderson's *Rushmore*)

### What we are NOT
- *Celeste* — too saturated, too "vibrant indie"
- *Hades* — too maximalist, too "epic"
- *Terraria* — too cluttered, too maximalist
- *Cuphead* — wrong era, wrong technique
- Anime-styled pixel art (*Persona*-adjacent) — wrong cultural register

---

## Specifications

| Setting | Value |
|---|---|
| **Internal resolution** | 480 × 270 |
| **Tile size** | 32 × 32 |
| **Pixels per unit** | 32 |
| **Filter** | Point (no filter) |
| **Compression** | None |
| **Color space** | Linear (URP default) |
| **Color depth** | 32-bit RGBA (limited palette is artistic, not technical) |

### Character sprites
- Player and NPC characters: roughly **24 × 32 pixels** within the 32 × 32 cell
- Heads ~ 8 × 8 pixels with internal feature pixels
- 4-directional movement
- 4-frame idle, 8-frame walk per direction
- Slight 1-pixel head bob in walk cycles (subtle, not bouncy)

### Tiles
- 32 × 32 base
- Some tile pieces extend upward into the cell above (walls, signs, tall props) — handled via Z-sort

### Props
- Mostly 32 × 32 or 64 × 32 or 32 × 64
- Larger props (cars, trees) up to 96 × 96
- Anchor point: bottom-center for Y-sorting

---

## Palette

We do not commit to a single global palette. We use **per-scene mood palettes** drawn from a master "Marenne palette" — roughly 64 colors total, harmonized.

### Master palette principles
- **No pure black** (`#000000`) — use a deep desaturated navy or warm dark brown
- **No pure white** (`#FFFFFF`) — use warm off-white or cool ivory
- **No saturated primaries** — all colors lean toward earthy desaturation
- **Warm vs. cool by time:** mornings cooler-warm, afternoons golden, evenings amber, nights dim blue-grey

### Time-of-day shifts (applied via global light tint, not repainted assets)
| Time | Tint | Notes |
|---|---|---|
| Dawn (06:00–07:00) | Cool lavender + warm sun | Long shadows |
| Morning (07:00–11:00) | Neutral warm | Clear light |
| Midday (11:00–14:00) | Slightly cool, neutral | Flat light |
| Afternoon (14:00–17:00) | Warm golden | The "Hopper hour" |
| Dusk (17:00–18:30) | Amber → dim blue | The signature mood |
| Night (18:30–06:00) | Cool dim blue + warm pools from interior light | Streetlamps as pools |

### Examples of "right" palettes
- Café interior: warm browns, cream walls, dark green accents, brass
- Café exterior afternoon: orange awning, dusty sidewalk, soft sky
- Apartment kitchen morning: pale yellow walls, white-tile splashback, brown wood
- Street at dusk: indigo sky, amber streetlamps, dim brick storefronts

---

## Composition rules

### Camera and framing
- Camera is locked at 3/4 top-down oblique
- Player character is roughly centered with Cinemachine framing
- Furniture and walls are drawn from this same fixed angle, never rotated
- No rotating buildings, no isometric flipping

### Z-sorting
- Sprites sort by Y (custom axis) so characters can stand "in front of" or "behind" props
- Tall objects (lamps, signs) extend upward; their pivot remains at the bottom

### Negative space
- Don't fill every pixel. Empty floors, empty walls, sky, silence — all valid.
- Stardew's most beloved shots are usually *uncluttered*

### Readability over detail
- Silhouette first, detail second
- A player should be able to identify any object at a glance from across the screen

---

## Light

Light is the single most important element of our visual identity.

### Sources we model
- **The sun** — global 2D light, color and intensity animated by time-of-day curve
- **Interior bulbs** — warm point lights inside buildings, on by default
- **Streetlamps** — small warm pools of light, turn on at dusk
- **The TV** — flickering cool blue point light in living rooms when on
- **The grinder/oven indicators** — tiny LED-style point sources for café equipment

### What we don't do
- **No bloom** — bloom undermines pixel discipline
- **No screen-space reflections** — wrong tech for the style
- **No volumetric god rays** — too "epic"
- **No flashing/strobing**
- **No neon** beyond a single deliberately placed sign or two per scene (the Crow's bar sign, the OPEN sign on the café)

---

## Animation principles

- **Frame-stepped, no interpolation.** This is pixel art; we don't tween subpixel motion.
- **4–8 frames per loop is plenty.** More is rarely better.
- **Subtle is better than emphatic.** A character's idle should not look like they're about to fight. They should look like they're standing.
- **Anticipation > snap.** Doors open in 3–4 frames with a hint of windup. Drawers slide in 4 frames with a small overshoot.
- **No squash and stretch** outside of comic micro-moments (one bounce on a hot pastry, etc.).

---

## Characters

### Diversity
- The world is racially, ethnically, and body-type diverse without being a checklist
- Character archetypes (skin tones, hair textures, body sizes) drawn naturally from the Marennese setting
- No exotic-other framing of any group; everyone is normal here

### Clothing
- Period-appropriate for early 2000s
- Working-class to middle-class registers
- Subtle stylistic variation suggests profession (apron, courier bag, journalist's notebook tucked under arm)
- Avoid costume-y "this character is a cop because they're in full uniform 24/7" — most NPCs wear regular clothes most of the time

### Faces
- Pixel faces are tiny; expression done through:
  - Eyebrow position (1–2 pixel shift)
  - Mouth shape (4–5 variations)
  - Head tilt
- Portrait sprites (used in dialogue UI) are larger (96 × 96) with more facial detail

---

## Environments

### Building exteriors
- Brick (red, brown, painted)
- Wood siding (white, weathered)
- Stucco (cream, tan)
- Glass storefronts with mullions
- Awnings (orange, green, striped)

### Interiors
- Wood floors (warm brown variants)
- Plaster walls (off-white, cream, painted)
- Tile floors (subway tile, hex tile)
- Functional furniture
- Personal clutter (mail on counter, dishes in sink, books on shelf)

### Streets
- Sidewalks: poured concrete with cracks and weathering
- Asphalt: dark grey with subtle texture
- Crosswalks, parking lines, gutters — drawn but not loud
- Litter is rare but present (a single coffee cup, a leaf, a discarded flyer)

### Vehicles
- Period-appropriate sedans, station wagons, pickup trucks, mid-2000s minivans
- Drawn as **static props in MVP**; drivable in v1.0+
- Faded paint colors (no neon yellow racing stripes)

---

## Foliage

- Hand-pixeled trees with seasonal variants (planned: spring & summer for MVP)
- Plants in pots (in cafés, on porches)
- Lawns rendered as gentle tile variants, not flat green

---

## UI sprites

- **Frame-based UI**, not flat squares — small pixel-art frames, bevels, tabs
- Iconography is pixel-art (5–8 × 5–8 px icons inside larger frames)
- Fonts: see below

---

## Typography

- **Pixel font** for all in-game text
- Recommended: **Determination Mono** (free) or a custom hand-drawn font (TBD)
- Sizes:
  - Body text: 8–10 px tall (raw pixels) scaled with the camera
  - Headers: 12–16 px tall
  - HUD: 8 px tall
- Always nearest-neighbor scaling, never anti-aliased
- All caps reserved for signage and headlines, never used for body text

---

## What needs an artist *most*

If the project recruits one external pixel artist, prioritize in this order:

1. **Player character + Rowan + June + 6 named customers** (the 9 humans for the slice)
2. **Café interior** (the most-visited space)
3. **Apartment interior**
4. **Café exterior + Linden Street**
5. **Props and UI**
6. **Animation polish pass**

---

## Reference board

When the project grows, maintain a private reference board (Pinterest, Milanote, or a shared `references/` folder) with:
- Screenshots from reference games
- Hopper / cinematography stills
- Early-2000s small-town photography
- Color palette swatches

Keep it organized. Update it. Remove things that no longer fit.

---

## The single test (visual)

For any image you make, ask:

> *Could this be a frame from a moment in a quiet Sunday afternoon in 2003?*

If yes, probably right.
If no, probably wrong.


---

# Audio Direction

The sonic identity of *legacy*. Read this if you compose music, design sound, or implement audio for the game.

For tone in general, see [04a-tone-and-style-guide.md](04a-tone-and-style-guide.md).
For the audio pipeline (formats, import settings), see [10-asset-pipeline.md](10-asset-pipeline.md).
For the runtime audio architecture, see [11-systems/audio-architecture.md](11-systems/audio-architecture.md).

---

## North star (sonic)

> Acoustic, sparse, intimate. The hum of a kitchen at 7am. The murmur of a café at 3pm. The quiet of a porch at sunset. The hush of a town that hasn't decided to be loud today.

If you remember nothing else, remember that.

---

## Reference cocktail

### Music reference (mood)
- *Stardew Valley* OST — sparse, melodic, emotionally direct
- *Disco Elysium* OST (esp. *Whirling-in-Rags*, *Doomsday*) — acoustic textures, ambient melancholy
- *Night in the Woods* OST — small-town indie warmth
- *Spiritfarer* OST — gentle, dignified, hopeful melancholy
- Nils Frahm — sparse piano
- Tom Waits' quieter records — barroom intimacy
- Bonnie "Prince" Billy — quiet folk
- Late-night jazz radio (Bill Evans, *Sunday at the Village Vanguard*)

### What we are NOT
- *Hades* OST — too kinetic, too "epic"
- *Skyrim* OST — wrong scale, wrong genre
- *Celeste* OST — too synth-driven, too "indie chiptune"
- Anime/JRPG OST tropes
- Trailer-music orchestral swells
- Lo-fi hip-hop beats to study/relax to (too algorithm-coded)

---

## Compositional guidelines

### Instrumentation (primary palette)
- **Piano** — felt-hammer or upright, no concert grand
- **Acoustic guitar** — fingerstyle, occasional strum
- **Brushed drums** — quiet, jazz-influenced kit
- **Upright bass** — pizzicato, melodic
- **Strings** — solo cello, solo violin, small ensemble; never lush
- **Analog synth pad** — sparingly, for evening/night moods only
- **Vibraphone, glockenspiel, music box** — accent colors, used rarely

### Instruments to avoid
- Full orchestra
- Big drums (rock kit, hip-hop kit, taiko)
- Distorted guitar
- Brass section
- Choir
- "Epic" trailer percussion
- Heavy synths (analog or otherwise)
- 808 / trap kit
- Anything that screams "soundtrack"

### Tempo
- **60–90 BPM** for most pieces
- Faster pieces are rare (a busy lunch rush might allow 100 BPM; nothing above 110)
- Pulse should feel like breathing, not running

### Dynamic range
- **Quiet.** Most music sits at -20 to -14 LUFS.
- No big crescendos
- A piece can have a single emotional peak — it should arrive gently
- Endings fade, not slam

### Structure
- Loops between **60–90 seconds**
- Loops should not feel like loops — gentle melodic variation, no obvious "and here we go again"
- Where layered: 2–3 stems that can fade in/out per game state

---

## Score by moment (slice)

| Moment | Mood | Instrumentation | Length |
|---|---|---|---|
| Morning (06:30–11:00) | Soft awakening, hopeful | Piano + guitar | 90s loop |
| Midday (11:00–14:00) | Steady, present | Guitar + brushed drums + bass | 90s loop |
| Afternoon (14:00–17:00) | Warm golden, slightly melancholic | Piano + cello | 90s loop |
| Evening porch (17:55–18:30) | Hush, signature emotional beat | Solo piano with optional cello | One-shot, 90s+ |
| Title menu | Slow, inviting | Music box + piano | 60s loop |
| End-of-day fade | Tiny resolution | Single piano motif, ~10s | One-shot |

---

## Ambient beds

Beds are the **most important non-music audio** in the game. They are what makes the world feel real.

### Slice beds (MVP)

| Bed | What it contains | Loops |
|---|---|---|
| Café interior murmur | Quiet conversation, dish clinks, low ventilation hum | Seamless 60s |
| Café morning quiet | Faint kitchen noise, distant traffic | Seamless 60s |
| Café evening (closing) | Reduced murmur, single distant siren | Seamless 60s |
| Apartment interior morning | Fridge hum, kitchen tap, gentle radio voice (unintelligible) | Seamless 60s |
| Apartment interior evening | Same with TV in the next room (faint, unintelligible) | Seamless 60s |
| Linden Street day | Faint traffic, occasional birds, distant child voices | Seamless 60s |
| Linden Street dusk | Traffic thinning, evening insects, distant dog | Seamless 60s |

### Beds for v1.0+ (planning)
- Different bed per neighborhood (Old North, The Mills, Riverwalk, University Hill)
- Weather variants per bed (rain on roof, snow muffling)

### Authoring beds
- Record or compose stems separately, mix together
- Avoid recognizable speech (no lyrics, no parsable English)
- Avoid recognizable real-world music in the bed
- Never have a bed loud enough to overpower dialogue or score

---

## SFX

### Style
- **Real, recorded, slightly compressed.** Not synthesized retro-game blips.
- **Short.** Most SFX are under 1 second.
- **Layered for richness.** A door chime is the bell + the door creak + the foot on the threshold.
- **Variants for repetition.** Footsteps in particular get 4–8 variants and randomized.

### SFX categories for slice

| Category | Examples |
|---|---|
| Footsteps | Wood, concrete, tile, grass (1 stage in MVP — wood interior + concrete street) |
| Doors | Wooden door open/close, bell chime on café door |
| Café equipment | Coffee grinder loop, espresso machine pull, milk steamer hiss, oven hum, oven bell ding, tea kettle whistle (rare) |
| Cash | Register drawer open/close, coins clink, bills rustle |
| UI | Dialogue blip (3 variants — masculine/feminine/neutral), menu select, menu cancel, notification chime |
| Ambient props | Light switch click, paper rustle, chair scrape, glass placed on counter |
| Outside | Distant car drive-by (rare), bird chirp variants |
| Body | Player breath (very subtle), Rowan/June acknowledge huh/mm |

### Dialogue blips
- Tiny "speech" sounds played per character per spoken letter (or sub-letter group) — the *Animal Crossing* / *Stardew* / *Undertale* approach
- One blip per character archetype (Mara, Rowan, June, Holland, Sasha, Edie, Frances, Pell, Stranger)
- Pitched slightly differently per character; very short (50–100 ms)

---

## Spatialization

- **In-world SFX** use 3D AudioSources with spatial blend set to 1.0 but constrained to 2D pan based on screen position
- **Music** is 2D (no spatialization)
- **UI sounds** are 2D
- **Ambient beds** are 2D with low-pass filtering when player moves into a different room

### Doors and rooms
- Audio occlusion is minimal — we don't simulate real acoustics
- When the player walks from the café into the apartment, the café bed crossfades out over ~1s and the apartment bed fades in

---

## Music director behavior

The system implementing this lives in [11-systems/audio-architecture.md](11-systems/audio-architecture.md).

- Music selection is driven by **scene + time-of-day band + special state**
- Crossfades take **4 seconds** between bands
- The **porch scene at sunset** is a special-case one-shot that plays uninterrupted until the day ends
- During **dialogue**, music does NOT pause; ambient beds slightly duck (-3 dB)
- During the **end-of-day cash-up**, all music fades to a brief silence, then a small piano motif plays

---

## Dialogue audio

In MVP, dialogue is **text-only with character blips**. No recorded VO.

In v1.0+:
- Optional recorded VO for the seeded NPCs (very brief — greetings, signature lines)
- Player-recorded VO never (privacy + tone risk)

---

## Music in the world (v1.0+ planning)

Once the music creation system ships, the world contains:
- **Radio stations** broadcasting player music (per region)
- **Jukeboxes** in bars and cafés (Mara could load CDs into her café's jukebox)
- **CD players** in cars
- **Live concerts** at player venues
- **Buskers** in the street

For the slice, we keep this simple:
- A small radio in the café plays one of the morning/midday/afternoon beds at low volume
- The apartment has a CD player with a single seeded canon track

---

## Voice chat (future)

Out of scope for MVP. When multiplayer arrives:
- Spatial proximity voice
- Per-region channels
- Mute/block per user
- Region-owner controls (e.g. "no voice chat in this private property")

---

## Audio production notes

- Master through a gentle bus compressor (1–2 dB max gain reduction)
- True-peak limit at -1 dBTP
- No mastering loudness war — game audio should be quieter than typical media so player can crank their system if they want
- Render at 24-bit / 44.1 kHz; export to game at 16-bit / 44.1 kHz

---

## The single test (audio)

For anything you make, ask:

> *Could this play through a small café's overhead speakers at 3pm on a Tuesday and no one would mind?*

If yes, probably right.
If no, probably wrong.


---

# System — Time and Day

## Purpose
Authoritative game-world clock. Drives NPC schedules, audio music director, lighting, save autosaves, and dialogue context.

## Scope
- **[SPINE]** required for MVP
- Single-region in MVP
- No timezone/DST simulation
- No leap years in MVP
- Calendar runs forward only

---

## Time scale

- **1 real second = 1 in-game minute** (configurable in settings)
- 1 in-game day = 24 in-game hours = 1440 in-game minutes = **24 real minutes**
- The first playable shared-world prototype uses the clock to drive citizen schedules, property/business activity, and world history. The old Café Day testbed used 06:30–18:30 as a narrow scenario, but the current milestone is broader than that single day.

### Sleep skip
At end of day, time advances to next day's 06:30 with a quick fade and ambient sleep audio. No "+8 hours of real waiting." Sleep is a hard cut.

---

## Calendar

- Date format: ISO 8601 internally (`2003-05-14`), Marennese friendly `Tuesday, 14 May 2003` in UI
- 7 days per week (Mon–Sun)
- 12 months per year
- 30 days per month (simplified — no leap, no varying month length) in MVP
  - Calendar with real month lengths is a v1.0+ enhancement

### Holidays / special days
- Out of scope for MVP
- v1.0+ adds named festival days (provincial fair, founder's day, etc.) with NPC dialogue shifts

---

## Core data types

```csharp
namespace Legacy.Time
{
    public readonly struct TimeOfDay : IEquatable<TimeOfDay>, IComparable<TimeOfDay>
    {
        public int Hour { get; }    // 0–23
        public int Minute { get; }  // 0–59

        public TimeOfDay(int hour, int minute);

        public int TotalMinutes => Hour * 60 + Minute;
        public TimeOfDay Add(int minutes);
        public override string ToString() => $"{Hour:00}:{Minute:00}";
    }

    public readonly struct GameDate : IEquatable<GameDate>
    {
        public int Year { get; }
        public int Month { get; }   // 1–12
        public int Day { get; }     // 1–30

        public string DayOfWeek { get; }  // "Monday"..."Sunday"

        public GameDate AddDays(int days);
    }

    public readonly struct GameDateTime
    {
        public GameDate Date { get; }
        public TimeOfDay Time { get; }
    }
}
```

---

## Core class

```csharp
namespace Legacy.Time
{
    public sealed class WorldClock
    {
        public GameDateTime Now { get; private set; }
        public float TimeScale { get; set; } = 60f; // in-game minutes per real second

        public event Action<TimeOfDay> MinuteTicked;
        public event Action<TimeOfDay> HourTicked;
        public event Action<GameDate> DayStarted;
        public event Action<GameDate> DayEnded;
        public event Action<TimeBand> TimeBandChanged;

        public bool IsPaused { get; set; }

        public void Tick(float deltaRealSeconds);

        public void SetTime(GameDateTime time);
        public void SkipTo(TimeOfDay target);

        public TimeBand CurrentBand { get; }
    }

    public enum TimeBand
    {
        Dawn,     // 04:00–07:00
        Morning,  // 07:00–11:00
        Midday,   // 11:00–14:00
        Afternoon,// 14:00–17:00
        Dusk,     // 17:00–19:00
        Evening,  // 19:00–22:00
        Night     // 22:00–04:00
    }
}
```

---

## Scheduler

Reads `ScheduledEvent` ScriptableObjects and dispatches via the EventBus when the clock crosses them.

```csharp
[CreateAssetMenu(menuName = "Legacy/Schedule/ScheduledEvent")]
public sealed class ScheduledEvent : ScriptableObject
{
    public TimeOfDay TriggerAt;
    public string EventKey;       // dispatched via EventBus
    public bool RepeatDaily = true;
}

public sealed class Scheduler : MonoBehaviour
{
    [SerializeField] private List<ScheduledEvent> _events;

    private void Awake() => _clock.MinuteTicked += OnMinute;

    private void OnMinute(TimeOfDay now)
    {
        foreach (var e in _events) {
            if (e.TriggerAt.Equals(now)) {
                EventBus.Publish(new ScheduledEventFired(e.EventKey));
            }
        }
    }
}
```

---

## Lighting integration

Single global gradient texture sampled by time of day:

```csharp
public sealed class TimeOfDayLighting : MonoBehaviour
{
    [SerializeField] private Light2D _sunLight;
    [SerializeField] private Gradient _sunColor;
    [SerializeField] private AnimationCurve _sunIntensity;

    private void Update()
    {
        float t = _clock.Now.Time.TotalMinutes / 1440f;
        _sunLight.color = _sunColor.Evaluate(t);
        _sunLight.intensity = _sunIntensity.Evaluate(t);
    }
}
```

Veyne_Exterior scene only; interiors use static lighting in MVP.

---

## Music director integration

The music director subscribes to `TimeBandChanged` and crossfades over 4 seconds. See [11-systems/audio-architecture.md](11-systems/audio-architecture.md).

---

## Pause behavior

- Modal screens (Inventory, Menu, Save) pause the clock (set `IsPaused = true`)
- Dialogue does NOT pause the clock — small time pressure during conversations matches the café-day pacing
- Special scripted moments (sunset porch, end-of-day fade) explicitly set the time scale and pause as needed

---

## Save integration

- `WorldClock.Now` and pause state are saved
- On load, clock resumes from the saved time

---

## UI

- HUD shows: `[Day name] [hh:mm AM/PM]` in the top-right corner
- Day counter (small "Day 1") below the clock, day 1, 2, 3... player can see how long they've been playing

---

## Testing

- Unit test `TimeOfDay.Add` with wraparound
- Unit test `GameDate.AddDays` with month/year rollover
- Integration test: scheduled event fires once per day at the right minute
- Integration test: load mid-day, verify next scheduled event still fires correctly

---

## Future considerations (V1.0+)

- Real month lengths (28/30/31 days, leap years)
- Holidays and seasonal NPC schedule overrides
- Per-region timezones (regions may be on different in-game clocks)
- Player-controllable time scale slider for cozy mode (1 real sec = 0.5 in-game min for slower play)


---

# System — NPCs

## Purpose
Bring the world to life. Each NPC has a generated personality, a schedule, dialogue, and memory of the player.

## Scope
- **[SPINE]** required for MVP
- 9 named NPCs in MVP (player family + 6 customers)
- Generated personalities, not LLM-driven
- Templated dialogue + signature lines
- Daily schedules with simple actions
- Per-NPC relationship score and memory

---

## Concepts

### NPC Archetype (template)
Authored as a ScriptableObject. Defines the *kind* of NPC — appearance, personality range, dialogue pool, default schedule.

### NPC Instance (runtime)
Spawned from an archetype. Holds runtime state — current schedule step, current mood, relationship with the player, memory, position.

The same archetype can spawn many instances (e.g. "background pedestrian" archetype spawning many randomly-named NPCs walking the street). Named MVP NPCs each have a unique 1-to-1 archetype.

---

## Archetype definition

```csharp
[CreateAssetMenu(menuName = "Legacy/NPCs/Archetype")]
public sealed class NPCArchetype : ScriptableObject
{
    public string ArchetypeId;        // "holland"
    public string DefaultName;        // "Mr. Holland"
    public int AgeRange_Min = 62;
    public int AgeRange_Max = 62;
    public Sprite Portrait;

    // Personality (rolled at spawn within these ranges)
    [Range(0, 1)] public float WarmthMin = 0.7f;
    [Range(0, 1)] public float WarmthMax = 0.9f;
    [Range(0, 1)] public float TalkativenessMin = 0.6f;
    [Range(0, 1)] public float TalkativenessMax = 0.8f;
    [Range(0, 1)] public float GenerosityMin = 0.6f;
    [Range(0, 1)] public float GenerosityMax = 0.8f;

    public List<Recipe> PreferredOrders;
    public DialogueAsset DialoguePool;
    public DialogueLine SignatureLine;
    public int SignatureThreshold = 3;

    public ScheduleAsset DefaultSchedule;
    public string CharacterSpriteId;  // resolves to sprite layers
}
```

---

## Instance state (runtime + saved)

```csharp
public sealed class NPCInstance
{
    public string ArchetypeId;        // links to archetype
    public string Id;                 // unique runtime id (often same as archetype for named NPCs)
    public string DisplayName;        // may differ for procedural NPCs

    // Rolled personality (within archetype ranges)
    public PersonalityTraits Traits;

    // Persistent state
    public int Relationship;          // 0..10 for slice
    public Memory Memory;
    public Mood CurrentMood;

    // Position
    public string CurrentScene;
    public Vector2 Position;
    public Facing Facing;

    // Schedule
    public int CurrentScheduleStepIndex;
    public bool ScheduleCompleteForDay;
}
```

```csharp
public readonly struct PersonalityTraits
{
    public float Warmth;            // 0..1
    public float Talkativeness;     // 0..1
    public float Generosity;        // 0..1
}

public enum Mood { Bright, Tired, Sad, Anxious, Content }

public enum Facing { Down, Up, Left, Right }
```

---

## Memory

Minimal in MVP. Tracks per-NPC:

```csharp
public sealed class Memory
{
    public int LastSeenDay;
    public TimeOfDay LastSeenTime;
    public float LastQualityServed;
    public string LastDialogueTopic;
    public int TotalVisits;
    public bool HasHeardSignatureLine;
    public List<string> OpenLoops;     // unresolved topics for callback
}
```

### Greeting variation buckets
Based on memory, greetings split into 3 buckets:
- **Never met** (`TotalVisits == 0`): "Hi, what can I get you?"
- **Met before** (`TotalVisits >= 1 && Relationship < 3`): "Hey, welcome back. The usual?"
- **Regular** (`Relationship >= 3`): "Morning, you." / character-specific warm greeting

---

## Schedule

```csharp
[CreateAssetMenu(menuName = "Legacy/Schedule/Schedule")]
public sealed class ScheduleAsset : ScriptableObject
{
    public List<ScheduleStep> Steps;
}

[Serializable]
public sealed class ScheduleStep
{
    public TimeOfDay StartTime;
    public ScheduleAction Action;
    public string LocationKey;          // resolves to a scene location
    public string TargetKey;            // e.g. recipe id for OrderAt
    public int DurationMinutes;
}

public enum ScheduleAction
{
    WalkTo,        // pathfind to location, stop
    Sit,           // sit at the location's seat
    OrderAt,       // order from a player-run business
    Wait,          // do nothing for DurationMinutes
    Leave          // exit the scene
}
```

### Example schedule — Mr. Holland (Tuesday)

```text
07:35  WalkTo  Cafe_Entrance
07:36  WalkTo  Cafe_Counter
07:36  OrderAt Cafe_Counter   target=coffee_black
07:42  WalkTo  Cafe_Window_Seat
07:42  Sit     Cafe_Window_Seat   duration=8min
07:50  Leave   Cafe_Entrance
```

NPCs out of their schedule windows simply don't exist in the scene (despawned from view).

---

## NPCController (MonoBehaviour)

Bridges the runtime `NPCInstance` (pure logic) to the Unity view (transform, animator, sprite renderer).

```csharp
public sealed class NPCController : MonoBehaviour
{
    [SerializeField] private NPCArchetype _archetype;

    private NPCInstance _instance;
    private CharacterMover _mover;
    private CustomerVisitState _visitState;

    private void Update()
    {
        _instance.UpdatePosition(transform.position);
        _visitState?.Tick(Time.deltaTime);
    }

    public void Interact(PlayerContext player)
    {
        if (_visitState?.AwaitingPlayer == true) {
            _visitState.OnPlayerEngaged(player);
        } else {
            // ambient greeting only
            _dialogueSystem.PlayBubble(_instance, GreetingLine());
        }
    }
}
```

---

## Pathfinding

- **A\* on the Unity Tilemap**
- Per-region walkability grid built from tilemap layers tagged `Walkable`
- NPCs claim their next tile and wait if blocked (no RVO)
- For slice scale (≤ 10 active NPCs per scene), grid A\* is plenty fast

---

## Personality → behavior mapping

These map traits to small behavioral knobs. Tuning numbers — adjust during playtest.

| Trait | Affects |
|---|---|
| **Warmth** | Greeting tone selection; tip base multiplier (× 0.9 to × 1.1); preference for short vs. extended chat |
| **Talkativeness** | Probability the "Chat?" prompt extends to 2nd exchange; ambient bubble frequency while idle |
| **Generosity** | Base tip amount (× 0.7 cold, × 1.0 average, × 1.3 generous) |
| **Mood** | Selects a slight dialogue tone shift; can override Warmth temporarily (a Warm NPC having a Sad day) |

---

## Background NPCs (out of MVP scope, planning)

For v1.0+:

- Procedurally generated NPCs spawning at scene edges
- Random first/last name from a Marennese name pool
- Trait sets rolled at spawn, persisted only while they're "named" to player
- Default schedule: walk through the scene and leave
- A small percentage become "regulars" after random number of visits — get persisted

---

## Save integration

Per-NPC saved fields (see [09-save-system.md](09-save-system.md)):

- `id`
- `displayName`
- `relationship`
- `lastSeenDay`
- `lastTopic`
- `mood`
- `metPlayer`
- `totalVisits`
- `hasHeardSignatureLine`

Archetype data is not saved — it's authored content.

---

## Spawning

- At scene load, spawn all NPCs whose schedule starts within the loaded time window
- At schedule trigger, NPC fades in at scene edge (or wakes from idle if pre-spawned at home)
- After `Leave`, NPC fades out at scene edge

---

## Testing

- Unit: personality trait rolling stays within archetype ranges
- Unit: schedule step progression with time advance
- Integration: load scene at 07:30 → Mr. Holland visible at 07:35
- Integration: save mid-visit, load, NPC resumes correct schedule step
- Integration: serve customer at quality 100 → tip in expected range


---

# System — Dialogue

## Purpose
Drive all in-game spoken/written text between characters. Includes ambient bubbles, interactive dialogue boxes, NPC inner monologues (rare), and signature memory lines.

## Scope
- **[SPINE]** required for MVP
- Templated dialogue with conditional branching
- No LLM-driven generation
- Voice acting is *not* in MVP — text + character blips only

---

## Concepts

### Two surfaces
- **Bubble:** floating text above a character; ambient or single-line interactions. Auto-dismisses.
- **Dialogue Box:** bottom-of-screen panel; interactive multi-line exchanges with player input.

### Composition
- A **DialogueAsset** is a *pool* of `DialogueLine`s with their conditions
- A **DialogueLine** is a single utterance with optional responses

---

## Data types

```csharp
[CreateAssetMenu(menuName = "Legacy/Dialogue/Asset")]
public sealed class DialogueAsset : ScriptableObject
{
    public string AssetId;          // "holland_morning_pool"
    public List<DialogueLine> Lines;
}

[Serializable]
public sealed class DialogueLine
{
    public string Id;               // "holland_001"
    public string SpeakerArchetypeId;
    public LocalizedString Text;
    public List<DialogueResponse> Responses;
    public List<DialogueCondition> Conditions;
    public bool IsSignature = false;
    public string Tag;              // "greeting", "smalltalk", "memorable"
}

[Serializable]
public sealed class DialogueResponse
{
    public LocalizedString Text;
    public string NextLineId;       // optional
    public int RelationshipDelta;
}

[Serializable]
public sealed class DialogueCondition
{
    public ConditionKind Kind;
    public string Key;              // e.g. "relationship", "totalVisits"
    public ComparisonOp Op;
    public float Value;
}
```

---

## Selecting a line

Given an `NPCInstance` and a `DialogueAsset`, pick the most-specific line whose conditions are satisfied.

Selection rules:
1. Filter all lines whose conditions are satisfied by current `NPCContext`
2. From those, prefer lines with more conditions (more specific wins)
3. Tie-break by line `Tag` matching the current situation (greeting / smalltalk / payment)
4. Randomize among remaining ties (with last-line-spoken filter to avoid immediate repeats)
5. If the line is `IsSignature`, only fire once per NPC and mark `Memory.HasHeardSignatureLine = true`

---

## Conditions (MVP set)

| Condition | Example |
|---|---|
| `relationship >= 3` | Signature line gate |
| `totalVisits == 0` | First-time greeting |
| `totalVisits >= 1 && relationship < 3` | "Met before" greeting |
| `mood == Sad` | Sad-day dialogue variant |
| `currentDay >= 3 && lastSeenDay < currentDay - 3` | "Long time no see" line |
| `playerQualityLastServed >= 80` | Praise dialogue |
| `playerQualityLastServed < 30` | Disappointed dialogue |
| `currentTimeBand == Morning` | Morning-specific line |

Conditions are evaluated at line-pick time, not authored once.

---

## NPCContext

Passed into the dialogue selector. Built from the NPC instance + world state.

```csharp
public sealed class NPCContext
{
    public NPCInstance Npc;
    public int Relationship;
    public int TotalVisits;
    public Mood Mood;
    public TimeBand TimeBand;
    public float LastQualityServed;
    public bool HasHeardSignatureLine;
    public string LastDialogueTopic;
}
```

---

## DialogueSystem

```csharp
public sealed class DialogueSystem
{
    public DialogueLine SelectLine(DialogueAsset asset, NPCContext context, string tag = null);

    public IDialogueSession StartSession(DialogueAsset asset, NPCContext context);
}

public interface IDialogueSession
{
    DialogueLine CurrentLine { get; }
    DialogueResponse[] Responses { get; }
    bool IsFinished { get; }

    void Choose(int responseIndex);
    void Advance();   // for lines with no responses; advances to next or ends
    void Cancel();
}
```

---

## Dialogue UI

### Bubble UI
- Floating sprite-frame above the character's head
- Tail points to the character
- Fades in over 0.15s; persists 1.5s + 0.05s per character of text; fades out over 0.15s
- Pixel font; max 2 lines of ~20 characters
- Character blip SFX per ~3 letters (per character archetype)

### Dialogue Box UI
- Bottom of screen, pixel-frame panel
- Speaker name + portrait on the left
- Text appears typed-out at ~30 chars/second
- Press `[Space]` to skip to full text; press again to advance
- If responses available, list 1–4 options bound to `[1]–[4]` keys
- World does NOT pause; ambient beds duck slightly (-3 dB)

---

## Localization

All `LocalizedString` fields use Unity Localization tables. Each line has a stable Key. English-only at launch; keys allow later translation.

---

## Authoring workflow

1. Designer/writer creates a new `DialogueAsset` in `Assets/_Project/Data/Dialogue/`
2. Names it descriptively: `holland_morning_pool`
3. Adds lines, each with:
 - Speaker archetype id
 - Text
 - Optional conditions (small dropdown UI in inspector)
 - Optional responses
 - Optional `IsSignature` flag
4. Assigns the asset to the NPC archetype's `DialoguePool`
5. Playtest: walk into the café before 7am, after 7am, before relationship 3, after relationship 3

---

## Sample asset (excerpt — Mr. Holland)

```csharp
// asset: holland_morning_pool
Lines = [
    {
        Id: "holland_greet_first",
        SpeakerArchetypeId: "holland",
        Text: "Morning. Don't think I've had the pleasure. ...Black coffee, when you can.",
        Conditions: [ totalVisits == 0 ],
        Tag: "greeting"
    },
    {
        Id: "holland_greet_known",
        SpeakerArchetypeId: "holland",
        Text: "Morning, you. Black coffee, the usual.",
        Conditions: [ totalVisits >= 1 ],
        Tag: "greeting"
    },
    {
        Id: "holland_smalltalk_june",
        SpeakerArchetypeId: "holland",
        Text: "Saw your kid run past. ...She's getting tall.",
        Conditions: [ relationship >= 2 ],
        Tag: "smalltalk"
    },
    {
        Id: "holland_signature",
        SpeakerArchetypeId: "holland",
        Text: "You know, I taught for forty years in this town. Never had a worse student than this one kid in '78. Wonder where he ended up. Probably running for mayor.",
        Conditions: [ relationship >= 3, hasHeardSignatureLine == false ],
        Tag: "memorable",
        IsSignature: true
    }
]
```

---

## Dialogue blips (audio)

- Per character archetype, a short ~80ms blip sample
- Plays per ~3 displayed characters in the typed-out text
- Pitched slightly to match age/voice (Mara medium, Mr. Holland low, June higher, Sasha medium-low, Pell low, etc.)
- See [11-systems/audio-architecture.md](11-systems/audio-architecture.md) for the audio side

---

## Testing

- Unit: condition evaluation for each operator
- Unit: line selection respects "more specific wins"
- Unit: signature line fires once per NPC then never again
- Integration: walk up to Mr. Holland on day 1 → greeting matches `totalVisits == 0`
- Integration: walk up to Mr. Holland on day 4 with relationship 4 → signature line plays once
- Integration: save mid-conversation, load, session resumes correctly

---

## Future considerations (V1.0+)

- Branching conversations longer than 2 exchanges
- Multi-NPC group conversations (cafeteria scene)
- Dynamic interjection (a third NPC overhears and reacts)
- Player text input for limited free-form (writing a note, leaving a tip jar message)
- Optional voice acting for seeded NPCs
- Player-created dialogue (in-game NPC scripting? Out of scope for years.)


---

# System — Interaction

## Purpose
Provide the unified pattern for "the player approaches a thing and does something to it."

## Scope
- **[SPINE]** required for MVP
- Player → NPC (talk)
- Player → object (use, open, read, take)
- Player → station (start mini-game)
- Player → door (transition scene)

---

## Pattern

### Interface

```csharp
namespace Legacy.Interaction
{
    public interface IInteractable
    {
        InteractionPriority Priority { get; }   // for tie-breaking when multiple are in range
        string PromptVerb { get; }              // "Talk", "Open", "Take", "Read", "Use"
        string PromptObject { get; }            // "Mr. Holland", "Door", "Letter"

        bool CanInteract(PlayerContext player);
        void Interact(PlayerContext player);
    }

    public enum InteractionPriority
    {
        Low,        // ambient interactables (paintings)
        Normal,     // standard (NPCs, doors)
        High        // active context (the customer currently waiting at your counter)
    }
}
```

### Behaviour

```csharp
public sealed class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float _searchRadius = 1.2f;
    [SerializeField] private LayerMask _interactableLayer;

    private IInteractable _currentTarget;

    private void Update()
    {
        _currentTarget = FindBestInRange();
        _promptUI.SetTarget(_currentTarget);
    }

    public void OnInteractPressed()
    {
        if (_currentTarget != null && _currentTarget.CanInteract(_player)) {
            _currentTarget.Interact(_player);
        }
    }

    private IInteractable FindBestInRange()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, _searchRadius, _interactableLayer);
        IInteractable best = null;
        InteractionPriority bestPrio = InteractionPriority.Low;
        float bestDist = float.MaxValue;

        foreach (var hit in hits) {
            var inter = hit.GetComponent<IInteractable>();
            if (inter == null || !inter.CanInteract(_player)) continue;

            // higher priority wins; ties broken by distance
            if (inter.Priority > bestPrio ||
               (inter.Priority == bestPrio && Vector2.Distance(transform.position, hit.transform.position) < bestDist))
            {
                best = inter;
                bestPrio = inter.Priority;
                bestDist = Vector2.Distance(transform.position, hit.transform.position);
            }
        }

        return best;
    }
}
```

---

## Prompt UI

- Small floating label above the interactable
- Format: `[E] Talk to Mr. Holland`
- Hotkey is the configured `Interact` binding (defaults: `E` on keyboard, `A`/south button on gamepad)
- Appears when interactable comes into range; disappears when leaves
- Uses pixel-frame style, see [11-systems/ui-architecture.md](11-systems/ui-architecture.md)

---

## Examples

### NPC

```csharp
public sealed class NPCController : MonoBehaviour, IInteractable
{
    public InteractionPriority Priority { get; private set; } = InteractionPriority.Normal;
    public string PromptVerb => "Talk to";
    public string PromptObject => _instance.DisplayName;

    public bool CanInteract(PlayerContext player) => !_busy;

    public void Interact(PlayerContext player)
    {
        if (_visitState?.AwaitingPlayer == true) {
            _visitState.OnPlayerEngaged(player);
        } else {
            _dialogueSystem.PlayBubble(_instance, GreetingLine());
        }
    }
}
```

### Door (scene transition)

```csharp
public sealed class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string _targetSceneKey;
    [SerializeField] private string _spawnPointId;

    public InteractionPriority Priority => InteractionPriority.Normal;
    public string PromptVerb => "Open";
    public string PromptObject => "Door";

    public bool CanInteract(PlayerContext player) => !_locked;

    public void Interact(PlayerContext player)
    {
        _audio.Play("sfx_door_open");
        _sceneTransition.LoadAsync(_targetSceneKey, _spawnPointId).Forget();
    }
}
```

### Espresso machine (prep station)

```csharp
public sealed class PrepStation : MonoBehaviour, IInteractable
{
    [SerializeField] private RecipeStepKind _kind;

    public InteractionPriority Priority => _shift.HasPendingRecipeStep(_kind)
        ? InteractionPriority.High
        : InteractionPriority.Low;

    public string PromptVerb => "Use";
    public string PromptObject => _kind.ToString();

    public bool CanInteract(PlayerContext player) => _shift.HasPendingRecipeStep(_kind);

    public void Interact(PlayerContext player)
    {
        _shift.BeginStep(_kind, player);
    }
}
```

### Counter (give item to customer)

```csharp
public sealed class CafeCounter : MonoBehaviour, IInteractable
{
    public InteractionPriority Priority => _currentCustomer != null
        ? InteractionPriority.High
        : InteractionPriority.Normal;

    public string PromptVerb => _currentCustomer != null ? "Serve" : "Stand at";
    public string PromptObject => "Counter";

    public bool CanInteract(PlayerContext player) => true;

    public void Interact(PlayerContext player)
    {
        if (_currentCustomer != null && player.HoldingItem != null) {
            _currentCustomer.Receive(player.HoldingItem);
            player.Drop();
        }
    }
}
```

---

## What is NOT an interactable

- Tilemap (the world itself; the player walks on it but doesn't "interact")
- Cosmetic props with no behavior (paintings, plants — though they may become interactable in v1.0+)
- Other players (in MVP — out of scope)

---

## Testing

- Unit: priority tie-breaking by distance
- Unit: `CanInteract` returning false suppresses the prompt
- Integration: walk up to NPC, prompt appears; press `E`, dialogue starts
- Integration: door transitions player to correct scene at correct spawn point
- Integration: customer at counter takes priority over generic counter prompt

---

## Future considerations (V1.0+)

- Context-sensitive verbs: `Open` for unlocked doors, `Knock` for locked
- Hold-to-interact for long actions (clean tables, restock)
- Carry-and-deliver mechanics (carry pastry to specific table)
- Multi-target interactions (combine two interactables: pour coffee from pot into cup)


---

# System — Inventory

## Purpose
Manage items the player and the world contain. Players carry a small inventory; containers (café storage, fridge, register, briefcase) hold their own inventories.

## Scope
- **[SPINE]** required for MVP
- Slot-based player inventory
- Container inventories for storage
- Items as ScriptableObject definitions
- No weight/encumbrance in MVP

---

## Concepts

### ItemDefinition
A ScriptableObject describing a *kind* of item — its name, sprite, stackability, max stack size, category.

### ItemStack
A runtime pair of (`ItemDefinition def`, `int count`).

### Inventory
A collection of `ItemStack` slots with a capacity.

---

## Data types

```csharp
[CreateAssetMenu(menuName = "Legacy/Inventory/Item")]
public sealed class ItemDefinition : ScriptableObject
{
    public string ItemId;            // "coffee_beans"
    public LocalizedString DisplayName;
    public Sprite Icon;
    public ItemCategory Category;
    public bool Stackable = true;
    public int MaxStack = 99;
    public LocalizedString Description;
    public float UnitPrice;          // base market price
}

public enum ItemCategory
{
    Ingredient,
    Tool,
    Document,
    Key,
    Money,           // (denominations stored separately, but unified pattern)
    Personal,
    Container,
    Other
}

[Serializable]
public sealed class ItemStack
{
    public ItemDefinition Def;
    public int Count;

    public bool IsEmpty => Count <= 0 || Def == null;
}
```

```csharp
public sealed class Inventory
{
    public int Capacity { get; }
    public IReadOnlyList<ItemStack> Slots { get; }

    public event Action<int> SlotChanged;     // emits slot index

    public Inventory(int capacity);

    public bool TryAdd(ItemDefinition item, int count, out int leftover);
    public bool TryRemove(ItemDefinition item, int count);
    public int CountOf(ItemDefinition item);
    public void Clear();

    public ItemStack StackAt(int index);
    public bool SwapSlots(int a, int b);
}
```

---

## Player inventory (MVP)

- **12 slots** for player carry
- Items used in slice:
 - Wallet (special; doesn't take a slot)
 - Recipe card (1 slot)
 - Café key (1 slot)
 - Notebook (1 slot, future use)
 - Carried item (currently-held food/drink to deliver) — uses a dedicated "hand" slot, not a backpack slot

### Hand slot
- Single dedicated slot for "what the player is currently carrying with both hands"
- Mutually exclusive with backpack
- Used in the café for carrying prepared items from prep station to customer

```csharp
public sealed class PlayerCarry
{
    public ItemStack HandStack { get; private set; }
    public bool HasHand => HandStack != null && !HandStack.IsEmpty;

    public bool TryPickUp(ItemStack stack);
    public ItemStack Drop();
}
```

---

## Container inventories (MVP)

| Container | Capacity | Purpose |
|---|---|---|
| Café storage (behind counter) | 24 slots | Ingredients (beans, milk, tea, pastries) |
| Café register (till) | special | Currency, not inventory items |
| Apartment fridge | 12 slots | Personal food |
| Apartment closet | 8 slots | Personal items |

Containers expose the same `Inventory` API. Player accesses them via interaction (open container → inventory swap UI).

---

## Recipe ingredient consumption

When player begins a recipe:
- Look up `Recipe.Ingredients[]` (ItemDef + count)
- Pull from café storage first; if missing, the recipe cannot start
- UI surfaces "out of stock" if ingredient missing

```csharp
public bool TryStartRecipe(Recipe recipe)
{
    foreach (var ingredient in recipe.Ingredients) {
        if (_storage.CountOf(ingredient.Def) < ingredient.Count) {
            _ui.Show($"Out of {ingredient.Def.DisplayName}.");
            return false;
        }
    }

    foreach (var ingredient in recipe.Ingredients) {
        _storage.TryRemove(ingredient.Def, ingredient.Count);
    }

    return true;
}
```

---

## Inventory UI

Modal screen. Opens on `[Tab]` (configurable). Pauses the clock per pause behavior.

### Layout
- Left: player inventory grid (4 × 3)
- Right: (when interacting with a container) container grid
- Bottom: item details panel showing icon, name, description, count
- Top: title, close button

### Interactions
- Click slot → select
- Click second slot → swap
- Right-click → context menu (Use, Drop, Inspect)
- Drag-drop → swap

---

## Item categories specific behaviors

| Category | Default behavior on Use |
|---|---|
| Ingredient | Pulled by recipes; can't be "used" alone |
| Tool | Activate context (cleaning rag → clean tables) |
| Document | Open as text (notebook, recipe card → opens reader UI) |
| Key | Opens specific door if held |
| Money | Not held in inventory; in Wallet (see [11-systems/currency.md](11-systems/currency.md)) |
| Personal | Display-only (souvenirs, gifts, mementos) |
| Container | Open to show its inventory (briefcase) |

---

## Save integration

Each container saved as a list of `(itemId, count)` per slot:

```jsonc
"player": {
  "inventory": [
    { "slot": 0, "itemId": "recipe_card", "count": 1 },
    { "slot": 1, "itemId": "cafe_key", "count": 1 }
  ],
  "hand": null
},
"cafe": {
  "storage": [
    { "slot": 0, "itemId": "coffee_beans", "count": 100 },
    { "slot": 1, "itemId": "milk", "count": 20 },
    { "slot": 2, "itemId": "pastry_croissant", "count": 4 },
    { "slot": 3, "itemId": "pastry_muffin", "count": 4 },
    { "slot": 4, "itemId": "tea_black", "count": 30 }
  ]
}
```

---

## Testing

- Unit: `TryAdd` respects capacity and stacking
- Unit: `TryRemove` correctly reduces or removes empty slots
- Unit: `SwapSlots` is symmetric
- Integration: start a recipe → ingredients consumed
- Integration: out-of-stock recipe → start fails, UI shows correct message
- Integration: save → load → all slots preserved

---

## Future considerations (V1.0+)

- Weight / encumbrance for travel
- Container hierarchies (box inside cabinet)
- Hot-bar for quick access during work
- Drag-drop crafting (drag ingredient onto station)
- Stash sharing in households
- Bank-deposit boxes


---

# System — Currency

## Purpose
Cash economy for the slice. The Marennese dollar ($). Players take cash, give change, count their till at end of day.

## Scope
- **[SPINE]** required for MVP
- Cash only — no banks, no cards, no digital payment
- Multiple denominations (bills + coins)
- Player wallet + café till
- Manual change-making interaction

---

## Denominations

| Denomination | Type | Image notes |
|---|---|---|
| $0.05 | Coin | Nickel-style |
| $0.10 | Coin | Dime-style |
| $0.25 | Coin | Quarter-style |
| $1.00 | Coin | Large coin (or smallest bill — choose during art pass) |
| $1 | Bill | Smallest bill |
| $5 | Bill | |
| $10 | Bill | |
| $20 | Bill | |
| $50 | Bill | |
| $100 | Bill | Largest bill |

Decision: keep both $1 coin and $1 bill in the data layer; visually the slice can ship one or the other. Art pass decides.

---

## Data types

```csharp
public enum Denomination
{
    Coin_05,    // $0.05
    Coin_10,    // $0.10
    Coin_25,    // $0.25
    Coin_100,   // $1.00
    Bill_1,
    Bill_5,
    Bill_10,
    Bill_20,
    Bill_50,
    Bill_100
}

public static class DenominationValues
{
    public static decimal ValueOf(Denomination d) => d switch
    {
        Denomination.Coin_05  => 0.05m,
        Denomination.Coin_10  => 0.10m,
        Denomination.Coin_25  => 0.25m,
        Denomination.Coin_100 => 1.00m,
        Denomination.Bill_1   => 1.00m,
        Denomination.Bill_5   => 5.00m,
        Denomination.Bill_10  => 10.00m,
        Denomination.Bill_20  => 20.00m,
        Denomination.Bill_50  => 50.00m,
        Denomination.Bill_100 => 100.00m,
        _ => 0m
    };
}
```

```csharp
public sealed class Wallet
{
    private readonly Dictionary<Denomination, int> _counts = new();

    public event Action Changed;

    public decimal Total => _counts.Sum(kvp => DenominationValues.ValueOf(kvp.Key) * kvp.Value);

    public int CountOf(Denomination d);
    public void Add(Denomination d, int count);
    public bool TryRemove(Denomination d, int count);
    public bool TryRemoveAmount(decimal amount, ChangeStrategy strategy = ChangeStrategy.MinCoins);
    public IReadOnlyDictionary<Denomination, int> Snapshot();
}

public enum ChangeStrategy
{
    MinCoins,    // greedy — fewest physical pieces
    MaxCoins,    // pay everything in small (for splitting)
    Specific     // use a provided set
}
```

---

## Use of `decimal`

All money math uses `decimal`, never `float`. Reasons:
- No floating-point rounding errors
- Exact representation of $0.05 increments
- Matches accounting expectations

---

## Change-making algorithm

Greedy works for Marennese denominations because they are canonical (each larger denomination is a clean multiple of smaller ones).

```csharp
public static bool TryMakeChange(IReadOnlyDictionary<Denomination, int> available,
                                  decimal amount,
                                  out Dictionary<Denomination, int> result)
{
    result = new();
    var denoms = Enum.GetValues<Denomination>()
                     .OrderByDescending(DenominationValues.ValueOf)
                     .ToArray();

    decimal remaining = amount;
    foreach (var d in denoms) {
        var value = DenominationValues.ValueOf(d);
        int have = available.GetValueOrDefault(d, 0);
        int need = (int)(remaining / value);
        int take = Math.Min(have, need);
        if (take > 0) {
            result[d] = take;
            remaining -= take * value;
        }
    }

    return remaining == 0m;
}
```

If greedy fails (not enough of a denomination), the player must use a different mix — surfaced in the UI as "Can't make exact change."

---

## Payment flow at the café

1. Customer presents an order
2. Player serves item (mini-game produces a quality score and final price)
3. Customer drops a *single* bill or coin set on the counter (procedurally chosen — biased toward "convenient bill above the price")
4. Cash drawer UI opens
5. Player selects denominations to make change
6. Player presses "Confirm" — change goes to customer, payment goes to café till
7. Customer drops tip into jar (based on quality + relationship)
8. Customer leaves

### Wrong change handling
- If player gives short change: customer takes the short amount, tip reduced, relationship -1
- If player overpays: customer keeps it ("appreciated"), wallet drains, small relationship +
- Cancel button always available — payment reverts

---

## Cash drawer UI

Modal that appears on payment. World does not advance time during this UI (pauses for clarity).

```
+------------------------------------+
| Total due:     $4.25               |
| Customer paid: $5.00 (Bill_5)      |
| You owe back:  $0.75               |
|                                    |
| [Coin_05] x 0  [-] [+]             |
| [Coin_10] x 0  [-] [+]             |
| [Coin_25] x 3  [-] [+]             |
| [Coin_100] x 0 [-] [+]             |
| [Bill_1]  x 0  [-] [+]             |
| ...                                |
|                                    |
| Selected change: $0.75 (3 coins)   |
| [Confirm] [Cancel]                 |
+------------------------------------+
```

Auto-fill button: "Make Change (Optimal)" — runs greedy on player's wallet.

---

## Café till

Separate `Wallet` instance held by `CafeShift`. Receives payment (in source denominations from customer), gives out change (from till).

End of day:
- Player closes shop
- "Count till" interaction at register: opens till UI showing current total
- Total is recorded as "tillToday"; "tillLifetime" increments

---

## Wallet HUD

- Small icon + total amount in the HUD (top-right under clock)
- Click expands to show denominations (optional in MVP — could be inventory-screen only)

---

## Pricing

For MVP all prices are fixed in `Recipe.BasePrice` ScriptableObject field. No dynamic supply/demand yet. Tip computed per [05-vertical-slice-cafe-day.md](05-vertical-slice-cafe-day.md).

---

## Save integration

```jsonc
"player": {
  "wallet": {
    "Coin_05": 4,
    "Coin_10": 2,
    "Coin_25": 7,
    "Coin_100": 3,
    "Bill_1": 5,
    "Bill_5": 3,
    "Bill_10": 1,
    "Bill_20": 2,
    "Bill_50": 0,
    "Bill_100": 0
  }
},
"cafe": {
  "till": {
    "Coin_05": 20,
    "Coin_25": 10,
    "Bill_1": 30,
    "Bill_5": 10,
    "Bill_20": 2
  },
  "tillToday": 42.50,
  "tillLifetime": 42.50
}
```

---

## Testing

- Unit: `TryMakeChange` for canonical denominations always succeeds when total available ≥ amount
- Unit: `TryMakeChange` fails cleanly when no combination works
- Unit: `Total` matches sum of denominations
- Integration: payment flow start to end debits/credits correctly
- Integration: customer overpaying does not bug out
- Integration: save → load preserves both wallet and till

---

## Future considerations (V1.0+)

- Bank accounts (deposit till at end of day)
- Cheques (rare in MVP era; more relevant in v1.0)
- Credit cards (slightly anachronistic at 2003 small-town café but plausible later)
- Loans and debt
- Income tax (filed annually in-game; player makes a return)
- Foreign currency (other regions/countries)


---

# System — Café Work Loop

## Purpose
The core MVP gameplay: working a shift at Linden Café. Take orders, make drinks and pastries via mini-games, serve, handle payment, optionally chat.

## Scope
- **[SPINE]** required for MVP
- Single café in MVP (the player's)
- Recipes: black coffee, espresso, cappuccino, latte, tea, croissant, muffin (~6–8 items)
- Mini-games: grind, tamp, pull shot, pour milk, oven bake, tea steep
- Customer visit state machine

---

## Open/close lifecycle

```
Closed → OpenAttempt → Open → CloseAttempt → Closing → Closed
```

| Action | Trigger |
|---|---|
| Open | Player interacts with door (interior side) → "Open shop" verb |
| Close | Player interacts with door (interior side) → "Close shop" verb |

While Closed, no NPC customers enter. While Open, scheduled customers may arrive within their windows.

---

## Recipe

```csharp
[CreateAssetMenu(menuName = "Legacy/Recipes/Recipe")]
public sealed class Recipe : ScriptableObject
{
    public string RecipeId;
    public LocalizedString DisplayName;
    public Sprite Icon;
    public decimal BasePrice;
    public List<IngredientRequirement> Ingredients;
    public List<RecipeStep> Steps;
    public int EstimatedSecondsToPrep;
}

[Serializable]
public sealed class IngredientRequirement
{
    public ItemDefinition Def;
    public int Count;
}

[Serializable]
public sealed class RecipeStep
{
    public RecipeStepKind Kind;
    public float StepWeight;  // 0–1 contribution to total quality
}

public enum RecipeStepKind
{
    Grind,
    Tamp,
    PullShot,
    PourMilk,
    OvenIn,
    OvenOut,
    Steep,
    Plate
}
```

### MVP recipes

| Recipe | Ingredients | Steps | Base Price |
|---|---|---|---|
| Black coffee | beans×1 | Grind → PullShot | $2.50 |
| Espresso | beans×1 | Grind → Tamp → PullShot | $2.75 |
| Cappuccino | beans×1, milk×1 | Grind → Tamp → PullShot → PourMilk | $3.50 |
| Latte | beans×1, milk×1 | Grind → Tamp → PullShot → PourMilk | $3.75 |
| Tea (black) | tea×1 | Steep | $2.25 |
| Croissant | pastry_croissant×1 | OvenIn → OvenOut (warmed) | $2.50 |
| Muffin | pastry_muffin×1 | OvenIn → OvenOut (warmed) | $2.75 |

---

## Customer visit state machine

```
Enter → Approach → Order → AwaitPrep → Receive → Pay → Chat? → Leave
```

```csharp
public enum CustomerVisitStateKind
{
    Enter,
    Approach,
    Order,
    AwaitPrep,
    Receive,
    Pay,
    Chat,
    Leave
}

public sealed class CustomerVisitState
{
    public CustomerVisitStateKind Current { get; private set; }
    public NPCInstance Customer { get; }
    public Recipe Order { get; private set; }
    public float TimeInState { get; private set; }
    public float QualityScore { get; private set; }
    public bool AwaitingPlayer => Current is CustomerVisitStateKind.Order
        or CustomerVisitStateKind.Receive
        or CustomerVisitStateKind.Pay
        or CustomerVisitStateKind.Chat;

    public void Tick(float dt);
    public void OnPlayerEngaged(PlayerContext player);
    public void OnRecipeCompleted(float quality);
    public void OnChangeGiven(decimal amount);
    public void OnPlayerChatted();
    public void OnPlayerDeclinedChat();
}
```

### Timeouts (NPC impatience)

| State | Timeout | If hit |
|---|---|---|
| Order | 60s | NPC says "Maybe another time" and leaves; relationship -1 |
| AwaitPrep | recipe base × 3 | NPC leaves; pays half price; relationship -1 |
| Receive | 30s | NPC walks to counter, takes item; -5 quality |
| Pay | 60s | Treated as customer overpaid (takes no change); customer cool |
| Chat | 20s | Default to "no chat"; no penalty |

---

## Prep stations

Each kind of recipe step is performed at a specific station:

| Station | Steps it handles |
|---|---|
| Grinder | Grind |
| Espresso machine | Tamp, PullShot, PourMilk |
| Oven | OvenIn, OvenOut |
| Kettle / steeping pot | Steep |
| Counter | Plate (final assembly) |

Stations are `IInteractable` (see [11-systems/interaction.md](11-systems/interaction.md)). Their `Priority` becomes `High` when the active recipe has a pending step that matches.

---

## Mini-games

See [05-vertical-slice-cafe-day.md](05-vertical-slice-cafe-day.md) for the exact numbers per mini-game.

Common contract:

```csharp
public interface IMiniGame
{
    void Begin();
    void Tick(float dt);
    void OnInput(MiniGameInput input);
    bool IsComplete { get; }
    int Score { get; }       // 0..MaxScore for that step
    int MaxScore { get; }
}
```

Each mini-game has its own UI overlay and its own audio. Mini-games run with the world clock un-paused (small time pressure for the busy rush) but inputs other than the mini-game are disabled.

### Quality aggregation

```csharp
float totalQuality = 0;
foreach (var step in recipe.Steps) {
    var stepQuality = miniGames[step.Kind].Score / (float)miniGames[step.Kind].MaxScore;
    totalQuality += stepQuality * step.StepWeight;
}
totalQuality = Mathf.Clamp01(totalQuality) * 100f;  // 0..100
```

---

## Pricing and tip math (recap)

See [11-systems/currency.md](11-systems/currency.md) for the math and the cash-drawer UI. Tip computed per [05-vertical-slice-cafe-day.md](05-vertical-slice-cafe-day.md):

```
finalTip = baseTip × (0.5 + (quality / 100) × 1.5) × (1.0 + (relationship / 10) × 0.5)
```

Rounded to nearest $0.05.

---

## Chat after pay

After the change is given, the "Chat?" prompt appears if the NPC's `Talkativeness` roll allows it (50% chance baseline at trait 0.5, scaled by trait).

If accepted:
- 1–2 dialogue exchanges from the NPC's `DialoguePool`
- Adds +1 relationship
- At relationship threshold, signature line fires

If declined:
- NPC nods, says farewell, leaves

---

## End of shift

Player interacts with door from interior side → "Close shop"
- OPEN sign flips to CLOSED
- Door visually locks
- Music shifts to "evening" band
- Any NPC currently in the café finishes their visit and leaves; no new NPCs arrive

Then player interacts with the register → "Count till"
- Modal shows day's earnings, tips, total, comparison to previous day
- Press confirm to log the day in history and proceed

---

## History log entries written by the café loop

| Event | Logged as |
|---|---|
| Customer served | `kind: CustomerServed`, `actor: <customerId>`, `details: { recipe, quality, tip }` |
| Signature line heard | `kind: NPCMemoryUnlocked`, `actor: <customerId>` |
| Day's total | `kind: DayClosed`, `details: { total, customers, tips }` |

---

## Save integration

```jsonc
"cafe": {
  "isOpen": false,
  "currentCustomerId": null,
  "currentRecipeId": null,
  "tillToday": 42.50,
  "tillLifetime": 42.50,
  "shiftsCompleted": 1,
  "storage": [ ... ],
  "till": { ... }
}
```

The current visit state is *not* saved across sessions — if the player saves mid-visit, the visit is cancelled on load (NPC leaves the scene cleanly).

---

## Testing

- Unit: quality aggregation across all step kinds
- Unit: tip math at boundary cases (quality 0, 100; rel 0, 10)
- Integration: full visit from enter → leave with high quality → tip in expected range
- Integration: timeout on AwaitPrep triggers NPC departure
- Integration: signature line fires at relationship threshold, not before, not twice
- Integration: close shop with NPC inside → NPC finishes and leaves

---

## Future considerations (V1.0+)

- Employees (NPC and player) running stations in parallel
- Menu customization (player chooses what to sell)
- Pricing control (player sets price; affects volume and tip mix)
- Inventory management (auto-ordering, supplier contracts)
- Health inspectors (rare, dramatic event)
- Reviews on the in-game forum (M5) affecting customer volume
- Multiple café locations (chains)


---

# System — Modular Building Kit

## Purpose
Assemble buildings (interiors and exteriors) from snap-together parts instead of authoring every building bespoke. Essential for scaling content beyond the slice.

## Scope
- **[SPINE]** required for MVP
- MVP scope: two buildings (Café and Apartment) assembled with the kit
- Authoring tool used to produce final prefabs
- Tool itself is minimal in MVP — its real value is from Slice 2 onward

---

## Why

Hand-authoring every building in pixels does not scale. *Stardew Valley* is one valley with maybe 30 buildings authored over years by one person. *legacy* aims at dozens of buildings in a single town and many towns over time. A modular kit collapses authoring cost from "draw every building" to "drop pieces, save prefab."

This is also how *Project Zomboid*, *RimWorld*, and *Don't Starve* ship enormous worlds with small art teams.

---

## Concepts

### Building part
A reusable 32×32 piece authored in Aseprite — wall segments, floors, doors, windows, roofs, trim.

### Building kit
A categorized collection of parts that share a visual style — e.g. "Brick storefront" kit contains red-brick walls, white-trim windows, glass-front doors, awnings.

### Building prefab
The result of composing parts into a unit (a single café, a single apartment). Stored as a Unity Prefab and instanced into scenes.

---

## Part categories (MVP)

| Category | Examples |
|---|---|
| Floor | wood plank, tile small, tile hex, concrete, carpet — per kit |
| Wall (exterior) | brick straight, brick corner NE/NW/SE/SW, brick T-junction, brick end |
| Wall (interior) | plaster straight + corners, painted variant, half-wall |
| Door (exterior) | shop glass, residential wood, double door |
| Door (interior) | wood panel, swinging kitchen door |
| Window | small, large, shop-front, sash |
| Roof | gable, flat, slate, shingle, awning |
| Trim | baseboard, ceiling molding, window casing |
| Stairs | up, down, landing |
| Fixed prop | counter, shelf, sign mount |

Furniture and decoration are *not* part of the kit. They are placed separately as props (see [10-asset-pipeline.md](10-asset-pipeline.md)).

---

## Data types

```csharp
[CreateAssetMenu(menuName = "Legacy/Building/Part")]
public sealed class BuildingPart : ScriptableObject
{
    public string PartId;
    public BuildingPartCategory Category;
    public BuildingKit Kit;        // which visual kit it belongs to
    public Sprite Sprite;
    public Vector2Int TileSize = new(1, 1);
    public Vector2Int PivotTile = new(0, 0);
    public BuildingPartFlags Flags;
}

public enum BuildingPartCategory
{
    FloorTile,
    WallSegment,
    Door,
    Window,
    Roof,
    Trim,
    Stairs,
    FixedProp
}

[Flags]
public enum BuildingPartFlags
{
    None = 0,
    Walkable = 1 << 0,
    BlocksLight = 1 << 1,
    SnapsToWall = 1 << 2,
    SnapsToFloor = 1 << 3,
    HasCollider = 1 << 4
}

[CreateAssetMenu(menuName = "Legacy/Building/Kit")]
public sealed class BuildingKit : ScriptableObject
{
    public string KitId;        // "brick_storefront", "wood_apartment"
    public string DisplayName;
    public List<BuildingPart> Parts;
}
```

---

## Authoring tool — Building Kit Composer

A custom Unity editor window for placing parts on a grid.

### Behaviour
- Grid view (32×32 snap)
- Palette of parts from selected kit
- Click-drag to place parts
- Eraser tool
- Auto-connect walls (when placing a wall next to an existing wall, the part is replaced with the appropriate corner/T-piece variant)
- Export as Prefab → saves to `Assets/_Project/Prefabs/Buildings/`

### Files
```
Assets/_Project/Scripts/World/BuildingKit/Editor/
  ├── BuildingComposerWindow.cs
  ├── BuildingComposerGrid.cs
  ├── BuildingComposerPalette.cs
  └── BuildingComposerSerializer.cs
```

### MVP minimum
- Manual placement is fine; auto-connect walls is a stretch goal
- We only need to author 2 buildings (Café + Apartment); the tool can be *very* primitive

### V1.0+ enhancements
- Auto-connect walls
- Random variant selection for "weather" diversity (different brick tints)
- Building "footprint" validation (no floating walls)
- Procedural facade generation (give me a 5-tile-wide 2-story shop in this kit)

---

## Runtime use

A Building Prefab is just a hierarchy of GameObjects with SpriteRenderers and colliders. At runtime:

- Spawn the prefab into a scene
- Position by tile coordinate
- The prefab's child GameObjects are automatically sorted via the existing Y-sort
- Doors and stairs are recognized via `IInteractable` components attached during authoring

---

## Naming conventions

| Kit | Example parts |
|---|---|
| `brick_storefront` | `kit_wall_brick_straight`, `kit_door_glass_storefront`, `kit_window_storefront_large`, `kit_awning_brick_orange` |
| `wood_apartment` | `kit_wall_plaster_straight`, `kit_door_residential_wood`, `kit_window_sash_small` |
| `concrete_factory` | (V1.0+) |
| `clapboard_house` | (V1.0+) |

---

## Walkability and collision

- Floor tiles tagged `Walkable` are passable
- Wall segments tagged `BlocksLight` block 2D light beyond them (used in v1.0+ for interior lighting; in MVP only sun is global)
- Wall colliders generated automatically from `HasCollider` flag

---

## Lighting interaction

Each kit has a default interior light spawn — when a building prefab is instanced, it places point lights at predefined anchor points (kitchen lamp position, hallway bulb position, etc.). Author once per kit; reuse everywhere.

---

## Asset budget (MVP)

| Kit | Parts |
|---|---|
| `brick_storefront` | ~25 |
| `wood_apartment` | ~20 |

Plus shared trim/stairs/floor variants: ~15.

**Total: ~60 part sprites** for the slice. Equivalent to fully painting two buildings bespoke, but with the result that *every future building of the same kit costs zero new art*.

---

## Testing

- Editor test: composer round-trip (place parts → save prefab → load → matches)
- Runtime test: building prefab instances correctly without missing references
- Runtime test: walkable tiles passable, wall tiles block player

---

## Future considerations (V1.0+)

- Procedural facade generation
- Player in-game building tool (place down a kit; build your house at runtime)
- Renovation system (replace parts in place)
- Damage/wear states (broken windows, peeling paint)
- Per-kit weather variants
- Seasonal overlays (snow on roofs, fallen leaves on stairs)


---

# System — UI Architecture

## Purpose
Render the heads-up display, floating interaction prompts, dialogue panels, modal screens, and notifications. Stay minimal, diegetic where possible, period-appropriate.

## Scope
- **[SPINE]** required for MVP
- UGUI (not UI Toolkit) — see [06-tech-stack.md](06-tech-stack.md)
- Pixel-perfect at 480×270 internal resolution

---

## UI layers

Five layers, one Canvas each, all `Screen Space - Overlay`. Layered by sort order.

| Layer | Sort order | Purpose |
|---|---|---|
| Background | 0 | Letterbox bars, fade-to-black overlay |
| HUD | 100 | Clock, wallet, status |
| World-space prompts | (separate world-space canvas, per-prompt) | Floating "[E] Talk" labels |
| Dialogue | 200 | Bottom-of-screen dialogue box |
| Modals | 300 | Inventory, menu, save, cash drawer |
| Notifications | 400 | Corner toasts |

---

## UI managers

```csharp
public sealed class UIManager : MonoBehaviour
{
    public HUDController HUD { get; private set; }
    public DialogueUIController Dialogue { get; private set; }
    public ModalUIController Modals { get; private set; }
    public NotificationUIController Notifications { get; private set; }
    public PromptUIController Prompts { get; private set; }

    public bool IsBlockingGameplay => Modals.IsAnyOpen;
}
```

Modal screens **pause the world clock**; dialogue does **not** (see [11-systems/time-and-day.md](11-systems/time-and-day.md)).

---

## HUD

Minimal. Top-right corner.

```
+--------------+
| Tue 06:30 AM |
| Day 1        |
|  $34.05      |
+--------------+
```

Components:
- `ClockHUD` — observes `WorldClock.MinuteTicked`, updates time
- `WalletHUD` — observes `Wallet.Changed`, updates total

**No XP bar. No health bar. No quest tracker. No minimap.**

When player is in dialogue, HUD slightly dims. When player is in cutscene (porch sunset), HUD fully fades out.

---

## Dialogue UI

See [11-systems/dialogue.md](11-systems/dialogue.md) for the dialogue system data. The UI:

### Dialogue Box
- Bottom of screen, ~80px tall, ~440px wide
- Pixel-frame border
- Speaker portrait (left, 64×64)
- Speaker name above text
- Typed-out text at ~30 chars/sec
- Press Space to skip / advance
- Responses listed as buttons bound to `[1]`..`[4]` keys

### Dialogue Bubble
- Floats above character's head
- Tail points to character
- Fades in (0.15s) → persists (1.5s + 0.05s per char) → fades out (0.15s)
- Max 2 lines, ~20 chars per line

---

## Modal screens

Each modal is a separate prefab loaded under the Modals canvas. Mutually exclusive — opening one closes others.

### MVP modals
- **Inventory & Character Sheet** — `[Tab]`
- **Pause Menu** — `[Esc]`
- **Cash Drawer** — opens during customer payment
- **Save / Load Slots** — from Pause Menu

### Modal behaviour
- Opening pauses the clock
- Background dim overlay (alpha 0.5 over the gameplay layer)
- `[Esc]` always closes the topmost modal
- Click outside modal area: configurable per modal (Inventory: yes; Cash Drawer: no)

---

## Notifications

Small corner toasts. Top-left in MVP (to avoid HUD).

| Kind | Tone | Example |
|---|---|---|
| Game | Quiet | "Saved." |
| Story | Slightly warmer | "June left her sandwich." |
| World | Neutral | "Rowan called." |
| Money | Subtle | "The till is $42 short." |

Toast structure:
- Small pixel-frame box
- Single line, ~30 chars max
- Slides in over 0.2s, holds 3.0s, fades out 0.5s
- Stacks vertically if multiple

No exclamation marks. No emoji. No "QUEST UPDATED!" energy.

---

## Floating prompts (world-space)

Per-interactable label that appears when player is in range.

```
[E] Talk to Mr. Holland
```

- World-space Canvas attached to the interactable
- Pixel-frame label with hotkey icon and verb + target
- Hotkey adapts to current input device (keyboard vs. gamepad)
- Disappears when interactable leaves range or `CanInteract` becomes false

---

## Pixel fonts

- Body: 8 px tall (raw pixel-art font)
- Headers: 12–16 px
- HUD: 8 px
- All nearest-neighbor scaled
- No bold/italic (use color or alternate font for emphasis if needed)

Recommended fonts (one or two for MVP):
- **Determination Mono** (free, pixel-perfect)
- **Pixel Operator** (free)
- Custom hand-drawn font once style is locked

---

## Input bindings (UI-relevant)

| Action | Default Key | Default Gamepad |
|---|---|---|
| Open Inventory | Tab | Y / Triangle |
| Pause | Esc | Start |
| Interact | E | A / X |
| Cancel / Back | Esc | B / Circle |
| Skip dialogue | Space | A / X |
| Confirm | Enter | A / X |
| Navigate | WASD / Arrows | D-pad / Left stick |
| Select response | 1–4 | Up/Down + A |

All rebindable via Input System.

---

## Localization

All player-facing strings via Unity Localization keys. Even English-only at launch.

Strings live in `Assets/_Project/Data/Localization/` tables. Format: `key | en | (future: other locales)`.

---

## Accessibility (MVP minimum)

See [06-tech-stack.md](06-tech-stack.md) for the full list. UI-specific:
- All text legible at 1×, 2×, 3×, 4× scale
- No information conveyed by color alone
- Subtitle/dialogue text always on (no toggle to hide)
- Settings menu has audio mute toggles per category

---

## Style discipline

The UI's job is to disappear. Players notice the HUD only when checking it. The dialogue box only when reading. The modal only when actively using. *Stardew Valley*'s UI is the reference.

If a piece of UI persistently draws attention away from the world, it is wrong.

---

## Testing

- Visual regression: HUD layout at 1080p, 1440p, 4K
- Integration: open inventory → clock pauses; close → clock resumes
- Integration: dialogue advances clock; modal does not
- Integration: notifications stack correctly when 3 arrive in quick succession
- Integration: rebind a key, persists across launches


---

# System — Audio Architecture

## Purpose
Runtime audio. Music director, SFX manager, ambient bed manager, mixer routing. Implements the [10b-audio-direction.md](10b-audio-direction.md) direction.

## Scope
- **[SPINE]** required for MVP
- Unity built-in `AudioSource` + `AudioMixer`
- Music director crossfades on time-of-day band changes
- Ambient bed crossfades on scene transitions
- SFX pooled, spatial pan based on world position

---

## Mixer routing

Single `AudioMixer` asset (`Assets/_Project/Audio/MasterMixer.mixer`) with this structure:

```
Master (0 dB)
├── Music (-6 dB default)
├── SFX (0 dB)
├── Ambient (-12 dB)
└── UI (-4 dB)
```

Each group has volume exposed parameters (`_volume_music`, `_volume_sfx`, etc.) for settings menu sliders.

Snapshots per major state (optional in MVP):
- `Default`
- `InDialogue` (ambient -3 dB, music unchanged)
- `InModal` (music -6 dB, ambient -6 dB)

---

## AudioManager

```csharp
public sealed class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _musicSourceA;   // for crossfade
    [SerializeField] private AudioSource _musicSourceB;

    private SFXPool _sfxPool;
    private MusicDirector _musicDirector;
    private AmbientBedManager _ambientManager;

    public void PlaySFX(string sfxId, Vector3 worldPosition = default, float volume = 1f);
    public void PlayUISound(string sfxId);
    public void SetMixerVolume(string group, float linearVolume);
    public void SetSnapshot(string snapshotName, float transitionSeconds);
}
```

---

## Music director

Drives which music loop plays based on:
- Current scene
- Current time band ([11-systems/time-and-day.md](11-systems/time-and-day.md))
- Special states (porch sunset)

### Selection table (MVP)

| Scene | Time band | Music id |
|---|---|---|
| Veyne_ApartmentInterior | Dawn / Morning | `mus_apartment_morning` |
| Veyne_ApartmentInterior | Evening / Night | `mus_apartment_evening` |
| Veyne_CafeInterior | Morning | `mus_cafe_morning` |
| Veyne_CafeInterior | Midday | `mus_cafe_midday` |
| Veyne_CafeInterior | Afternoon | `mus_cafe_afternoon` |
| Veyne_CafeInterior | Dusk | `mus_cafe_evening` |
| Veyne_Exterior | (matches the interior of the building closest by) | |
| (Special: porch sunset) | n/a | `mus_porch_sunset` (one-shot, plays uninterrupted) |
| MainMenu | n/a | `mus_menu_quiet` |

### Crossfade

Standard 4-second equal-power crossfade between music sources A and B. When switching:

```csharp
public void Crossfade(AudioClip nextClip, float seconds = 4f)
{
    var fadingOut = _activeSource;
    var fadingIn = (fadingOut == _musicSourceA) ? _musicSourceB : _musicSourceA;

    fadingIn.clip = nextClip;
    fadingIn.volume = 0f;
    fadingIn.Play();

    StartCoroutine(FadeRoutine(fadingOut, fadingIn, seconds));
    _activeSource = fadingIn;
}
```

### Special states

- **Porch sunset:** music director switches to `mus_porch_sunset` regardless of time band; plays full track once; auto-returns to band-appropriate music when scene changes.
- **End-of-day cash-up:** music fades to silence; on confirm, a small piano motif plays as a one-shot before fade-out.

---

## Ambient bed manager

Plays the per-scene ambient bed via a dedicated `AudioSource` separate from music. Crossfades on scene transition or when transitioning between bed-different regions of the same scene.

### Bed selection
| Scene | Bed id |
|---|---|
| Veyne_ApartmentInterior | `amb_apartment_morning` / `amb_apartment_evening` (time-band gated) |
| Veyne_CafeInterior (open) | `amb_cafe_murmur` |
| Veyne_CafeInterior (closed) | `amb_cafe_quiet` |
| Veyne_Exterior | `amb_street_day` / `amb_street_dusk` |

Bed crossfade: 1 second.

---

## SFX pool

A pool of 16 `AudioSource` instances on the AudioManager GameObject for fire-and-forget SFX.

```csharp
public sealed class SFXPool
{
    private readonly Queue<AudioSource> _free;
    private readonly List<AudioSource> _busy;
    private readonly Dictionary<string, AudioClip> _library;

    public void Play(string sfxId, Vector3 worldPosition, float volume = 1f)
    {
        if (!_library.TryGetValue(sfxId, out var clip)) return;

        var src = Dequeue();
        src.transform.position = worldPosition;
        src.clip = clip;
        src.volume = volume;
        src.pitch = JitterPitch(0.05f);   // small natural variation
        src.spatialBlend = 1f;           // 3D
        src.panStereo = 0f;
        src.Play();

        // returns to pool when finished (coroutine or callback)
    }
}
```

### Random variants
SFX with `_01`, `_02`, etc. variants are randomized at play time. The Manager exposes:

```csharp
audio.PlaySFX("sfx_footstep_wood", playerPos);  // resolves to random _NN variant
```

---

## Spatialization

- In-world SFX use 3D AudioSources with `spatialBlend = 1.0`
- Distance attenuation: linear rolloff from 0 to 10 world units
- 2D pan derived from screen position (Unity handles for us with the 2D Audio Listener)

We do **not** simulate real acoustics, reflections, occlusion. Cozy game, not a horror sim.

---

## Dialogue blips

Per-character archetype blip sample. Plays every ~3 displayed characters during typewriter text.

```csharp
public sealed class DialogueBlipPlayer
{
    public void Play(string archetypeId)
    {
        var clip = _clipsByArchetype[archetypeId];
        var pitch = _pitchByArchetype[archetypeId];
        _audio.PlayUISound(clip.name, volume: 0.6f, pitch: pitch);
    }
}
```

---

## Volume policy

| Source | Default mixer volume | Notes |
|---|---|---|
| Music | -6 dB | Quiet enough to talk over |
| Ambient beds | -12 dB | Background; should never overpower |
| SFX | 0 dB | Natural |
| UI | -4 dB | Slightly quieter than world SFX |

Ducking on dialogue snapshot: ambient -3 dB.

---

## Memory budget

Audio is one of the most memory-hungry asset categories. Discipline:
- Music **streamed** (not preloaded)
- Ambient beds **streamed**
- SFX **decompressed on load** (small files, fast access at runtime)

MVP estimate:
- 3 music tracks × ~3 MB each = ~9 MB
- 7 ambient beds × ~2 MB each = ~14 MB
- ~30 SFX × ~50 KB each = ~1.5 MB
- Total: **~25 MB audio** for the slice

---

## Save integration

Audio volumes per group are saved in `profile.json` user settings, not in `world.json`.

```jsonc
"audioSettings": {
  "master": 1.0,
  "music": 0.7,
  "sfx": 1.0,
  "ambient": 0.6,
  "ui": 0.8
}
```

---

## Testing

- Integration: time band changes → music crossfades over 4s without click
- Integration: scene change → ambient bed crossfades over 1s without click
- Integration: dialogue triggers snapshot, ambient ducks; ends, restores
- Integration: 16 concurrent SFX trigger; pool does not run out (rare warning if exceeded)
- Unit: blip volume + pitch resolution from archetype id

---

## Future considerations (V1.0+)

- Per-region beds (Old North, The Mills, Riverwalk sound different)
- Weather-aware beds (rain on roof, snow muffling)
- Music creation system integration: player-uploaded songs play on in-world radios / jukeboxes
- Music charts / radio rotations (player music plays via the music director)
- Spatialized voice chat
- Adaptive music (mini-game tension layers)


---

# Risk Register

What can kill this project. And how we don't let it.

Each risk has: a likelihood, an impact, a mitigation, and (where relevant) an owner.

This document gets re-read **monthly** and updated. Risks that pass without materializing get downgraded. New risks get added.

---

## Top risks

### R1 — Scope creep
- **Likelihood:** Very high (default state of game projects)
- **Impact:** Project death
- **Description:** Teammates suggesting "small additions" beyond the slice spec — combat, multiplayer prototype, music creation, government, vehicles, second town. Each one feels small. Together they kill the project before Phase 9 ships.
- **Mitigation:**
  - [05-vertical-slice-cafe-day.md](05-vertical-slice-cafe-day.md) explicitly lists out-of-scope items
  - Any new feature request goes to `docs/backlog/` (parking lot), never directly into the build
  - Weekly review: are we still building what the spec says?
  - The pinned vision message (single-message GDD intro) is the public commitment
- **Owner:** Dev lead

### R2 — Volunteer churn
- **Likelihood:** High
- **Impact:** Project stalls or dies
- **Description:** Recruited volunteers from Reddit/Discord lose interest, leave, or never deliver. Unpaid contributors have ~95% drop-off over multi-year projects.
- **Mitigation:**
  - Don't depend on any one contributor for blocking work
  - Keep tasks scoped to ship in a single weekend
  - Default to small atomic PRs that don't require ongoing presence
  - Recognize and credit contributors visibly (devlog mentions, contributor list)
  - Build the slice to a state where a real artifact attracts better contributors
- **Owner:** Dev lead

### R3 — Lose motivation around month 3
- **Likelihood:** High
- **Impact:** Project death
- **Description:** The novelty wears off. Phase 4–5 work is the longest grind without visible payoff. Solo devs frequently abandon at this point.
- **Mitigation:**
  - Ship something playable every 2 weeks (even tiny)
  - Show real friends, not just the team
  - Weekly devlog (even private) — momentum tool
  - Phase 7 polish work is the "comeback phase" — design for it as something to look forward to
- **Owner:** Dev lead

### R4 — Art quality lags behind code
- **Likelihood:** High
- **Impact:** Game feels bad despite working systems; can't ship at slice quality
- **Description:** Placeholder art accumulates; we get used to it; we ship something that looks like a tech demo.
- **Mitigation:**
  - Use *consistent* placeholder art (one author or one style guide) from day 1
  - Reserve Phase 7 specifically for art polish (4 weeks dedicated)
  - Consider one paid commission for character sprites before Phase 7 if budget permits later
  - Reference [10a-art-direction.md](10a-art-direction.md) for the bar
- **Owner:** Art lead (when one exists)

### R5 — AI-generated code accumulates technical debt
- **Likelihood:** High (we are explicitly using AI assistance per [06-tech-stack.md](06-tech-stack.md))
- **Impact:** Refactor cost balloons; bugs compound
- **Description:** AI writes plausible code that subtly contradicts itself across files. Especially dangerous in: networking, save/load, complex state machines, distributed logic.
- **Mitigation:**
  - **Code-review every AI-produced PR** before merge
  - Write tests for non-trivial logic immediately
  - Refactor early if the shape feels wrong; don't accumulate
  - Single source of truth via shared interfaces (`IInteractable`, `ISaveable`, etc.)
  - Architecture review at the end of each phase
- **Owner:** Dev lead

### R6 — Multiplayer attempted prematurely
- **Likelihood:** Medium-high (the vision wants multiplayer; the temptation is constant)
- **Impact:** 6+ months wasted on infra that never ships
- **Description:** Someone on the team adds Mirror/FishNet "just to prototype" and the project dies in netcode hell.
- **Mitigation:**
  - **HARD RULE:** No networking work before Phase 9 ships
  - No networking packages in `Packages/manifest.json` until then
  - This risk is called out in [06-tech-stack.md](06-tech-stack.md), [12-roadmap.md](12-roadmap.md), and now here — visibility is the cure
- **Owner:** Dev lead

### R7 — Trying to recruit a team before slice ships
- **Likelihood:** High
- **Impact:** Volunteer churn + management overhead drains the lead's time from actually building
- **Description:** The lead spends 50% of their week answering Discord questions, managing onboarding, replying to "how can I help?" — instead of building. The slice doesn't progress.
- **Mitigation:**
  - Limit recruitment to people who actually deliver in week 1
  - Don't onboard new contributors during deep-work phases (Phase 1–7)
  - Use the pinned vision message as a passive filter (people who read it and then ask thoughtful questions are good; people who ask "what should I do?" without reading are bad)
- **Owner:** Dev lead

### R8 — Save format change breaks playtester saves
- **Likelihood:** Medium
- **Impact:** Tester frustration; bad early reviews; lost goodwill
- **Description:** Schema bump during playtest period silently corrupts everyone's progress.
- **Mitigation:**
  - **Versioned schema + migrations from Phase 3** per [09-save-system.md](09-save-system.md)
  - Every schema change ships a migration in the same PR
  - Backup rotation in save folder lets us recover
- **Owner:** Dev lead

### R9 — Building kit takes too long to author
- **Likelihood:** Medium
- **Impact:** Delays Phase 1
- **Description:** We over-engineer the building composer tool when we only need to author 2 buildings for the slice.
- **Mitigation:**
  - MVP buildings can be hand-placed (manually drop parts in Unity scene)
  - Composer tool is a stretch goal, not a Phase 1 blocker
  - Tool's real value is in Slice 2+ when many buildings need authoring
- **Owner:** Dev lead

### R10 — Permadeath/death systems leak into MVP scope
- **Likelihood:** Medium
- **Impact:** Scope creep + thematic dissonance in slice
- **Description:** Death and permadeath are central to the full vision and someone wants to "show it off" in the slice. But the slice is a single cozy day; introducing mortality breaks the tone.
- **Mitigation:**
  - Explicitly out of slice scope per [05-vertical-slice-cafe-day.md](05-vertical-slice-cafe-day.md)
  - Death is a Year 2 feature, not Year 1
- **Owner:** Dev lead

### R11 — Real-world life events interrupt the lead
- **Likelihood:** High (over a 24-week timeline)
- **Impact:** Timeline slip
- **Description:** School, job, family, illness. None of us are full-time on this.
- **Mitigation:**
  - Plan for halved hours in some weeks
  - Keep tasks small enough to ship in single sessions
  - No hard external deadlines until Phase 8
  - Slack in the schedule: 24 planned weeks easily becomes 30
- **Owner:** Dev lead (life)

### R12 — Tooling breaks (Unity version, package incompatibility)
- **Likelihood:** Low
- **Impact:** Days to weeks of lost time
- **Description:** Unity push a breaking change; a package becomes incompatible; LFS quota maxed.
- **Mitigation:**
  - **Lock Unity version** (currently 6.0.1f1 per `ProjectSettings/ProjectVersion.txt`)
  - Commit `Packages/manifest.json` and respect locked package versions
  - **Do not chase package updates** mid-project unless required for a bug
- **Owner:** Dev lead

### R13 — Mod or storefront policy issues
- **Likelihood:** Low (in slice phase); High (when player upload becomes possible)
- **Impact:** Cannot ship on Steam, itch, etc.
- **Description:** When player-created culture (uploads) ships, content moderation becomes a Steam/Itch policy concern. Out of scope for slice but worth flagging as a future risk.
- **Mitigation:** Tight moderation per [04-design-principles.md](04-design-principles.md); plan moderation infrastructure *before* uploads ship.
- **Owner:** Dev lead, future

---

## Lower-priority risks (monitor)

| Risk | Notes |
|---|---|
| Audio author can't be recruited | Use library music as placeholder; commission later |
| Performance drops on low-end hardware | Profile early; optimize before Phase 7 |
| Localization tech adds complexity | Use Unity Localization from day 1 to avoid retrofit pain |
| Save bloats due to history log | Cap history log size per slot; archive older entries to separate file |
| Pixel-perfect rendering breaks on certain monitors | Test at 1080p, 1440p, 4K; document any caveats |
| Team disagreement on a design pillar | Use [04-design-principles.md](04-design-principles.md) as tie-breaker; in stalemate, dev lead decides |

---

## Risk review cadence

- **Weekly:** quick scan of top 5 risks during devlog write-up
- **Monthly:** full register review; downgrade resolved risks; add new ones
- **End of each phase:** retrospective specifically including "what risks did we underestimate?"

---

## Calibration check

When something *does* go wrong, ask:
1. Was it in the register? (good — mitigation worked or didn't)
2. Was it not in the register? (failure of imagination — add it now)

The register is a living document. Update it.


---

# Tools to Install

Everything you need installed before doing real work on *legacy*.

---

## Required

Install these before Phase 0 of [12-roadmap.md](12-roadmap.md).

### Unity 6.0.1f1
- Get it via **Unity Hub** (https://unity.com/download)
- Install **exact version 6.0.1f1** (locked per `ProjectSettings/ProjectVersion.txt`)
- Required modules:
  - Microsoft Visual Studio Build Tools (Windows) **OR** install Rider/VS Code separately
  - Build support modules: Windows Build Support (mandatory); macOS + Linux build support (optional)
- Disk space: ~15 GB

### Git
- https://git-scm.com/
- Verify: `git --version`
- On Windows, Git Bash comes bundled — use it for any shell commands in docs

### Git LFS
- https://git-lfs.github.com/
- After installing, run once globally: `git lfs install`
- Verify: `git lfs version`

### IDE — pick one

#### JetBrains Rider (recommended)
- Free for non-commercial use via **JetBrains Toolbox** (https://www.jetbrains.com/toolbox-app/)
- Best Unity integration, fastest C# editing experience
- Bundled with ReSharper-style refactoring

#### Visual Studio Code (alternative — free)
- https://code.visualstudio.com/
- Required extensions:
  - **C# Dev Kit** (Microsoft)
  - **Unity** (Microsoft)
  - **EditorConfig for VS Code** (Microsoft)
- Optional but recommended:
  - **GitLens** (Eric Amodio)
  - **Error Lens** (Alexander)

### Aseprite
- Pixel art standard
- $20 one-time on Steam (https://store.steampowered.com/app/431730/Aseprite/)
- Or buy direct: https://www.aseprite.org/

### Audacity
- Free audio editor for SFX cleanup
- https://www.audacityteam.org/

---

## Recommended

Install when you start working in that area.

### REAPER (DAW for composers)
- Free indefinite evaluation, $60 personal license
- https://www.reaper.fm/
- For music + advanced SFX composition

### OBS Studio
- Free
- https://obsproject.com/
- For recording playtests, devlog footage, gameplay clips

### Tiled
- Free, alternative tilemap editor
- https://www.mapeditor.org/
- Only if you prefer it over Unity's Tile Palette workflow

### GitHub Desktop
- Free GUI for git
- https://desktop.github.com/
- Optional — for those who prefer GUI over CLI

---

## Accounts to create

### GitHub
- https://github.com/
- Free private repo for the project
- Add team members as collaborators with appropriate permissions

### itch.io
- https://itch.io/
- Free
- Where the slice eventually ships
- Set up the project page in Phase 8

### Discord
- https://discord.com/
- Team communication + community channel
- Create dedicated server for the project

---

## Unity packages to add

These are not yet in [`Packages/manifest.json`](../Packages/manifest.json). Add them during Phase 0 via the Unity Package Manager.

Required:

| Package | Identifier | Version |
|---|---|---|
| Aseprite Importer | `com.unity.2d.aseprite` | latest stable |
| Pixel Perfect Camera | `com.unity.2d.pixel-perfect` | latest stable |
| Cinemachine | `com.unity.cinemachine` | latest stable |
| Newtonsoft Json | `com.unity.nuget.newtonsoft-json` | latest stable |
| Localization | `com.unity.localization` | latest stable |

Optional:

| Package | Identifier | Reason |
|---|---|---|
| 2D Sprite Shape | `com.unity.2d.spriteshape` | Curved geometry (rivers, paths) |
| Addressables | `com.unity.addressables` | Asset streaming — defer until needed |

---

## System requirements (developer machine)

### Minimum
- OS: Windows 10/11 64-bit (primary) **or** macOS 11+ **or** Ubuntu 22.04+
- CPU: 4-core, 2.5 GHz+
- RAM: 16 GB
- GPU: Anything from the last 5 years; DirectX 11 / Metal compatible
- Disk: 30 GB free (Unity + project + caches)

### Recommended
- CPU: 8-core
- RAM: 32 GB (large scenes and asset import gets memory-hungry)
- Disk: SSD with 100 GB free
- 1440p or higher monitor for the editor

---

## Configuration steps after installation

### Git config (per machine, not per repo)
```bash
git config --global user.name "Your Name"
git config --global user.email "you@example.com"
git config --global core.autocrlf input    # if on Windows: true
```

### Git LFS (per machine, once)
```bash
git lfs install
```

### Unity Editor settings (once per machine, for this project)
- **Edit → Preferences → External Tools → External Script Editor:** Rider or VS Code
- **Edit → Project Settings → Editor → Asset Serialization:** Force Text
- **Edit → Project Settings → Editor → Version Control:** Visible Meta Files

### IDE settings (Rider)
- Enable "Save documents on focus loss" so your scene changes commit naturally
- Configure C# formatting to match the conventions in [08-code-conventions.md](08-code-conventions.md) (most defaults are fine)

---

## Verifying your setup

After installation, you should be able to:

1. `git clone <repo>` → successful
2. `git lfs pull` → fetches binary assets
3. Open the project in Unity Hub → Unity opens without errors
4. Wait for asset import (5–15 min first time)
5. Open `Bootstrap.unity` → press Play → no errors, no pink textures, no missing scripts
6. Open Rider or VS Code → C# IntelliSense works on a `MonoBehaviour`

If any of these fail, ask in the team Discord — don't suffer in silence.

---

## What NOT to install

- **Unity 2023 / 2022 / 6000.x other versions** — version is locked at 6.0.1f1
- **Wwise / FMOD** — not used in MVP
- **Unity Cloud Build** — not used; we use GitHub Actions for CI later
- **Random Asset Store packages** — anything we use lives in `ThirdParty/`; don't pull random things in
- **Unity Hub auto-update** — pin the Hub version too if possible, since Hub updates can change behavior


---

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
| **Frontier** | New regions periodically opened for new-player settlement. Primary onboarding strategy. Long-term system. |
| **History log** | Append-only world record of named events. The "world that remembers" backbone. |
| **Generated personality** | NPC traits (warmth, talkativeness, generosity, mood) rolled at spawn from archetype ranges. Not LLM-driven. |
| **Seeded canon** | Commissioned cultural content (music, books, films) shipped with the game as the world's pre-existing "classical era." |
| **Player-driven** | A system whose primary inputs come from players, not developers or scripts |
| **Authored content** | Developer-written narrative — quests, scripted stories, fixed plot. We do not ship this. |
| **Diegetic** | Existing within the fiction of the game. The wall clock telling time is diegetic; a "TIME: 06:30" debug overlay is not. |

## Game-world entities

| Term | Definition |
|---|---|
| **Marenne** [PLACEHOLDER] | The fictional country the game is set in |
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


---

# Multiplayer Proof Plan

This is the next step after the local command/state boundary is stable. Do not start here until the local café loop still works through `GameRuntime.Execute(...)`.

## Goal

Prove that the café can run as an authoritative multiplayer simulation with two clients.

This is not the MMO. This is only the first proof that the architecture can support server-owned world state.

## Test Scenario

```text
host starts café region
client 1 joins
client 2 joins
both clients see the same time, same NPCs, same active order, same till
only one client can successfully prepare/serve an order
server owns WorldState
clients render snapshots
```

## Architecture

```text
client input
-> command object
-> transport
-> server GameRuntime
-> WorldState mutation
-> state snapshot/event
-> client presentation update
```

## First Commands To Network

- `MoveCharacterCommand`
- `TalkToNpcCommand`
- `PrepareOrderCommand`
- `ServeOrderCommand`

## First Server Rules

- server owns `WorldState`
- clients never mutate world state directly
- clients submit commands
- server validates commands
- server broadcasts state changes
- local single-player should keep using the same command path

## First Technical Version

Use an in-editor or local-host setup first.

Phase order:

1. keep current local command path working
2. add an `IWorldTransport` interface
3. create a fake local transport that just calls the same runtime
4. create a second Unity player view using the fake transport
5. only then test a real networking package

## Success Criteria

- two clients can move in one café scene
- both see the same clock/till/order state
- Holland/Sasha orders are server-owned
- if both players try to serve, only the valid first command succeeds
- save/load serializes the server `WorldState`

## Do Not Build Yet

- accounts
- login
- dedicated server deployment
- database
- regions/shards
- voice chat
- anti-cheat
- authoritative physics

Those come after this proof.


---

