#!/bin/bash
cd "$(dirname "$0")"

rm -rf "dependencies_source"
mkdir "dependencies_source"
cd "dependencies_source"

EXIT_CODE=0

#SonicAudioTools
mkdir "SonicAudioTools"
cd "SonicAudioTools"
url=$(curl -LIs -w %{url_effective} -o /dev/null https://github.com/blueskythlikesclouds/SonicAudioTools/releases/latest)
[ "$?" != "0" ] && EXIT_CODE=1
version=$(basename $url)
curl -L https://github.com/blueskythlikesclouds/SonicAudioTools/releases/download/$version/SonicAudioTools.7z > SonicAudioTools.7z
[ "$?" != "0" ] && EXIT_CODE=1
curl https://raw.githubusercontent.com/blueskythlikesclouds/SonicAudioTools/master/LICENSE.md > LICENSE.md
[ "$?" != "0" ] && EXIT_CODE=1
7z e SonicAudioTools.7z CsbEditor.exe SonicAudioLib.dll

cp ./CsbEditor.exe ./../../dependencies/SonicAudioTools/CsbEditor.exe
[ "$?" != "0" ] && EXIT_CODE=1
cp ./SonicAudioLib.dll ./../../dependencies/SonicAudioTools/SonicAudioLib.dll
[ "$?" != "0" ] && EXIT_CODE=1
cp ./LICENSE.md ./../../dependencies/SonicAudioTools/LICENSE
[ "$?" != "0" ] && EXIT_CODE=1

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
[ "$?" != "0" ] && EXIT_CODE=1
cp 7z.dll       ./../../dependencies/7-Zip/7z.dll
[ "$?" != "0" ] && EXIT_CODE=1
cp License.txt  ./../../dependencies/7-Zip/License.txt
[ "$?" != "0" ] && EXIT_CODE=1

cd ..

#AliceModLoader
git clone --depth=1 https://github.com/RadiantDerg/AliceModLoader
cd "AliceModLoader"

cat ./LICENSE > ./../../dependencies/AliceModLoader/LICENSE
[ "$?" != "0" ] && EXIT_CODE=1
echo "" >> ./../../dependencies/AliceModLoader/LICENSE
[ "$?" != "0" ] && EXIT_CODE=1
cat ./docs/OpenSource.md >> ./../../dependencies/AliceModLoader/LICENSE
[ "$?" != "0" ] && EXIT_CODE=1
cp -r ./UpdateServer ./../../dependencies/AliceModLoader
[ "$?" != "0" ] && EXIT_CODE=1
rm ./../../dependencies/AliceModLoader/UpdateServer/update.ini
[ "$?" != "0" ] && EXIT_CODE=1

cd ..

exit $EXIT_CODE