use std::{ffi::{OsStr}, fs, path::Path};
use crate::{amb::Amb};
use common::walk_dir;
use common_binary::error::CommonBinaryError;

fn continue_extraction(amb: Amb, base_dir: &Path) {
    for binary_object in amb.objects {
        let probably_amb = Amb::new_from_binary_object(&binary_object);
        match probably_amb {
            Ok(inner_amb) => {
                println!("Extracting {base_dir:?}");
                continue_extraction(inner_amb, &base_dir.join(&binary_object.name));
            }
            Err(_) => {
                let file_path = base_dir.join(&binary_object.name);
                fs::create_dir_all(file_path.parent().unwrap()).unwrap();
                match fs::write(file_path, &binary_object.data) {
                    Ok(_) => println!("Extracted {}", &binary_object.name),
                    Err(e) => eprintln!("Error: {e}"),
                }
            },
        }
    }
}

pub fn extract_amb(file_or_dir: String) -> Result<(), CommonBinaryError> {
    let path = Path::new(&file_or_dir);
    let probably_amb_files = if path.is_file() {
        vec![path.to_path_buf()]
    } else {
        walk_dir::walk_dir(path, Some(OsStr::new("amb")))
    };

    for entry in probably_amb_files {
        let path = entry.to_str().unwrap().to_string();
        println!("Extracting {path}");
        let amb = Amb::new_from_file_name(&path)?;
        let base_dir = format!("{path}_extracted");
        let base_dir = Path::new(&base_dir);
        continue_extraction(amb, &base_dir.to_path_buf());
    }
    println!("Done!");
    Ok(())
}