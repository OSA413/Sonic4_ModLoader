#!/bin/bash
set -e

echo "Preparing AliceModLoader version $ALICE_MOD_LOADER_VERSION"

git clone https://github.com/RadiantDerg/AliceModLoader
cd "AliceModLoader"
git checkout $ALICE_MOD_LOADER_VERSION

cat ./LICENSE > ./../../dependencies/AliceModLoader/LICENSE
echo "" >> ./../../dependencies/AliceModLoader/LICENSE
cat ./docs/OpenSource.md >> ./../../dependencies/AliceModLoader/LICENSE
cp -r ./UpdateServer ./../../dependencies/AliceModLoader
rm ./../../dependencies/AliceModLoader/UpdateServer/update.ini

cd ..