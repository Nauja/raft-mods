# raft-mods

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/Nauja/raft-mods/master/LICENSE)

My own mods for the [Raft](https://store.steampowered.com/app/648800/Raft/) game.

## Installation

You can find pre-built versions of my mods at https://www.nexusmods.com (see links below).

It requires you to first install [BepInEx](https://www.nexusmods.com/site/mods/287) for Raft.

You will have to launch the game and close it once.

Then you can install each mod by dropping the dll file in the game's `BepInEx/plugins` folder.

## Build

Please refer to https://docs.bepinex.dev/master/articles/dev_guide/plugin_tutorial/1_setup.html for
setting up your development environment.

The commands below are for Visual Studio 2022, .NET Framework 4.8 Developer Pack, and BepInEx 5 Plugin.

Clone the repo with:

```
git clone https://github.com/Nauja/raft-mods.git
```

Next go the installation directory of Raft and copy `Raft_Data/Managed/Assembly-CSharp.dll` to `raft-mods/`.

Open `RaftMods.sln` with Visual Studio 2022.

Generate the solution which should produces `.dll` for all mods, i.e.:

```
InfiniteDurability\bin\Release\net48\InfiniteDurability.dll
```

## InfiniteDurability

![Raft](https://img.shields.io/badge/Raft-1.09+-blue)

Lets you activate infinite durability for equipment, consumables, and tools.

Mod at: https://www.nexusmods.com/raft/mods/87

<img src="https://staticdelivery.nexusmods.com/mods/3240/images/87/87-1712250118-1496865532.png" width="500px"/>
