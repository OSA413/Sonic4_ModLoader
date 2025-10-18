use std::{fs, path::Path};
use crate::amb::Amb;

pub fn extract_amb(target_file: String, dir_to_extract: Option<String>) {
    let amb = Amb::new_from_file_name(&target_file);
    match amb {
        Ok(amb) => {
            let base_dir = dir_to_extract.unwrap_or(format!("{}_extracted", &target_file));
            let base_dir = Path::new(&base_dir);
            for binary_object in amb.objects {
                match fs::write(base_dir.join(&binary_object.name), &binary_object.data) {
                    Ok(_) => println!("Extracted {}", &binary_object.name),
                    Err(e) => println!("Error: {}", e),
                }
            }
        },
        Err(e) => println!("Error: {}", e),
    };
}