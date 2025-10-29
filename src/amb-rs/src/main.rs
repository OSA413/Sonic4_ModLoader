use std::{env, ops, path::Path};
use amb_rs_lib::amb_management;

mod help;
mod mod_management;
mod sha_checker;

fn main() -> () {
    let mut args = env::args().skip(1);
    match args.next() {
        Some(arg) => {
            match &arg[ops::RangeFull] {
                "--help" | "-h" => help::print(),
                "--version" | "-v" => println!("amb-rs version: {}", common::global::VERSION),
                "recover" => mod_management::full_recover_of_files(),
                "add" => {
                    match args.next() {
                        Some(target_file) => {
                            let target_file = Path::new(&target_file);
                            match args.next() {
                                Some(path_to_add) => {
                                    let path_to_add = Path::new(&path_to_add);
                                    match path_to_add.is_dir() {
                                        true => amb_management::add::directory::add_dir_to_amb_from_dir_path(target_file, path_to_add),
                                        false => amb_management::add::file::add_file_to_file(target_file, path_to_add, args.next()),
                                    }
                                },
                                None => panic!("Usage: add <target_file> <file_to_add> [internal_file_name]
Or: add <target_file> <dir_of_files_to_add>"),
                            }
                        }
                        None => panic!("Usage: add <target_file> <file_to_add> [internal_file_name]
Or: add <target_file> <dir_of_files_to_add>"),
                    }
                },
                "remove" => {
                    let target_file = args.next();
                    match target_file {
                        Some(target_file) => {
                            let object_name = args.next();
                            match object_name {
                                Some(object_name) => amb_management::remove::remove_object_from_file_and_write_to_file(target_file, object_name),
                                None => panic!("Usage: remove <target_file> <object_name>"),
                            }
                        },
                        None => panic!("Usage: remove <target_file> <object_name>"),
                    }
                },
                "extract" => {
                    match args.next() {
                        Some(file) => amb_management::extract::extract_amb(file, args.next()),
                        None => panic!("Usage: extract <file>"),
                    }
                },
                "read" => {
                    match args.next() {
                        Some(file) => amb_management::json::print_from_file_to_stdout(file),
                        None => panic!("Usage: read <file>"),
                    }
                },
                "swap_endianness" => {
                    match args.next() {
                        Some(file) => amb_management::endianness::swap_endianness_and_save(file, args.next()),
                        None => panic!("Usage: swap_endianness <file> [save_as_file_name]"),
                    }
                },
                "create" => {
                    match args.next() {
                        Some(file_name) => amb_management::create::create_amb(file_name),
                        None => panic!("Usage: create <file_name>"),
                    }
                },
                "recreate" => {
                    match args.next() {
                        Some(file) => amb_management::recreate::recreate_amb(file, args.next()),
                        None => panic!("Usage: recreate <file> [save_as_file_name]"),
                    }
                },
                _ => {
                    let path = Path::new(&arg);
                    match path.is_dir() {
                        true => amb_management::recreate::recreate_amb_from_dir(arg),
                        false => amb_management::extract::extract_amb(arg, args.next()),
                    }
                },
            }
        },
        None => mod_management::load_file_mods(),
    };
}