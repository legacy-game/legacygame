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
