  ____              _        _  _     __  __           _   _                    _           
 / ___|  ___  _ __ (_) ___  | || |   |  \/  | ___   __| | | |    ___   __ _  __| | ___ _ __ 
 \___ \ / _ \| '_ \| |/ __| | || |_  | |\/| |/ _ \ / _` | | |   / _ \ / _` |/ _` |/ _ \ '__|
  ___) | (_) | | | | | (__  |__   _| | |  | | (_) | (_| | | |__| (_) | (_| | (_| |  __/ |   
 |____/ \___/|_| |_|_|\___|    |_|   |_|  |_|\___/ \__,_| |_____\___/ \__,_|\__,_|\___|_|   
                                                                                            

Last updated: 12.23.2018

===================
Table of content:
* What is this?
* How to install the Mod Loader
* How to uninstall the Mod Loader
* How to install mods
* Third party works
* Bug reports and suggestions
* Useful documentation
===================


===================
What is this?
===================

This program is a mod loader for Sonic 4 (works with both episodes).
The whole Mod Loader includes:
* Mod Manager to manage your mods (enabling/disabling and changing mod priority)
* 1-Click Mod Installer for an easier mod installation through the web and local archives
* AMBPatcher for editing .AMB files
* CsbEditor for editing .CSB files
* 7-Zip as a dependency for 1-Click Mod Installer


===================
How to install the Mod Loader
===================

1. Extract all files from the archive to the root directory of the game.

2. Launch `Sonic4ModManager.exe` and press "Yes" button on the First Launch Dialogue.

If you want to enable 1-Click integration, launch `OneClickModInstaller.exe` and press "Install" button (requires administrator privileges).


===================
How to uninstall the Mod Loader
===================

0. Install the Mod Loader in the Mod Manager settings menu (yes, that's right)
1. Go to the settings menu in the Mod Manager, select the "Installation" tab.
2. Select the "Delete all Mod Loader files" radio button, check the "Recover original game files" box if you want it.
3. Click "Uninstall"

1-Click Mod Installer won't be uninstalled automatically. If you have enabled the 1-Click integration, launch the program and click "Uninstall" button. Then you can delete the `OneClickModInstaller.exe`


===================
How to install mods
===================

    Manually:

0. Create a `mods` folder in the root directory of the game if it's not present.
1. Place/extract your mod folder into that `mods` folder. The path to the `mod.ini` file must be something like `/mods/My Cool Mod/mod.ini`
2. Enable the mod in the Mod Manager.

    Automatically:

Installation from a local archive:
1. Drag and drop a mod archive on `OneClickModInstaller.exe`
2. Press the "Install" button.
3. Enable the mod in the Mod Manager.

From a web-site with 1-Click integration (e.g. GameBanana):
1. Press the "1-CLICK INSTALL" button on the mod page.
2. Press the "Download" button in the 1-Click Mod Installer
3. Enable the mod in the Mod Manager.


===================
Third party works
===================

CsbEditor (from SonicAudioTools) by Skyth under MIT License.
https://github.com/blueskythlikesclouds/SonicAudioTools

7-Zip Copyright (C) 1999-2018 Igor Pavlov.
License - http://7-zip.org/license.txt
https://7-zip.org


===================
Bug reports and suggestions
===================

Bugs reports and suggestions should be sent to GitHub's issues page of the Mod Loader:
https://github.com/OSA413/Sonic4_ModLoader/issues

For other types of discussions contact my somewhere on the Internet.


===================
Useful documentation
===================
Documentation index - https://github.com/OSA413/Sonic4_ModLoader/blob/master/docs/Index.md
* How to use AMBPatcher - https://github.com/OSA413/Sonic4_ModLoader/blob/master/docs/How%20to%20use%20AMBPatcher.md
* Mod structure - https://github.com/OSA413/Sonic4_ModLoader/blob/master/docs/Mod%20structure.md
* How to compile this program on Windows - https://github.com/OSA413/Sonic4_ModLoader/blob/master/docs/Compile%20on%20Windows.md
* How to compile this program on Linux - https://github.com/OSA413/Sonic4_ModLoader/blob/master/docs/Compile%20on%20Linux.md

~OSA413