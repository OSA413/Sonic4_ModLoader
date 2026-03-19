# How to compile this program on Windows

Note: See `.github/workflows/release_win.yml` for up-to-date instructions.

Things you'll need:

* [.Net 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [Python 3](https://www.python.org)
* [Bash from Git](https://gitforwindows.org/)
* [Rust with Rustup](https://rustup.rs/)
* [gvsbuild](https://github.com/wingtk/gvsbuild)

Prepare GTK4:

```sh
gvsbuild build gtk4 libadwaita librsvg
```

Run the following commands:

```sh
# This command may fail, doesn't affect Mod Loader-only compilation
bash update_dependencies_win.sh
bash src/common/src/generate-version.sh
dotnet publish src_old -c Release -m
cargo build --release
bash pack_win.sh

# Run old tests for AMBPatcher
bash tests/run_tests.sh --no-crash
```

The files will go to `dist/Sonic4ModLoader` directory.
