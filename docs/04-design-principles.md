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
