use std::{fs, path::Path};
use crate::amb::Amb;

pub fn add_file_to_amb(target_file: &Path, file_to_add: &Path, internal_file_name: Option<String>) {
    let amb_result = Amb::new_from_file_name(&target_file.display().to_string());
    match amb_result {
        Ok(mut amb) => {
            amb.add(file_to_add.display().to_string(), internal_file_name);
            match fs::write(target_file, amb.write()) {
                Ok(_) => (),
                Err(e) => println!("Error: {}", e),
            }
        },
        Err(e) => println!("Error: {}", e),
    };
}