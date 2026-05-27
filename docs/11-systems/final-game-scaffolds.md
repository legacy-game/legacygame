# Final-Game Scaffolds

These scaffolds reserve save-aware world records for systems that should exist in the final game, without adding active gameplay to the current cafe/day slice.

## Current Scope

The seed world creates disabled placeholder records for:

- family and legacy
- health and death
- government and laws
- crime and justice expansion
- frontier and settlement
- culture and artifacts
- moderation

Each scaffold is data-only: stable id, kind, display name, summary, implementation notes, creation time, and an explicit `IsGameplayEnabled` flag. The flag is false for every seeded placeholder.

## Non-Goals

Do not add full gameplay here yet:

- no generations, inheritance, aging, or death simulation
- no law engine, tax rules, courts, arrests, or punishment loops
- no autonomous settlement growth
- no artifact economy or culture scoring
- no network moderation workflow or policy automation

Future work should graduate one scaffold at a time into a focused slice with commands, history events, UI, and tests. Until then, these records are only map points so saves and tooling have stable names for the eventual systems.
