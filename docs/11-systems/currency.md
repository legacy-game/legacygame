# System — Currency

## Purpose
Cash economy for the slice. The Marennese dollar ($). Players take cash, give change, count their till at end of day.

## Scope
- **[SPINE]** required for MVP
- Cash only — no banks, no cards, no digital payment
- Multiple denominations (bills + coins)
- Player wallet + café till
- Manual change-making interaction

---

## Denominations

| Denomination | Type | Image notes |
|---|---|---|
| $0.05 | Coin | Nickel-style |
| $0.10 | Coin | Dime-style |
| $0.25 | Coin | Quarter-style |
| $1.00 | Coin | Large coin (or smallest bill — choose during art pass) |
| $1 | Bill | Smallest bill |
| $5 | Bill | |
| $10 | Bill | |
| $20 | Bill | |
| $50 | Bill | |
| $100 | Bill | Largest bill |

Decision: keep both $1 coin and $1 bill in the data layer; visually the slice can ship one or the other. Art pass decides.

---

## Data types

```csharp
public enum Denomination
{
    Coin_05,    // $0.05
    Coin_10,    // $0.10
    Coin_25,    // $0.25
    Coin_100,   // $1.00
    Bill_1,
    Bill_5,
    Bill_10,
    Bill_20,
    Bill_50,
    Bill_100
}

public static class DenominationValues
{
    public static decimal ValueOf(Denomination d) => d switch
    {
        Denomination.Coin_05  => 0.05m,
        Denomination.Coin_10  => 0.10m,
        Denomination.Coin_25  => 0.25m,
        Denomination.Coin_100 => 1.00m,
        Denomination.Bill_1   => 1.00m,
        Denomination.Bill_5   => 5.00m,
        Denomination.Bill_10  => 10.00m,
        Denomination.Bill_20  => 20.00m,
        Denomination.Bill_50  => 50.00m,
        Denomination.Bill_100 => 100.00m,
        _ => 0m
    };
}
```

```csharp
public sealed class Wallet
{
    private readonly Dictionary<Denomination, int> _counts = new();

    public event Action Changed;

    public decimal Total => _counts.Sum(kvp => DenominationValues.ValueOf(kvp.Key) * kvp.Value);

    public int CountOf(Denomination d);
    public void Add(Denomination d, int count);
    public bool TryRemove(Denomination d, int count);
    public bool TryRemoveAmount(decimal amount, ChangeStrategy strategy = ChangeStrategy.MinCoins);
    public IReadOnlyDictionary<Denomination, int> Snapshot();
}

public enum ChangeStrategy
{
    MinCoins,    // greedy — fewest physical pieces
    MaxCoins,    // pay everything in small (for splitting)
    Specific     // use a provided set
}
```

---

## Use of `decimal`

All money math uses `decimal`, never `float`. Reasons:
- No floating-point rounding errors
- Exact representation of $0.05 increments
- Matches accounting expectations

---

## Change-making algorithm

Greedy works for Marennese denominations because they are canonical (each larger denomination is a clean multiple of smaller ones).

```csharp
public static bool TryMakeChange(IReadOnlyDictionary<Denomination, int> available,
                                  decimal amount,
                                  out Dictionary<Denomination, int> result)
{
    result = new();
    var denoms = Enum.GetValues<Denomination>()
                     .OrderByDescending(DenominationValues.ValueOf)
                     .ToArray();

    decimal remaining = amount;
    foreach (var d in denoms) {
        var value = DenominationValues.ValueOf(d);
        int have = available.GetValueOrDefault(d, 0);
        int need = (int)(remaining / value);
        int take = Math.Min(have, need);
        if (take > 0) {
            result[d] = take;
            remaining -= take * value;
        }
    }

    return remaining == 0m;
}
```

If greedy fails (not enough of a denomination), the player must use a different mix — surfaced in the UI as "Can't make exact change."

---

## Payment flow at the café

1. Customer presents an order
2. Player serves item (mini-game produces a quality score and final price)
3. Customer drops a *single* bill or coin set on the counter (procedurally chosen — biased toward "convenient bill above the price")
4. Cash drawer UI opens
5. Player selects denominations to make change
6. Player presses "Confirm" — change goes to customer, payment goes to café till
7. Customer drops tip into jar (based on quality + relationship)
8. Customer leaves

### Wrong change handling
- If player gives short change: customer takes the short amount, tip reduced, relationship -1
- If player overpays: customer keeps it ("appreciated"), wallet drains, small relationship +
- Cancel button always available — payment reverts

---

## Cash drawer UI

Modal that appears on payment. World does not advance time during this UI (pauses for clarity).

```
+------------------------------------+
| Total due:     $4.25               |
| Customer paid: $5.00 (Bill_5)      |
| You owe back:  $0.75               |
|                                    |
| [Coin_05] x 0  [-] [+]             |
| [Coin_10] x 0  [-] [+]             |
| [Coin_25] x 3  [-] [+]             |
| [Coin_100] x 0 [-] [+]             |
| [Bill_1]  x 0  [-] [+]             |
| ...                                |
|                                    |
| Selected change: $0.75 (3 coins)   |
| [Confirm] [Cancel]                 |
+------------------------------------+
```

Auto-fill button: "Make Change (Optimal)" — runs greedy on player's wallet.

---

## Café till

Separate `Wallet` instance held by `CafeShift`. Receives payment (in source denominations from customer), gives out change (from till).

End of day:
- Player closes shop
- "Count till" interaction at register: opens till UI showing current total
- Total is recorded as "tillToday"; "tillLifetime" increments

---

## Wallet HUD

- Small icon + total amount in the HUD (top-right under clock)
- Click expands to show denominations (optional in MVP — could be inventory-screen only)

---

## Pricing

For MVP all prices are fixed in `Recipe.BasePrice` ScriptableObject field. No dynamic supply/demand yet. Tip computed per [05-vertical-slice-cafe-day.md](05-vertical-slice-cafe-day.md).

---

## Save integration

```jsonc
"player": {
  "wallet": {
    "Coin_05": 4,
    "Coin_10": 2,
    "Coin_25": 7,
    "Coin_100": 3,
    "Bill_1": 5,
    "Bill_5": 3,
    "Bill_10": 1,
    "Bill_20": 2,
    "Bill_50": 0,
    "Bill_100": 0
  }
},
"cafe": {
  "till": {
    "Coin_05": 20,
    "Coin_25": 10,
    "Bill_1": 30,
    "Bill_5": 10,
    "Bill_20": 2
  },
  "tillToday": 42.50,
  "tillLifetime": 42.50
}
```

---

## Testing

- Unit: `TryMakeChange` for canonical denominations always succeeds when total available ≥ amount
- Unit: `TryMakeChange` fails cleanly when no combination works
- Unit: `Total` matches sum of denominations
- Integration: payment flow start to end debits/credits correctly
- Integration: customer overpaying does not bug out
- Integration: save → load preserves both wallet and till

---

## Future considerations (V1.0+)

- Bank accounts (deposit till at end of day)
- Cheques (rare in MVP era; more relevant in v1.0)
- Credit cards (slightly anachronistic at 2003 small-town café but plausible later)
- Loans and debt
- Income tax (filed annually in-game; player makes a return)
- Foreign currency (other regions/countries)
