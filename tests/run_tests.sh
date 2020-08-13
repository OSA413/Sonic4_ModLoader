#!/bin/bash
cd "$(dirname "$0")"

echo
echo "Running tests..."

EXIT_CODE=0

for dir in $(ls ./); do
    [ ! -d $dir ] && continue
    [ $dir == "__pycache__" ] && continue
    echo
    echo "$dir"
    python3 $dir"/run.py" $1
    
    [ "$?" != "0" ] && EXIT_CODE=1

done

exit $EXIT_CODE
