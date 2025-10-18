use std::{fs, path::Path};
use glob::glob;
use crate::amb::Amb;

pub fn add_dir_to_amb(target_file: &Path, dir_to_add: &Path) {
    let amb_result = Amb::new_from_file_name(&target_file.display().to_string());
    match amb_result {
        Ok(mut amb) => {
            let files_to_add = glob(&format!("{}/**/*.*", dir_to_add.display().to_string()));
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
                    match fs::write(target_file, amb.write()) {
                        Ok(_) => (),
                        Err(e) => println!("Error: {}", e),
                    }
                },
                Err(e) => println!("Error reading directory: {}", e),
            }
        },
        Err(e) => println!("Error reading AMB file: {}", e),
    }
}
