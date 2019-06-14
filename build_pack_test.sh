#!/bin/bash
cd "$(dirname "$0")"

msbuild ./Sonic4_ModLoader/Sonic4_ModLoader.sln /p:Configuration=Release

rm -r "./dist"
mkdir -p "./dist/Sonic4ModLoader/Mod Loader - licenses"

#Sonic4ModLoader
#License
cp "LICENSE" "./dist/Sonic4ModLoader/Mod Loader - licenses/LICENSE-Sonic4_ModLoader"
cp "./docs/files" "./dist/Sonic4ModLoader/Mod Loader - licenses/LICENSE-Sonic4_ModLoader_files"
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

#This copies all files listed in "files" to dist
for dir in $(ls ./dependencies); do
    [ $dir == "readme.md" ] && continue

    for file in $(cat ./dependencies/$dir/files); do
        cp "./dependencies/$dir/$file" "./dist/Sonic4ModLoader/$file"
    done
    
    #And the "files" file
    cp "./dependencies/$dir/files" "./dist/Sonic4ModLoader/Mod Loader - licenses/LICENSE-"$dir"_files"
done

cp "./dependencies/7-Zip/License.txt"       "./dist/Sonic4ModLoader/Mod Loader - licenses/LICENSE-7-Zip"
cp "./dependencies/SonicAudioTools/LICENSE" "./dist/Sonic4ModLoader/Mod Loader - licenses/LICENSE-SonicAudioTools"

#Archiving
7z a "./dist/Sonic4ModLoader.7z" ./dist/* -mx=9

#Unit-testing
bash ./unit-tests/run_tests.sh
