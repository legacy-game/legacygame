# CI and Test Runner Notes

The repo is still early, so the most reliable CI target is the headless Unity EditMode suite. Build jobs should come after the project has a stable boot scene and build target settings.

## Local EditMode Validation

Use the Unity version pinned in `Unity/ProjectSettings/ProjectVersion.txt`.

Windows PowerShell:

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.1.1f1\Editor\Unity.exe" `
  -runTests `
  -batchmode `
  -projectPath "$PWD\Unity" `
  -testPlatform EditMode `
  -testResults "$PWD\Unity\EditModeResults.xml" `
  -logFile "$PWD\Unity\EditModeRun.log" `
  -quit
```

macOS/Linux shell:

```bash
Unity \
  -runTests \
  -batchmode \
  -projectPath "$PWD/Unity" \
  -testPlatform EditMode \
  -testResults "$PWD/Unity/EditModeResults.xml" \
  -logFile "$PWD/Unity/EditModeRun.log" \
  -quit
```

## CI Shape

Use `game-ci/unity-test-runner` first, with a matrix left for later:

```yaml
name: Unity EditMode Tests

on:
  pull_request:
  push:
    branches: [dev, main]

jobs:
  editmode:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
        with:
          lfs: true
      - uses: actions/cache@v4
        with:
          path: Unity/Library
          key: Library-${{ runner.os }}-${{ hashFiles('Unity/Packages/packages-lock.json') }}
          restore-keys: Library-${{ runner.os }}-
      - uses: game-ci/unity-test-runner@v4
        env:
          UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
          UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
          UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
        with:
          projectPath: Unity
          testMode: editmode
          artifactsPath: Unity/TestResults
```

## Merge Gate

For simulation, save, command, and multiplayer-prep changes, a PR should include:

- EditMode test command used.
- Result file or log summary.
- Any expected Unity warnings that are unrelated to the change.
- Whether `Unity/Library`, generated project files, and logs were excluded from the commit.
