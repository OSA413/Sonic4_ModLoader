export SEVEN_ZIP_VERSION="26.01"
export ALICE_MOD_LOADER_VERSION="4301e9b"
export SONIC_AUDIO_TOOLS_VERSION="v1.0.1"

rm -rf "dependencies_source"
mkdir "dependencies_source"
cd "dependencies_source"

if [[ "$OSTYPE" != "linux-gnu"* ]]; then
    bash ../dependencies/7-Zip/update.sh
fi
bash ../dependencies/SonicAudioTools/update.sh
bash ../dependencies/AliceModLoader/update.sh