Last updated: 08.17.2018

===================
What is this?
===================

This program is a mod loader for Sonic 4 (both episodes).
It contains a Mod Manager to manage your mods (enabling/disabling and mods priority), AMB Patcher to edit .AMB files, and CsbEditor to edit .CSB files.

Mod Manager and AMB Patcher by OSA413 under MIT License.
For information about third party programs refer to the "Third party works" section.


===================
How to install the Mod Loader
===================

1. Move all the files to root folder of Sonic 4 Episode (I or II).

2. Launch "Sonic4ModManager.exe". Mod Manager will offer to be configured automatically, press "Yes" button (if you press "No" button, refer to the "How to install mods manually" section).


===================
How to uninstall the Mod Loader
===================

If you haven't installed the Mod Loader yet, remove all the files that were added:

* AMBPatcher.exe
* CsbEditor.exe
* LICENSE
* LICENSE-SonicAudioTools
* ManagerLauncher.exe
* PatchLauncher.exe
* README (.txt, .rtf, .md)
* Sonic4ModManager.exe
* SonicAudioLib.dll

But if you have already installed it:

1.
~~~~~ Episode 1
Check if "Sonic_vis.orig.exe" and "SonicLauncher.orig.exe" are present. If they are, delete "Sonic_vis.exe" and "SonicLauncher.exe" and remove ".orig" part in the names of that files.

~~~~~ Episode 2
The same as for Episode 1, but "Sonic.orig.exe" and "Launcher.orig.exe"

2. Delete "mod_manager.cfg", "/mods/mods.ini" and "/mods/mods_prev", if exist.

3. Delete all the files listed above.


===================
How to install mods
===================

1. Create a "mods" folder in the root directory of the game (or press "Save" button in the Mod Loader)

2. Place your mod folder in that "mods" folder.
For more information refer to "How to make this Mod Loader work with your mod" section.

3. Enable the mod in the Mod Loader.


===================
How to install mods manually
===================

0. Follow the steps from "How to install mods" section.

1. Every time you change something in mods (files, mods priority, etc.), run "AMBPatcher.exe"


===================
How to make this Mod Loader work with your mod
===================

Changed something in a .CBS file? Name the folder with no extension.
Changed something in an .AMB file? Name the folder as the file.

Note: recursive AMB patching is not available right now.
For example, if you changed something in CPIT_MAIN.AMB/G_FIX.AMB file, you need to patch G_FIX.AMB manually.

Here's an example of mod structure:
~~~~~~~~
/mods/My Cool Mod/mod.ini
/mods/My Cool Mod/G_COM/CPIT/CPIT_MAIN.AMB/G_FIX.AMB
/mods/My Cool Mod/SOUND/SONICDL_SNG01/Synth/materials/SNG_EMERALD_AIF.aax/Intro.adx
~~~~~~~~


===================
mod.ini structure
===================

"mod.ini" file is not necessary, but recommended if you want to share your mod.

Place "mod.ini" in the root directory of your mod.
/mods/My Cool Mod/mod.ini

Here's an example of mod.ini structure:
~~~~~~~~
Name=My Cool Mod
Authors=Author0, Author1, and Author2
Version=0.1.0.0-rc3
~~~~~~~~
If "Name" is missed, folder name will be used instead.
Any other missing parameter will be replaced with "???"


===================
How to compile this program
===================

0. Download Visual Studio 2017 (C#)

1. Download source code of this Mod Loader: https://github.com/OSA413/Sonic4_ModLoader
Build everything from it.

2. Download source code of SonicAudioTools: https://github.com/blueskythlikesclouds/SonicAudioTools
Build only CsbEditor from it (SonicAudioLib.dll will be built as well).
Note: you will need to replace all ".csb" with ".CSB" in the source code of CsbEditor, otherwise it will not work.

3. Take all the executables (EXEs and DLLs) and place them somewhere together.
That's all.


===================
Third party works
===================

CbsEditor (from SonicAudioTools) by Skyth under MIT License.
https://github.com/blueskythlikesclouds/SonicAudioTools

Read LICENSE-SonicAudioTools for license (should be included in distributable versions).


===================
Managed to break the program? Found an error? Have suggestions?
===================

There are many ways to contact me in the Internet. One of them is through GitHub's issues:
https://github.com/OSA413/Sonic4_ModLoader/issues

~OSA413