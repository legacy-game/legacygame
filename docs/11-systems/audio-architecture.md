# System — Audio Architecture

## Purpose
Runtime audio. Music director, SFX manager, ambient bed manager, mixer routing. Implements the [10b-audio-direction.md](10b-audio-direction.md) direction.

## Scope
- **[SPINE]** required for MVP
- Unity built-in `AudioSource` + `AudioMixer`
- Music director crossfades on time-of-day band changes
- Ambient bed crossfades on scene transitions
- SFX pooled, spatial pan based on world position

---

## Mixer routing

Single `AudioMixer` asset (`Assets/_Project/Audio/MasterMixer.mixer`) with this structure:

```
Master (0 dB)
├── Music (-6 dB default)
├── SFX (0 dB)
├── Ambient (-12 dB)
└── UI (-4 dB)
```

Each group has volume exposed parameters (`_volume_music`, `_volume_sfx`, etc.) for settings menu sliders.

Snapshots per major state (optional in MVP):
- `Default`
- `InDialogue` (ambient -3 dB, music unchanged)
- `InModal` (music -6 dB, ambient -6 dB)

---

## AudioManager

```csharp
public sealed class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _musicSourceA;   // for crossfade
    [SerializeField] private AudioSource _musicSourceB;

    private SFXPool _sfxPool;
    private MusicDirector _musicDirector;
    private AmbientBedManager _ambientManager;

    public void PlaySFX(string sfxId, Vector3 worldPosition = default, float volume = 1f);
    public void PlayUISound(string sfxId);
    public void SetMixerVolume(string group, float linearVolume);
    public void SetSnapshot(string snapshotName, float transitionSeconds);
}
```

---

## Music director

Drives which music loop plays based on:
- Current scene
- Current time band ([11-systems/time-and-day.md](11-systems/time-and-day.md))
- Special states (porch sunset)

### Selection table (MVP)

| Scene | Time band | Music id |
|---|---|---|
| Veyne_ApartmentInterior | Dawn / Morning | `mus_apartment_morning` |
| Veyne_ApartmentInterior | Evening / Night | `mus_apartment_evening` |
| Veyne_CafeInterior | Morning | `mus_cafe_morning` |
| Veyne_CafeInterior | Midday | `mus_cafe_midday` |
| Veyne_CafeInterior | Afternoon | `mus_cafe_afternoon` |
| Veyne_CafeInterior | Dusk | `mus_cafe_evening` |
| Veyne_Exterior | (matches the interior of the building closest by) | |
| (Special: porch sunset) | n/a | `mus_porch_sunset` (one-shot, plays uninterrupted) |
| MainMenu | n/a | `mus_menu_quiet` |

### Crossfade

Standard 4-second equal-power crossfade between music sources A and B. When switching:

```csharp
public void Crossfade(AudioClip nextClip, float seconds = 4f)
{
    var fadingOut = _activeSource;
    var fadingIn = (fadingOut == _musicSourceA) ? _musicSourceB : _musicSourceA;

    fadingIn.clip = nextClip;
    fadingIn.volume = 0f;
    fadingIn.Play();

    StartCoroutine(FadeRoutine(fadingOut, fadingIn, seconds));
    _activeSource = fadingIn;
}
```

### Special states

- **Porch sunset:** music director switches to `mus_porch_sunset` regardless of time band; plays full track once; auto-returns to band-appropriate music when scene changes.
- **End-of-day cash-up:** music fades to silence; on confirm, a small piano motif plays as a one-shot before fade-out.

---

## Ambient bed manager

Plays the per-scene ambient bed via a dedicated `AudioSource` separate from music. Crossfades on scene transition or when transitioning between bed-different regions of the same scene.

### Bed selection
| Scene | Bed id |
|---|---|
| Veyne_ApartmentInterior | `amb_apartment_morning` / `amb_apartment_evening` (time-band gated) |
| Veyne_CafeInterior (open) | `amb_cafe_murmur` |
| Veyne_CafeInterior (closed) | `amb_cafe_quiet` |
| Veyne_Exterior | `amb_street_day` / `amb_street_dusk` |

Bed crossfade: 1 second.

---

## SFX pool

A pool of 16 `AudioSource` instances on the AudioManager GameObject for fire-and-forget SFX.

```csharp
public sealed class SFXPool
{
    private readonly Queue<AudioSource> _free;
    private readonly List<AudioSource> _busy;
    private readonly Dictionary<string, AudioClip> _library;

    public void Play(string sfxId, Vector3 worldPosition, float volume = 1f)
    {
        if (!_library.TryGetValue(sfxId, out var clip)) return;

        var src = Dequeue();
        src.transform.position = worldPosition;
        src.clip = clip;
        src.volume = volume;
        src.pitch = JitterPitch(0.05f);   // small natural variation
        src.spatialBlend = 1f;           // 3D
        src.panStereo = 0f;
        src.Play();

        // returns to pool when finished (coroutine or callback)
    }
}
```

### Random variants
SFX with `_01`, `_02`, etc. variants are randomized at play time. The Manager exposes:

```csharp
audio.PlaySFX("sfx_footstep_wood", playerPos);  // resolves to random _NN variant
```

---

## Spatialization

- In-world SFX use 3D AudioSources with `spatialBlend = 1.0`
- Distance attenuation: linear rolloff from 0 to 10 world units
- 2D pan derived from screen position (Unity handles for us with the 2D Audio Listener)

We do **not** simulate real acoustics, reflections, occlusion. Cozy game, not a horror sim.

---

## Dialogue blips

Per-character archetype blip sample. Plays every ~3 displayed characters during typewriter text.

```csharp
public sealed class DialogueBlipPlayer
{
    public void Play(string archetypeId)
    {
        var clip = _clipsByArchetype[archetypeId];
        var pitch = _pitchByArchetype[archetypeId];
        _audio.PlayUISound(clip.name, volume: 0.6f, pitch: pitch);
    }
}
```

---

## Volume policy

| Source | Default mixer volume | Notes |
|---|---|---|
| Music | -6 dB | Quiet enough to talk over |
| Ambient beds | -12 dB | Background; should never overpower |
| SFX | 0 dB | Natural |
| UI | -4 dB | Slightly quieter than world SFX |

Ducking on dialogue snapshot: ambient -3 dB.

---

## Memory budget

Audio is one of the most memory-hungry asset categories. Discipline:
- Music **streamed** (not preloaded)
- Ambient beds **streamed**
- SFX **decompressed on load** (small files, fast access at runtime)

MVP estimate:
- 3 music tracks × ~3 MB each = ~9 MB
- 7 ambient beds × ~2 MB each = ~14 MB
- ~30 SFX × ~50 KB each = ~1.5 MB
- Total: **~25 MB audio** for the slice

---

## Save integration

Audio volumes per group are saved in `profile.json` user settings, not in `world.json`.

```jsonc
"audioSettings": {
  "master": 1.0,
  "music": 0.7,
  "sfx": 1.0,
  "ambient": 0.6,
  "ui": 0.8
}
```

---

## Testing

- Integration: time band changes → music crossfades over 4s without click
- Integration: scene change → ambient bed crossfades over 1s without click
- Integration: dialogue triggers snapshot, ambient ducks; ends, restores
- Integration: 16 concurrent SFX trigger; pool does not run out (rare warning if exceeded)
- Unit: blip volume + pitch resolution from archetype id

---

## Future considerations (V1.0+)

- Per-region beds (Old North, The Mills, Riverwalk sound different)
- Weather-aware beds (rain on roof, snow muffling)
- Music creation system integration: player-uploaded songs play on in-world radios / jukeboxes
- Music charts / radio rotations (player music plays via the music director)
- Spatialized voice chat
- Adaptive music (mini-game tension layers)
