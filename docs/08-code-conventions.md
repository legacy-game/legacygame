# Code Conventions

How we write C# in this project. Read once, follow forever.

---

## Language

- **C# 12** (Unity 6 supports it via the Roslyn compiler)
- **.NET Standard 2.1** API compatibility (Unity default)
- **Nullable reference types: OFF** project-wide (Unity ecosystem fights nullable; not worth the noise in MVP)

---

## Naming

| Element | Style | Example |
|---|---|---|
| Types (class, struct, enum, interface) | `PascalCase` | `NPCController`, `RecipeDefinition` |
| Interfaces | `IPascalCase` | `IInteractable`, `ISaveable` |
| Methods | `PascalCase` | `OpenShop()`, `TryServeCustomer()` |
| Public properties | `PascalCase` | `CurrentTime`, `IsOpen` |
| Public fields | `PascalCase` *(rare; prefer properties)* | |
| Private fields | `_camelCase` | `_clock`, `_currentCustomer` |
| Constants | `PascalCase` | `MaxInventorySlots = 12` |
| Static readonly | `PascalCase` | |
| Parameters & locals | `camelCase` | `customer`, `qualityScore` |
| Type parameters | `TPascalCase` | `TItem`, `TComponent` |
| Async methods | suffix `Async` | `SaveAsync()`, `LoadWorldAsync()` |
| Event names | `PascalCase`, past or present participle | `Saved`, `Saving`, `CustomerArrived` |
| Booleans | `Is/Has/Can/Should` prefix | `IsOpen`, `HasInventoryRoom`, `CanInteract` |

---

## Formatting

- **Braces on new line** for types, methods, properties
- **Braces on same line** for control flow (if/for/while/switch)
- **Always use braces** even for single-statement bodies
- **4-space indentation**, no tabs
- **120-column soft wrap**

```csharp
public sealed class NPCController : MonoBehaviour
{
    private readonly Memory _memory = new();

    public bool TryServeCustomer(NPC customer)
    {
        if (customer == null) {
            return false;
        }

        for (int i = 0; i < _stations.Length; i++) {
            // ...
        }

        return true;
    }
}
```

---

## Architecture rules

### Composition over inheritance
- No more than **one level** of inheritance for game-logic classes (component or interface preferred)
- Deep MonoBehaviour hierarchies are forbidden
- Use `interface` for polymorphic capability (`IInteractable`, `ISaveable`, `IDamageable`)

### MonoBehaviours vs. plain C# classes
- **MonoBehaviours** are the *view layer* — they listen for input, render state, and bridge Unity engine features (transforms, colliders, animators)
- **Plain C# classes** hold all *game logic* — they are testable, headless, and have no Unity dependencies
- Example: `NPCController : MonoBehaviour` (the view) wraps `NPCInstance : class` (the logic)

### Singletons
- **Forbidden** except for the `ServiceLocator` and `EventBus`
- Every other "global" is a service registered with the locator at boot
- Bootstrap scene registers all services in a known order

### Dependency direction
- `Core` depends on nothing
- `Time`, `World`, `Audio`, `Save` depend on `Core`
- `Characters`, `NPCs`, `Inventory`, `Currency`, `Dialogue`, `Interaction` depend on `Core` (+ each other narrowly)
- `Jobs` depends on the above
- `UI` depends on everything (it observes)
- **No cycles.** If you need a cycle, you're missing an abstraction.

### Communication between systems
- **Direct method calls** when one system owns another (Player owns Inventory)
- **EventBus** when systems must be decoupled (Jobs raises `CustomerServed`; UI listens)
- **No `FindObjectOfType` chains.** That pattern is forbidden in non-Editor code.

### Async vs. coroutines
- **`async/await`** for I/O and one-shot async work (file save, file load, future network)
- **Coroutines** still allowed for animation/timing where they read naturally (camera shake, fade, scheduled in-game events)
- **`UniTask`** is *not* used — built-in async/await is sufficient for our scale

### ScriptableObjects for data
- All *static* game content lives in ScriptableObjects:
  - Items, Recipes, NPC archetypes, Dialogue trees, Tile sets, Building parts, Schedules, etc.
- No hardcoded magic strings or numbers for game content
- Editor-authored, serialized, version-controlled as text

### Public surface area
- Default to `private` and `[SerializeField]` for everything inspector-bound
- Use `internal` for assembly-private types
- `public` only when an external caller actually needs it

```csharp
public sealed class CafeShift : MonoBehaviour
{
    [SerializeField] private PrepStation _coffeeStation;
    [SerializeField] private Recipe _defaultRecipe;

    private CafeShiftState _state;

    public bool IsOpen => _state == CafeShiftState.Open;

    public void Open()
    {
        // ...
    }
}
```

---

## Type modifiers

- **`sealed`** on all classes unless explicitly designed for inheritance
- **`readonly`** on all fields that don't change after construction
- **`record`** for plain data carriers where appropriate (C# 12 supports record class & record struct)
- **`struct`** only for small, immutable value types (rare in gameplay code)

---

## Performance defaults

- Atlas sprites per category; never one atlas per sprite
- **Object pooling** for: dialogue bubbles, particles, customer NPCs entering and leaving
- No `string` concatenation in hot paths — use `StringBuilder` or pre-formatted strings
- Avoid LINQ in tight loops (per-frame); LINQ is fine in setup, save/load, UI updates
- Profile before optimizing — but profile, don't guess

---

## Error handling

- **Throw** for *programmer errors* (a null where one is never valid; a switch reaching an impossible case)
- **Return result types or bools** for *expected failures* (a customer who couldn't be served because the queue is full)
- **Log warnings** for *recoverable anomalies* (missing dialogue line — substitute filler)
- **Log errors** for *unexpected bugs that don't crash* (save file missing field — apply default and warn)
- **Crashes are allowed** in development; never in shipping builds

```csharp
public bool TrySaveAsync(string slot, out SaveError error)
{
    if (string.IsNullOrEmpty(slot)) {
        throw new ArgumentException("Slot must not be empty.", nameof(slot));
    }

    if (!_saveDir.Exists) {
        error = SaveError.SaveDirectoryMissing;
        return false;
    }

    // ...
}
```

---

## Logging

- Use a single `Logger` wrapper (in `Scripts/Core/Logger.cs`), never raw `Debug.Log` in gameplay code
- Levels: `Trace`, `Info`, `Warn`, `Error`
- `Trace` and `Info` stripped from shipping builds
- Tag every log with module: `Logger.Info(Tag.NPCs, "Customer arrived: " + name)`

---

## Comments

- **Comments explain *why*, not *what*.** The code shows what. The comment shows intent, trade-off, constraint.
- **Avoid narrating comments.** `// Increment counter` is forbidden.
- **Public APIs get XML doc comments** (`///`) when the name isn't self-explanatory
- **TODO/FIXME/HACK** tagged comments include a name or ticket: `// TODO(@noah): handle dst-aware dates`
- **Don't comment out code.** Delete it. Git remembers.

---

## File layout

- One **public type** per file
- File name matches the type name exactly
- Private nested types allowed in the same file
- Order within a file:
  1. Usings (sorted, `System` first, then `Unity`, then `Legacy.*`, then `static using` last)
  2. Namespace
  3. Class declaration
  4. Constants
  5. Static fields
  6. Instance fields (`[SerializeField]` first, then private)
  7. Properties
  8. Events
  9. Unity lifecycle methods (Awake, OnEnable, Start, Update, OnDisable, OnDestroy)
  10. Public methods
  11. Private methods
  12. Inner types

---

## Testing

- **Unit tests** (Edit-mode) for all pure logic:
  - Quality scoring math
  - Time arithmetic
  - Dialogue state machine
  - Inventory operations
  - Save data round-trip
  - Wallet change-making
- **Integration tests** (Play-mode) for orchestration:
  - NPC schedule fires at expected in-game time
  - Save → load → state preserved
  - Customer visit state machine completes
- **Test file** mirrors source: `Scripts/Jobs/Cafe/CafeShift.cs` → `Tests/Jobs/Cafe/CafeShiftTests.cs`
- **Test name** describes behavior: `TrySaveAsync_WithEmptySlot_Throws()`

---

## Source-control hygiene

- One PR per system or per slice subtask
- Commit messages: `[Module] short summary` (`[NPCs] add scheduled arrival timer`)
- No commits of `Library/`, `Temp/`, `Logs/`, `Build/`, autogenerated `*.csproj`/`*.sln`
- Scenes and prefabs ForceText serialization (set in Editor → Project Settings)
- LFS for binary assets per [07-project-structure.md](07-project-structure.md)

---

## What we don't do

- **No Update spam.** Every `MonoBehaviour.Update` is a small global tax. Prefer central scheduling.
- **No magic strings for content.** Use enums or ScriptableObject references.
- **No premature optimization.** Profile before refactoring for perf.
- **No premature abstraction.** Build the second instance before generalizing the first.
- **No silent failures.** Either log, or surface, or crash. Never absorb.
- **No global mutable state** outside ServiceLocator-managed services.
- **No god classes.** If a class is over 300 lines, ask why. Over 500, refactor.

---

## A worked example

```csharp
// File: Assets/_Project/Scripts/Jobs/Cafe/MiniGames/GrindMiniGame.cs

using System;
using Legacy.Core;

namespace Legacy.Jobs.Cafe.MiniGames
{
    /// <summary>
    /// Grind step of the espresso prep loop. Pure logic; UI sits on top.
    /// </summary>
    public sealed class GrindMiniGame
    {
        private const float DurationSeconds = 2.0f;
        private const float SweetSpotCenter = 1.4f;
        private const float SweetSpotWidth = 0.4f;

        private float _elapsed;
        private bool _resolved;

        public bool IsResolved => _resolved;
        public float NormalizedTime => _elapsed / DurationSeconds;

        public void Tick(float deltaSeconds)
        {
            if (_resolved) {
                return;
            }

            _elapsed += deltaSeconds;
            if (_elapsed >= DurationSeconds) {
                _elapsed = DurationSeconds;
                Resolve();
            }
        }

        public int Resolve()
        {
            if (_resolved) {
                throw new InvalidOperationException("Already resolved.");
            }

            _resolved = true;

            float distance = MathF.Abs(_elapsed - SweetSpotCenter);
            float halfWidth = SweetSpotWidth * 0.5f;

            if (distance > halfWidth) {
                return 0;
            }

            float accuracy = 1f - (distance / halfWidth);
            return (int)MathF.Round(accuracy * 40f); // max 40 quality points
        }
    }
}
```
