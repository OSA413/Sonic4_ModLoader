# Sonic 4 Mod Loader

Looking for midday builds? Get it from [GitHub Actions](https://github.com/OSA413/Sonic4_ModLoader/actions)!

[![Midday build (Windows)](https://github.com/OSA413/Sonic4_ModLoader/actions/workflows/main_win.yml/badge.svg)](https://github.com/OSA413/Sonic4_ModLoader/actions/workflows/main_win.yml)

[![Midday build (Linux)](https://github.com/OSA413/Sonic4_ModLoader/actions/workflows/main_linux.yml/badge.svg)](https://github.com/OSA413/Sonic4_ModLoader/actions/workflows/main_linux.yml)

## To run it on Windows

In order to run it, you need [.Net 6 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and [.Net Framework 4.5.2 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net452) installed.

In order to run the games you will also need [Microsoft Visual C++ 2015 Redistributable](https://www.microsoft.com/en-us/download/details.aspx?id=52685) and [Latest Microsoft Visual C++ Redistributable](https://learn.microsoft.com/en-gb/cpp/windows/latest-supported-vc-redist?view=msvc-170#latest-microsoft-visual-c-redistributable-version) **both x86 and x64 for both packages**.

## To run it on Linux

Note for Linux users: [v0.1.5.1](https://github.com/OSA413/Sonic4_ModLoader/releases/tag/v0.1.5.1) is the last version that works with Mono (and probably Wine/Proton) (as for 2023).

(as for 2025) In order to run Mod Manager, you need to install these:
```
# Debian/Ubuntu and derivatives
# TODO

# Arch/Steam Deck and derivatives
sudo pacman -S gtk4 libadwaita
```

You also should use the `_linux` distribution of the Mod Loader

----------------

*If you are looking for AMBPatcher* (the predecessor of amb-rs):

* Rewritten version of AMBPatcher (used for PC modding as for 2025): [Release v0.1.6.2p1](https://github.com/OSA413/Sonic4_ModLoader/releases/tag/v0.1.6.2p1)
* Pre-rewritten version of AMBPatcher (should work with most AMB formats but is slow): [Release v0.1.4.3](https://github.com/OSA413/Sonic4_ModLoader/releases/tag/v0.1.4.3)

Join our [Sonic 4 Modding Discord server](https://discord.gg/WCp8BFyFxN)

FAQ: https://github.com/OSA413/Sonic4_ModLoader/blob/main/docs/FAQ.md

**Table of content:**

- [Sonic 4 Mod Loader](#sonic-4-mod-loader)
  - [What is this?](#what-is-this)
  - [How to install the Mod Loader](#how-to-install-the-mod-loader)
  - [How to uninstall the Mod Loader](#how-to-uninstall-the-mod-loader)
  - [How to install mods](#how-to-install-mods)
    - [Manually](#manually)
    - [Automatically](#automatically)
  - [Third party works](#third-party-works)
  - [How to automatically update Mod Loader](#how-to-automatically-update-mod-loader)
  - [Useful documentation for modders and contributors](#useful-documentation-for-modders-and-contributors)
  - [See also](#see-also)

## What is this?

This program is a mod loader for Sonic 4 (both episodes supported).

The Mod Loader includes:

* Mod Manager to manage your mods (enabling/disabling and changing mod priority)
* One-Click Mod Installer for an easier mod installation through the web and local archives
* AMBPatcher for editing .AMB files
* CsbEditor for editing .CSB files
* 7-Zip as a dependency for One-Click Mod Installer

## How to install the Mod Loader

TL;DR - a quick installation video guide is here: https://www.youtube.com/watch?v=CbeBXJief7w

Note: you need to install the Mod Loader separately for each Episode.

0. Download Mod Loader. [Latest stable release](https://github.com/OSA413/Sonic4_ModLoader/releases/latest) is recommended, but you also can try the latest alpha version (if any available).

1. Extract all files from the archive to the root directory of the game (if you don't know what root directory is/where to find it, check the quick installation video guide above).

2. Launch `Sonic4ModManager.exe` and press "Yes" button on the First Launch Dialogue. You can also install it in the settings menu.

If you want to enable 1-Click installation, launch `OneClickModInstaller.exe` and press "Install" button (requires administrator privileges).

## How to uninstall the Mod Loader

1. Go to the settings menu in the Mod Manager and select the "Installation" tab.
2. If "Uninstall" button is disabled or is named "Install", check "Force uninstall" box
3. Choose how Mod Loader should be uninstalled using uninstallation settings below.
4. Click "Uninstall" button

## How to install mods

### Manually

0. Create a `mods` folder in the root directory of the game if it's not present.
1. Place/extract your mod folder into that `mods` folder. The path to the `mod.ini` file should be something like `/mods/My Cool Mod/mod.ini`
2. Enable the mod in the Mod Manager.

### Automatically

Installation from a local archive/directory:

1. Drag and drop a mod archive/directory on `OneClickModInstaller.exe`
2. Press the "Install" button.
3. Enable the mod in the Mod Manager.

## Third party works

* Alice Mod Loader by RadiantDerg under the MIT License.
https://github.com/RadiantDerg/AliceModLoader

* CsbEditor (from SonicAudioTools) by Skyth under the MIT License.
https://github.com/blueskythlikesclouds/SonicAudioTools

* 7-Zip Copyright (C) Igor Pavlov.
[(License)](https://7-zip.org/license.txt)
https://7-zip.org

* GTK4 by GTK Team/GNOME under the LGPLv2.1+ license
https://gtk.org/

* GTK4 via gtk-rs under the MIT license https://gtk-rs.org/

* For other licenses, see `Sonic4ModLoader/Mod Loader - licenses` folder of a downloaded copy of the Mod Loader.

## How to automatically update Mod Loader

1. Do one of the following things:
* Drag and drop archive/directory with Mod Loader on One-Click Mod Installer
* Copy-past the direct link to the Mod Loader archive (e.g. from GitHub) into the OCMI's url mod field in "Install mod" tab.
* Click `1-CLICK INSTALL` button on a service that supports 1-Click integration (e.g. GameBanana)
2. Press "Install"
3. Agree to replace current version with the downloaded one.

## Useful documentation for modders and contributors
* How to use AMBPatcher: `AMBPatcher.exe --help`
* [Mod structure](https://github.com/OSA413/Sonic4_ModLoader/blob/main/docs/Mod%20structure.md)
* [Description of file extensions](https://github.com/OSA413/Sonic4_Tools/blob/master/docs/File%20description.md)
* [Modding as for its current state](https://gamebanana.com/tuts/14585)
* [How to compile Mod Loader](https://github.com/OSA413/Sonic4_ModLoader/blob/main/docs/compile.md)

## See also

* [darealshinji's rewritten launcher for Episode 1](https://github.com/darealshinji/sonic-4-launcher) so you don't have to install Java to run it.

*~OSA413*
