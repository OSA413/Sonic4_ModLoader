use std::{fs, path::Path};
use crate::{amb::Amb, error::AmbLibRsError};

pub fn extract_amb(target_file: String, dir_to_extract: Option<String>) -> Result<(), AmbLibRsError> {
    let amb = Amb::new_from_file_name(&target_file)?;
    let base_dir = dir_to_extract.unwrap_or(format!("{target_file}_extracted"));
    let base_dir = Path::new(&base_dir);
    fs::create_dir_all(base_dir).unwrap();
    for binary_object in amb.objects {
        match fs::write(base_dir.join(&binary_object.name), &binary_object.data) {
            Ok(_) => println!("Extracted {}", &binary_object.name),
            Err(e) => println!("Error: {e}"),
        }
    };
    Ok(())
}