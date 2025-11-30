use std::{env, ops, path::Path};
use amb_rs_lib::{amb_management, error::AmbLibRsError};

mod help;

fn exit_with_error(error: String) {
    eprintln!("{error}");
    std::process::exit(1);
}

fn handle_result(result: Result<(), AmbLibRsError>) {
    match result {
        Ok(_) => (),
        Err(e) => exit_with_error(format!("{e:?}")),
    }
}

fn main() {
    let mut args = env::args().skip(1);
    match args.next() {
        Some(arg) => {
            match &arg[ops::RangeFull] {
                "--help" | "-h" => help::print(),
                "--version" | "-v" => println!("amb-rs version: {}", common::global::VERSION),
                "add" => {
                    match args.next() {
                        Some(target_file) => {
                            let target_file = Path::new(&target_file);
                            match args.next() {
                                Some(path_to_add) => {
                                    let path_to_add = Path::new(&path_to_add);
                                    match path_to_add.is_dir() {
                                        true => handle_result(amb_management::add::directory::add_dir_to_amb_from_dir_path(target_file, path_to_add)),
                                        false => handle_result(amb_management::add::file::add_file_to_file(target_file, path_to_add, args.next())),
                                    }
                                },
                                None => exit_with_error("Usage: add <target_file> <file_to_add> [internal_file_name]
Or: add <target_file> <dir_of_files_to_add>".to_string()),
                            }
                        }
                        None => exit_with_error("Usage: add <target_file> <file_to_add> [internal_file_name]
Or: add <target_file> <dir_of_files_to_add>".to_string()),
                    }
                },
                "remove" => {
                    let target_file = args.next();
                    match target_file {
                        Some(target_file) => {
                            let object_name = args.next();
                            match object_name {
                                Some(object_name) => handle_result(amb_management::remove::remove_object_from_file_and_write_to_file(target_file, object_name)),
                                None => exit_with_error("Usage: remove <target_file> <object_name>".to_string()),
                            }
                        },
                        None => exit_with_error("Usage: remove <target_file> <object_name>".to_string()),
                    }
                },
                "extract" => {
                    match args.next() {
                        Some(file) => handle_result(amb_management::extract::extract_amb(file, args.next())),
                        None => exit_with_error("Usage: extract <file>".to_string()),
                    }
                },
                "read" => {
                    match args.next() {
                        Some(file) => handle_result(amb_management::json::print_from_file_to_stdout(file)),
                        None => exit_with_error("Usage: read <file>".to_string()),
                    }
                },
                "swap_endianness" => {
                    match args.next() {
                        Some(file) => handle_result(amb_management::endianness::swap_endianness_and_save(file, args.next())),
                        None => exit_with_error("Usage: swap_endianness <file> [save_as_file_name]".to_string()),
                    }
                },
                "create" => {
                    match args.next() {
                        Some(file_name) => handle_result(amb_management::create::create_amb(file_name)),
                        None => exit_with_error("Usage: create <file_name>".to_string()),
                    }
                },
                "recreate" => {
                    match args.next() {
                        Some(file) => handle_result(amb_management::recreate::recreate_amb(file, args.next())),
                        None => exit_with_error("Usage: recreate <file> [save_as_file_name]".to_string()),
                    }
                },
                _ => {
                    let path = Path::new(&arg);
                    match path.is_dir() {
                        true => handle_result(amb_management::recreate::recreate_amb_from_dir(arg)),
                        false => handle_result(amb_management::extract::extract_amb(arg, args.next())),
                    }
                },
            }
        },
        None => help::print(),
    };
}