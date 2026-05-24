# Territory and Frontier System

The long-term world starts with one dense seeded capital city, then expands outward into wilderness, rural land, lakes, forests, and player-founded settlements.

This system exists to make that expansion possible without hardcoding "new country" as a button.

---

## Design Intent

The capital proves dense ordinary life: jobs, property, NPCs, shops, public records, money, and history.

The frontier proves civilization growth: players transform land into places, settlements, infrastructure, and eventually jurisdictions.

The frontier must still obey the core tone:

- quiet lives remain first-class
- distance matters
- history records meaningful changes
- countries emerge from civic systems, not UI shortcuts

---

## Core Concepts

### Territory Chunk

A large unit of land above plots/parcels.

Example fields:

```text
TerritoryChunkState
- id
- regionId
- coord
- biome
- claimStatus
- claimOwnerId optional
- settlementId optional
- jurisdictionId optional
- buildability
- discovered/known status
```

### Biome

Initial terrain identity.

Examples:

```text
Urban
Forest
Grassland
Lakeside
Hill
Wetland
FarmlandPotential
IndustrialRuin
```

### Claim

Who, if anyone, has legal/use control over the land.

Examples:

```text
Unclaimed
Private
Municipal
Settlement
Protected
Jurisdiction
```

### Settlement

A named place founded through world actions. It may start as a camp, hamlet, or road stop and grow into a village, town, city, or capital.

### Jurisdiction

A governing area with public records, rules, borders, and authority. Countries are late-stage jurisdictions, not early-game objects.

---

## Minimum Prototype

Do not build full frontier gameplay yet. The first useful slice is:

```text
capital block = developed territory
surrounding chunks = wilderness/unclaimed territory
player/dev command can inspect a territory chunk
claiming a chunk writes history
save/load preserves chunk state
```

Definition of done:

```text
The world can answer:
what land is this?
what biome is it?
is it claimed?
who controls it?
is it part of a settlement or jurisdiction?
what happened here?
```

---

## Future Expansion Flow

A mature version should support:

```text
discover land
claim or lease land
clear/build access
build first structures
name a settlement
attract residents and jobs
create public records
expand infrastructure
establish jurisdiction
negotiate borders
gain recognition
```

The system should not assume every frontier claim becomes a country. Most should become farms, cabins, businesses, villages, or suburbs.

---

## History Events

Meaningful territory events should be recorded.

Examples:

```text
TerritoryInspected
TerritoryClaimed
TerritoryReleased
RoadBuilt
SettlementFounded
SettlementExpanded
JurisdictionCreated
BorderChanged
```

---

## Non-Goals for MVP

Do not build yet:

- full governments
- diplomatic recognition
- wars over borders
- automated city simulation
- full procedural terrain
- multi-region server handoff

The MVP only needs the data shape to avoid painting the world model into a fixed-town corner.
