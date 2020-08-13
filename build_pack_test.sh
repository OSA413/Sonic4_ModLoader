#!/bin/bash
#please, keep all sh scripts in Unix new line (LF)
cd "$(dirname "$0")"

echo "Compiling..."
msbuild ./Sonic4_ModLoader/Sonic4_ModLoader.sln /p:Configuration=Release -m

EXIT_CODE="$?"
if [ "$EXIT_CODE" != "0" ]; then
    exit $EXIT_CODE
fi

echo "Removing old distribution package..."
rm -rf "./dist"
mkdir -p "./dist/Sonic4ModLoader/Mod Loader - licenses"

echo "Copying new distribution files..."
#Sonic4ModLoader
#License
cp "LICENSE" "./dist/Sonic4ModLoader/Mod Loader - licenses/LICENSE-Sonic4_ModLoader"
#EXEs
cp "./Sonic4_ModLoader/AMBPatcher/bin/Release/AMBPatcher.exe" "./dist/Sonic4ModLoader/AMBPatcher.exe"
cp "./Sonic4_ModLoader/ManagerLauncher/bin/Release/ManagerLauncher.exe" "./dist/Sonic4ModLoader/ManagerLauncher.exe"
cp "./Sonic4_ModLoader/PatchLauncher/bin/Release/PatchLauncher.exe" "./dist/Sonic4ModLoader/PatchLauncher.exe"
cp "./Sonic4_ModLoader/Sonic4ModManager/bin/Release/Sonic4ModManager.exe" "./dist/Sonic4ModLoader/Sonic4ModManager.exe"
cp "./Sonic4_ModLoader/OneClickModInstaller/bin/Release/OneClickModInstaller.exe" "./dist/Sonic4ModLoader/OneClickModInstaller.exe"
#READMEs
cp "./README.md" "./dist/Sonic4ModLoader/README.md"
pandoc -s -f gfm -t rtf -o "./dist/Sonic4ModLoader/README.rtf" "./README.md"
#Change log
cp "./docs/Mod Loader - Whats new.txt" "./dist/Sonic4ModLoader/Mod Loader - Whats new.txt"

#Dependencies
for dir in $(ls ./dependencies); do
    [ $dir == "readme.md" ] && continue

    #Files
    for file in $(cat ./dependencies/$dir/files); do
        cp "./dependencies/$dir/$file" "./dist/Sonic4ModLoader/$file"
    done
    
    #License
    for file in $(ls ./dependencies/$dir); do
        if [ $file == "LICENSE" ] || [ $file == "License.txt" ]; then
            cp "./dependencies/$dir/$file" "./dist/Sonic4ModLoader/Mod Loader - licenses/LICENSE-"$dir
        fi
        if [ $file == "LINK" ]; then
            cp "./dependencies/$dir/$file" "./dist/Sonic4ModLoader/Mod Loader - licenses/LICENSE-"$dir"_link"
        fi
    done
done

echo "Creating SHA256SUMS..."
cd dist
find * -type f -exec sha256sum {} \; >> "SHA256SUMS"
cd ..

echo "Archiving..."
7z a "./dist/Sonic4ModLoader.7z" ./dist/* -mx=9

#7-Zip-less package
cp "./dist/Sonic4ModLoader.7z" "./dist/Sonic4ModLoader_7zip-less.7z"
7z d "./dist/Sonic4ModLoader_7zip-less.7z" "Sonic4ModLoader/Mod Loader - licenses/LICENSE-7-Zip" "Sonic4ModLoader/7z.exe" "Sonic4ModLoader/7z.dll"

#Tests
bash ./tests/run_tests.sh

#EXIT_CODE="$?"
#if [ "$EXIT_CODE" != "0" ]; then
#    exit $EXIT_CODE
#fi

