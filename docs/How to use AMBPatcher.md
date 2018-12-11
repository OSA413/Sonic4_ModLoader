# How to use AMBPatcher

How to use commands:

1. Open your favorite terminal/command prompt (Win + R -> `cmd` on Windows, Ctrl + Alt + T on Linux/Ubuntu)
2. `cd` to the AMBPatcher's directory.
3. Enter commands.

Available commands:

* `AMBPatcher -h` and `AMBPatcher --help` - Show help message.
* `AMBPatcher` - Patch all files used by enabled mods.
* `AMBPatcher [AMB file]` and `AMBPatcher extract [AMB file]` - Extract all files from `[AMB file]` to `[AMB file]_extracted` directory.
* `AMBPatcher extract [AMB file] [dest dir]` - Extract all files from `[AMB file]` to `[dest dir]` directory.
* `AMBPatcher read [AMB file]` - Prints content of `[AMB file]` (File names, pointers, and lengths).
* `AMBPatcher patch [AMB file] [another file]` - Patch `[AMB file]` by `[another file]` if `[another file]` is in `[AMB file]`.
* `AMBPatcher [AMB file] [directory]` and `AMBPatcher patch [AMB file] [directory]` - Patch `[AMB file]` by all files in `[directory]` (recursively) if those files are in `[AMB file]`.