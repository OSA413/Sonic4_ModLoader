use std::{env, ops, path::Path};

mod amb;
mod binary_reader;
mod help;
mod amb_management;
mod mod_management;

fn main() -> () {
    let mut args = env::args().skip(1);
    match args.next() {
        Some(arg) => {
            match &arg[ops::RangeFull] {
                "--help" | "-h" => help::print(),
                "recover" => mod_management::recover(),
                "add" => {
                    match args.next() {
                        Some(target_file) => {
                            let target_file = Path::new(&target_file);
                            match args.next() {
                                Some(path_to_add) => {
                                    let path_to_add = Path::new(&path_to_add);
                                    match path_to_add.is_dir() {
                                        true => amb_management::add_dir_to_amb(target_file, path_to_add),
                                        false => amb_management::add_file_to_amb(target_file, path_to_add, args.next()),
                                    }
                                },
                                None => println!("Usage: add <target_file> <file_to_add> <internal_file_name>
Or: add <target_file> <dir_of_files_to_add>"),
                            }
                        }
                        None => println!("Usage: add <target_file> <file_to_add> <internal_file_name>
Or: add <target_file> <dir_of_files_to_add>"),
                    }
                },
                "extract" => {
                    match args.next() {
                        Some(file) => amb_management::extract_amb(file, args.next()),
                        None => println!("Usage: extract <file>"),
                    }
                },
                "read" => {
                    match args.next() {
                        Some(file) => amb_management::print_amb_table_of_content_from_file(file),
                        None => println!("Usage: read <file>"),
                    }
                },
                "read_json" => {
                    match args.next() {
                        Some(file) => amb_management::print_json_amb_table_of_content_from_file(file),
                        None => println!("Usage: read_json <file>"),
                    }
                },
                "swap_endianness" => {
                    match args.next() {
                        Some(file) => amb_management::swap_endianness_and_save(file),
                        None => println!("Usage: swap_endianness <file>"),
                    }
                },
                "endianness" => {
                    match args.next() {
                        Some(file) => amb_management::print_endianness(file),
                        None => println!("Usage: endianness <file>"),
                    }
                },
                "create" => {
                    match args.next() {
                        Some(file_name) => amb_management::create_amb(file_name),
                        None => println!("Usage: create <file_name>"),
                    }
                },
                "recreate" => {
                    match args.next() {
                        Some(file) => amb_management::recreate_amb(file),
                        None => println!("Usage: recreate <file>"),
                    }
                },
                _ => {
                    let path = Path::new(&arg);
                    match path.is_dir() {
                        true => amb_management::recreate_amb_from_dir(path),
                        false => amb_management::extract_amb_from_file(path, args.next()),
                    }
                },
            }
        },
        None => mod_management::load_file_mods(),
    };
}