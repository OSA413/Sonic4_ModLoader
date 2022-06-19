#!/bin/bash
cd "$(dirname "$0")"

rm -rf "dependencies_source"
mkdir "dependencies_source"
cd "dependencies_source"

EXIT_CODE=0

#SonicAudioTools
git clone --depth=1 https://github.com/blueskythlikesclouds/SonicAudioTools
[ "$?" != "0" ] && EXIT_CODE=1
cd "SonicAudioTools"
nuget restore SonicAudioTools.sln
[ "$?" != "0" ] && EXIT_CODE=1
msbuild.exe SonicAudioTools.sln /p:Configuration=Release -m
[ "$?" != "0" ] && EXIT_CODE=1

cp ./Release/CsbEditor.exe ./../../dependencies/SonicAudioTools/CsbEditor.exe
cp ./Release/SonicAudioLib.dll ./../../dependencies/SonicAudioTools/SonicAudioLib.dll
cp ./LICENSE.md ./../../dependencies/SonicAudioTools/LICENSE

cd ..

#7-Zip
mkdir "7-Zip"
cd "7-Zip"
url=$(curl -LIs -w %{url_effective} -o /dev/null https://sourceforge.net/projects/sevenzip/files/latest/download)
[ "$?" != "0" ] && EXIT_CODE=1
version=$(basename $(dirname $url))
curl  $url > 7z_install.exe
[ "$?" != "0" ] && EXIT_CODE=1
echo https://sourceforge.net/projects/sevenzip/files/7-Zip/$version > ./../../dependencies/7-Zip/LINK

7z e 7z_install.exe 7z.exe 7z.dll License.txt

cp 7z.exe       ./../../dependencies/7-Zip/7z.exe
cp 7z.dll       ./../../dependencies/7-Zip/7z.dll
cp License.txt  ./../../dependencies/7-Zip/License.txt

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

exit $EXIT_CODE