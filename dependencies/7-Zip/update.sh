#!/bin/bash
set -e

echo "Preparing 7-Zip version $SEVEN_ZIP_VERSION"

mkdir "7-Zip"
cd "7-Zip"
url=$(curl -LIs -w %{url_effective} -o /dev/null https://sourceforge.net/projects/sevenzip/files/$SEVEN_ZIP_VERSION/download)
curl  $url > 7z_install.exe
echo https://sourceforge.net/projects/sevenzip/files/7-Zip/$SEVEN_ZIP_VERSION > ./../../dependencies/7-Zip/LINK

7z e 7z_install.exe 7z.exe 7z.dll License.txt

cp 7z.exe       ./../../dependencies/7-Zip/7z.exe
cp 7z.dll       ./../../dependencies/7-Zip/7z.dll
cp License.txt  ./../../dependencies/7-Zip/License.txt

cd ..
