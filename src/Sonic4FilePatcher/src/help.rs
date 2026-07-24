pub fn print() {
    println!("        Sonic4FilePatcher by OSA413
        Released under the MIT License
        {}

Usage:
    `Sonic4FilePatcher`
        If \"mods\" directory exists, patch all files used by enabled mods, else show this help message.

    `Sonic4FilePatcher recover`
        Recover original files that were changed by enabled mods.

    `Sonic4FilePatcher -v` and
    `Sonic4FilePatcher --version`
        Show versions of used tools.

    `Sonic4FilePatcher -h` and
    `Sonic4FilePatcher --help`
        Show this help message.", env!("CARGO_PKG_REPOSITORY"));
}