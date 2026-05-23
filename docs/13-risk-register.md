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
