# System — Inventory

## Purpose
Manage items the player and the world contain. Players carry a small inventory; containers (café storage, fridge, register, briefcase) hold their own inventories.

## Scope
- **[SPINE]** required for MVP
- Slot-based player inventory
- Container inventories for storage
- Items as ScriptableObject definitions
- No weight/encumbrance in MVP

---

## Concepts

### ItemDefinition
A ScriptableObject describing a *kind* of item — its name, sprite, stackability, max stack size, category.

### ItemStack
A runtime pair of (`ItemDefinition def`, `int count`).

### Inventory
A collection of `ItemStack` slots with a capacity.

---

## Data types

```csharp
[CreateAssetMenu(menuName = "Legacy/Inventory/Item")]
public sealed class ItemDefinition : ScriptableObject
{
    public string ItemId;            // "coffee_beans"
    public LocalizedString DisplayName;
    public Sprite Icon;
    public ItemCategory Category;
    public bool Stackable = true;
    public int MaxStack = 99;
    public LocalizedString Description;
    public float UnitPrice;          // base market price
}

public enum ItemCategory
{
    Ingredient,
    Tool,
    Document,
    Key,
    Money,           // (denominations stored separately, but unified pattern)
    Personal,
    Container,
    Other
}

[Serializable]
public sealed class ItemStack
{
    public ItemDefinition Def;
    public int Count;

    public bool IsEmpty => Count <= 0 || Def == null;
}
```

```csharp
public sealed class Inventory
{
    public int Capacity { get; }
    public IReadOnlyList<ItemStack> Slots { get; }

    public event Action<int> SlotChanged;     // emits slot index

    public Inventory(int capacity);

    public bool TryAdd(ItemDefinition item, int count, out int leftover);
    public bool TryRemove(ItemDefinition item, int count);
    public int CountOf(ItemDefinition item);
    public void Clear();

    public ItemStack StackAt(int index);
    public bool SwapSlots(int a, int b);
}
```

---

## Player inventory (MVP)

- **12 slots** for player carry
- Items used in slice:
 - Wallet (special; doesn't take a slot)
 - Recipe card (1 slot)
 - Café key (1 slot)
 - Notebook (1 slot, future use)
 - Carried item (currently-held food/drink to deliver) — uses a dedicated "hand" slot, not a backpack slot

### Hand slot
- Single dedicated slot for "what the player is currently carrying with both hands"
- Mutually exclusive with backpack
- Used in the café for carrying prepared items from prep station to customer

```csharp
public sealed class PlayerCarry
{
    public ItemStack HandStack { get; private set; }
    public bool HasHand => HandStack != null && !HandStack.IsEmpty;

    public bool TryPickUp(ItemStack stack);
    public ItemStack Drop();
}
```

---

## Container inventories (MVP)

| Container | Capacity | Purpose |
|---|---|---|
| Café storage (behind counter) | 24 slots | Ingredients (beans, milk, tea, pastries) |
| Café register (till) | special | Currency, not inventory items |
| Apartment fridge | 12 slots | Personal food |
| Apartment closet | 8 slots | Personal items |

Containers expose the same `Inventory` API. Player accesses them via interaction (open container → inventory swap UI).

---

## Recipe ingredient consumption

When player begins a recipe:
- Look up `Recipe.Ingredients[]` (ItemDef + count)
- Pull from café storage first; if missing, the recipe cannot start
- UI surfaces "out of stock" if ingredient missing

```csharp
public bool TryStartRecipe(Recipe recipe)
{
    foreach (var ingredient in recipe.Ingredients) {
        if (_storage.CountOf(ingredient.Def) < ingredient.Count) {
            _ui.Show($"Out of {ingredient.Def.DisplayName}.");
            return false;
        }
    }

    foreach (var ingredient in recipe.Ingredients) {
        _storage.TryRemove(ingredient.Def, ingredient.Count);
    }

    return true;
}
```

---

## Inventory UI

Modal screen. Opens on `[Tab]` (configurable). Pauses the clock per pause behavior.

### Layout
- Left: player inventory grid (4 × 3)
- Right: (when interacting with a container) container grid
- Bottom: item details panel showing icon, name, description, count
- Top: title, close button

### Interactions
- Click slot → select
- Click second slot → swap
- Right-click → context menu (Use, Drop, Inspect)
- Drag-drop → swap

---

## Item categories specific behaviors

| Category | Default behavior on Use |
|---|---|
| Ingredient | Pulled by recipes; can't be "used" alone |
| Tool | Activate context (cleaning rag → clean tables) |
| Document | Open as text (notebook, recipe card → opens reader UI) |
| Key | Opens specific door if held |
| Money | Not held in inventory; in Wallet (see [11-systems/currency.md](11-systems/currency.md)) |
| Personal | Display-only (souvenirs, gifts, mementos) |
| Container | Open to show its inventory (briefcase) |

---

## Save integration

Each container saved as a list of `(itemId, count)` per slot:

```jsonc
"player": {
  "inventory": [
    { "slot": 0, "itemId": "recipe_card", "count": 1 },
    { "slot": 1, "itemId": "cafe_key", "count": 1 }
  ],
  "hand": null
},
"cafe": {
  "storage": [
    { "slot": 0, "itemId": "coffee_beans", "count": 100 },
    { "slot": 1, "itemId": "milk", "count": 20 },
    { "slot": 2, "itemId": "pastry_croissant", "count": 4 },
    { "slot": 3, "itemId": "pastry_muffin", "count": 4 },
    { "slot": 4, "itemId": "tea_black", "count": 30 }
  ]
}
```

---

## Testing

- Unit: `TryAdd` respects capacity and stacking
- Unit: `TryRemove` correctly reduces or removes empty slots
- Unit: `SwapSlots` is symmetric
- Integration: start a recipe → ingredients consumed
- Integration: out-of-stock recipe → start fails, UI shows correct message
- Integration: save → load → all slots preserved

---

## Future considerations (V1.0+)

- Weight / encumbrance for travel
- Container hierarchies (box inside cabinet)
- Hot-bar for quick access during work
- Drag-drop crafting (drag ingredient onto station)
- Stash sharing in households
- Bank-deposit boxes
