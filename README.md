# Sonic 4 Mod Loader

Looking for midday builds? Get it from [GitHub Actions](https://github.com/OSA413/Sonic4_ModLoader/actions)!

FAQ: https://github.com/OSA413/Sonic4_ModLoader/blob/master/docs/FAQ.md

**Table of content:**
* [What is this?](#what-is-this)
* [How to install the Mod Loader](#how-to-install-the-mod-loader)
* [How to uninstall the Mod Loader](#how-to-uninstall-the-mod-loader)
* [How to install mods](#how-to-install-mods)
* [Third party works](#third-party-works)
* [How to use a copy of 7-Zip from your system](#third-party-works)
* [How to automatically update Mod Loader](#how-to-automatically-update-mod-loader)
* [Bug reporting and suggestions](#bug-reporting-and-suggestions)
* [Useful documentation for modders](#useful-documentation-for-modders)
* [See also](#see-also)

## What is this?

This program is a mod loader for Sonic 4 (both episodes supported).

The Mod Loader includes:
* Mod Manager to manage your mods (enabling/disabling and changing mod priority)
* One-Click Mod Installer for an easier mod installation through the web and local archives
* AMBPatcher for editing .AMB files
* CsbEditor for editing .CSB files
* 7-Zip as a dependency for One-Click Mod Installer (you can use a copy from your system)

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

* CsbEditor (from SonicAudioTools) by Skyth under the MIT License.
https://github.com/blueskythlikesclouds/SonicAudioTools

* 7-Zip Copyright (C) Igor Pavlov.
[(License)](https://7-zip.org/license.txt)
https://7-zip.org

## How to use copy of 7-Zip from your system

1. Launch One-Click Mod Installer
2. Go to the Settings tab
3. Select "Paths" tab
4. Check "Use a local copy of 7-Zip from this computer"
5. Specify path to `7z.exe` in the text field below the checkbox

Note: if OCMI fails to find `7z.exe` in your path, it will try to use a copy that comes with Mod Loader.

## How to automatically update Mod Loader

1. Do one of the following things:
* Drag and drop archive/directory with Mod Loader on One-Click Mod Installer
* Copy-past the direct link to the Mod Loader archive (e.g. from GitHub) into the OCMI's url mod field in "Install mod" tab.
* Click `1-CLICK INSTALL` button on a service that supports 1-Click integration (e.g. GameBanana)
2. Press "Install"
3. Agree to replace current version with the downloaded one.

## Bug reporting and suggestions

Bugs reports and suggestions should be sent to GitHub's issues page of the Mod Loader:
https://github.com/OSA413/Sonic4_ModLoader/issues

For other types of discussions contact my on my [Twitter](https://twitter.com/OSA_413), [Discussion page on GitHub](https://github.com/OSA413/Sonic4_ModLoader/discussions), or [Join Sonic 4 Modding Discord server](https://discord.gg/WCp8BFyFxN).

## Useful documentation for modders
* How to use AMBPatcher: `AMBPatcher.exe --help`
* [Mod structure](https://github.com/OSA413/Sonic4_ModLoader/blob/master/docs/Mod%20structure.md)
* [Description of file extensions](https://github.com/OSA413/Sonic4_Tools/blob/master/docs/File%20description.md)
* [Tools by me + extra docs](https://github.com/OSA413/Sonic4_Tools)
* [Mod examples](https://github.com/OSA413/Sonic4_ModLoader_examples)

## See also

* [darealshinji's rewritten launcher for Episode 1](https://github.com/darealshinji/sonic-4-launcher) so you don't have to install Java to run it.

*~OSA413*
