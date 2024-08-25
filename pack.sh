#!/bin/bash
cd "$(dirname "$0")"

echo "Removing old distribution package..."
rm -rf "./dist"
mkdir -p "./dist/Sonic4ModLoader/Mod Loader - licenses"

echo "Copying new distribution files..."
#Sonic4ModLoader
#License
cp "LICENSE" "./dist/Sonic4ModLoader/Mod Loader - licenses/LICENSE-Sonic4_ModLoader"
#EXEs
cp "./Sonic4_ModLoader/AMBPatcher/bin/Release/net6.0/win-x64/publish/AMBPatcher.exe" "./dist/Sonic4ModLoader/AMBPatcher.exe"
cp "./Sonic4_ModLoader/ManagerLauncher/bin/Release/net6.0-windows/win-x64/publish/ManagerLauncher.exe" "./dist/Sonic4ModLoader/ManagerLauncher.exe"
cp "./Sonic4_ModLoader/Sonic4ModManager/bin/Release/net6.0-windows/win-x64/publish/Sonic4ModManager.exe" "./dist/Sonic4ModLoader/Sonic4ModManager.exe"
cp "./Sonic4_ModLoader/OneClickModInstaller/bin/Release/net6.0-windows/win-x64/publish/OneClickModInstaller.exe" "./dist/Sonic4ModLoader/OneClickModInstaller.exe"
#README
cp "./README.md" "./dist/Sonic4ModLoader/README.md"
#Change log
cp "./docs/Mod Loader - Whats new.txt" "./dist/Sonic4ModLoader/Mod Loader - Whats new.txt"

#Dependencies
for dir in $(ls ./dependencies); do
    [ $dir == "readme.md" ] && continue

    #Files
    for file in $(cat ./dependencies/$dir/files); do
        if [[ $file == */. ]]
        then
            cp -r "./dependencies/$dir/$file" "./dist/Sonic4ModLoader/"
        else
            cp "./dependencies/$dir/$file" "./dist/Sonic4ModLoader/$file"
        fi
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

# Rust
cargo install copydeps

echo "Creating SHA256SUMS..."
cd dist
find * -type f -exec sha256sum {} \; >> "SHA256SUMS"
cd ..

echo "Archiving..."
7z a "./dist/Sonic4ModLoader.7z" ./dist/* -mx=9

#7-Zip-less package
cp "./dist/Sonic4ModLoader.7z" "./dist/Sonic4ModLoader_7zip-less.7z"
7z d "./dist/Sonic4ModLoader_7zip-less.7z" "Sonic4ModLoader/Mod Loader - licenses/LICENSE-7-Zip" "Sonic4ModLoader/7z.exe" "Sonic4ModLoader/7z.dll"

