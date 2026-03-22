# How to compile this program on Linux (Ubuntu)

Note: See `.github/workflows/release_linux.yml` for up-to-date instructions.

You will need .Net 6 and Rust.

In order to make the game load all needed files via the patcher, you will need to cross compile some of the executables:

```sh
sudo apt install mingw-w64
rustup target add x86_64-pc-windows-gnu
cargo build --bin Sonic4FilePatcher --target x86_64-pc-windows-gnu
```

After that, run these commands:

```sh
sudo apt install libgtk-4-dev libadwaita-1-dev build-essential
bash update_dependencies_linux.sh
bash src/common/src/generate-version.sh
dotnet publish src_old/AMBPatcher -c Release -m
cargo build --release
bash pack_linux.sh
```

The files will go to `dist/Sonic4ModLoader` directory.
