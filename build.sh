cd "$(dirname "$0")"

bash Sonic4_ModLoader/generate-version.sh

echo "Compiling..."
dotnet publish Sonic4_ModLoader -c Release -m

EXIT_CODE="$?"
if [ "$EXIT_CODE" != "0" ]; then
    exit $EXIT_CODE
fi