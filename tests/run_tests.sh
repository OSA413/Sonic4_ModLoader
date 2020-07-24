#!/bin/bash
cd "$(dirname "$0")"

echo
echo "Running tests..."

for dir in $(ls ./); do
    [ ! -d $dir ] && continue
    [ $dir == "__pycache__" ] && continue
    echo
    echo "$dir"
    python3 $dir"/run.py"
done
