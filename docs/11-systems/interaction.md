# System — Interaction

## Purpose
Provide the unified pattern for "the player approaches a thing and does something to it."

## Scope
- **[SPINE]** required for MVP
- Player → NPC (talk)
- Player → object (use, open, read, take)
- Player → station (start mini-game)
- Player → door (transition scene)

---

## Pattern

### Interface

```csharp
namespace Legacy.Interaction
{
    public interface IInteractable
    {
        InteractionPriority Priority { get; }   // for tie-breaking when multiple are in range
        string PromptVerb { get; }              // "Talk", "Open", "Take", "Read", "Use"
        string PromptObject { get; }            // "Mr. Holland", "Door", "Letter"

        bool CanInteract(PlayerContext player);
        void Interact(PlayerContext player);
    }

    public enum InteractionPriority
    {
        Low,        // ambient interactables (paintings)
        Normal,     // standard (NPCs, doors)
        High        // active context (the customer currently waiting at your counter)
    }
}
```

### Behaviour

```csharp
public sealed class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float _searchRadius = 1.2f;
    [SerializeField] private LayerMask _interactableLayer;

    private IInteractable _currentTarget;

    private void Update()
    {
        _currentTarget = FindBestInRange();
        _promptUI.SetTarget(_currentTarget);
    }

    public void OnInteractPressed()
    {
        if (_currentTarget != null && _currentTarget.CanInteract(_player)) {
            _currentTarget.Interact(_player);
        }
    }

    private IInteractable FindBestInRange()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, _searchRadius, _interactableLayer);
        IInteractable best = null;
        InteractionPriority bestPrio = InteractionPriority.Low;
        float bestDist = float.MaxValue;

        foreach (var hit in hits) {
            var inter = hit.GetComponent<IInteractable>();
            if (inter == null || !inter.CanInteract(_player)) continue;

            // higher priority wins; ties broken by distance
            if (inter.Priority > bestPrio ||
               (inter.Priority == bestPrio && Vector2.Distance(transform.position, hit.transform.position) < bestDist))
            {
                best = inter;
                bestPrio = inter.Priority;
                bestDist = Vector2.Distance(transform.position, hit.transform.position);
            }
        }

        return best;
    }
}
```

---

## Prompt UI

- Small floating label above the interactable
- Format: `[E] Talk to Mr. Holland`
- Hotkey is the configured `Interact` binding (defaults: `E` on keyboard, `A`/south button on gamepad)
- Appears when interactable comes into range; disappears when leaves
- Uses pixel-frame style, see [11-systems/ui-architecture.md](11-systems/ui-architecture.md)

---

## Examples

### NPC

```csharp
public sealed class NPCController : MonoBehaviour, IInteractable
{
    public InteractionPriority Priority { get; private set; } = InteractionPriority.Normal;
    public string PromptVerb => "Talk to";
    public string PromptObject => _instance.DisplayName;

    public bool CanInteract(PlayerContext player) => !_busy;

    public void Interact(PlayerContext player)
    {
        if (_visitState?.AwaitingPlayer == true) {
            _visitState.OnPlayerEngaged(player);
        } else {
            _dialogueSystem.PlayBubble(_instance, GreetingLine());
        }
    }
}
```

### Door (scene transition)

```csharp
public sealed class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string _targetSceneKey;
    [SerializeField] private string _spawnPointId;

    public InteractionPriority Priority => InteractionPriority.Normal;
    public string PromptVerb => "Open";
    public string PromptObject => "Door";

    public bool CanInteract(PlayerContext player) => !_locked;

    public void Interact(PlayerContext player)
    {
        _audio.Play("sfx_door_open");
        _sceneTransition.LoadAsync(_targetSceneKey, _spawnPointId).Forget();
    }
}
```

### Espresso machine (prep station)

```csharp
public sealed class PrepStation : MonoBehaviour, IInteractable
{
    [SerializeField] private RecipeStepKind _kind;

    public InteractionPriority Priority => _shift.HasPendingRecipeStep(_kind)
        ? InteractionPriority.High
        : InteractionPriority.Low;

    public string PromptVerb => "Use";
    public string PromptObject => _kind.ToString();

    public bool CanInteract(PlayerContext player) => _shift.HasPendingRecipeStep(_kind);

    public void Interact(PlayerContext player)
    {
        _shift.BeginStep(_kind, player);
    }
}
```

### Counter (give item to customer)

```csharp
public sealed class CafeCounter : MonoBehaviour, IInteractable
{
    public InteractionPriority Priority => _currentCustomer != null
        ? InteractionPriority.High
        : InteractionPriority.Normal;

    public string PromptVerb => _currentCustomer != null ? "Serve" : "Stand at";
    public string PromptObject => "Counter";

    public bool CanInteract(PlayerContext player) => true;

    public void Interact(PlayerContext player)
    {
        if (_currentCustomer != null && player.HoldingItem != null) {
            _currentCustomer.Receive(player.HoldingItem);
            player.Drop();
        }
    }
}
```

---

## What is NOT an interactable

- Tilemap (the world itself; the player walks on it but doesn't "interact")
- Cosmetic props with no behavior (paintings, plants — though they may become interactable in v1.0+)
- Other players (in MVP — out of scope)

---

## Testing

- Unit: priority tie-breaking by distance
- Unit: `CanInteract` returning false suppresses the prompt
- Integration: walk up to NPC, prompt appears; press `E`, dialogue starts
- Integration: door transitions player to correct scene at correct spawn point
- Integration: customer at counter takes priority over generic counter prompt

---

## Future considerations (V1.0+)

- Context-sensitive verbs: `Open` for unlocked doors, `Knock` for locked
- Hold-to-interact for long actions (clean tables, restock)
- Carry-and-deliver mechanics (carry pastry to specific table)
- Multi-target interactions (combine two interactables: pour coffee from pot into cup)
