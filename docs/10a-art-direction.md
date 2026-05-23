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
