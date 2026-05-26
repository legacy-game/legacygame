# Contributing to legacy

Thanks for helping with *legacy*. This is a hobby/volunteer project, so the workflow is designed to be lightweight: pick small tasks, avoid pressure, and keep changes easy to review.

## Current Goal

The current project goal is **One Veyne Morning**: a rough 10-15 minute playable morning where a tester can walk around one Veyne block, get or confirm a job, start a shift, complete job tasks, earn money, see history update, and save/load the consequences.

This is not a polish phase. Rough UI, programmer art, placeholder audio, and incomplete visuals are fine if the underlying loop is clearer and more testable.

## How Work Is Picked

No one is assigned work. Pick an issue only if you want to work on it.

1. Go to the Month 1 project board or issue list.
2. Pick an issue that fits your energy and skill set.
3. Comment `grabbing this`.
4. Create a branch from `dev`.
5. Open a pull request back into `dev`.
6. If you need to stop, comment `dropping this` or just leave it. No guilt.

Small PRs are better than perfect PRs. If something starts getting large, open a draft PR early.

## Branches

- `main` is for stable playable builds.
- `dev` is the integration branch for current work.
- Feature branches are for individual tasks.

Do not push directly to `main`.

Branch names should be short and descriptive:

```text
feature/current-shift-hud
feature/job-task-queue
fix/save-load-job-state
art/player-idle-sprite
audio/cafe-ambience-loop
tools/world-validator
```

## Pull Requests

Open pull requests into `dev`.

Every PR should include:

- what changed
- why it changed
- how to test it
- screenshots, video, or audio preview if relevant

Keep PRs focused. Do not mix unrelated code, art, audio, and refactors unless the issue explicitly requires it.

## Review Expectations

Review is about keeping the project coherent, not judging contributors.

- Simulation/world-state code should be reviewed by the backend/simulation owner and Noah.
- Unity gameplay/UI code should be reviewed by the gameplay owner and Noah.
- Tools/pipeline changes should be reviewed by the tools owner and Noah.
- Art/audio should be checked for readability and consistency, not final polish.

We may ask for changes if a PR:

- breaks the playable scene
- bypasses `WorldState`/commands for simulation changes
- adds unrelated scope
- makes a large rewrite without discussion
- includes generated noise or unrelated files

## Code Guidelines

- Keep gameplay state in `WorldState` or related pure C# state classes.
- Mutate world state through commands, not directly from UI.
- Unity UI and scene objects should present state and submit commands.
- Save/load must be considered for any durable world state.
- Meaningful actions should write history events.
- Prefer small, boring, testable systems over clever abstractions.

## Art Guidelines

The current target is readable placeholder art, not final assets.

- Use 32x32-friendly pixel art.
- Keep silhouettes readable.
- Prefer consistent rough style over polished mismatched assets.
- Name files clearly, e.g. `npc_holland_idle.png`, `prop_cafe_counter.png`.
- Source files such as `.ase` or `.aseprite` are welcome.

## Audio Guidelines

The current target is useful feedback and mood, not final mix.

- Short WAV/OGG files are preferred.
- Keep volume consistent.
- Avoid harsh spikes.
- Name files clearly, e.g. `ui_confirm.wav`, `door_open.wav`, `amb_cafe_loop.ogg`.

## Testing Guidelines

If you test the game, include:

- what scene you started from
- what you tried first
- where you got confused
- whether job flow made sense
- whether money/history/save-load behaved as expected

Current gameplay test loop:

1. Open `Unity/Assets/_Project/Scenes/WorldMemory_Exterior.unity`.
2. Register at the civic desk.
3. Open the Jobs UI.
4. Apply/accept/start a shift.
5. Enter the workplace.
6. Interact with the counter.
7. Complete the placeholder task UI.
8. Check money, history, inventory, and save/load.

## Git LFS

Install Git LFS before working with Unity assets:

```powershell
git lfs install
```

Binary assets such as images, audio, Unity scenes, prefabs, and ScriptableObject assets should stay under Git LFS according to the repo's `.gitattributes`.

## Communication

Use Discord for discussion and quick questions. Use GitHub issues and PRs for decisions, tasks, and review history.

Before starting a large rewrite or new system, discuss it first.

