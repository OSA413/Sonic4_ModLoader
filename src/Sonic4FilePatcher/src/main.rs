use std::{env, ops};

use common_binary::cli;

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
                "recover" => cli::handle_result(mod_management::full_recover_of_files()),
                _ => help::print(),
            }
        },
        None => cli::handle_result(mod_management::load_file_mods()),
    };
}