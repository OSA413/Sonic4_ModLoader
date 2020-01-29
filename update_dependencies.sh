#!/bin/bash
#please, keep all sh scripts in Unix new line (LF)
cd "$(dirname "$0")"

rm -rf "dependencies_source"
mkdir "dependencies_source"
cd "dependencies_source"


#SonicAudioTools
git clone --depth=1 https://github.com/blueskythlikesclouds/SonicAudioTools
cd "SonicAudioTools"
nuget restore SonicAudioTools.sln
msbuild SonicAudioTools.sln /p:Configuration=Release

cp ./Release/CsbEditor.exe ./../../dependencies/SonicAudioTools/CsbEditor.exe
cp ./Release/SonicAudioLib.dll ./../../dependencies/SonicAudioTools/SonicAudioLib.dll
cp ./LICENSE.md ./../../dependencies/SonicAudioTools/LICENSE

cd ..

#7-Zip
mkdir "7-Zip"
cd "7-Zip"
url=$(curl -LIs -w %{url_effective} -o /dev/null https://sourceforge.net/projects/sevenzip/files/latest/download)
version=$(basename $(dirname $url))
curl  $url > 7z_install.exe
echo https://sourceforge.net/projects/sevenzip/files/7-Zip/$version > ./../../dependencies/7-Zip/LINK

7z x 7z_install.exe

cp 7z.exe       ./../../dependencies/7-Zip/7z.exe
cp 7z.dll       ./../../dependencies/7-Zip/7z.dll
cp License.txt  ./../../dependencies/7-Zip/License.txt
