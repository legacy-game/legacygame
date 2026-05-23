# System — NPCs

## Purpose
Bring the world to life. Each NPC has a generated personality, a schedule, dialogue, and memory of the player.

## Scope
- **[SPINE]** required for MVP
- 9 named NPCs in MVP (player family + 6 customers)
- Generated personalities, not LLM-driven
- Templated dialogue + signature lines
- Daily schedules with simple actions
- Per-NPC relationship score and memory

---

## Concepts

### NPC Archetype (template)
Authored as a ScriptableObject. Defines the *kind* of NPC — appearance, personality range, dialogue pool, default schedule.

### NPC Instance (runtime)
Spawned from an archetype. Holds runtime state — current schedule step, current mood, relationship with the player, memory, position.

The same archetype can spawn many instances (e.g. "background pedestrian" archetype spawning many randomly-named NPCs walking the street). Named MVP NPCs each have a unique 1-to-1 archetype.

---

## Archetype definition

```csharp
[CreateAssetMenu(menuName = "Legacy/NPCs/Archetype")]
public sealed class NPCArchetype : ScriptableObject
{
    public string ArchetypeId;        // "holland"
    public string DefaultName;        // "Mr. Holland"
    public int AgeRange_Min = 62;
    public int AgeRange_Max = 62;
    public Sprite Portrait;

    // Personality (rolled at spawn within these ranges)
    [Range(0, 1)] public float WarmthMin = 0.7f;
    [Range(0, 1)] public float WarmthMax = 0.9f;
    [Range(0, 1)] public float TalkativenessMin = 0.6f;
    [Range(0, 1)] public float TalkativenessMax = 0.8f;
    [Range(0, 1)] public float GenerosityMin = 0.6f;
    [Range(0, 1)] public float GenerosityMax = 0.8f;

    public List<Recipe> PreferredOrders;
    public DialogueAsset DialoguePool;
    public DialogueLine SignatureLine;
    public int SignatureThreshold = 3;

    public ScheduleAsset DefaultSchedule;
    public string CharacterSpriteId;  // resolves to sprite layers
}
```

---

## Instance state (runtime + saved)

```csharp
public sealed class NPCInstance
{
    public string ArchetypeId;        // links to archetype
    public string Id;                 // unique runtime id (often same as archetype for named NPCs)
    public string DisplayName;        // may differ for procedural NPCs

    // Rolled personality (within archetype ranges)
    public PersonalityTraits Traits;

    // Persistent state
    public int Relationship;          // 0..10 for slice
    public Memory Memory;
    public Mood CurrentMood;

    // Position
    public string CurrentScene;
    public Vector2 Position;
    public Facing Facing;

    // Schedule
    public int CurrentScheduleStepIndex;
    public bool ScheduleCompleteForDay;
}
```

```csharp
public readonly struct PersonalityTraits
{
    public float Warmth;            // 0..1
    public float Talkativeness;     // 0..1
    public float Generosity;        // 0..1
}

public enum Mood { Bright, Tired, Sad, Anxious, Content }

public enum Facing { Down, Up, Left, Right }
```

---

## Memory

Minimal in MVP. Tracks per-NPC:

```csharp
public sealed class Memory
{
    public int LastSeenDay;
    public TimeOfDay LastSeenTime;
    public float LastQualityServed;
    public string LastDialogueTopic;
    public int TotalVisits;
    public bool HasHeardSignatureLine;
    public List<string> OpenLoops;     // unresolved topics for callback
}
```

### Greeting variation buckets
Based on memory, greetings split into 3 buckets:
- **Never met** (`TotalVisits == 0`): "Hi, what can I get you?"
- **Met before** (`TotalVisits >= 1 && Relationship < 3`): "Hey, welcome back. The usual?"
- **Regular** (`Relationship >= 3`): "Morning, you." / character-specific warm greeting

---

## Schedule

```csharp
[CreateAssetMenu(menuName = "Legacy/Schedule/Schedule")]
public sealed class ScheduleAsset : ScriptableObject
{
    public List<ScheduleStep> Steps;
}

[Serializable]
public sealed class ScheduleStep
{
    public TimeOfDay StartTime;
    public ScheduleAction Action;
    public string LocationKey;          // resolves to a scene location
    public string TargetKey;            // e.g. recipe id for OrderAt
    public int DurationMinutes;
}

public enum ScheduleAction
{
    WalkTo,        // pathfind to location, stop
    Sit,           // sit at the location's seat
    OrderAt,       // order from a player-run business
    Wait,          // do nothing for DurationMinutes
    Leave          // exit the scene
}
```

### Example schedule — Mr. Holland (Tuesday)

```text
07:35  WalkTo  Cafe_Entrance
07:36  WalkTo  Cafe_Counter
07:36  OrderAt Cafe_Counter   target=coffee_black
07:42  WalkTo  Cafe_Window_Seat
07:42  Sit     Cafe_Window_Seat   duration=8min
07:50  Leave   Cafe_Entrance
```

NPCs out of their schedule windows simply don't exist in the scene (despawned from view).

---

## NPCController (MonoBehaviour)

Bridges the runtime `NPCInstance` (pure logic) to the Unity view (transform, animator, sprite renderer).

```csharp
public sealed class NPCController : MonoBehaviour
{
    [SerializeField] private NPCArchetype _archetype;

    private NPCInstance _instance;
    private CharacterMover _mover;
    private CustomerVisitState _visitState;

    private void Update()
    {
        _instance.UpdatePosition(transform.position);
        _visitState?.Tick(Time.deltaTime);
    }

    public void Interact(PlayerContext player)
    {
        if (_visitState?.AwaitingPlayer == true) {
            _visitState.OnPlayerEngaged(player);
        } else {
            // ambient greeting only
            _dialogueSystem.PlayBubble(_instance, GreetingLine());
        }
    }
}
```

---

## Pathfinding

- **A\* on the Unity Tilemap**
- Per-region walkability grid built from tilemap layers tagged `Walkable`
- NPCs claim their next tile and wait if blocked (no RVO)
- For slice scale (≤ 10 active NPCs per scene), grid A\* is plenty fast

---

## Personality → behavior mapping

These map traits to small behavioral knobs. Tuning numbers — adjust during playtest.

| Trait | Affects |
|---|---|
| **Warmth** | Greeting tone selection; tip base multiplier (× 0.9 to × 1.1); preference for short vs. extended chat |
| **Talkativeness** | Probability the "Chat?" prompt extends to 2nd exchange; ambient bubble frequency while idle |
| **Generosity** | Base tip amount (× 0.7 cold, × 1.0 average, × 1.3 generous) |
| **Mood** | Selects a slight dialogue tone shift; can override Warmth temporarily (a Warm NPC having a Sad day) |

---

## Background NPCs (out of MVP scope, planning)

For v1.0+:

- Procedurally generated NPCs spawning at scene edges
- Random first/last name from a Marennese name pool
- Trait sets rolled at spawn, persisted only while they're "named" to player
- Default schedule: walk through the scene and leave
- A small percentage become "regulars" after random number of visits — get persisted

---

## Save integration

Per-NPC saved fields (see [09-save-system.md](09-save-system.md)):

- `id`
- `displayName`
- `relationship`
- `lastSeenDay`
- `lastTopic`
- `mood`
- `metPlayer`
- `totalVisits`
- `hasHeardSignatureLine`

Archetype data is not saved — it's authored content.

---

## Spawning

- At scene load, spawn all NPCs whose schedule starts within the loaded time window
- At schedule trigger, NPC fades in at scene edge (or wakes from idle if pre-spawned at home)
- After `Leave`, NPC fades out at scene edge

---

## Testing

- Unit: personality trait rolling stays within archetype ranges
- Unit: schedule step progression with time advance
- Integration: load scene at 07:30 → Mr. Holland visible at 07:35
- Integration: save mid-visit, load, NPC resumes correct schedule step
- Integration: serve customer at quality 100 → tip in expected range
