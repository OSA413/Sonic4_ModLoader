use std::{fs, path::{Path, PathBuf}};
use crate::{amb::Amb, amb_management, error::AmbLibRsError};

pub fn recreate_amb(file: String, save_as_file_name: Option<String>) -> Result<(), AmbLibRsError> {
    let amb = Amb::new_from_file_name(&file)?;
    fs::write(save_as_file_name.unwrap_or(file), amb.write())?;
    Ok(())
}

pub fn recreate_amb_from_dir(dir: String) -> Result<(), AmbLibRsError> {
    let dir_path = Path::new(&dir);
    if !dir_path.is_dir() {
        return Err(AmbLibRsError::Io(std::io::Error::other("Error: {dir:?} is not a directory")));
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

    amb_management::add::directory::add_dir_to_amb_from_dir_path(&amb_file_path, dir_path)
}