use std::{fs, path::Path};
use glob::glob;
use crate::amb::Amb;

pub fn recreate_amb(file: String, save_as_file_name: Option<String>) {
    let amb = Amb::new_from_file_name(&file);
    match amb {
        Ok(amb) => {
            match fs::write(save_as_file_name.unwrap_or(file), amb.write()) {
                Ok(_) => (),
                Err(e) => println!("Error: {}", e),
            }
        },
        Err(e) => println!("Error: {}", e),
    };
}

pub fn recreate_amb_from_dir(dir: String) {
    let dir_path = Path::new(&dir);
    if !dir_path.is_dir() {
        println!("Error: {:?} is not a directory", dir);
        return;
    }

    let extracted_prefix = "_extracted";

    let amb_file_path = if dir.ends_with(extracted_prefix) {
        let possible_file = &dir;
        let possible_file = possible_file.chars().take(possible_file.len() - extracted_prefix.len()).collect::<String>();
        if Path::new(&possible_file).is_file() {
            Some(possible_file)
        } else {
            None
        }
    } else {
        let mut result = None;
        let possible_file = dir_path.join(".amb");
        if possible_file.is_file() {
            result = possible_file.as_os_str().to_str().and_then(|x| Some(x.to_string()))
        }
        
        let possible_file = dir_path.join(".AMB");
        if possible_file.is_file() {
            result = possible_file.as_os_str().to_str().and_then(|x| Some(x.to_string()))
        }

        result
    };

    match amb_file_path {
        Some(amb_file_path) => {
            let amb_result = Amb::new_from_file_name(&amb_file_path);
            match amb_result {
                Ok(mut amb) => {
                    let files_to_add = glob(&format!("{}/**/*.*", dir));
                    match files_to_add {
                        Ok(files) => {
                            for entry in files {
                                match entry {
                                    Ok(path) => {
                                        let file_path = path.display().to_string();
                                        amb.add(file_path, None);
                                    },
                                    Err(e) => println!("Glob error: {}", e),
                                }
                            }
                            match fs::write(amb_file_path, amb.write()) {
                                Ok(_) => (),
                                Err(e) => println!("Error: {}", e),
                            }
                        },
                        Err(e) => println!("Error reading directory: {}", e),
                    }
                },
                Err(e) => println!("Error reading AMB file: {}", e),
            }
        },
        None => println!("No AMB file found for provided directory"),
    }
}