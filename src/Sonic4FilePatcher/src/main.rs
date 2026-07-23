use std::{env, ops};

mod help;
mod mod_management;
mod sha_checker;
mod version;

fn main() {
    let mut args = env::args().skip(1);
    match args.next() {
        Some(arg) => {
            match &arg[ops::RangeFull] {
                "--help" | "-h" => help::print(),
                "--version" | "-v" => version::print(),
                "recover" => mod_management::full_recover_of_files(),
                _ => help::print(),
            }
        },
        None => mod_management::load_file_mods(),
    };
}