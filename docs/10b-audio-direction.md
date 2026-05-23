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
