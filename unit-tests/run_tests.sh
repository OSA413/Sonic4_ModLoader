#!/bin/bash
cd "$(dirname "$0")"

echo
echo "Running unit tests..."

for dir in $(ls ./actual-tests); do
    echo
    echo "$dir"
    python3 "actual-tests/$dir/update_from_release.py"
    python3 "actual-tests/$dir/unit_test.py"
done