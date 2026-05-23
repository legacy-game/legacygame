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
