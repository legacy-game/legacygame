# Audio Authoring Pipeline

This folder is for source and imported game audio. It defines routing and import conventions only; final music, ambience, and sound effects should come later as content work.

## Folder Conventions

- `Music`: long-form music loops and stems.
- `Ambience`: long-form scene beds and environmental loops.
- `Sfx/UI`: short interface feedback sounds.
- `Sfx/World`: short world interaction, movement, object, and action sounds.
- `Mixers`: Unity audio mixer assets and routing notes.
- `Snapshots`: mixer snapshot planning and transition notes.
- `Placeholders`: temporary silent or simple test clips used only while wiring systems.

## Mixer Routing

Route runtime audio through a master mixer with child groups for `Music`, `Ambience`, `Sfx`, and `UI`. The `AudioManager` scaffold exposes mixer group references and volume parameters so content can be assigned without changing gameplay code.

## Import Defaults

Music and ambience under `Music` and `Ambience` should stream, load in the background, and use compressed long-form settings. UI and world SFX under `Sfx` should decompress on load for responsive playback. Run `Legacy > Art Audio > Apply Import Defaults` after adding or moving audio files, then run `Legacy > Art Audio > Validate Pipeline`.
