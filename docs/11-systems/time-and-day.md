# System — Time and Day

## Purpose
Authoritative game-world clock. Drives NPC schedules, audio music director, lighting, save autosaves, and dialogue context.

## Scope
- **[SPINE]** required for MVP
- Single-region in MVP
- No timezone/DST simulation
- No leap years in MVP
- Calendar runs forward only

---

## Time scale

- **1 real second = 1 in-game minute** (configurable in settings)
- 1 in-game day = 24 in-game hours = 1440 in-game minutes = **24 real minutes**
- The first playable shared-world prototype uses the clock to drive citizen schedules, property/business activity, and world history. The old Café Day testbed used 06:30–18:30 as a narrow scenario, but the current milestone is broader than that single day.

### Sleep skip
At end of day, time advances to next day's 06:30 with a quick fade and ambient sleep audio. No "+8 hours of real waiting." Sleep is a hard cut.

---

## Calendar

- Date format: ISO 8601 internally (`2003-05-14`), Marennese friendly `Tuesday, 14 May 2003` in UI
- 7 days per week (Mon–Sun)
- 12 months per year
- 30 days per month (simplified — no leap, no varying month length) in MVP
  - Calendar with real month lengths is a v1.0+ enhancement

### Holidays / special days
- Out of scope for MVP
- v1.0+ adds named festival days (provincial fair, founder's day, etc.) with NPC dialogue shifts

---

## Core data types

```csharp
namespace Legacy.Time
{
    public readonly struct TimeOfDay : IEquatable<TimeOfDay>, IComparable<TimeOfDay>
    {
        public int Hour { get; }    // 0–23
        public int Minute { get; }  // 0–59

        public TimeOfDay(int hour, int minute);

        public int TotalMinutes => Hour * 60 + Minute;
        public TimeOfDay Add(int minutes);
        public override string ToString() => $"{Hour:00}:{Minute:00}";
    }

    public readonly struct GameDate : IEquatable<GameDate>
    {
        public int Year { get; }
        public int Month { get; }   // 1–12
        public int Day { get; }     // 1–30

        public string DayOfWeek { get; }  // "Monday"..."Sunday"

        public GameDate AddDays(int days);
    }

    public readonly struct GameDateTime
    {
        public GameDate Date { get; }
        public TimeOfDay Time { get; }
    }
}
```

---

## Core class

```csharp
namespace Legacy.Time
{
    public sealed class WorldClock
    {
        public GameDateTime Now { get; private set; }
        public float TimeScale { get; set; } = 60f; // in-game minutes per real second

        public event Action<TimeOfDay> MinuteTicked;
        public event Action<TimeOfDay> HourTicked;
        public event Action<GameDate> DayStarted;
        public event Action<GameDate> DayEnded;
        public event Action<TimeBand> TimeBandChanged;

        public bool IsPaused { get; set; }

        public void Tick(float deltaRealSeconds);

        public void SetTime(GameDateTime time);
        public void SkipTo(TimeOfDay target);

        public TimeBand CurrentBand { get; }
    }

    public enum TimeBand
    {
        Dawn,     // 04:00–07:00
        Morning,  // 07:00–11:00
        Midday,   // 11:00–14:00
        Afternoon,// 14:00–17:00
        Dusk,     // 17:00–19:00
        Evening,  // 19:00–22:00
        Night     // 22:00–04:00
    }
}
```

---

## Scheduler

Reads `ScheduledEvent` ScriptableObjects and dispatches via the EventBus when the clock crosses them.

```csharp
[CreateAssetMenu(menuName = "Legacy/Schedule/ScheduledEvent")]
public sealed class ScheduledEvent : ScriptableObject
{
    public TimeOfDay TriggerAt;
    public string EventKey;       // dispatched via EventBus
    public bool RepeatDaily = true;
}

public sealed class Scheduler : MonoBehaviour
{
    [SerializeField] private List<ScheduledEvent> _events;

    private void Awake() => _clock.MinuteTicked += OnMinute;

    private void OnMinute(TimeOfDay now)
    {
        foreach (var e in _events) {
            if (e.TriggerAt.Equals(now)) {
                EventBus.Publish(new ScheduledEventFired(e.EventKey));
            }
        }
    }
}
```

---

## Lighting integration

Single global gradient texture sampled by time of day:

```csharp
public sealed class TimeOfDayLighting : MonoBehaviour
{
    [SerializeField] private Light2D _sunLight;
    [SerializeField] private Gradient _sunColor;
    [SerializeField] private AnimationCurve _sunIntensity;

    private void Update()
    {
        float t = _clock.Now.Time.TotalMinutes / 1440f;
        _sunLight.color = _sunColor.Evaluate(t);
        _sunLight.intensity = _sunIntensity.Evaluate(t);
    }
}
```

Veyne_Exterior scene only; interiors use static lighting in MVP.

---

## Music director integration

The music director subscribes to `TimeBandChanged` and crossfades over 4 seconds. See [11-systems/audio-architecture.md](11-systems/audio-architecture.md).

---

## Pause behavior

- Modal screens (Inventory, Menu, Save) pause the clock (set `IsPaused = true`)
- Dialogue does NOT pause the clock — small time pressure during conversations matches the café-day pacing
- Special scripted moments (sunset porch, end-of-day fade) explicitly set the time scale and pause as needed

---

## Save integration

- `WorldClock.Now` and pause state are saved
- On load, clock resumes from the saved time

---

## UI

- HUD shows: `[Day name] [hh:mm AM/PM]` in the top-right corner
- Day counter (small "Day 1") below the clock, day 1, 2, 3... player can see how long they've been playing

---

## Testing

- Unit test `TimeOfDay.Add` with wraparound
- Unit test `GameDate.AddDays` with month/year rollover
- Integration test: scheduled event fires once per day at the right minute
- Integration test: load mid-day, verify next scheduled event still fires correctly

---

## Future considerations (V1.0+)

- Real month lengths (28/30/31 days, leap years)
- Holidays and seasonal NPC schedule overrides
- Per-region timezones (regions may be on different in-game clocks)
- Player-controllable time scale slider for cozy mode (1 real sec = 0.5 in-game min for slower play)
