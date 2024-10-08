#!/bin/bash
cd "$(dirname "$0")"
current_commit="$(git rev-parse HEAD)"
version="$(git describe --tags --abbrev=0)"
[ $? = 0 ] && last_tag_commit="$(git rev-list -n 1 $version)" || last_tag_commit="0"

[ $current_commit != $last_tag_commit ] && version=${current_commit:0:7}

# echo $version > version

cp ./config.rs.in ./config.rs
sed -i -e "s/@VERSION@/\"$version\"/g" ./config.rs