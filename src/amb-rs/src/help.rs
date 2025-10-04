pub fn get_text() -> &'static str {
"        amb-rs by OSA413
        Released under the MIT License
        https://github.com/OSA413

Usage:
    note: this help message is outdated and is not correct
    amb-rs - If \"mods\" directory exists, patch all files used by enabled mods, else show help message.
    amb-rs [AMB] and
    amb-rs extract [AMB] - Extract all files from [AMB] to \"[AMB]_extracted\" directory.
    amb-rs extract [AMB] [dir] - Extract all files from [AMB] to [dir].
    amb-rs read [AMB file] - Prints content of [AMB]
    amb-rs [AMB] [dir] and
    amb-rs add [AMB] [dir] - Add all files from [dir] to [AMB].
    amb-rs recover - Recover original files that were changed.
    amb-rs add [AMB] [file] - Add [file] to [AMB].
    amb-rs add [AMB] [file] [name] - Add [file] to [AMB] as [name].
    amb-rs endianness [AMB] - Print endianness of [AMB].
    amb-rs swap_endianness [AMB] - Swaps endianness of [AMB].
    amb-rs delete [AMB] [file] - Delete [file] from [AMB].
    amb-rs create [name] - Creates an empty AMB file with [name].
    amb-rs extract_all [path] - Extract all files from [path] (can be a file or directory) to be Mod Loader compatible.
    amb-rs -h and
    amb-rs --help - Show help message."
}

pub fn print() {
    println!("{}", get_text());
}