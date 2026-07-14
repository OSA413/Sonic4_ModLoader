# Welcome to the FAQ of the Mod Loader!

Can't find your question? Try to contact me somewhere on the Internet.

----------------------------

## How do I install Mod Loader?

TL;DR - a quick installation video guide is here: https://www.youtube.com/watch?v=CbeBXJief7w

0. Download Mod Loader. [Latest stable release](https://github.com/OSA413/Sonic4_ModLoader/releases/latest) is recommended, but you also can try the latest alpha version (if any available).

1. Extract all files from the archive to the root directory of the game (if you don't know what root directory is/where to find it, check the quick installation video guide above).

2. Launch `Sonic4ModManager.exe` and press "Yes" button on the First Launch Dialogue. You can also install it in the settings menu.

## What is `mods_sha` folder for?

This folder stores file hashes of enabled mods. This is used to track every file change in mods (e.g. when you enable or disable mods or you install a newer version of it). If any file is changed, it will patch/repack original file. If no files were changed, it will do nothing (this saves time when you launch the game with patcher enabled).

## I want to create a mod! Where should I start?

[See here.](https://github.com/OSA413/Sonic4_ModLoader#useful-documentation-for-modders-and-contributors)

## Linux?

Linux! Read the `README.md` file in the root of the repository to get more info on using the Mod Loader.

Here's a guide to make Episode 1 to work on Linux: https://steamcommunity.com/sharedfiles/filedetails/?id=1726034225 (you may not need it with the latest Proton)

Episode 2 should work right out of the box using Proton `4.11-2` or later.
