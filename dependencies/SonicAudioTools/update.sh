#!/bin/bash
set -e

echo "Preparing SonicAudioTools version $SONIC_AUDIO_TOOLS_VERSION"

mkdir "SonicAudioTools"
cd "SonicAudioTools"
curl -L https://github.com/blueskythlikesclouds/SonicAudioTools/releases/download/$SONIC_AUDIO_TOOLS_VERSION/SonicAudioTools.7z > SonicAudioTools.7z
curl https://raw.githubusercontent.com/blueskythlikesclouds/SonicAudioTools/master/LICENSE.md > LICENSE.md
7z e SonicAudioTools.7z CsbEditor.exe SonicAudioLib.dll
ls
cp ./CsbEditor.exe ./../../dependencies/SonicAudioTools/CsbEditor.exe
cp ./SonicAudioLib.dll ./../../dependencies/SonicAudioTools/SonicAudioLib.dll
cp ./LICENSE.md ./../../dependencies/SonicAudioTools/LICENSE

cd ..
