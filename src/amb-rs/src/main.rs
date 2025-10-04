use std::path::Path;

mod amb;
mod binary_reader;
mod show_help;

fn main() -> () {
    let mut args = std::env::args().skip(1);
    let arg = args.next();
    match arg {
        Some(arg) => {
            match &arg[..] {
                "--help" | "-h" => show_help::print_help(),
                "recover" => todo!("mod_management::recover"),
                "add" => {
                    match args.next() {
                        Some(target_file) => {
                            match args.next() {
                                Some(path_to_add) => {
                                    let path_to_add = Path::new(&path_to_add);
                                    if path_to_add.is_dir() {
                                        todo!("amb_management::add_dir_to_amb");
                                    } else if path_to_add.is_file() {
                                        let internal_file_name = args.next();
                                        todo!("amb_management::add_file_to_amb");
                                    }
                                },
                                None => println!(),
                            }
                        }
                        None => println!("Usage: add <target_file> <file_to_add> <internal_file_name>
Or: add <target_file> <dir_of_files_to_add>"),
                    }
                },
                "extract" => {
                    match args.next() {
                        Some(file) => {
                            let dir = args.next();
                            todo!("amb_management::extract_amb_from_file (guess ordinary or WP version)");
                        },
                        None => println!("Usage: extract <file>"),
                    }
                },
                "read" => {
                    match args.next() {
                        Some(file) => todo!("amb_management::print_amb_table_of_content_from_file"),
                        None => println!("Usage: extract <file>"),
                    }
                },
                "swap_endianness" => {
                    match args.next() {
                        Some(file) => todo!("amb_management::swap_endianness_and_save"),
                        None => println!("Usage: swap_endianness <file>"),
                    }
                },
                "endianness" => {
                    match args.next() {
                        Some(file) => todo!("amb_management::print_endianness"),
                        None => println!("Usage: endianness <file>"),
                    }
                },
                "create" => {
                    match args.next() {
                        Some(file_name) => todo!("amb_management::create_amb"),
                        None => println!("Usage: create <file_name>"),
                    }
                },
                // Is this needed?
                "recreate" => {
                    match args.next() {
                        Some(file) => todo!("amb_management::recreate_amb"),
                        None => println!("Usage: recreate <file>"),
                    }
                },
                _ => {
                    let path = Path::new(&arg);
                    if path.exists() {
                        if path.is_dir() {
                            todo!("amb_management::recreate_amb_from_dir");
                        } else if path.is_file() {
                            let dir = args.next();
                            todo!("amb_management::extract_amb_from_file");
                        }
                    }
                },
            }
        },
        None => todo!("mod_management::load_file_mods"),
    };
}