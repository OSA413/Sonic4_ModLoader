#!/bin/bash

msbuild ./Sonic4_ModLoader/AMBPatcher/AMBPatcher.csproj /p:Configuration=Release
msbuild ./Sonic4_ModLoader/ManagerLauncher/ManagerLauncher.csproj /p:Configuration=Release
msbuild ./Sonic4_ModLoader/PatchLauncher/PatchLauncher.csproj /p:Configuration=Release
msbuild ./Sonic4_ModLoader/Sonic4ModManager/Sonic4ModManager.csproj /p:Configuration=Release
