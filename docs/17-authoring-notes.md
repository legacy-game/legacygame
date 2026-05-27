# Authoring Notes

These notes are for small Month 1 content changes that touch the seeded Veyne block. Keep additions data-first and make the shared world more testable before adding polish.

## Seeded World Validation

`WorldFactory.CreateVeyneSeedWorld()` is the source of truth for the current hand-authored seed. Edit-mode validation checks that seeded citizens, places, buildings, workplaces, accounts, job postings, visits, and job tasks only point at ids that exist.

Before opening a PR that changes the seed, run the edit-mode tests and include the result in the PR test plan.

## Adding Citizens

- Add a stable id such as `citizen_firstname` near the other ids in `WorldFactory`.
- Add the citizen with a real home building, workplace building, current region, current scene, and current place.
- If the citizen needs scheduled movement, assign an existing routine id or add a new routine in the schedule catalog.
- Give the citizen a money account when they participate in jobs, purchases, payments, or testing.
- Prefer a small role assignment over special-case UI or scene behavior.

## Adding Workplaces

- Add the building and interior place first, then add a `WorkplaceState` that points at them.
- Every workplace must have a business money account and a `WorkplaceInventoryState`, even if the starting inventory is tiny.
- Use owner citizen ids that already exist in the seed.
- Keep opening hours in minutes after midnight and make them plausible for the current morning test loop.
- Add or update job postings only after the workplace, business account, and inventory exist.

## Adding Job Tasks

- Add reusable task definitions to `JobTaskCatalog`; do not hard-code task behavior in the seeded world or UI.
- Point each task definition at an existing role and action kind.
- If the task consumes or produces inventory, seed enough workplace inventory to complete at least one task.
- Job postings should use job definitions from `JobCatalog` and point to an existing workplace.
- Runtime-created `JobTaskState` ids should include the workplace or scenario name, for example `task_linden_cafe_serve_001`.

## Month 1 Scope

For this milestone, authoring should prove the shared-world spine: citizens, workplaces, jobs, money, inventory, history, and save/load. Avoid adding gameplay/UI work just to make authored content visible unless the issue specifically calls for it.
