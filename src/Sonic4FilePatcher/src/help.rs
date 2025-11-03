pub fn get_text() -> &'static str {
"Usage:
    `Sonic4FilePatcher`
        If \"mods\" directory exists, patch all files used by enabled mods, else show this help message.

    `Sonic4FilePatcher recover`
        Recover original files that were changed by enabled mods.

    `Sonic4FilePatcher -h` and
    `Sonic4FilePatcher --help`
        Show this help message."
}

pub fn print() {
    println!("{}", get_text());
}