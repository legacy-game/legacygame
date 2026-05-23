# System — Dialogue

## Purpose
Drive all in-game spoken/written text between characters. Includes ambient bubbles, interactive dialogue boxes, NPC inner monologues (rare), and signature memory lines.

## Scope
- **[SPINE]** required for MVP
- Templated dialogue with conditional branching
- No LLM-driven generation
- Voice acting is *not* in MVP — text + character blips only

---

## Concepts

### Two surfaces
- **Bubble:** floating text above a character; ambient or single-line interactions. Auto-dismisses.
- **Dialogue Box:** bottom-of-screen panel; interactive multi-line exchanges with player input.

### Composition
- A **DialogueAsset** is a *pool* of `DialogueLine`s with their conditions
- A **DialogueLine** is a single utterance with optional responses

---

## Data types

```csharp
[CreateAssetMenu(menuName = "Legacy/Dialogue/Asset")]
public sealed class DialogueAsset : ScriptableObject
{
    public string AssetId;          // "holland_morning_pool"
    public List<DialogueLine> Lines;
}

[Serializable]
public sealed class DialogueLine
{
    public string Id;               // "holland_001"
    public string SpeakerArchetypeId;
    public LocalizedString Text;
    public List<DialogueResponse> Responses;
    public List<DialogueCondition> Conditions;
    public bool IsSignature = false;
    public string Tag;              // "greeting", "smalltalk", "memorable"
}

[Serializable]
public sealed class DialogueResponse
{
    public LocalizedString Text;
    public string NextLineId;       // optional
    public int RelationshipDelta;
}

[Serializable]
public sealed class DialogueCondition
{
    public ConditionKind Kind;
    public string Key;              // e.g. "relationship", "totalVisits"
    public ComparisonOp Op;
    public float Value;
}
```

---

## Selecting a line

Given an `NPCInstance` and a `DialogueAsset`, pick the most-specific line whose conditions are satisfied.

Selection rules:
1. Filter all lines whose conditions are satisfied by current `NPCContext`
2. From those, prefer lines with more conditions (more specific wins)
3. Tie-break by line `Tag` matching the current situation (greeting / smalltalk / payment)
4. Randomize among remaining ties (with last-line-spoken filter to avoid immediate repeats)
5. If the line is `IsSignature`, only fire once per NPC and mark `Memory.HasHeardSignatureLine = true`

---

## Conditions (MVP set)

| Condition | Example |
|---|---|
| `relationship >= 3` | Signature line gate |
| `totalVisits == 0` | First-time greeting |
| `totalVisits >= 1 && relationship < 3` | "Met before" greeting |
| `mood == Sad` | Sad-day dialogue variant |
| `currentDay >= 3 && lastSeenDay < currentDay - 3` | "Long time no see" line |
| `playerQualityLastServed >= 80` | Praise dialogue |
| `playerQualityLastServed < 30` | Disappointed dialogue |
| `currentTimeBand == Morning` | Morning-specific line |

Conditions are evaluated at line-pick time, not authored once.

---

## NPCContext

Passed into the dialogue selector. Built from the NPC instance + world state.

```csharp
public sealed class NPCContext
{
    public NPCInstance Npc;
    public int Relationship;
    public int TotalVisits;
    public Mood Mood;
    public TimeBand TimeBand;
    public float LastQualityServed;
    public bool HasHeardSignatureLine;
    public string LastDialogueTopic;
}
```

---

## DialogueSystem

```csharp
public sealed class DialogueSystem
{
    public DialogueLine SelectLine(DialogueAsset asset, NPCContext context, string tag = null);

    public IDialogueSession StartSession(DialogueAsset asset, NPCContext context);
}

public interface IDialogueSession
{
    DialogueLine CurrentLine { get; }
    DialogueResponse[] Responses { get; }
    bool IsFinished { get; }

    void Choose(int responseIndex);
    void Advance();   // for lines with no responses; advances to next or ends
    void Cancel();
}
```

---

## Dialogue UI

### Bubble UI
- Floating sprite-frame above the character's head
- Tail points to the character
- Fades in over 0.15s; persists 1.5s + 0.05s per character of text; fades out over 0.15s
- Pixel font; max 2 lines of ~20 characters
- Character blip SFX per ~3 letters (per character archetype)

### Dialogue Box UI
- Bottom of screen, pixel-frame panel
- Speaker name + portrait on the left
- Text appears typed-out at ~30 chars/second
- Press `[Space]` to skip to full text; press again to advance
- If responses available, list 1–4 options bound to `[1]–[4]` keys
- World does NOT pause; ambient beds duck slightly (-3 dB)

---

## Localization

All `LocalizedString` fields use Unity Localization tables. Each line has a stable Key. English-only at launch; keys allow later translation.

---

## Authoring workflow

1. Designer/writer creates a new `DialogueAsset` in `Assets/_Project/Data/Dialogue/`
2. Names it descriptively: `holland_morning_pool`
3. Adds lines, each with:
 - Speaker archetype id
 - Text
 - Optional conditions (small dropdown UI in inspector)
 - Optional responses
 - Optional `IsSignature` flag
4. Assigns the asset to the NPC archetype's `DialoguePool`
5. Playtest: walk into the café before 7am, after 7am, before relationship 3, after relationship 3

---

## Sample asset (excerpt — Mr. Holland)

```csharp
// asset: holland_morning_pool
Lines = [
    {
        Id: "holland_greet_first",
        SpeakerArchetypeId: "holland",
        Text: "Morning. Don't think I've had the pleasure. ...Black coffee, when you can.",
        Conditions: [ totalVisits == 0 ],
        Tag: "greeting"
    },
    {
        Id: "holland_greet_known",
        SpeakerArchetypeId: "holland",
        Text: "Morning, you. Black coffee, the usual.",
        Conditions: [ totalVisits >= 1 ],
        Tag: "greeting"
    },
    {
        Id: "holland_smalltalk_june",
        SpeakerArchetypeId: "holland",
        Text: "Saw your kid run past. ...She's getting tall.",
        Conditions: [ relationship >= 2 ],
        Tag: "smalltalk"
    },
    {
        Id: "holland_signature",
        SpeakerArchetypeId: "holland",
        Text: "You know, I taught for forty years in this town. Never had a worse student than this one kid in '78. Wonder where he ended up. Probably running for mayor.",
        Conditions: [ relationship >= 3, hasHeardSignatureLine == false ],
        Tag: "memorable",
        IsSignature: true
    }
]
```

---

## Dialogue blips (audio)

- Per character archetype, a short ~80ms blip sample
- Plays per ~3 displayed characters in the typed-out text
- Pitched slightly to match age/voice (Mara medium, Mr. Holland low, June higher, Sasha medium-low, Pell low, etc.)
- See [11-systems/audio-architecture.md](11-systems/audio-architecture.md) for the audio side

---

## Testing

- Unit: condition evaluation for each operator
- Unit: line selection respects "more specific wins"
- Unit: signature line fires once per NPC then never again
- Integration: walk up to Mr. Holland on day 1 → greeting matches `totalVisits == 0`
- Integration: walk up to Mr. Holland on day 4 with relationship 4 → signature line plays once
- Integration: save mid-conversation, load, session resumes correctly

---

## Future considerations (V1.0+)

- Branching conversations longer than 2 exchanges
- Multi-NPC group conversations (cafeteria scene)
- Dynamic interjection (a third NPC overhears and reacts)
- Player text input for limited free-form (writing a note, leaving a tip jar message)
- Optional voice acting for seeded NPCs
- Player-created dialogue (in-game NPC scripting? Out of scope for years.)
