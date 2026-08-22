# How to test

This doc will describe how I usually test the mod loader before each major changes release.

## On Windows (Episode 1) (base regression test)

1. Uninstall One Click Mod Installer
2. Clean install Episode 1 from Steam
3. Install the rewritten launcher for it.
4. Unarchive the new version of the mod loader to the root of the game.
5. Launch Mod Manager, it must ask for installation, install. Close it.
6. Launch One Click Mod Installer, install it. Close it.
7. Select a few mods from GameBanana. Regular mods and music/sound mods.
8. Install them one by one with the "1-CLICK INSTALL" button.
9. Install the mod with One Click Mod Installer, no errors should appear.
10. After installing the mods you want, use the OCMI's feature to launch Mod Manager after successful mod installation.
11. Tick some mods in the list of Mod Manager. Save and Play.
12. See that the mods installed.
13. Suffle the mods (e.g. with the random button). Save and Play. Do that several times. Don't forget to remember what mods are enabled and see that they are actually enabled and no other mods are.
14. Uncheck all mods. Save and Play. Get the vanilla game.
15. Testing complete.
