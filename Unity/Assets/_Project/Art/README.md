# Art Authoring Pipeline

This folder is for source and imported game art. It defines conventions only; final pixel art should come later as content work.

## Folder Conventions

- `Sprites/Characters`: character sprites, portraits, and animation sheets.
- `Sprites/Environment`: tiles, props, buildings, interiors, signs, and world dressing.
- `Sprites/UI`: icons, cursors, HUD elements, and panel artwork.
- `Materials`: shared Unity materials for authored art.
- `Palettes`: palette references and swatches.
- `Placeholders`: temporary blockout sprites used only to wire gameplay or scenes.

## Import Defaults

Pixel art textures in `Sprites` and `Placeholders` should import as `Sprite (2D and UI)`, use `16` pixels per unit, point filtering, no mip maps, and no texture compression. Run `Legacy > Art Audio > Apply Import Defaults` after adding or moving art files, then run `Legacy > Art Audio > Validate Pipeline`.

Existing legacy placeholder art outside these folders is intentionally not rewritten by the validator; migrate it into the convention folders when it is ready to be standardized.

## Placeholder Rules

Placeholder art should be simple geometry, flat color, or clearly labeled blockout sprites. Do not treat placeholder art as shippable content, and keep any temporary hookups isolated enough to replace without gameplay rewrites.
