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
