# raft-mods

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/Nauja/raft-mods/master/LICENSE)

My own mods for the [Raft](https://store.steampowered.com/app/648800/Raft/) game.

## Installation

You can find pre-built versions of my mods at https://www.raftmodding.com/ (see links below).

It requires you to first install [Raft Mod Loader](https://api.raftmodding.com/getting-started/installing-raft-mod-loader) for Raft.

Then you can install each mod with the Install mod button from the website.

## Installation (BepInEx)

You can find pre-built versions of my mods at https://www.nexusmods.com (see links below).

It requires you to first install [BepInEx](https://www.nexusmods.com/site/mods/287) for Raft.

You will have to launch the game and close it once.

Then you can install each mod by dropping the dll file in the game's `BepInEx/plugins` folder.

## Build

Simply zip the `banner.jpg`, `icon.png`, `.cs`, `modinfo.json` files of desired mod and change the extension to `.rmod`.

Then right click on the file and choose to open it with Raft Mod Loader.

## Build (BepInEx)

Please refer to https://docs.bepinex.dev/master/articles/dev_guide/plugin_tutorial/1_setup.html for
setting up your development environment.

The commands below are for Visual Studio 2022, .NET Framework 4.8 Developer Pack, and BepInEx 5 Plugin.

Clone the repo with:

```
git clone https://github.com/Nauja/raft-mods.git
```

Next go the installation directory of Raft and copy the following files to `raft-mods/`:
* `Raft_Data/Managed/Assembly-CSharp.dll`
* `Raft_Data/Managed/UnityEngine.CoreModule.dll`
* `Raft_Data/Managed/UnityEngine.UI.dll`

Open `RaftMods.sln` with Visual Studio 2022.

Generate the solution which should produces `.dll` for all mods, i.e.:

```
InfiniteDurability\bin\Release\net48\InfiniteDurability.dll
```

## BepInExInfo (BepInEx)

![Raft](https://img.shields.io/badge/Raft-1.09+-blue)

Display BepInEx info in the main menu.

Mod at: https://www.nexusmods.com/raft/mods/88

<img src="https://staticdelivery.nexusmods.com/mods/3240/images/88/88-1712435073-429972946.jpeg" width="500px"/>

## InfiniteDurability (BepInEx)

![Raft](https://img.shields.io/badge/Raft-1.09+-blue)

Lets you activate infinite durability for equipment, consumables, and tools.

Mod at: https://www.nexusmods.com/raft/mods/87

<img src="https://staticdelivery.nexusmods.com/mods/3240/images/87/87-1712250118-1496865532.png" width="500px"/>

## InfiniteUse

![Raft](https://img.shields.io/badge/Raft-1.09+-blue)

Allow to toggle infinite use of items.

Mod at: https://www.raftmodding.com/mods/infinity-use

<img src="https://raw.githubusercontent.com/Nauja/raft-mods/main/InifiniteUse/banner.jpg" width="500px"/>
