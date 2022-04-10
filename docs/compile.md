# How to compile this program on Windows

Things you'll need:
* [.Net 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [Python 3](https://www.python.org/) to run tests
* [Bash from Git](https://gitforwindows.org/)

Steps:

0. Clone or download the repo
1. Change current directory to the root of repo (or if you are not using CLI, open folder of the repo)
2. If you want to run tests, run `python3-is-python` to fix Python calls in scripts (requires admin privileges)
3. Run `open-bash` to open Bash.
4. You are in MinGW's Bash now. Run the following commands:
```
bash update_dependencies.sh #This command may fail, doesn't affect Mod Loader-only compilation
bash build_pack.sh
bash tests/run_tests.sh --no-crash
```

5. The files will go to `dist/Sonic4ModLoader` directory.
