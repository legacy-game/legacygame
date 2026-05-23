# System — UI Architecture

## Purpose
Render the heads-up display, floating interaction prompts, dialogue panels, modal screens, and notifications. Stay minimal, diegetic where possible, period-appropriate.

## Scope
- **[SPINE]** required for MVP
- UGUI (not UI Toolkit) — see [06-tech-stack.md](06-tech-stack.md)
- Pixel-perfect at 480×270 internal resolution

---

## UI layers

Five layers, one Canvas each, all `Screen Space - Overlay`. Layered by sort order.

| Layer | Sort order | Purpose |
|---|---|---|
| Background | 0 | Letterbox bars, fade-to-black overlay |
| HUD | 100 | Clock, wallet, status |
| World-space prompts | (separate world-space canvas, per-prompt) | Floating "[E] Talk" labels |
| Dialogue | 200 | Bottom-of-screen dialogue box |
| Modals | 300 | Inventory, menu, save, cash drawer |
| Notifications | 400 | Corner toasts |

---

## UI managers

```csharp
public sealed class UIManager : MonoBehaviour
{
    public HUDController HUD { get; private set; }
    public DialogueUIController Dialogue { get; private set; }
    public ModalUIController Modals { get; private set; }
    public NotificationUIController Notifications { get; private set; }
    public PromptUIController Prompts { get; private set; }

    public bool IsBlockingGameplay => Modals.IsAnyOpen;
}
```

Modal screens **pause the world clock**; dialogue does **not** (see [11-systems/time-and-day.md](11-systems/time-and-day.md)).

---

## HUD

Minimal. Top-right corner.

```
+--------------+
| Tue 06:30 AM |
| Day 1        |
|  $34.05      |
+--------------+
```

Components:
- `ClockHUD` — observes `WorldClock.MinuteTicked`, updates time
- `WalletHUD` — observes `Wallet.Changed`, updates total

**No XP bar. No health bar. No quest tracker. No minimap.**

When player is in dialogue, HUD slightly dims. When player is in cutscene (porch sunset), HUD fully fades out.

---

## Dialogue UI

See [11-systems/dialogue.md](11-systems/dialogue.md) for the dialogue system data. The UI:

### Dialogue Box
- Bottom of screen, ~80px tall, ~440px wide
- Pixel-frame border
- Speaker portrait (left, 64×64)
- Speaker name above text
- Typed-out text at ~30 chars/sec
- Press Space to skip / advance
- Responses listed as buttons bound to `[1]`..`[4]` keys

### Dialogue Bubble
- Floats above character's head
- Tail points to character
- Fades in (0.15s) → persists (1.5s + 0.05s per char) → fades out (0.15s)
- Max 2 lines, ~20 chars per line

---

## Modal screens

Each modal is a separate prefab loaded under the Modals canvas. Mutually exclusive — opening one closes others.

### MVP modals
- **Inventory & Character Sheet** — `[Tab]`
- **Pause Menu** — `[Esc]`
- **Cash Drawer** — opens during customer payment
- **Save / Load Slots** — from Pause Menu

### Modal behaviour
- Opening pauses the clock
- Background dim overlay (alpha 0.5 over the gameplay layer)
- `[Esc]` always closes the topmost modal
- Click outside modal area: configurable per modal (Inventory: yes; Cash Drawer: no)

---

## Notifications

Small corner toasts. Top-left in MVP (to avoid HUD).

| Kind | Tone | Example |
|---|---|---|
| Game | Quiet | "Saved." |
| Story | Slightly warmer | "June left her sandwich." |
| World | Neutral | "Rowan called." |
| Money | Subtle | "The till is $42 short." |

Toast structure:
- Small pixel-frame box
- Single line, ~30 chars max
- Slides in over 0.2s, holds 3.0s, fades out 0.5s
- Stacks vertically if multiple

No exclamation marks. No emoji. No "QUEST UPDATED!" energy.

---

## Floating prompts (world-space)

Per-interactable label that appears when player is in range.

```
[E] Talk to Mr. Holland
```

- World-space Canvas attached to the interactable
- Pixel-frame label with hotkey icon and verb + target
- Hotkey adapts to current input device (keyboard vs. gamepad)
- Disappears when interactable leaves range or `CanInteract` becomes false

---

## Pixel fonts

- Body: 8 px tall (raw pixel-art font)
- Headers: 12–16 px
- HUD: 8 px
- All nearest-neighbor scaled
- No bold/italic (use color or alternate font for emphasis if needed)

Recommended fonts (one or two for MVP):
- **Determination Mono** (free, pixel-perfect)
- **Pixel Operator** (free)
- Custom hand-drawn font once style is locked

---

## Input bindings (UI-relevant)

| Action | Default Key | Default Gamepad |
|---|---|---|
| Open Inventory | Tab | Y / Triangle |
| Pause | Esc | Start |
| Interact | E | A / X |
| Cancel / Back | Esc | B / Circle |
| Skip dialogue | Space | A / X |
| Confirm | Enter | A / X |
| Navigate | WASD / Arrows | D-pad / Left stick |
| Select response | 1–4 | Up/Down + A |

All rebindable via Input System.

---

## Localization

All player-facing strings via Unity Localization keys. Even English-only at launch.

Strings live in `Assets/_Project/Data/Localization/` tables. Format: `key | en | (future: other locales)`.

---

## Accessibility (MVP minimum)

See [06-tech-stack.md](06-tech-stack.md) for the full list. UI-specific:
- All text legible at 1×, 2×, 3×, 4× scale
- No information conveyed by color alone
- Subtitle/dialogue text always on (no toggle to hide)
- Settings menu has audio mute toggles per category

---

## Style discipline

The UI's job is to disappear. Players notice the HUD only when checking it. The dialogue box only when reading. The modal only when actively using. *Stardew Valley*'s UI is the reference.

If a piece of UI persistently draws attention away from the world, it is wrong.

---

## Testing

- Visual regression: HUD layout at 1080p, 1440p, 4K
- Integration: open inventory → clock pauses; close → clock resumes
- Integration: dialogue advances clock; modal does not
- Integration: notifications stack correctly when 3 arrive in quick succession
- Integration: rebind a key, persists across launches
