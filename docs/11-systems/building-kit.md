# System — Modular Building Kit

## Purpose
Assemble buildings (interiors and exteriors) from snap-together parts instead of authoring every building bespoke. Essential for scaling content beyond the slice.

## Scope
- **[SPINE]** required for MVP
- MVP scope: two buildings (Café and Apartment) assembled with the kit
- Authoring tool used to produce final prefabs
- Tool itself is minimal in MVP — its real value is from Slice 2 onward

---

## Why

Hand-authoring every building in pixels does not scale. *Stardew Valley* is one valley with maybe 30 buildings authored over years by one person. *legacy* aims at dozens of buildings in a single town and many towns over time. A modular kit collapses authoring cost from "draw every building" to "drop pieces, save prefab."

This is also how *Project Zomboid*, *RimWorld*, and *Don't Starve* ship enormous worlds with small art teams.

---

## Concepts

### Building part
A reusable 32×32 piece authored in Aseprite — wall segments, floors, doors, windows, roofs, trim.

### Building kit
A categorized collection of parts that share a visual style — e.g. "Brick storefront" kit contains red-brick walls, white-trim windows, glass-front doors, awnings.

### Building prefab
The result of composing parts into a unit (a single café, a single apartment). Stored as a Unity Prefab and instanced into scenes.

---

## Part categories (MVP)

| Category | Examples |
|---|---|
| Floor | wood plank, tile small, tile hex, concrete, carpet — per kit |
| Wall (exterior) | brick straight, brick corner NE/NW/SE/SW, brick T-junction, brick end |
| Wall (interior) | plaster straight + corners, painted variant, half-wall |
| Door (exterior) | shop glass, residential wood, double door |
| Door (interior) | wood panel, swinging kitchen door |
| Window | small, large, shop-front, sash |
| Roof | gable, flat, slate, shingle, awning |
| Trim | baseboard, ceiling molding, window casing |
| Stairs | up, down, landing |
| Fixed prop | counter, shelf, sign mount |

Furniture and decoration are *not* part of the kit. They are placed separately as props (see [10-asset-pipeline.md](10-asset-pipeline.md)).

---

## Data types

```csharp
[CreateAssetMenu(menuName = "Legacy/Building/Part")]
public sealed class BuildingPart : ScriptableObject
{
    public string PartId;
    public BuildingPartCategory Category;
    public BuildingKit Kit;        // which visual kit it belongs to
    public Sprite Sprite;
    public Vector2Int TileSize = new(1, 1);
    public Vector2Int PivotTile = new(0, 0);
    public BuildingPartFlags Flags;
}

public enum BuildingPartCategory
{
    FloorTile,
    WallSegment,
    Door,
    Window,
    Roof,
    Trim,
    Stairs,
    FixedProp
}

[Flags]
public enum BuildingPartFlags
{
    None = 0,
    Walkable = 1 << 0,
    BlocksLight = 1 << 1,
    SnapsToWall = 1 << 2,
    SnapsToFloor = 1 << 3,
    HasCollider = 1 << 4
}

[CreateAssetMenu(menuName = "Legacy/Building/Kit")]
public sealed class BuildingKit : ScriptableObject
{
    public string KitId;        // "brick_storefront", "wood_apartment"
    public string DisplayName;
    public List<BuildingPart> Parts;
}
```

---

## Authoring tool — Building Kit Composer

A custom Unity editor window for placing parts on a grid.

### Behaviour
- Grid view (32×32 snap)
- Palette of parts from selected kit
- Click-drag to place parts
- Eraser tool
- Auto-connect walls (when placing a wall next to an existing wall, the part is replaced with the appropriate corner/T-piece variant)
- Export as Prefab → saves to `Assets/_Project/Prefabs/Buildings/`

### Files
```
Assets/_Project/Scripts/World/BuildingKit/Editor/
  ├── BuildingComposerWindow.cs
  ├── BuildingComposerGrid.cs
  ├── BuildingComposerPalette.cs
  └── BuildingComposerSerializer.cs
```

### MVP minimum
- Manual placement is fine; auto-connect walls is a stretch goal
- We only need to author 2 buildings (Café + Apartment); the tool can be *very* primitive

### V1.0+ enhancements
- Auto-connect walls
- Random variant selection for "weather" diversity (different brick tints)
- Building "footprint" validation (no floating walls)
- Procedural facade generation (give me a 5-tile-wide 2-story shop in this kit)

---

## Runtime use

A Building Prefab is just a hierarchy of GameObjects with SpriteRenderers and colliders. At runtime:

- Spawn the prefab into a scene
- Position by tile coordinate
- The prefab's child GameObjects are automatically sorted via the existing Y-sort
- Doors and stairs are recognized via `IInteractable` components attached during authoring

---

## Naming conventions

| Kit | Example parts |
|---|---|
| `brick_storefront` | `kit_wall_brick_straight`, `kit_door_glass_storefront`, `kit_window_storefront_large`, `kit_awning_brick_orange` |
| `wood_apartment` | `kit_wall_plaster_straight`, `kit_door_residential_wood`, `kit_window_sash_small` |
| `concrete_factory` | (V1.0+) |
| `clapboard_house` | (V1.0+) |

---

## Walkability and collision

- Floor tiles tagged `Walkable` are passable
- Wall segments tagged `BlocksLight` block 2D light beyond them (used in v1.0+ for interior lighting; in MVP only sun is global)
- Wall colliders generated automatically from `HasCollider` flag

---

## Lighting interaction

Each kit has a default interior light spawn — when a building prefab is instanced, it places point lights at predefined anchor points (kitchen lamp position, hallway bulb position, etc.). Author once per kit; reuse everywhere.

---

## Asset budget (MVP)

| Kit | Parts |
|---|---|
| `brick_storefront` | ~25 |
| `wood_apartment` | ~20 |

Plus shared trim/stairs/floor variants: ~15.

**Total: ~60 part sprites** for the slice. Equivalent to fully painting two buildings bespoke, but with the result that *every future building of the same kit costs zero new art*.

---

## Testing

- Editor test: composer round-trip (place parts → save prefab → load → matches)
- Runtime test: building prefab instances correctly without missing references
- Runtime test: walkable tiles passable, wall tiles block player

---

## Future considerations (V1.0+)

- Procedural facade generation
- Player in-game building tool (place down a kit; build your house at runtime)
- Renovation system (replace parts in place)
- Damage/wear states (broken windows, peeling paint)
- Per-kit weather variants
- Seasonal overlays (snow on roofs, fallen leaves on stairs)
