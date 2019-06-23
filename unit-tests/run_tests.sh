#!/bin/bash
cd "$(dirname "$0")"

echo
echo "Running unit tests..."

for dir in $(ls ./); do
    [ $dir == "run_tests.sh" ] && continue
    echo
    echo "$dir"
    python3 $dir"/update_from_release.py"
    python3 $dir"/unit_test.py"
done
