cd "$(dirname "$0")"

bash src_old/generate-version.sh
bash src/common/src/generate-version.sh

echo "Compiling..."
dotnet publish src_old -c Release -m

EXIT_CODE="$?"
if [ "$EXIT_CODE" != "0" ]; then
    exit $EXIT_CODE
fi