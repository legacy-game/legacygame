# Tools to Install

Everything you need installed before doing real work on *legacy*.

---

## Required

Install these before Phase 0 of [12-roadmap.md](12-roadmap.md).

### Unity 6.0.1f1
- Get it via **Unity Hub** (https://unity.com/download)
- Install **exact version 6.0.1f1** (locked per `ProjectSettings/ProjectVersion.txt`)
- Required modules:
  - Microsoft Visual Studio Build Tools (Windows) **OR** install Rider/VS Code separately
  - Build support modules: Windows Build Support (mandatory); macOS + Linux build support (optional)
- Disk space: ~15 GB

### Git
- https://git-scm.com/
- Verify: `git --version`
- On Windows, Git Bash comes bundled — use it for any shell commands in docs

### Git LFS
- https://git-lfs.github.com/
- After installing, run once globally: `git lfs install`
- Verify: `git lfs version`

### IDE — pick one

#### JetBrains Rider (recommended)
- Free for non-commercial use via **JetBrains Toolbox** (https://www.jetbrains.com/toolbox-app/)
- Best Unity integration, fastest C# editing experience
- Bundled with ReSharper-style refactoring

#### Visual Studio Code (alternative — free)
- https://code.visualstudio.com/
- Required extensions:
  - **C# Dev Kit** (Microsoft)
  - **Unity** (Microsoft)
  - **EditorConfig for VS Code** (Microsoft)
- Optional but recommended:
  - **GitLens** (Eric Amodio)
  - **Error Lens** (Alexander)

### Aseprite
- Pixel art standard
- $20 one-time on Steam (https://store.steampowered.com/app/431730/Aseprite/)
- Or buy direct: https://www.aseprite.org/

### Audacity
- Free audio editor for SFX cleanup
- https://www.audacityteam.org/

---

## Recommended

Install when you start working in that area.

### REAPER (DAW for composers)
- Free indefinite evaluation, $60 personal license
- https://www.reaper.fm/
- For music + advanced SFX composition

### OBS Studio
- Free
- https://obsproject.com/
- For recording playtests, devlog footage, gameplay clips

### Tiled
- Free, alternative tilemap editor
- https://www.mapeditor.org/
- Only if you prefer it over Unity's Tile Palette workflow

### GitHub Desktop
- Free GUI for git
- https://desktop.github.com/
- Optional — for those who prefer GUI over CLI

---

## Accounts to create

### GitHub
- https://github.com/
- Free private repo for the project
- Add team members as collaborators with appropriate permissions

### itch.io
- https://itch.io/
- Free
- Where the slice eventually ships
- Set up the project page in Phase 8

### Discord
- https://discord.com/
- Team communication + community channel
- Create dedicated server for the project

---

## Unity packages to add

These are not yet in [`Packages/manifest.json`](../Packages/manifest.json). Add them during Phase 0 via the Unity Package Manager.

Required:

| Package | Identifier | Version |
|---|---|---|
| Aseprite Importer | `com.unity.2d.aseprite` | latest stable |
| Pixel Perfect Camera | `com.unity.2d.pixel-perfect` | latest stable |
| Cinemachine | `com.unity.cinemachine` | latest stable |
| Newtonsoft Json | `com.unity.nuget.newtonsoft-json` | latest stable |
| Localization | `com.unity.localization` | latest stable |

Optional:

| Package | Identifier | Reason |
|---|---|---|
| 2D Sprite Shape | `com.unity.2d.spriteshape` | Curved geometry (rivers, paths) |
| Addressables | `com.unity.addressables` | Asset streaming — defer until needed |

---

## System requirements (developer machine)

### Minimum
- OS: Windows 10/11 64-bit (primary) **or** macOS 11+ **or** Ubuntu 22.04+
- CPU: 4-core, 2.5 GHz+
- RAM: 16 GB
- GPU: Anything from the last 5 years; DirectX 11 / Metal compatible
- Disk: 30 GB free (Unity + project + caches)

### Recommended
- CPU: 8-core
- RAM: 32 GB (large scenes and asset import gets memory-hungry)
- Disk: SSD with 100 GB free
- 1440p or higher monitor for the editor

---

## Configuration steps after installation

### Git config (per machine, not per repo)
```bash
git config --global user.name "Your Name"
git config --global user.email "you@example.com"
git config --global core.autocrlf input    # if on Windows: true
```

### Git LFS (per machine, once)
```bash
git lfs install
```

### Unity Editor settings (once per machine, for this project)
- **Edit → Preferences → External Tools → External Script Editor:** Rider or VS Code
- **Edit → Project Settings → Editor → Asset Serialization:** Force Text
- **Edit → Project Settings → Editor → Version Control:** Visible Meta Files

### IDE settings (Rider)
- Enable "Save documents on focus loss" so your scene changes commit naturally
- Configure C# formatting to match the conventions in [08-code-conventions.md](08-code-conventions.md) (most defaults are fine)

---

## Verifying your setup

After installation, you should be able to:

1. `git clone <repo>` → successful
2. `git lfs pull` → fetches binary assets
3. Open the project in Unity Hub → Unity opens without errors
4. Wait for asset import (5–15 min first time)
5. Open `Bootstrap.unity` → press Play → no errors, no pink textures, no missing scripts
6. Open Rider or VS Code → C# IntelliSense works on a `MonoBehaviour`

If any of these fail, ask in the team Discord — don't suffer in silence.

---

## What NOT to install

- **Unity 2023 / 2022 / 6000.x other versions** — version is locked at 6.0.1f1
- **Wwise / FMOD** — not used in MVP
- **Unity Cloud Build** — not used; we use GitHub Actions for CI later
- **Random Asset Store packages** — anything we use lives in `ThirdParty/`; don't pull random things in
- **Unity Hub auto-update** — pin the Hub version too if possible, since Hub updates can change behavior
