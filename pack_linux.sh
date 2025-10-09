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
cp "./src_old/AMBPatcher/bin/Release/net6.0/win-x64/publish/AMBPatcher.exe" "./dist/Sonic4ModLoader/AMBPatcher.exe"
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
mkdir -p "./dist/Sonic4ModLoader"

cp ./target/release/Sonic4ModManager ./dist/Sonic4ModLoader/Sonic4ModManager

# SHA256SUMS
echo "Creating SHA256SUMS..."
cd dist
find * -type f -exec sha256sum {} \; >> "SHA256SUMS_linux"

7z a "./Sonic4ModLoader_linux.zip" ./* -mx=9