#!/bin/bash

mkdir -p "./dist/Sonic4ModLoader"

#Sonic4ModLoader
#License
cp "LICENSE" "./dist/Sonic4ModLoader/LICENSE-Sonic4_ModLoader"
#EXEs
cp "./Sonic4_ModLoader/AMBPatcher/bin/Release/AMBPatcher.exe" "./dist/Sonic4ModLoader/AMBPatcher.exe"
cp "./Sonic4_ModLoader/ManagerLauncher/bin/Release/ManagerLauncher.exe" "./dist/Sonic4ModLoader/ManagerLauncher.exe"
cp "./Sonic4_ModLoader/PatchLauncher/bin/Release/PatchLauncher.exe" "./dist/Sonic4ModLoader/PatchLauncher.exe"
cp "./Sonic4_ModLoader/Sonic4ModManager/bin/Release/Sonic4ModManager.exe" "./dist/Sonic4ModLoader/Sonic4ModManager.exe"
#READMEs
cp "./docs/README.rtf" "./dist/Sonic4ModLoader/README.rtf"
cp "./docs/README.txt" "./dist/Sonic4ModLoader/README.txt"
cp "./docs/README-tldr.txt" "./dist/Sonic4ModLoader/README-tldr.txt"


#Archiving
7z a "./dist/Sonic4ModLoader.7z" ./dist/* -mx=9

