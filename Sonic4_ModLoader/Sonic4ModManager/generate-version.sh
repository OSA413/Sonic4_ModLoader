#!/bin/bash
echo "Script is running"
cd "$(dirname "$0")"
current_commit="$(git rev-parse HEAD)"
echo "$current_commit"
version="$(git describe --tags --abbrev=0)"
echo "$version"
last_tag_commit="$(git rev-list -n 1 $version)"
echo "$last_tag_commit"

[ $current_commit != $last_tag_commit ] && version=${current_commit:0:7}

echo $version > version