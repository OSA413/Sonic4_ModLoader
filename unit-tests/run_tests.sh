#!/bin/bash
cd "$(dirname "$0")"

echo "Running unit tests..."

python3 "actual-tests/AMBPatcher/update_from_release.py"
python3 "actual-tests/AMBPatcher/unit_test.py"
