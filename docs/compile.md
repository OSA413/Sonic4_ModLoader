# How to compile this program on Windows

Things you'll need:
* [.Net 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [Bash from Git](https://gitforwindows.org/)

Steps:
0. Clone or download the repo
1. Change current directory to the root of repo
2. Run `open-bash` to open Bash.
3. You are in MinGW's Bash now. Run the following comands:
```
bash update_dependencies.sh
bash build_pack.sh
bash ./tests/run_tests.sh --no-crash
```

4. The files will go to `dist/Sonic4ModLoader` directory.
