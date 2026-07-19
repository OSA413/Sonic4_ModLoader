# How to compile this program on Windows

Note: See `.github/workflows/main_win.yml` for up-to-date instructions.

Things you'll need:

* [Python 3](https://www.python.org)
* [Bash from Git](https://gitforwindows.org/)
* [Rust with Rustup](https://rustup.rs/)
* [gvsbuild](https://github.com/wingtk/gvsbuild)

Prepare GTK4:

```bash
gvsbuild build gtk4 libadwaita librsvg
```

Run the following commands from Git Bash (not WSL Bash):

```bash
# This command may fail, doesn't affect Mod Loader-only compilation
bash update_dependencies.sh
bash src/common_modloader/src/generate-version.sh
cargo build --release
bash pack_win.sh
```

The files will go to `dist/Sonic4ModLoader` directory.
