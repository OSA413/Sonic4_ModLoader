#!/bin/bash
set -e
cd "$(dirname "$0")"

rm -rf "dependencies_source"
mkdir "dependencies_source"
cd "dependencies_source"

#SonicAudioTools
mkdir "SonicAudioTools"
cd "SonicAudioTools"
url=$(curl -LIs -w %{url_effective} -o /dev/null https://github.com/blueskythlikesclouds/SonicAudioTools/releases/latest)
version=$(basename $url)
curl -L https://github.com/blueskythlikesclouds/SonicAudioTools/releases/download/$version/SonicAudioTools.7z > SonicAudioTools.7z
curl https://raw.githubusercontent.com/blueskythlikesclouds/SonicAudioTools/master/LICENSE.md > LICENSE.md
7z e SonicAudioTools.7z CsbEditor.exe SonicAudioLib.dll
ls
cp ./CsbEditor.exe ./../../dependencies/SonicAudioTools/CsbEditor.exe
cp ./SonicAudioLib.dll ./../../dependencies/SonicAudioTools/SonicAudioLib.dll
cp ./LICENSE.md ./../../dependencies/SonicAudioTools/LICENSE

cd ..


#AliceModLoader
git clone --depth=1 https://github.com/RadiantDerg/AliceModLoader
cd "AliceModLoader"

cat ./LICENSE > ./../../dependencies/AliceModLoader/LICENSE
echo "" >> ./../../dependencies/AliceModLoader/LICENSE
cat ./docs/OpenSource.md >> ./../../dependencies/AliceModLoader/LICENSE
cp -r ./UpdateServer ./../../dependencies/AliceModLoader
rm ./../../dependencies/AliceModLoader/UpdateServer/update.ini

cd ..
