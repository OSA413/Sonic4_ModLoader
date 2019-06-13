#!/bin/bash

msbuild ./Sonic4_ModLoader/Sonic4_ModLoader.sln /p:Configuration=Release

mkdir -p "./dist/Sonic4ModLoader"

#Sonic4ModLoader
#License
cp "LICENSE" "./dist/Sonic4ModLoader/LICENSE-Sonic4_ModLoader"
cp "./docs/files" "./dist/Sonic4ModLoader/LICENSE-Sonic4_ModLoader_files"
#EXEs
cp "./Sonic4_ModLoader/AMBPatcher/bin/Release/AMBPatcher.exe" "./dist/Sonic4ModLoader/AMBPatcher.exe"
cp "./Sonic4_ModLoader/ManagerLauncher/bin/Release/ManagerLauncher.exe" "./dist/Sonic4ModLoader/ManagerLauncher.exe"
cp "./Sonic4_ModLoader/PatchLauncher/bin/Release/PatchLauncher.exe" "./dist/Sonic4ModLoader/PatchLauncher.exe"
cp "./Sonic4_ModLoader/Sonic4ModManager/bin/Release/Sonic4ModManager.exe" "./dist/Sonic4ModLoader/Sonic4ModManager.exe"
#READMEs
cp "./docs/README.rtf" "./dist/Sonic4ModLoader/README.rtf"
cp "./docs/README.txt" "./dist/Sonic4ModLoader/README.txt"
#Change log
cp "./docs/Mod Loader - Whats new.txt" "./dist/Sonic4ModLoader/Mod Loader - Whats new.txt"

#Archiving
7z a "./dist/Sonic4ModLoader.7z" ./dist/* -mx=9

#Unit-testing
bash ./unit-tests/run_tests.sh
