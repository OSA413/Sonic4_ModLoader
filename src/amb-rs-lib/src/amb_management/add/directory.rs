use std::{fs, path::Path};
use crate::{amb::Amb, amb_management};

pub fn add_dir_of_files_to_amb(amb: &mut Amb, dir_to_add: &Path) -> usize {
    let mut files_chain = common::walk_dir::walk_dir(dir_to_add, None);

    files_chain.sort();

    for file_path in &files_chain {
        match amb_management::add::file::add_file_to_amb(amb, &file_path, None) {
            Ok(_) => (),
            Err(e) => println!("Error: {e}"),
        };
    }

    files_chain.len()
}

pub fn add_dir_to_amb_from_dir_path(target_file: &Path, dir_to_add: &Path) {
    let amb_result = Amb::new_from_file_name(&target_file.display().to_string());
    match amb_result {
        Ok(mut amb) => {
            if add_dir_of_files_to_amb(&mut amb, dir_to_add) == 0 {
                println!("No files were added to AMB file, I'm not rewriting the file now.");
            } else {
                match fs::write(&amb.amb_path, amb.write()) {
                    Ok(_) => (),
                    Err(e) => println!("Error: {}", e),
                }
            }
        },
        Err(e) => println!("Error reading AMB file: {}", e),
    }
}
