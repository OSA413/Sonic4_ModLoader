# How to compile this program on Linux (Ubuntu)

Note: See `.github/workflows/release_linux.yml` for up-to-date instructions.

You will need .Net 6 and Rust.

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
