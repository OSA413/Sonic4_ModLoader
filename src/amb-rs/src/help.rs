pub fn get_text() -> &'static str {
"        amb-rs by OSA413
        Released under the MIT License
        https://github.com/OSA413/Sonic4_ModLoader

Usage:
    `amb-rs`
        If \"mods\" directory exists, patch all files used by enabled mods, else show this help message.

    `amb-rs recover`
        Recover original files that were changed by enabled mods.

    `amb-rs add <target_file> <file_to_add> [internal_file_name]`
        Add file to AMB file with internal file name.
        If internal file name is not specified, determine it from path to file.

    `amb-rs add <target_file> <dir_of_files_to_add>`
        Add all files from directory to AMB file.
        Internal names of files are determined from path to each file.

    `amb-rs extract <file>`
        Extract files from AMB to \"<file>_extracted\" directory.
        Same as `amb-rs <file>`.

    `amb-rs read <file>`
        Print the contents of the AMB file to the stdout in JSON format.

    `amb-rs swap_endianness <file> [save_as_file_name]`
        Swap endianness of AMB (root) file and save.
        If [save_as_file_name] is not specified, overwrites input file.

    `amb-rs create <file_name>`
        Creates an empty AMB file at <file_name>.

    `amb-rs recreate <file> [save_as_file_name]`
        Recreate (read and save) the input AMB file.
        If [save_as_file_name] is not specified, overwrites input file.

    `amb-rs <file> [dir_to_extract]`
        Extract <file> (AMB file) to [dir_to_extract] directory.
        If [dir_to_extract] is not specified, extracts to \"<file>_extracted\" directory.
        Via GUI, you can drag&drop AMB file on amb-rs executable to extract it.

    `amb-rs <dir>`
        Recreate AMB file from <dir>.
        Needs target AMB file to exist near the directory.
        Via GUI, you can drag&drop AMB file on amb-rs executable to recreate it.

    `amb-rs -h` and
    `amb-rs --help`
        Show this help message."
}

pub fn print() {
    println!("{}", get_text());
}