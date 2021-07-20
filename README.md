
# P4G PC Tiny Fixes

Various tiny fixes for Persona 4 Golden (Steam).

## Requirements

- [Reloaded II](https://github.com/Reloaded-Project/Reloaded-II/releases) - by Sewer56
- [Persona 4 Golden](https://store.steampowered.com/app/1113000/) - game version shouldn't matter (unless some future version fixes these issues :))

## Usage

1. Download Reloaded II.
2. Download and extract the latest release of this mod to `<Reloaded-II Path>/Mods/`.
3. Run Reloaded II.
4. Via the Reloaded II sidebar:
    1. Select *Add an Application*, add `P4G.exe`.
    2. Select *Download Mods*, download `reloaded.universal.steamhook`.
    3. Select *Manage Mods*, select *Steam Hook* and enable `P4G.exe`.
5. Select the P4G icon in the sidebar and enable *Steam Hook* and *Persona 4 Golden PC Tiny Fixes*.
6. Select *Launch Application* to start the game and load the mod.
7. Optionally, select *Create Shortcut* to create a desktop shortcut that will launch the game via Reloaded II automatically.

## Included Patches

### Intro Skip

Skips the boot logos and intro movie and boots directly to the main menu.
Disabled by default - can be enabled via the Reloaded II *Configure Mod* button.

### P4 Title Movie

Enable to play the vanilla P4 title movie on boot.
Disabled by default - can be enabled via the Reloaded II *Configure Mod* button.

## Included Fixes

### NG+ Save Slot Fix

Fixes a bug which causes Clear Data and New Game+ saves to appear incorrectly in menus.

This fix should work for all versions of the game (Japanese, English, Korean and Chinese).

| Original           | Fixed                  |
|:------------------:|:----------------------:|
| ![x](img/slot.png) | ![x](img/slot-fix.png) |

### Subtitle Fixes

Adjusts some English subtitles to fix cutoff issues with the **in-game** versions of some movies (these issues don't occur in the *TV Listings* section due to different subtitle scaling).

<details>
    <summary>Preview (click to expand)</summary>

| Original           | Fixed                  |
|:------------------:|:----------------------:|
| ![x](img/sub1.png) | ![x](img/sub1-fix.png) |
| ![x](img/sub2.png) | ![x](img/sub2-fix.png) |

</details>

### TexWrap Fix

Fixes an issue with the way some repeating textures (`GmoTexWrap`) are applied to surfaces.
Note that this fix hasn't been thoroughly tested and was mainly meant to fix the texture bug in the scene pictured below.

<details>
    <summary>Preview (click to expand)</summary>

| Original               | Fixed                      |
|:----------------------:|:--------------------------:|
| ![x](img/texwrap1.png) | ![x](img/texwrap1-fix.png) |
| ![x](img/texwrap2.png) | ![x](img/texwrap2-fix.png) |
| ![x](img/texwrap3.png) | ![x](img/texwrap3-fix.png) |

</details>
