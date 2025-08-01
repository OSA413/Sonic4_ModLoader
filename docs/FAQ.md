# Welcome to the FAQ of the Mod Loader!

Can't find your question? Try to contact me somewhere on the Internet.

----------------------------

## How do I install Mod Loader?

TL;DR - a quick installation video guide is here: https://www.youtube.com/watch?v=CbeBXJief7w

0. Download Mod Loader. [Latest stable release](https://github.com/OSA413/Sonic4_ModLoader/releases/latest) is recommended, but you also can try the latest alpha version (if any available).

1. Extract all files from the archive to the root directory of the game (if you don't know what root directory is/where to find it, check the quick installation video guide above).

2. Launch `Sonic4ModManager.exe` and press "Yes" button on the First Launch Dialogue. You can also install it in the settings menu.

## Does this Mod Loader work with both Episode 1 and 2?

Yes!

## What is `mods_sha` folder for?

This folder stores file hashes of enabled mods (you can select which SHA algorithm to use, default is SHA-1). This is used to track every file change in mods (e.g. when you enable or disable mods or you install a newer version of it). If any file is changed, it will patch/repack original file. If no files were changed, it will do nothing (this saves time when you launch the game).

## I want to create a mod! Where should I start?

[See here.](https://github.com/OSA413/Sonic4_ModLoader#useful-documentation-for-modders-and-contributors)

## Linux?

Linux! At this moment you can use AMBPatcher and other programs written by me through Mono (except for One-Click Mod Installer, because of [Mono's implementation of Windows Forms](https://www.mono-project.com/docs/faq/winforms/#my-multithreaded-application-crashes-or-locks-up), I'll try to do something with this). You can also use Wine/Proton to run them.

**Note**: the last version to support Mono is [0.1.5.2](https://github.com/OSA413/Sonic4_ModLoader/releases/tag/v0.1.5.1). You can use it with Mono (like `$ mono ./AMBPatcher.exe`) to make mods work. The GUI apps should also work.

Modern versions of the Mod Loader components won't work with Mono and Wine/Proton (as of 2025). You can wait until I rewrite all the components to Rust so you will be able to run the programs natively or via Wine/Proton.

Here's a guide to make Episode 1 to work on Linux: https://steamcommunity.com/sharedfiles/filedetails/?id=1726034225

Episode 2 should work rigth out of the box using Proton `4.11-2` or later.

## Roadmap?

Check [Project page on GitHub](https://github.com/OSA413/Sonic4_ModLoader/projects).
