# Roadmap

The current milestone is **the first playable shared-world prototype**.

This is not a café slice. The café is only the first test building inside Veyne. The real milestone is a small playable block where citizens, places, plots, ownership, jobs, money, time, and history all exist in one shared world state.

Long-term, the world is not only one fixed town. The full game starts with one dense seeded capital city and surrounding older towns, then opens into large wilderness/frontier territory. Players should eventually be able to claim land, found settlements, expand infrastructure, and create new jurisdictions that can become country-like over time. The current prototype proves the shared-world spine first; frontier expansion waits until that spine is reliable.

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
- Territory/chunk id model placeholder

### Definition of Done

Veyne exists as data, not just as a Unity scene. It contains citizens, plots, buildings, owners, and history records.

---

## Phase 1A — Geography and Territory Foundation

Keep the world model compatible with future expansion beyond the capital.

### Tasks

- Add `TerritoryChunkState` or equivalent large-land-chunk data
- Add biome tags: `Urban`, `Forest`, `Grassland`, `Lakeside`, `Hill`, `Wetland`, `FarmlandPotential`
- Add claim status: `Unclaimed`, `Private`, `Municipal`, `Settlement`, future `Jurisdiction`
- Add optional settlement/jurisdiction ids, but do not build full government yet
- Save/load territory chunks
- Add inspect command/dev UI for territory
- Write history when territory is claimed or converted

### Definition of Done

The world can represent:

```text
capital block = developed territory
surrounding land = wilderness/unclaimed territory
territory can be inspected, saved, loaded, and eventually claimed
```

This phase is a compatibility layer, not full frontier gameplay.

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
- Full frontier expansion / settlement founding / countries

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
- Territory claims and settlement founding prototype
- Basic government
- Basic family/legacy scaffolding

### Year 3+

- Multi-region world
- Player-founded towns and country-scale jurisdictions
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
