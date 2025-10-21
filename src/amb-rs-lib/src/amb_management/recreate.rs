use std::{fs, path::{Path, PathBuf}};
use crate::{amb::Amb, amb_management};

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
        let path = Path::new(&possible_file).to_path_buf();
        path
    } else {
        let mut result = PathBuf::new();
        let possible_file = dir_path.join(".amb");
        if possible_file.is_file() {
            result = possible_file
        }
        
        let possible_file = dir_path.join(".AMB");
        if possible_file.is_file() {
            result = possible_file
        }

        result
    };

    amb_management::add::directory::add_dir_to_amb_from_dir_path(&amb_file_path, &dir_path);
}