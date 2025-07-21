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
cp ./target/release/ManagerLauncher.exe ./dist/Sonic4ModLoader/ManagerLauncher.exe
copydeps --search-dir C:/gtk-build/gtk/x64/release/bin ./dist/Sonic4ModLoader/ManagerLauncher.exe

# License - GTK4
dependenciesGtk=$'
cairo
fribidi
gtk4
harfbuzz
libadwaita
libepoxy
libffi
libjpeg-turbo
libpng
pango
pcre2
pixman
tiff
zlib
'

while IFS= read -r dep; do
    if [[ $dep != "" ]]; then 
        cp "C:/gtk-build/gtk/x64/release/share/doc/$dep/COPYING" "./dist/Sonic4ModLoader/Mod Loader - licenses/LICENSE-"$dep || \
        cp "C:/gtk-build/gtk/x64/release/share/doc/$dep/LICENSE" "./dist/Sonic4ModLoader/Mod Loader - licenses/LICENSE-"$dep || \
        cp "C:/gtk-build/gtk/x64/release/share/doc/$dep/LICENSE.md" "./dist/Sonic4ModLoader/Mod Loader - licenses/LICENSE-"$dep".md" || \
        cp "C:/gtk-build/gtk/x64/release/share/doc/$dep/README" "./dist/Sonic4ModLoader/Mod Loader - licenses/LICENSE-"$dep || \
        cp "C:/gtk-build/gtk/x64/release/share/doc/$dep/manual/html/project/license.html" "./dist/Sonic4ModLoader/Mod Loader - licenses/LICENSE-"$dep".html"
    fi
done <<< "$dependenciesGtk"

# SHA256SUMS
echo "Creating SHA256SUMS..."
cd dist
find * -type f -exec sha256sum {} \; >> "SHA256SUMS"

7z a "./Sonic4ModLoader.zip" ./* -mx=9