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
