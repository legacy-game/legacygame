# System — Café Work Loop

## Purpose
The core MVP gameplay: working a shift at Linden Café. Take orders, make drinks and pastries via mini-games, serve, handle payment, optionally chat.

## Scope
- **[SPINE]** required for MVP
- Single café in MVP (the player's)
- Recipes: black coffee, espresso, cappuccino, latte, tea, croissant, muffin (~6–8 items)
- Mini-games: grind, tamp, pull shot, pour milk, oven bake, tea steep
- Customer visit state machine

---

## Open/close lifecycle

```
Closed → OpenAttempt → Open → CloseAttempt → Closing → Closed
```

| Action | Trigger |
|---|---|
| Open | Player interacts with door (interior side) → "Open shop" verb |
| Close | Player interacts with door (interior side) → "Close shop" verb |

While Closed, no NPC customers enter. While Open, scheduled customers may arrive within their windows.

---

## Recipe

```csharp
[CreateAssetMenu(menuName = "Legacy/Recipes/Recipe")]
public sealed class Recipe : ScriptableObject
{
    public string RecipeId;
    public LocalizedString DisplayName;
    public Sprite Icon;
    public decimal BasePrice;
    public List<IngredientRequirement> Ingredients;
    public List<RecipeStep> Steps;
    public int EstimatedSecondsToPrep;
}

[Serializable]
public sealed class IngredientRequirement
{
    public ItemDefinition Def;
    public int Count;
}

[Serializable]
public sealed class RecipeStep
{
    public RecipeStepKind Kind;
    public float StepWeight;  // 0–1 contribution to total quality
}

public enum RecipeStepKind
{
    Grind,
    Tamp,
    PullShot,
    PourMilk,
    OvenIn,
    OvenOut,
    Steep,
    Plate
}
```

### MVP recipes

| Recipe | Ingredients | Steps | Base Price |
|---|---|---|---|
| Black coffee | beans×1 | Grind → PullShot | $2.50 |
| Espresso | beans×1 | Grind → Tamp → PullShot | $2.75 |
| Cappuccino | beans×1, milk×1 | Grind → Tamp → PullShot → PourMilk | $3.50 |
| Latte | beans×1, milk×1 | Grind → Tamp → PullShot → PourMilk | $3.75 |
| Tea (black) | tea×1 | Steep | $2.25 |
| Croissant | pastry_croissant×1 | OvenIn → OvenOut (warmed) | $2.50 |
| Muffin | pastry_muffin×1 | OvenIn → OvenOut (warmed) | $2.75 |

---

## Customer visit state machine

```
Enter → Approach → Order → AwaitPrep → Receive → Pay → Chat? → Leave
```

```csharp
public enum CustomerVisitStateKind
{
    Enter,
    Approach,
    Order,
    AwaitPrep,
    Receive,
    Pay,
    Chat,
    Leave
}

public sealed class CustomerVisitState
{
    public CustomerVisitStateKind Current { get; private set; }
    public NPCInstance Customer { get; }
    public Recipe Order { get; private set; }
    public float TimeInState { get; private set; }
    public float QualityScore { get; private set; }
    public bool AwaitingPlayer => Current is CustomerVisitStateKind.Order
        or CustomerVisitStateKind.Receive
        or CustomerVisitStateKind.Pay
        or CustomerVisitStateKind.Chat;

    public void Tick(float dt);
    public void OnPlayerEngaged(PlayerContext player);
    public void OnRecipeCompleted(float quality);
    public void OnChangeGiven(decimal amount);
    public void OnPlayerChatted();
    public void OnPlayerDeclinedChat();
}
```

### Timeouts (NPC impatience)

| State | Timeout | If hit |
|---|---|---|
| Order | 60s | NPC says "Maybe another time" and leaves; relationship -1 |
| AwaitPrep | recipe base × 3 | NPC leaves; pays half price; relationship -1 |
| Receive | 30s | NPC walks to counter, takes item; -5 quality |
| Pay | 60s | Treated as customer overpaid (takes no change); customer cool |
| Chat | 20s | Default to "no chat"; no penalty |

---

## Prep stations

Each kind of recipe step is performed at a specific station:

| Station | Steps it handles |
|---|---|
| Grinder | Grind |
| Espresso machine | Tamp, PullShot, PourMilk |
| Oven | OvenIn, OvenOut |
| Kettle / steeping pot | Steep |
| Counter | Plate (final assembly) |

Stations are `IInteractable` (see [11-systems/interaction.md](11-systems/interaction.md)). Their `Priority` becomes `High` when the active recipe has a pending step that matches.

---

## Mini-games

See [05-vertical-slice-cafe-day.md](05-vertical-slice-cafe-day.md) for the exact numbers per mini-game.

Common contract:

```csharp
public interface IMiniGame
{
    void Begin();
    void Tick(float dt);
    void OnInput(MiniGameInput input);
    bool IsComplete { get; }
    int Score { get; }       // 0..MaxScore for that step
    int MaxScore { get; }
}
```

Each mini-game has its own UI overlay and its own audio. Mini-games run with the world clock un-paused (small time pressure for the busy rush) but inputs other than the mini-game are disabled.

### Quality aggregation

```csharp
float totalQuality = 0;
foreach (var step in recipe.Steps) {
    var stepQuality = miniGames[step.Kind].Score / (float)miniGames[step.Kind].MaxScore;
    totalQuality += stepQuality * step.StepWeight;
}
totalQuality = Mathf.Clamp01(totalQuality) * 100f;  // 0..100
```

---

## Pricing and tip math (recap)

See [11-systems/currency.md](11-systems/currency.md) for the math and the cash-drawer UI. Tip computed per [05-vertical-slice-cafe-day.md](05-vertical-slice-cafe-day.md):

```
finalTip = baseTip × (0.5 + (quality / 100) × 1.5) × (1.0 + (relationship / 10) × 0.5)
```

Rounded to nearest $0.05.

---

## Chat after pay

After the change is given, the "Chat?" prompt appears if the NPC's `Talkativeness` roll allows it (50% chance baseline at trait 0.5, scaled by trait).

If accepted:
- 1–2 dialogue exchanges from the NPC's `DialoguePool`
- Adds +1 relationship
- At relationship threshold, signature line fires

If declined:
- NPC nods, says farewell, leaves

---

## End of shift

Player interacts with door from interior side → "Close shop"
- OPEN sign flips to CLOSED
- Door visually locks
- Music shifts to "evening" band
- Any NPC currently in the café finishes their visit and leaves; no new NPCs arrive

Then player interacts with the register → "Count till"
- Modal shows day's earnings, tips, total, comparison to previous day
- Press confirm to log the day in history and proceed

---

## History log entries written by the café loop

| Event | Logged as |
|---|---|
| Customer served | `kind: CustomerServed`, `actor: <customerId>`, `details: { recipe, quality, tip }` |
| Signature line heard | `kind: NPCMemoryUnlocked`, `actor: <customerId>` |
| Day's total | `kind: DayClosed`, `details: { total, customers, tips }` |

---

## Save integration

```jsonc
"cafe": {
  "isOpen": false,
  "currentCustomerId": null,
  "currentRecipeId": null,
  "tillToday": 42.50,
  "tillLifetime": 42.50,
  "shiftsCompleted": 1,
  "storage": [ ... ],
  "till": { ... }
}
```

The current visit state is *not* saved across sessions — if the player saves mid-visit, the visit is cancelled on load (NPC leaves the scene cleanly).

---

## Testing

- Unit: quality aggregation across all step kinds
- Unit: tip math at boundary cases (quality 0, 100; rel 0, 10)
- Integration: full visit from enter → leave with high quality → tip in expected range
- Integration: timeout on AwaitPrep triggers NPC departure
- Integration: signature line fires at relationship threshold, not before, not twice
- Integration: close shop with NPC inside → NPC finishes and leaves

---

## Future considerations (V1.0+)

- Employees (NPC and player) running stations in parallel
- Menu customization (player chooses what to sell)
- Pricing control (player sets price; affects volume and tip mix)
- Inventory management (auto-ordering, supplier contracts)
- Health inspectors (rare, dramatic event)
- Reviews on the in-game forum (M5) affecting customer volume
- Multiple café locations (chains)
